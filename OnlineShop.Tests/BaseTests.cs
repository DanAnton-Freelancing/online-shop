using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace OnlineShop.Tests;

[TestClass]
public abstract class BaseTests
{
    protected Mock<IMediator> MediatorMock;

    [TestInitialize]
    public virtual void Initialize()
    {
        MediatorMock = new Mock<IMediator>(MockBehavior.Strict) {CallBase = true};
    }
}