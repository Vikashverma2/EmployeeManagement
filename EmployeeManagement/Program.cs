using EmployeeManagement.MongoDb;
using EmployeeManagement.Repositories;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services;
using EmployeeManagement.Services.Interface;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("MongodbConnection"));

builder.Services.AddSingleton<DbContext>();

// ✅ Register repositories & services
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

// ✅ Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
