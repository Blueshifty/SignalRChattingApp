using Microsoft.Extensions.DependencyInjection;
using SignalRApp.Business.Services.Abstract;
using SignalRApp.Business.Services.Concrete;

namespace SignalRApp.Extensions
{
    public static class AddTransientExtension
    {
        public static IServiceCollection AddMyTransient(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}