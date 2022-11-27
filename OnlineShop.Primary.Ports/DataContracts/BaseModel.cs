using System;

namespace OnlineShop.Primary.Ports.DataContracts
{
    public abstract class BaseModel
    {
        public Guid? Id { get; set; }
    }
}