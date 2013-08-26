using DDD.Sample.Foundation;
using DDD.Sample.Console.EntityFramework;
using DDD.Sample.Services;
using System.Threading;

namespace DDD.Sample.Console
{
    class Program
    {
        static void Main()
        {
            IoC.Register<IUnitOfWork, SampleContext>();
            IoC.Register<IClock, LocalClock>();
            Thread.CurrentPrincipal = new UserPrincipal(new UserIdentity("manual", true, "Console"));

            var products = new Products();
            products.BuyNewProduct("s001", "p001", "product 1", 10.0M, 1.68M);
            System.Console.Write("Insert... Ok");

            var persistProduct = products.GetProduct("p001");
            System.Console.Write("Get... Ok");

            products.BuyProduct("s001", "p001", 11.0M, 1.62M);
            System.Console.Write("Update... Ok");

            products.UpdateName(persistProduct.CodeCatalog, "name update");
            System.Console.Write("Update Name... Ok");
        }
    }
}
