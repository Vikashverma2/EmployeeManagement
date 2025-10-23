using EmployeeManagement.MongoDb;
using EmployeeManagement.Repositories;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services;
using EmployeeManagement.Services.Interface;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DbContext>();
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("MongodbConnection"));

builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IDepartmentService, DepartmentService>();



var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
 