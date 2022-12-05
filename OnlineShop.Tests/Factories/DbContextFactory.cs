using System;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Adapters;
using OnlineShop.Tests.TestDouble;

namespace OnlineShop.Tests.Factories
{
    public static class DbContextFactory
    {
        public static FakeAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .EnableSensitiveDataLogging()
                          .Options;
            return new FakeAppDbContext(options);
        }
    }
}