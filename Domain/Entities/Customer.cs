namespace Domain.Entities
{
    public class Customer : Entity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Image { get; set; }

        public static Customer Create(string firstName, string lastName, string email, string phoneNo)
        {
            return new Customer
            {
                Email = email,
                PhoneNo = phoneNo,
                Lastname = lastName,
                Firstname = firstName
            };
        }
    }
}
