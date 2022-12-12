using System;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeDbEntity : Editable
    {
        public string Name { get; set; }
        public virtual FakeDbEntityChild Child { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}