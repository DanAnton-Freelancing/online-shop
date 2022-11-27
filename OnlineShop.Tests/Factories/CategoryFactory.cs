using System;
using System.Collections.Generic;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories
{
    public static class CategoryFactory
    {
        public static List<sp.Category> Create()
            => new List<sp.Category>
               {
                   new sp.Category
                   {
                       Name = "Category1"
                   },
                   new sp.Category
                   {
                       Name = "Category2"
                   },
                   new sp.Category
                   {
                       Name = "Category3"
                   }
               };

        public static sp.Category ToEntity(this sp.Category category)
        {
            category.Id = Guid.NewGuid();
            return category;
        }

        public static sp.Category CreateUpsert()
            => new sp.Category
               {
                   Name = "NewCategory"
               };

        public static pp.UpsertCategory CreateUpsertModel()
            => new pp.UpsertCategory
               {
                   Name = "NewCategory"
               };

        public static List<pp.UpsertCategory> CreateUpsertModels()
            => new List<pp.UpsertCategory>
               {
                   new pp.UpsertCategory
                   {
                       Name = "Category1"
                   },
                   new pp.UpsertCategory
                   {
                       Name = "Category2"
                   },
                   new pp.UpsertCategory
                   {
                       Name = "Category3"
                   }
               };
    }
}