using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeEntityChild : EditableEntity
    {
        public string Name { get; set; }

        public virtual IList<FakeEntity> FakeEntities { get; set; }
    }
}