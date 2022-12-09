using System;
using System.Collections.Generic;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories;

public static class CategoryFactory
{
    public static List<secondaryPorts.CategoryDb> Create()
        => new()
        {
            new secondaryPorts.CategoryDb
            {
                Name = "Category1"
            },
            new secondaryPorts.CategoryDb
            {
                Name = "Category2"
            },
            new secondaryPorts.CategoryDb
            {
                Name = "Category3"
            }
        };

    public static secondaryPorts.CategoryDb ToEntity(this secondaryPorts.CategoryDb categoryDb)
    {
        categoryDb.Id = Guid.NewGuid();
        return categoryDb;
    }

    public static secondaryPorts.CategoryDb CreateUpsert()
        => new()
        {
            Name = "NewCategory"
        };

    public static primaryPorts.UpsertCategoryModel CreateUpsertModel()
        => new()
        {
            Name = "NewCategory"
        };

    public static List<primaryPorts.UpsertCategoryModel> CreateUpsertModels()
        => new()
        {
            new primaryPorts.UpsertCategoryModel
            {
                Name = "Category1"
            },
            new primaryPorts.UpsertCategoryModel
            {
                Name = "Category2"
            },
            new primaryPorts.UpsertCategoryModel
            {
                Name = "Category3"
            }
        };
}