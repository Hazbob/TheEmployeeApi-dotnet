using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TheEmployeeApi;
using TheEmployeeApi.Abstractions;
using TheEmployeeApi.Employees;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IRepository<Employee>, EmployeeRepository>();

var app = builder.Build();
var employeeRoute = app.MapGroup("/employees");

employeeRoute.MapGet(string.Empty, (IRepository<Employee> repository) => {
    return Results.Ok(repository.GetAll().Select(employee => new GetEmployeeResponse {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        County = employee.County,
        PostCode = employee.PostCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email
    }));
});

employeeRoute.MapGet("{id:int}", (int id, IRepository<Employee> repository) => {
    var employee = repository.GetById(id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new GetEmployeeResponse {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        County = employee.County,
        PostCode = employee.PostCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email
    });
});

employeeRoute.MapPost(string.Empty, (CreateEmployeeRequest employeeRequest, IRepository<Employee> repository) => {
    var validationProblems = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(employeeRequest, new ValidationContext(employeeRequest), validationProblems, true);
    if (!isValid)
    {
        return Results.BadRequest(validationProblems);
    }

    var newEmployee = new Employee {
        FirstName = employeeRequest.FirstName!,
        LastName = employeeRequest.LastName!,
        NationalInsuranceNumber = employeeRequest.NationalInsuranceNumber,
        Address1 = employeeRequest.Address1,
        Address2 = employeeRequest.Address2,
        City = employeeRequest.City,
        County = employeeRequest.County,
        PostCode = employeeRequest.PostCode,
        PhoneNumber = employeeRequest.PhoneNumber,
        Email = employeeRequest.Email
    };
    repository.Create(newEmployee);
    return Results.Created($"/employees/{newEmployee.Id}", employeeRequest);
});

employeeRoute.MapPut("{id}", (UpdateEmployeeRequest employeeRequest, int id, IRepository<Employee> repository) => {
    var existingEmployee = repository.GetById(id);
    if (existingEmployee == null)
    {
        return Results.NotFound();
    }

    existingEmployee.Address1 = employeeRequest.Address1;
    existingEmployee.Address2 = employeeRequest.Address2;
    existingEmployee.City = employeeRequest.City;
    existingEmployee.County = employeeRequest.County;
    existingEmployee.County = employeeRequest.PostCode;
    existingEmployee.PhoneNumber = employeeRequest.PhoneNumber;
    existingEmployee.Email = employeeRequest.Email;

    repository.Update(existingEmployee);
    return Results.Ok(existingEmployee);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();

public partial class Program {}