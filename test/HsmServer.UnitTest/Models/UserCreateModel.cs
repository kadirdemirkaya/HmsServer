namespace HsmServer.UnitTest.Models
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<string> Emails { get; set; }


        public UserCustomer UserCustomer { get; set; }

        public List<UserBasket> UserBaskets { get; set; }
    }

    public class UserCustomer
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserBasket
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
