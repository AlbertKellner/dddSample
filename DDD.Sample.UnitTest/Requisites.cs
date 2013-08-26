using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDD.Sample.Foundation;
using DDD.Sample.Entityes;
using DDD.Sample.Services;

namespace DDD.Sample.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Requisites
    {
        /// <summary>
        /// Configurar o banco de dados em memória
        /// </summary>
        private static InMemoryDatabase PrepareEnviroment(string userName)
        {
            //autenticar
            Thread.CurrentPrincipal = new UserPrincipal(new UserIdentity("Test", true, userName));

            var database = new InMemoryDatabase(
                new DateTime(2013, 7, 21, 21, 50, 0),  
                typeof(Entity).Assembly);
            const string defaultWarehouseName = "buyWarehouse";

            //Parâmetros do Sistema
            new Repository<SystemParameters>().Insert(new SystemParameters { Code = SystemParameters.DefaultWarehouseCode, Value = defaultWarehouseName });
            //Fornecedor S001
            new Repository<Supplier>().Insert(new Supplier {Code = "S001"});
            //Deposito Padrão
            new Repository<Warehouse>().Insert(new Warehouse {Code = defaultWarehouseName});
            return database;
        }

        /// <summary>
        /// Compra de novos produtos
        /// </summary>
        [TestMethod]
        public void TestBuyNewProduct()
        {
            PrepareEnviroment("TestBuyNewProductUser");

            //comrar novos produtos
            var products = new Products();
            products.BuyNewProduct("S001", "P001", "Product 1", 10.0M, 1.68M);
            
            var product = products.GetProduct("P001");
            Assert.AreEqual("P001", product.CodeCatalog);
            Assert.AreEqual("Product 1", product.Name);
            Assert.IsFalse(product.LastPurchase.IsValueCreated);

            var lastPurchase = product.LastPurchase.Value;
            Assert.AreEqual("S001", lastPurchase.Supplier.Code);
            Assert.AreEqual(10.0M, lastPurchase.Quantity);
            Assert.AreEqual(1.68M, lastPurchase.Price);
            Assert.AreEqual(product, lastPurchase.Product);
        }

        /// <summary>
        /// Compra de produtos já cadastrados
        /// </summary>
        [TestMethod]
        public void TestBuyAExistingProduct()
        {
            //autenticar
            var database = PrepareEnviroment("TestBuyAExistingProduct");

            var products = new Products();
            //Primeira compra
            products.BuyNewProduct("S001", "P002", "Product 2", 10.3M, 1.70M);


            //Segunda compra
            database.NowFake = database.NowFake.AddDays(1);
            products.BuyProduct("S001", "P002", 10.0M, 1.72M);

            var product = products.GetProduct("P002");
            Assert.AreEqual("P002", product.CodeCatalog);
            Assert.AreEqual("Product 2", product.Name);
            Assert.AreEqual(product.CreatedAt.AddDays(1), product.ModifiedAt);

            Assert.IsFalse(product.LastPurchase.IsValueCreated);

            var lastPurchase = product.LastPurchase.Value;
            Assert.AreEqual("S001", lastPurchase.Supplier.Code);
            Assert.AreEqual(10M, lastPurchase.Quantity);
            Assert.AreEqual(1.72M, lastPurchase.Price);
            Assert.AreEqual(product, lastPurchase.Product);
        }

        /// <summary>
        /// Dados do estoque
        /// </summary>
        [TestMethod]
        public void TestProductsCountInStock()
        {
            //autenticar
            PrepareEnviroment("TestProductsCountInStock");

            var products = new Products();
            products.BuyNewProduct("S001", "P003", "Product 3", 4M, 1.75M);
            products.BuyProduct("S001", "P003", 5M, 1.50M);
            products.BuyProduct("S001", "P003", 2M, 1.50M);
            products.BuyProduct("S001", "P003", 1M, 1.68M);

            var warehouses = new Stock();
            var defaultWarehouse = warehouses.GetDefaultWarehouse();
            
            var totalInStock = warehouses.GetProductInWarehouse("P003", defaultWarehouse.Code);

            Assert.AreEqual(12M, totalInStock.Total);
            Assert.AreEqual(1.5983M, totalInStock.AveragePrice);
        }
    }
}
