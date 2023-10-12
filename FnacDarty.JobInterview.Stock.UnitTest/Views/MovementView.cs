using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.JobInterview.Stock.Views
{
    internal struct MovementView
    {
        public DateTime Date { get; }

        public string Label { get; }

        public long Quantity { get; }

        public string Product { get; }

        private MovementView(DateTime date, string label, string product, long quantity)
        {
            Label = label;
            Date = date;
            Product = product;
            Quantity = quantity;
        }

        public static IList<MovementView> Generate()
        {
            return new List<MovementView>
            {
                new MovementView(new DateTime(2020, 1, 1), "Achat N°1", "ean00001", 10),
                new MovementView(new DateTime(2020, 1, 1), "Achat N°2","ean00002", 10),
                new MovementView(new DateTime(2020, 1, 1), "Achat N°3","ean00003", 10),
                new MovementView(new DateTime(2020, 1, 2), "Cmd N°1", "ean00001",-3),
                new MovementView(new DateTime(2020, 1, 2), "Cmd N°1", "ean00002",-3),
                new MovementView(new DateTime(2020, 1, 2), "Cmd N°1", "ean00003",-3),
                new MovementView(new DateTime(2020, 1, 3), "Cmd N°2", "ean00001",-1),
                new MovementView(new DateTime(2020, 1, 3), "Cmd N°2", "ean00002",-10),
                new MovementView(new DateTime(2020, 1, 4), "inventaire", "ean00002", 1)
            };
        }
    }
}
