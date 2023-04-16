using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Attributes;
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
            if ((!context.User?.Identity?.IsAuthenticated) ?? true)
            {
                await next(context); 
                return;
            }
                

            var userName = context.User.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
            var password = context.User.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
            if (userName == null && password == null)
                throw new InvalidOperationException("Generate Token at First and input like -> Bearer ey7......mgl");
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;
            var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password)).SingleOrDefault();
            if (user == null)
                throw new EntityNotFoundException("Couldn't find the object");
            var userIsAdmin = user.Roles.Select(x => x.Role).Any(x => x.IsAdmin);
            if (userIsAdmin)
            {
                await next(context);
                return;
            }

            var controllerType = GetControllerType(context);

            var adminAuthorizeAttributes = controllerType.CustomAttributes.Where(x => x.AttributeType.Name == nameof(AdminPrivilegeAttribute));
            if (adminAuthorizeAttributes.Any())
            {
                throw new ValidationException("Access is denied");
            }
            var authorizeAttributes = controllerType.CustomAttributes.Where(x => x.AttributeType.Name == nameof(AuthorizeAttribute));

            if (authorizeAttributes.Any())
            {
                var persmissions = user.Roles.Select(x => x.Role.Persmissions);
                var flattenedArray = persmissions.SelectMany(flag => Enum.GetValues(typeof(PermissionType))
                                                    .OfType<PermissionType>()
                                                    .Where(f => flag.HasFlag(f))
                                                    .ToArray()).Distinct().ToArray().Select(x => x.ToString());
                if (flattenedArray.Any(x => x == context.Request.Method))
                    await next(context);
                else
                    throw new ValidationException("Access Is Denied");
            }
            else
            {
                await next(context);
            }
        }
    }
}
