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
using Z.EntityFramework.Extensions;
using static Amazon.S3.Util.S3EventNotification;

namespace OnlineShop.Tests.Secondary.Adapters;

[TestClass]
public class WriterRepositoryTests
{
    private FakeAppDbContext _context;
    private WriterRepository _repository;

    [TestInitialize]
    public void Initialize()
    {
        _context = DbContextFactory.GetDbContext();

        _repository = new WriterRepository(_context);
        EntityFrameworkManager.ContextFactory = context => _context;
    }

    [TestMethod]
    public async Task GivenEntity_WhenAddAsync_ThenShouldAddEntityToDbContext()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Name = "Fake entity"
            }
        };
        
        //Act
        var result = await _repository.AddAsync(entities.First(), CancellationToken.None);

        //Assert
        Assert.IsNotNull(result.Data.Id);
    }

    [TestMethod]
    public async Task GivenEntities_WhenAddAsync_ThenShouldAddEntitiesToDbContext()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Name = "Fake entity"
            }
        };

        //Act
        var result = await _repository.AddAsync(entities, CancellationToken.None);
        
        //Assert
        Assert.IsTrue(result.Data.Any(s => s.Id != null));
    }

    [TestMethod]
    public async Task GivenEntities_WhenSaveAsync_ThenShouldSaveEntitiesToDbContext()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Name = "Fake entity"
            }
        };
        
        await _repository.AddAsync(entities, CancellationToken.None);

        //Act
        var result = await _repository.SaveAsync(entities, CancellationToken.None);

        var dbContextEntity = await _context.Set<FakeEntity>().ToListAsync();

        //Assert
        Assert.IsTrue(AreListsEqual(result.Data, dbContextEntity.Select(c => c.Id.GetValueOrDefault()).AsEnumerable()));
    }

    [TestMethod]
    public async Task GivenEntity_WhenSaveAsync_ThenShouldSaveEntityToDbContext()
    {
        //Arrange
        var entities = new List<FakeEntity>
        {
            new()
            {
                Name = "Fake entity"
            }
        };

        await _repository.AddAsync(entities.First(), CancellationToken.None);

        //Act
        var result = await _repository.SaveAsync(entities.First(), CancellationToken.None);

        var dbContextEntity = await _context.Set<FakeEntity>().FirstAsync();

        //Assert
        Assert.AreEqual(result.Data, dbContextEntity.Id);
    }

    [TestMethod]
    public async Task GivenNotExistingEntityId_WhenDeleteAsync_ThenShouldMarkAsDeleted()
    {
        //Arrange
        var entity = new FakeEntity
        {
            Name = "Fake entity"
        };
        
        await _repository.AddAsync(entity, CancellationToken.None);
        await _repository.SaveAsync(entity, CancellationToken.None);

        //Act
        var result = await _repository.DeleteAsync<FakeEntity>(entity.Id.GetValueOrDefault(), CancellationToken.None);
       

        //Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(_context.Entry(entity).State, EntityState.Deleted);
    }


    [TestMethod]
    public async Task GivenExistingEntityId_WhenDeleteAsync_ThenShouldReturnError()
    {
        //Arrange
        var entity = new FakeEntity
        {
            Name = "Fake entity"
        };

        await _repository.AddAsync(entity, CancellationToken.None);
        await _repository.SaveAsync(entity, CancellationToken.None);

        //Act
        var result = await _repository.DeleteAsync<FakeEntity>(entity.Id.GetValueOrDefault(), CancellationToken.None);


        //Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(_context.Entry(entity).State, EntityState.Deleted);
    }


    [TestMethod]
    public async Task GivenExistingEntityId_WhenDeleteAsync_ThenShouldDelete()
    {
        //Arrange
        var entity = new FakeEntity
        {
            Name = "Fake entity"
        };

        await _repository.AddAsync(entity, CancellationToken.None);
        await _repository.SaveAsync(entity, CancellationToken.None);
        await _repository.DeleteAsync<FakeEntity>(entity.Id.GetValueOrDefault(), CancellationToken.None);
        
        //Act
        var result = await _repository.SaveAsync(CancellationToken.None);


        //Assert
        Assert.IsTrue(result.Success);
        Assert.IsNull(_context.Set<FakeEntity>().FirstOrDefault());
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

    private static bool AreListsEqual(IEnumerable<Guid> list1, IEnumerable<Guid> list2) => !list1.Except(list2).Any();
}