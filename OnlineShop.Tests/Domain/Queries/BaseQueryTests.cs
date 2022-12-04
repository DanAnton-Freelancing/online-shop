using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Tests.Domain.Queries;

public abstract class BaseQueryTests : BaseTests
{
    protected Mock<IReaderRepository> ReaderRepositoryMock;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        ReaderRepositoryMock = new Mock<IReaderRepository>(MockBehavior.Strict) { CallBase = true };
    }
}