using System;
using System.Collections.Generic;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories;

public static class CategoryFactory
{
    public static List<secondaryPorts.Category> Create()
        => new()
        {
            new secondaryPorts.Category
            {
                Name = "Category1"
            },
            new secondaryPorts.Category
            {
                Name = "Category2"
            },
            new secondaryPorts.Category
            {
                Name = "Category3"
            }
        };

    public static secondaryPorts.Category ToEntity(this secondaryPorts.Category Category)
    {
        Category.Id = Guid.NewGuid();
        return Category;
    }

    public static secondaryPorts.Category CreateUpsert()
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