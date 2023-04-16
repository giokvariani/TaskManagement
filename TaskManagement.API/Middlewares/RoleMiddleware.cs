using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

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
            var authorizeAttributes = controllerType.CustomAttributes.Select(x => x.AttributeType).Where(x => x.Name == nameof(AuthorizeAttribute));

            var userName = context.User.Claims.Single(x => x.Type == "UserName").Value;
            var password = context.User.Claims.Single(x => x.Type == "Password").Value;

            if (authorizeAttributes.Any())
            {
                
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;
                var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password)).SingleOrDefault();

                await next(context);
            }
            else
            {
                await next(context);
            }
        }
    }
}
