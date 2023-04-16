using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Enums;

namespace TaskManagement.API.Middlewares
{

    public class RoleMiddleware : IMiddleware
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RoleMiddleware(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        private Type GetControllerType(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor descriptor)
            {
                return descriptor.ControllerTypeInfo.AsType();
            }
            return null;

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var controllerType = GetControllerType(context);
            var authorizeAttributes = controllerType.CustomAttributes.Where(x => x.AttributeType.Name == nameof(AuthorizeAttribute));

            if (authorizeAttributes.Any())
            {
                var userName = context.User.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
                var password = context.User.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
                if (userName == null && password == null)
                    throw new InvalidOperationException("Please log in at first");
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;
                var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password)).SingleOrDefault();
                var passedStringArguments = authorizeAttributes
                    .SelectMany(x => x.ConstructorArguments.Where(x => x.ArgumentType == typeof(string)).Select(ca => ca.Value))
                    .OfType<string>();

                if (user == null)
                    throw new EntityNotFoundException("Could not find the object");

                var userIsAdmin = user.Roles.Select(x => x.Role.IsAdmin).Any();
                var controllerIsForAdmin = passedStringArguments.Any(x => x == "Admin");
                if (userIsAdmin)
                {
                    await next(context);
                }

                else
                {
                    if (controllerIsForAdmin)
                        throw new ValidationException("Access This method is forbidden");
                    else
                    {
                        var persmissions = user.Roles.Select(x => x.Role.Persmissions);
                        var flattenedArray = persmissions.SelectMany(flag => Enum.GetValues(typeof(PermissionType))
                                                            .OfType<PermissionType>()
                                                            .Where(f => flag.HasFlag(f))
                                                            .ToArray()).Distinct().ToArray();

                    }
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
