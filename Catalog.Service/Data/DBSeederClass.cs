using Catalog.Service.Entities;
using Catalog.Service.Helpers;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Service.Data
{
    public class DBSeederClass
    {
        public static async Task SeedDataAsync(Constants constants)
        {
            var _connection = new MongoClient(constants.ConnectionString);
            var db = _connection.GetDatabase(constants.DatabaseName);
            var products = db.GetCollection<Product>(constants.ProductsCollectionName);
            var brands = db.GetCollection<ProductBrand>(constants.BrandsCollectionName);
            var types = db.GetCollection<ProductType>(constants.TypesCollectionName);


            var SeedBasePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData");

            //Seed Brands
            List<ProductBrand> brandList = new List<ProductBrand>();
            if ((await brands.CountDocumentsAsync(_ => true)) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "brands.json"));
                brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandList);
            }
            else
            {
                var existingBrands = await brands.Find(_ => true).ToListAsync();
            }

            //Seed Types
            List<ProductType> typeList = new List<ProductType>();
            if ((await types.CountDocumentsAsync(_ => true)) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "types.json"));
                typeList = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typeList);
            }
            else
            {
                var existingTypes = await types.Find(_ => true).ToListAsync();
            }

            //Seed Products
            List<Product> productList = new List<Product>();
            if ((await products.CountDocumentsAsync(_ => true)) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "products.json"));
                productList = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var p in productList)
                {
                    //Reset Id to let mongo add new one
                    p.Id = null;
                    //Daefault CreatedDate if not set
                    if (p.CreatedDate == default)
                    {
                        p.CreatedDate = DateTimeOffset.UtcNow;
                    }
                }
                await products.InsertManyAsync(productList);
            }
            else
            {
                var existingProducts = await products.Find(_ => true).ToListAsync();
            }
        }
    }
}
