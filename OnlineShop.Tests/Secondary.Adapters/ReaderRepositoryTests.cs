using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.Secondary.Adapters.Implementation;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using OnlineShop.Tests.TestDouble;

namespace OnlineShop.Tests.Secondary.Adapters;

[TestClass]
public class ReaderRepositoryTests
{
    private FakeAppDbContext _context;
    private ReaderRepository _repository;

    [TestInitialize]
    public void Initialize()
    {
        _context = DbContextFactory.GetDbContext();

        _repository = new ReaderRepository(_context);
    }

    [TestMethod]
    public async Task WhenGetAsync_ThenShouldReturnEntities()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Id = Guid.NewGuid()
            }
        };
        _context.AddRange(entities);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetAsync<FakeEntity>(CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<FakeEntity>.AreListsEqual(result.Data, entities));
    }

    [TestMethod]
    public async Task WhenGetAsyncFromEmptyDb_ThenShouldReturnError()
    {
        //Act
        var result = await _repository.GetAsync<FakeEntity>(CancellationToken.None);

        //Assert
        Assert.IsTrue(result.HasErrors);
    }

    [TestMethod]
    public async Task WhenGetByIdAsync_ThenShouldReturnEntity()
    {
        //Arrange
        var entityId = Guid.NewGuid();
        var entities = new List<FakeEntity>
        {
            new()
            {
                Id = entityId
            }
        };
        _context.AddRange(entities);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetOneAsync<FakeEntity>(c => c.Id == entityId, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<FakeEntity>.AreEntriesEqual(result.Data, entities.First()));
    }

    [TestMethod]
    public async Task WhenGetWithFilterAsync_ThenShouldReturnEntities()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Entity 1",
                Deleted = false,
                Child = new FakeEntityChild
                {
                    Id = Guid.NewGuid()
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Entity 2",
                Deleted = true,
                Child = new FakeEntityChild
                {
                    Id = Guid.NewGuid()
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Entity 3",
                Deleted = false,
                Child = new FakeEntityChild
                {
                    Id = Guid.NewGuid()
                }
            }
        };
        _context.AddRange(entities);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetAsync<FakeEntity>(CancellationToken.None, c => c.Name.Contains("Entity"),
            c => c.OrderBy(a => a.Name), c => c.Include(a => a.Child));

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<FakeEntity>.AreListsEqual(result.Data, entities));
    }
}