using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SignalRApp.Business.Utilities.Mapper;
using SignalRApp.Business.Utilities.Security.Jwt;

namespace SignalRApp.Extensions
{
    public static class AddSingletonExtension
    {
        public static IServiceCollection AddMySingleton(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITokenHandler, TokenHandler>();
            serviceCollection.AddSingleton<IMapper, Mapper>();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return serviceCollection;
        }
    }
}