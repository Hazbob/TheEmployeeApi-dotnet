using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;
using TheEmployeeApi;
using TheEmployeeApi.Abstractions;

namespace TheEmployeeAPI.tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var repo = _factory.Services.GetRequiredService<IRepository<Employee>>();
        repo.Create(new Employee { FirstName = "John", LastName = "Doe" });
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult()
    {
        //arrange
        var client = _factory.CreateClient();
        //act
        var response = await client.GetAsync($"/employees");
        //assert
        Assert.True(response.IsSuccessStatusCode);
        response.EnsureSuccessStatusCode();
        
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsOkResult()
    {
        //arrange
        var client = _factory.CreateClient();
        //act
        var response = await client.GetAsync($"/employees/1");
        //assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateEmployee_ReturnsOkResponse()
    {
        //arrange
        var client = _factory.CreateClient();
        var body = new Employee
        {
            FirstName = "Harry",
            LastName = "Roobinson",
            NationalInsuranceNumber = "test"
        };
        //act
        var response = await client.PostAsJsonAsync("/employees", body);
        //assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateEmployee_ReturnsBadRequestResult()
    {
        //arrange
        var client = _factory.CreateClient();
        var body = new { };
        //act
        var response = await client.PostAsJsonAsync("/employees", body);
        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsOkResponse()
    {
        //arrange
        var client = _factory.CreateClient();
        var body = new Employee
        {
            FirstName = "new",
            LastName = "new",
            NationalInsuranceNumber = "new test"
        };
        //act
        var response = await client.PutAsJsonAsync("/employees/1", body);
        //assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsNotFoundWithUnknownEmployeeId()
    {
        //arrange
        var client = _factory.CreateClient();
        var body = new Employee
        {
            FirstName = "new",
            LastName = "new",
            NationalInsuranceNumber = "new test"
        };
        //act
        var response = await client.PutAsJsonAsync("/employees/999999999", body);
        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}