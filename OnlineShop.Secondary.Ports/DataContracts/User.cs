namespace OnlineShop.Secondary.Ports.DataContracts
{
    public class User : EditableEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }

        public virtual UserCart UserCart { get; set; }
    }
}