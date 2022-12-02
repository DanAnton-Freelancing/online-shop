using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Tests.Domain.Commands;

[TestClass]
public class BaseCommandTests<T,TR> : BaseTests
    where T : EditableEntity
    where TR: class, IBaseWriterRepository<T>
{
    protected List<T> Entities;
    protected Mock<TR> WriterRepositoryMock;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        WriterRepositoryMock = new Mock<TR>(MockBehavior.Strict) { CallBase = true };
    }
}