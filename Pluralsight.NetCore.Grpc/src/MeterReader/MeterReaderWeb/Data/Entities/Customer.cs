using System.Collections.Generic;

namespace MeterReaderWeb.Data.Entities
{
    public class Customer
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string PhoneNumber { get; set; }
    public Address Address { get; set; }
    public int AddressId { get; set; }
    public ICollection<MeterReading> Readings { get; set; }
  }
}
