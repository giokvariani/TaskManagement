using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.API.Middlewares
{

    public class CustomMiddleware : IMiddleware
    {
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

            await next(context);

            //var controllerType = GetControllerType(context);
            //var authorizeAttributes = controllerType.CustomAttributes.Select(x => x.AttributeType).OfType<AuthorizeAttribute>();
            //if (authorizeAttributes.Any())
            //{
            //    //Do something
            //    using (var scope = _serviceScopeFactory.CreateScope())
            //    {
            //        // Resolve the scoped service from the service scope
            //        var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            //        var user = (await userRepository.GetAsync(x => x.UserName == "Kvaro" && x.Password == "Hello")).SingleOrDefault();


            //        // Use scopedService in your middleware logic

            //        await _next(context);
            //    }
            //}
            //else
            //{
            //    await next(context);
            //}
        }
    }

    //public class RoleMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly IServiceScopeFactory _serviceScopeFactory;
    //    public RoleMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    //    {
    //        _next = next;
    //        _serviceScopeFactory = serviceScopeFactory;
    //    }
    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //    {

    //        var controllerType = GetControllerType(context);
    //        var authorizeAttributes = controllerType.CustomAttributes.Select(x => x.AttributeType).OfType<AuthorizeAttribute>();
    //        if (authorizeAttributes.Any())
    //        {
    //            //Do something
    //            using (var scope = _serviceScopeFactory.CreateScope())
    //            {
    //                // Resolve the scoped service from the service scope
    //                var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
    //                var user = (await userRepository.GetAsync(x => x.UserName == "Kvaro" && x.Password == "Hello")).SingleOrDefault();


    //                // Use scopedService in your middleware logic

    //                await _next(context);
    //            }
    //        }
    //        else
    //        {
    //            await next(context);
    //        }

    //    }

      
    //}


}
