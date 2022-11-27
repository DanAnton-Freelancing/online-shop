using System.Collections.Generic;

namespace OnlineShop.Secondary.Ports.DataContracts
{
    public class Category : EditableEntity
    {
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}