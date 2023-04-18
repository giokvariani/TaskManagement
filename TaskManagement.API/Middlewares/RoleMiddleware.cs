using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using TaskManagement.API.Attributes;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.ExtensionMethods;
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
        private Maybe<Type> GetControllerType(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor descriptor)
            {
                return descriptor.ControllerTypeInfo.AsType();
            }
            return Maybe<Type>.None;

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            if ((!context.User?.Identity?.IsAuthenticated) ?? true)
            {
                await next(context); 
                return;
            }

            var userName = context.User.GetDataFromClaims("UserName");
            var password = context.User.GetDataFromClaims("Password");
            if (userName == null && password == null)
                throw new InvalidOperationException("Generate Token at First and input like -> Bearer ey7......mgl");

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;
            var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password))
                .SingleOrDefault();
            if (user == null)
                throw new EntityNotFoundException();

            var userIsAdmin = user.Roles.Select(x => x.Role).Any(x => x.IsAdmin);
            if (userIsAdmin)
            {
                await next(context);
                return;
            }

            var potentialControllerType = GetControllerType(context);
            if (potentialControllerType.HasNoValue)
                throw new InvalidOperationException("Couldn't figure out which controller it is");

            var controllerType = potentialControllerType.Value;

            var adminAuthorizeAttributes = controllerType.CustomAttributes.Where(x => x.AttributeType.Name == nameof(AdminPrivilegeAttribute));
            if (adminAuthorizeAttributes.Any())
                throw new ValidationException("Access is denied");

            var authorizeAttributes = controllerType.CustomAttributes.Where(x => x.AttributeType.Name == nameof(AuthorizeAttribute));
            if (authorizeAttributes.Any())
            {
                var persmissions = user.Roles.Select(x => x.Role.Persmissions);
                var flattenedArray = persmissions.SelectMany(flag => Enum.GetValues(typeof(PermissionType))
                                                    .OfType<PermissionType>()
                                                    .Where(f => flag.HasFlag(f))
                                                    .ToArray()).Distinct().Select(x => x.ToString().ToUpper());
                var methodVerb = context.Request.Method;

                var targetHttpVerb = StaticHelper.StaticHelper.HttpVerbsMap.Where(x => x.Value.Length > 1 && x.Value.Any(v => v == methodVerb));
                var httpMethodTarget = targetHttpVerb.Any() ? targetHttpVerb.Single().Key.ToUpper() : methodVerb;

                if (flattenedArray.Any(x => x == httpMethodTarget))
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
