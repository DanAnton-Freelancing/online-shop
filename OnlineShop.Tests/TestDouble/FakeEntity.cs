using System;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeEntity : EditableEntity
    {
        public string Name { get; set; }
        public virtual FakeEntityChild Child { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}