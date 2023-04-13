using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManagement.Core.Application.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static void AddApplicatonLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
