using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    /// <summary>
    /// Fournit des méthodes pour générer des données fictives à des fins de test et de développement.
    /// </summary>
    internal class MockDataGenerator
    {
        private static MockDataGenerator _mockDataGenerator;

        private Random _random = new Random();
        private DateTime _referenceDate = DateTime.UtcNow.Date;
        private readonly string[] _loremWords = ("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                           "ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
                           "laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in " +
                           "voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat " +
                           "non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").Split(' ');

        public static MockDataGenerator Current => LazyInitializer.EnsureInitialized(ref _mockDataGenerator, () => new MockDataGenerator());

        /// <summary>
        /// Génère un identifiant de produit sous la forme "EANXXXXX", où XXXXX est un nombre aléatoire entre 10000 et 99999.
        /// </summary>
        /// <returns></returns>
        public string GenerateProductId()
        {
            return $"EAN{_random.Next(10000, 99999)}";
        }

        /// <summary>
        /// Génère une quantité aléatoire comprise entre 1 et 9999.
        /// </summary>
        /// <returns></returns>
        public long GenerateQuantity()
        {
            return _random.Next(1, 10000);
        }

        /// <summary>
        /// Génère un fragment de texte Lorem ipsum
        /// </summary>
        /// <param name="wordCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GenerateLabel(int wordCount)
        {
            if (wordCount <= 0 || wordCount > _loremWords.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(wordCount), "Le nombre de mots demandé est hors des limites.");
            }

            return string.Join(" ", _loremWords.Take(wordCount));
        }

        /// <summary>
        /// Génère une date antérieure à la date du jour.
        /// </summary>
        /// <param name="daysBeforeMax"></param>
        /// <returns></returns>
        public DateTime GenerateDateBefore(int daysBeforeMax = 365)
        {
            int daysBefore = _random.Next(1, daysBeforeMax + 1);
            return _referenceDate.AddDays(-daysBefore);
        }

        /// <summary>
        /// Génère une date postérieure à la date du jour.
        /// </summary>
        /// <param name="daysAfterMax"></param>
        /// <returns></returns>
        public DateTime GenerateDateAfter(int daysAfterMax = 365)
        {
            int daysAfter = _random.Next(1, daysAfterMax + 1);
            return _referenceDate.AddDays(daysAfter);
        }

        /// <summary>
        /// Génère un nouveau produit avec un identifiant aléatoire.
        /// </summary>
        /// <returns></returns>
        public Product GenerateProduct()
        {
            return new Product(GenerateProductId());
        }

        /// <summary>
        /// Génère un dictionnaire représentant une collection de produits et leurs quantités associées.
        /// </summary>
        /// <param name="numOfProducts"></param>
        /// <returns></returns>
        public IDictionary<string, long> GenerateProductDictionary(int numOfProducts)
        {
            var result = new Dictionary<string, long>();

            for(int index = 0; index < numOfProducts; index++)
            {
                var productId = $"EAN{_random.Next(10000, 99999)}";
                var quantity = GenerateQuantity();
                result[productId] = quantity;
            }

            return result;
        }

        /// <summary>
        /// Génère un mouvement de stock unique pour la date d'aujourd'hui.
        /// </summary>
        /// <returns></returns>
        public StockMovement GenerateMovementToday()
        {
            return new StockMovement(_referenceDate, GenerateLabel(10), GenerateProduct(), GenerateQuantity());
        }

        public IList<StockMovement> GenerateMovementToday(int numOfMovements = 2)
        {
            var dictionary = new Dictionary<string, StockMovement>();

            int index = 0;

            while (index < numOfMovements)
            {
                var label = GenerateLabel(index + 5);
                var quantity = GenerateQuantity();
                var product = GenerateProduct();

                var key = $"{_referenceDate.ToShortDateString()}-{label}-{product}-{quantity}";

                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new StockMovement(_referenceDate, label, product, quantity);
                    index++;
                }
            }

            return dictionary.Values.ToList();
        }

        /// <summary>
        /// Génère un mouvement de stock représentant un inventaire avec une date de référence, un produit généré aléatoirement et une quantité générée aléatoirement.
        /// </summary>
        /// <returns></returns>
        public StockMovement GenerateInventoryToday()
        {
            return new StockMovement(_referenceDate, GenerateProduct(), GenerateQuantity());
        }

        /// <summary>
        /// Génère un mouvement de stock représentant un inventaire avec une date de référence et une quantité générée aléatoirement pour un produit spécifié.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public StockMovement GenerateInventoryToday(string productId)
        {
            return new StockMovement(_referenceDate, new Product(productId), GenerateQuantity());
        }

        /// <summary>
        /// Génère une liste de mouvements de stock uniques pour un produit spécifié à une date donnée.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="numOfMovements"></param>
        /// <returns></returns>
        public IList<StockMovement> GenerateUniqueMovementsForProductAtDate(string productId, DateTime date, int numOfMovements = 4)
        {
            var dictionary = new Dictionary<string, StockMovement>();

            int index = 0;

            while (index < numOfMovements)
            {
                var label = GenerateLabel(index + 5);
                var quantity = GenerateQuantity();

                var key = $"{date.ToShortDateString()}-{label}-{productId}-{quantity}";

                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new StockMovement(date,label,new Product(productId), quantity);
                    index++;
                }
            }

            return dictionary.Values.ToList();
        }

        /// <summary>
        /// Génère une liste de mouvements de stock pour une date spécifiée.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="numberOfMovements"></param>
        /// <returns></returns>
        public IList<StockMovement> GenerateMovementsForDate(DateTime date, int numberOfMovements = 4)
        {
            var result = new List<StockMovement>();

            int index = 0;

            while (index < numberOfMovements)
            {
                var productId = GenerateProductId();
                var stocks = GenerateUniqueMovementsForProductAtDate(productId, date, numberOfMovements);
                result.AddRange(stocks);
                index++;
            }

            return result;
        }

        public IList<StockMovement> GenerateProductMovementsBetweenDates(string productId, DateTime startDate, DateTime endDate, int numOfMovements = 2)
        {
            var list = new List<StockMovement>(GenerateUniqueMovementsForProductAtDate(productId, startDate, numOfMovements));
            list.AddRange(GenerateUniqueMovementsForProductAtDate(productId, endDate, numOfMovements));
            return list;
        }

        ///// <summary>
        ///// Génère un mouvement de stock représentant un inventaire posterieur à la date de référence. 
        ///// Le produit est spécifié par son identifiant et la quantité est générée aléatoirement.
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public StockMovement GenerateInventoryAfterToday(string productId)
        //{
        //    var num = _random.Next(1, 365);
        //    return new StockMovement(GenerateDateAfter(num), new Product(productId), GenerateQuantity());
        //}

        ///// <summary>
        ///// Génère un mouvement de stock représentant un inventaire anterieur à la date de référence.
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public StockMovement GenerateInventoryBeforeToday(string productId)
        //{
        //    var num = _random.Next(1, 365);
        //    return new StockMovement(_referenceDate, new Product(productId), GenerateQuantity());
        //}

        ///// <summary>
        ///// Génère une liste de mouvements de stock pour la date d'aujourd'hui.
        ///// </summary>
        ///// <param name="numOfMovements"></param>
        ///// <returns></returns>
        //public IList<StockMovement> GenerateMovementsToday(int numOfMovements = 2) 
        //{
        //    var result = new HashSet<StockMovement>();
        //    var products = GenerateProductDictionary(numOfMovements);

        //    foreach(var product in products)
        //    {
        //        result.Add(new StockMovement(_referenceDate, GenerateLabel(10), new Product(product.Key), product.Value));
        //    }

        //    return result.ToList();
        //}

        ///// <summary>
        ///// Génère une liste de mouvements de stock postérieurs à la date d'aujourd'hui.
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <param name="numOfMovements"></param>
        ///// <returns></returns>
        //public IList<StockMovement> GenerateMovementsAfterToday(string productId = null, string label = null, int numOfMovements = 2)
        //{
        //    var result = new HashSet<StockMovement>();

        //    var labelIpnut = string.IsNullOrEmpty(label) ? GenerateLabel(10) : label;

        //    for (int index = 0; index < numOfMovements; index++)
        //    {
        //        var date = GenerateDateAfter(index);

        //        var product = string.IsNullOrEmpty(productId) ? GenerateProduct() : new Product(productId);

        //        result.Add(new StockMovement(date, label ?? labelIpnut, product, GenerateQuantity()));
        //    }

        //    return result.ToList();
        //}
    }
}
