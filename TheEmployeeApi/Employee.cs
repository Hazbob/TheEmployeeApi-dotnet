using System.Diagnostics;

namespace TheEmployeeApi;

public class Employee {
    public int Id { get; set; }
    public required string FirstName { get; set; }  
    public required string LastName { get; set; }
    public string? NationalInsuranceNumber { get; set; }
    
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? PostCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}