using System;
using DDD.Sample.Entityes;
using DDD.Sample.Foundation;

namespace DDD.Sample.Services
{
    /// <summary>
    /// Produtos
    /// </summary>
    public sealed class Products
    {

        /// <summary>
        /// Compra de um novo produto não registrado nos depósitos
        /// </summary>
        public void BuyNewProduct(
            string supplierCode,
            string productCatalogCode,
            string productName,
            decimal quantity,
            decimal price)
        {
            using (var unitOfWork = UnitOfWorkManager.GetInstance())
            using (var products = new Repository<Product>())
            using (var purchases = new Repository<Purchase>())
            using (var suppliers = new Repository<Supplier>())
            using (var warehouses = new Repository<Warehouse>())
            using (var warehousesProducts = new Repository<ProductInWarehouse>())
            {
                try
                {
                    //Obtem informações do fornecedor e depósito de entrada do produto
                    var supplier = suppliers.GetSupplierByCode(supplierCode);

                    if (supplier == null)
                        throw new SupplierNotFound(supplierCode);

                    var warehouse = warehouses.GetDefaultWarehouse();

                    if (warehouse == null)
                        throw new DefaultWarehouseNotFound();

                    //Cria um registro de novo produto
                    var product = Factory.Create.Product(productCatalogCode, productName);

                    //Cria um registro de compras do produto
                    var purchase = product.PurchaseFrom(supplier, price, quantity);

                    //Persiste os registros na base de dados
                    products.Insert(product);
                    purchases.Insert(purchase);

                    //Registra a entrada do produto em estoque
                    var productInWarehouse = warehouse.RegisterNewProductStock(purchase);

                    //Persiste a informação da entrada na base de dados
                    warehousesProducts.Insert(productInWarehouse);

                    //Atualiza as informações de modificação no depósito
                    warehouses.Update(warehouse);

                    //Processa a transação
                    unitOfWork.Commit();
                }
                catch
                {
                    //Cancela todas as entradas
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Compra de um produto que já foi registrado nos depósitos
        /// </summary>
        public void BuyProduct(string supplierCode, string productCatalogCode, decimal quantity, decimal price)
        {
            using (var unitOfWork = UnitOfWorkManager.GetInstance())
            using (var products = new Repository<Product>())
            using (var purchases = new Repository<Purchase>())
            using (var suppliers = new Repository<Supplier>())
            using (var warehouses = new Repository<Warehouse>())
            using (var warehousesProducts = new Repository<ProductInWarehouse>())
            {
                try
                {
                    //Obtem informações do fornecedor e depósito de entrada do produto
                    var supplier = suppliers.GetSupplierByCode(supplierCode);
                    var warehouse = warehouses.GetDefaultWarehouse();
                    var product = products.GetByCode(productCatalogCode);

                    //Cria um registro de compras do produto
                    var purchase = product.PurchaseFrom(supplier, price, quantity);

                    //Persiste os registros na base de dados
                    products.Update(product);
                    purchases.Insert(purchase);

                    //Busca o registro do produto no estoque
                    var productInWarehouse = warehousesProducts.GetProductsInWarehouse(product, warehouse);

                    //Atualiza as informações de modificação no depósito
                    warehouse.UpdateStock(productInWarehouse, purchase);
                    warehousesProducts.Update(productInWarehouse);

                    //Persiste os registros na base de dados
                    purchases.Insert(purchase);
                    products.Update(product);

                    //Atualiza os dados do depósito
                    warehouses.Update(warehouse);

                    //Processa a transação
                    unitOfWork.Commit();
                }
                catch
                {
                    //Cancela todas as entradas
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtém os dados do produto pelo código no catalogo
        /// </summary>
        public Product GetProduct(string productCatalogCode)
        {
            using (var products = new Repository<Product>())
            using (var purchases = new Repository<Purchase>())
            {
                // Obtém o produto pelo código
                var product = products.GetByCode(productCatalogCode);
                if (product == null)
                    throw new ArgumentException("Product not found");

                // Atualiza a informação de ultima compra
                product.LastPurchase = Helper.AsLazy(() => purchases.GetLastPurchasesByProduct(product)); 

                // Retorna o produto
                return product;
            }
        }

        /// <summary>
        /// Atuliza informãções de descrição do produto
        /// </summary>
        public void UpdateName(string productCatalogCode, string newName)
        {
            using (var unitOfWork = UnitOfWorkManager.GetInstance()) 
            using (var products = new Repository<Product>())
            {
                try
                {
                    // Obtém o produto pelo código
                    var product = products.GetByCode(productCatalogCode);
                    if (product == null)
                        throw new ArgumentException("Product not found.");
                    if (product.Name == newName)
                        throw new ArgumentException("New name is the same at last name.");

                    product.Name = newName;
                    products.Update(product);
                }
                catch
                {
                    //Cancela todas as entradas
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

    }
}
    

