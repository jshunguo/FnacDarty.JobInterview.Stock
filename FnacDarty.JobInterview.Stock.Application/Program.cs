using CsvHelper;
using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Repositories;
using FnacDarty.JobInterview.Stock.Validators;
using FnacDarty.JobInterview.Stock.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FnacDarty.JobInterview.Stock.Application
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            bool keepRunning = true;
            while (keepRunning)
            {
                ShowMainMenu();

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            HandleDisplayMovement(serviceProvider);
                            break;
                        case "2":
                            HandleDisplayMultipleStockMovements(serviceProvider);
                            break;
                        case "3":
                            HandleDisplayMultipleStockMovementsFromCsv(serviceProvider);
                            break;
                        case "4":
                            HandleDisplayStockOfProductAtDate(serviceProvider); 
                            break;
                        case "5":
                             HandleDisplayStockVariationForProduct(serviceProvider);
                            break;
                        case "6":
                            HandleDisplayStockOfProduct(serviceProvider);
                            break;
                        case "7":
                            HandleDisplayAllProductsInStock(serviceProvider);
                            break;
                        case "8":
                            HandleDisplayTotalProductsInStock(serviceProvider);
                            break;
                        case "9":
                            HandleDisplayStockAfterRegularizationForProduct(serviceProvider);
                            break;
                        case "q":
                            keepRunning = false;
                            Console.WriteLine("Au revoir ..... 😉");
                            Thread.Sleep(1000);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.Out.Flush();
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.WriteLine("================================");
            Console.WriteLine("        MENU PRINCIPALE         ");
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("Choisissez une option :");
            Console.WriteLine("(Entrez le numéro de l'option souhaitée, 'q' pour quitter)");
            Console.WriteLine();

            Console.WriteLine("--- Gestion des mouvements ---");
            Console.WriteLine("1 - Afficher le mouvement d'un produit");
            Console.WriteLine("2 - Afficher plusieurs mouvements de stock");
            Console.WriteLine("3 - Importer et afficher plusieurs mouvements de stock depuis un fichier CSV");
            Console.WriteLine();

            Console.WriteLine("--- Gestion des stocks ---");
            Console.WriteLine("4 - Afficher le stock d'un produit à une date specifique");
            Console.WriteLine("5 - Afficher la variation du stock d'un produit");
            Console.WriteLine("6 - Afficher le stock actuel d'un produit");
            Console.WriteLine("7 - Afficher tous les produits en stock");
            Console.WriteLine("8 - Afficher le nombre total de produits en stock");
            Console.WriteLine("9 - Régulariser et afficher le stock d'un produit");
            Console.WriteLine();
            Console.WriteLine("================================");
        }

        private static DateTime SelectDate()
        {
            Form prompt = new Form()
            {
                Width = 250,
                Height = 150,
                Text = "Sélectionnez une date",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            DateTimePicker dateTimePicker = new DateTimePicker() { Left = 10, Top = 20, Width = 200 };
            Button confirmation = new Button() { Text = "OK", Left = 60, Width = 100, Top = 50, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(dateTimePicker);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? dateTimePicker.Value.ToUniversalTime().Date : DateTime.UtcNow.Date;
        }

        private static string OpenCsvFileDialog()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers CSV (*.csv)|*.csv";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return string.Empty;
        }

        private static void HandleDisplayMovement(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Selectionner la date : ");
                DateTime date = SelectDate();

                Console.WriteLine("Entrez le libellé: ");
                string label = Console.ReadLine();

                Console.WriteLine("Entrez l'ID du produit: ");
                string productId = Console.ReadLine();

                Console.WriteLine("Entrez la quantité: ");
                long quantity = long.Parse(Console.ReadLine());

                var gridView = clientApp.DisplayMovement(date, label, productId, quantity);

                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayMultipleStockMovements(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Selectionner la date : ");
                DateTime date = SelectDate();

                Console.WriteLine("Entrez le libellé: ");
                string label = Console.ReadLine();

                var productQuantities = new Dictionary<string, long>();

                Console.WriteLine("Combien de produits souhaitez-vous ajouter?");
                int productCount;
                while (!int.TryParse(Console.ReadLine(), out productCount) || productCount < 1)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide supérieur à 0.");
                }

                for (int i = 0; i < productCount; i++)
                {
                    Console.WriteLine($"Entrez l'ID du produit {i + 1}:");
                    string productId = Console.ReadLine();

                    Console.WriteLine($"Entrez la quantité pour le produit {productId}:");
                    long quantity;
                    while (!long.TryParse(Console.ReadLine(), out quantity))
                    {
                        Console.WriteLine("Veuillez entrer une quantité valide.");
                    }

                    productQuantities[productId] = quantity;
                }

                var gridView = clientApp.DisplayMultipleStockMovements(date, label, productQuantities);

                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayMultipleStockMovementsFromCsv(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Selectionner la date : ");
                DateTime date = SelectDate();

                Console.WriteLine("Entrez le libellé: ");
                string label = Console.ReadLine();

                string filePath = OpenCsvFileDialog();
                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("Fichier non sélectionné.");
                    return;
                }

                var productQuantities = new Dictionary<string, long>();

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<dynamic>().ToList();
                        foreach (var record in records)
                        {
                            string productId = record.productId;
                            long quantity = Convert.ToInt64(record.quantity);
                            productQuantities[productId] = quantity;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la lecture du fichier CSV: {ex.Message}");
                    return;
                }

                var gridView = clientApp.DisplayMultipleStockMovements(date, label, productQuantities);
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayStockOfProductAtDate(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Entrez l'identifiant du produit: ");
                string productId = Console.ReadLine();

                Console.WriteLine("Selectionner la date : ");
                DateTime date = SelectDate();

                var gridView = clientApp.DisplayStockForProductAtDate(productId, date);
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayStockOfProduct(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Entrez l'identifiant du produit: ");
                string productId = Console.ReadLine();

                var gridView = clientApp.DisplayCurrentStockForProduct(productId);
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayStockVariationForProduct(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Entrez l'identifiant du produit: ");
                string productId = Console.ReadLine();

                Console.WriteLine("Selectionner la date de debut: ");
                DateTime startDate = SelectDate();

                Console.WriteLine("Selectionner la date de fin: ");
                DateTime endDate = SelectDate();

                var gridView = clientApp.DisplayStockVariationForProduct(productId, startDate, endDate);
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayAllProductsInStock(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                var gridView = clientApp.DisplayProductsInStock();
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayTotalProductsInStock(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                var gridView = clientApp.DisplayTotalProductsInStock();
                gridView.Render(Console.Out);
            }
        }

        private static void HandleDisplayStockAfterRegularizationForProduct(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var clientApp = scope.ServiceProvider.GetService<IClient>();

                Console.WriteLine("Entrez l'identifiant du produit: ");
                string productId = Console.ReadLine();

                Console.WriteLine("Entrez la quantité à laquelle régulariser le stock: ");
                long quantity = long.Parse(Console.ReadLine());

                var gridView = clientApp.DisplayStockAfterRegularizationForProduct(productId, quantity);
                gridView.Render(Console.Out);
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Repositories
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddSingleton<IStockMovementRepository, InMemoryStockMovementRepository>();

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
            services.AddTransient<IGridViewFactory, GridViewFactory>();

            // Client
            services.AddSingleton<IClient, ClientApp>();

            return services.BuildServiceProvider();
        }
    }
}
