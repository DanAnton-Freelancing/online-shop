using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Tests.Domain.Commands;

public abstract class BaseCommandTests<T> : BaseTests
    where T : EditableDbEntity
{
    protected List<T> Entities;
    protected Mock<IWriterRepository> WriterRepositoryMock;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        WriterRepositoryMock = new Mock<IWriterRepository>(MockBehavior.Strict) { CallBase = true };
    }
}