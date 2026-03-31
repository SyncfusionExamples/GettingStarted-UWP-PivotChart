using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted
{
    public class ProductSalesViewModel
    {
        private ProductSalesCollection productSales;

        public ProductSalesCollection ProductSales
        {
            get { return this.productSales; }
            set { this.productSales = value; }
        }

        public ProductSalesViewModel()
        {
            this.productSales = GetSalesData();
        }

        public static ProductSalesCollection GetSalesData()
        {
            // Geography
            string[] countries = new string[] { "Germany", "Canada", "United States" };

            // Time
            string[] dates = new string[] { "FY 2008", "FY 2009", "FY 2010", "FY 2012" };

            // Products
            string[] products = new string[] { "Bike", "Car" };
            Random r = new Random(123345);

            int numberOfRecords = 1000;
            ProductSalesCollection listOfProductSales = new ProductSalesCollection();
            for (int i = 0; i < numberOfRecords; i++)
            {
                ProductSale sales = new ProductSale();
                sales.Country = countries[r.Next(1, countries.GetLength(0))];
                sales.Amount = (3000 * r.Next(1, 12));
                sales.Date = dates[r.Next(r.Next(dates.GetLength(0) + 1))];
                sales.Product = products[r.Next(r.Next(products.GetLength(0) + 1))];
                listOfProductSales.Add(sales);
            }
            return listOfProductSales;
        }
    }

    public class ProductSale
    {
        public string Product { get; set; }
        public string Date { get; set; }
        public string Country { get; set; }
        public double Amount { get; set; }
    }

    public class ProductSalesCollection : ObservableCollection<ProductSale>
    {
    }
}
