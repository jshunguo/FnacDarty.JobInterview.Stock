using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Repositories;
using FnacDarty.JobInterview.Stock.Validators;
using FnacDarty.JobInterview.Stock.Views;
using Microsoft.Extensions.DependencyInjection;


namespace FnacDarty.JobInterview.Stock.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();
                // Utilisez clientApp comme d'habitude
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Repositories
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IStockMovementRepository, StockMovementRepository>();

            // Base Factories
            services.AddTransient<ProductFactory>();
            services.AddTransient<StockMovementFactory>();

            // Validator Factory
            services.AddTransient<IValidatorFactory, ValidatorFactory>();

            // Factories Decorated
            services.AddTransient<IProductFactory>(serviceProvider =>
                new ValidatingProductFactoryDecorator(
                    serviceProvider.GetRequiredService<ProductFactory>(),
                    serviceProvider.GetRequiredService<IValidatorFactory>()
                ));

            services.AddTransient<IStockMovementFactory>(serviceProvider =>
                new ValidatingStockMovementFactoryDecorator(
                    serviceProvider.GetRequiredService<StockMovementFactory>(),
                    serviceProvider.GetRequiredService<IValidatorFactory>()
                ));

            // Stock Manager & Grid Service
            services.AddSingleton<IStockManager, StockManager>();
            services.AddTransient<IGridService, GridService>();

            // Client
            services.AddSingleton<IClient, ClientApp>();

            return services.BuildServiceProvider();
        }
    }
}
