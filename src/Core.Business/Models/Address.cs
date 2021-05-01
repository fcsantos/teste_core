namespace Core.Business.Models
{
    public class Address : Entity
    {
        public string Location { get; set; }
        public string Number { get; set; } 
        public string Complement { get; set; }
        public string PostalCode { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Fornecedor Fornecedor { get; set; }
        public Patient Patient { get; set; }
    }
}