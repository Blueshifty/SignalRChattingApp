using Microsoft.Extensions.DependencyInjection;
using SignalRApp.Business.Services.Abstract;
using SignalRApp.Business.Services.Concrete;

namespace SignalRApp.Extensions
{
    public static class AddScopedExtension
    {
        public static IServiceCollection AddMyScoped(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
            return serviceCollection;
        }
    }
}