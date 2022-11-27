using System;
using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories
{
    public static class ProductFactory
    {
        public static List<Product> Create()
            => new List<Product>
               {
                   new Product
                   {
                       Name = "Product1",
                       Code = "RandomCode1",
                       Price = 10,
                       AvailableQuantity = 20,
                       Category = CategoryFactory.Create()[0].ToEntity(),
                       CategoryId = CategoryFactory.Create()[0].ToEntity().Id.GetValueOrDefault()
                   },

                   new Product
                   {
                       Name = "Product2",
                       Code = "RandomCode2",
                       Price = 23,
                       AvailableQuantity = 11,
                       Category = CategoryFactory.Create()[1].ToEntity(),
                       CategoryId = CategoryFactory.Create()[1].ToEntity().Id.GetValueOrDefault()
                   },
                   new Product
                   {
                       Name = "Product3",
                       Code = "RandomCode3",
                       Price = 30,
                       AvailableQuantity = 30,
                       Category = CategoryFactory.Create()[2].ToEntity(),
                       CategoryId = CategoryFactory.Create()[2].ToEntity().Id.GetValueOrDefault()
                   }
               };

        public static Product CreateUpsert()
            => new Product
               {
                   Name = "NewProduct",
                   Code = "NewProductCode",
                   Price = (decimal) 1000.12,
                   AvailableQuantity = 300,
                   CategoryId = new Guid("7b67fc8d-d0c4-4f02-88e7-fb3f9fd3d427")
               };

        public static pp.UpsertProduct CreateUpsertModel()
            => new pp.UpsertProduct
               {
                   Name = "NewProduct",
                   Price = (decimal) 1000.12,
                   AvailableQuantity = 300
               };

        public static List<pp.UpsertProduct> CreateUpsertModels()
            => new List<pp.UpsertProduct>
               {
                   new pp.UpsertProduct
                   {
                       Name = "Product1",
                       Price = 10,
                       AvailableQuantity = 10,
                       CategoryId = CategoryFactory.Create()[0].ToEntity().Id.GetValueOrDefault()
                   },

                   new pp.UpsertProduct
                   {
                       Name = "Product2",
                       Price = 23,
                       AvailableQuantity = 11,
                       CategoryId = CategoryFactory.Create()[1].ToEntity().Id.GetValueOrDefault()
                   },
                   new pp.UpsertProduct
                   {
                       Name = "Product3",
                       Price = 30,
                       AvailableQuantity = 30,
                       CategoryId = CategoryFactory.Create()[2].ToEntity().Id.GetValueOrDefault()
                   }
               };

        public static Product ToEntity(this Product product)
        {
            product.Id = Guid.NewGuid();
            return product;
        }
    }
}