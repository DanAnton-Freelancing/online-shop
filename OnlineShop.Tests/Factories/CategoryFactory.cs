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

    public static secondaryPorts.Category ToEntity(this secondaryPorts.Category category)
    {
        category.Id = Guid.NewGuid();
        return category;
    }

    public static secondaryPorts.Category CreateUpsert()
        => new()
        {
            Name = "NewCategory"
        };

    public static primaryPorts.UpsertCategory CreateUpsertModel()
        => new()
        {
            Name = "NewCategory"
        };

    public static List<primaryPorts.UpsertCategory> CreateUpsertModels()
        => new()
        {
            new primaryPorts.UpsertCategory
            {
                Name = "Category1"
            },
            new primaryPorts.UpsertCategory
            {
                Name = "Category2"
            },
            new primaryPorts.UpsertCategory
            {
                Name = "Category3"
            }
        };
}