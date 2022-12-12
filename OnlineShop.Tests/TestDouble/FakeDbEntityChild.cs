using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeDbEntityChild : Editable
    {
        public string Name { get; set; }

        public virtual IList<FakeDbEntity> FakeEntities { get; set; }
    }
}