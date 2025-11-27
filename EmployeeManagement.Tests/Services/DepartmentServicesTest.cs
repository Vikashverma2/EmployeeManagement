using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services;
using Moq;
using Xunit;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _repoMock;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _repoMock = new Mock<IDepartmentRepository>();
        _service = new DepartmentService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldThrow_WhenNameIsEmpty()
    {
        var dept = new Department { Name = "" };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateDepartmentAsync(dept));
    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldThrow_WhenDuplicateDepartmentExists()
    {
        var dept = new Department { Name = "IT" };

        _repoMock.Setup(r => r.GetDepartmentAsync())
                 .ReturnsAsync(new List<Department>
                 {
                     new Department { Name = "IT" }
                 });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateDepartmentAsync(dept));

        _repoMock.Verify();

    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldCreate_WhenValid()
    {
        // Arrange
        var dept = new Department { Name = "Accounts" };

        _repoMock.Setup(r => r.GetDepartmentAsync())
                 .ReturnsAsync(new List<Department>());

        _repoMock.Setup(r => r.CreateDepartmentAsync(dept))
                 .ReturnsAsync(dept);

        // Act
        var result = await _service.CreateDepartmentAsync(dept);

        // Assert
        Assert.Equal("Accounts", result.Name);

        _repoMock.Verify();
    }

    [Fact]
    public async Task GetDepartmentsAsync_ShouldReturnList()
    {
        _repoMock.Setup(r => r.GetDepartmentAsync())
                 .ReturnsAsync(new List<Department>
                 {
                     new Department { Name = "HR" }
                 });

        var result = await _service.GetDepartmentsAsync();

        Assert.Single(result);

        _repoMock.Verify();
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldThrow_WhenIdEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetDepartmentByIdAsync(""));
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldThrow_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync((Department)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetDepartmentByIdAsync("1"));

        _repoMock.Verify();
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnDepartment_WhenFound()
    {
        var dept = new Department { Id = "1", Name = "Finance" };

        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync(dept);

        var result = await _service.GetDepartmentByIdAsync("1");

        Assert.Equal("Finance", result.Name);

        _repoMock.Verify();
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldThrow_WhenIdEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateDepartmentAsync("", new Department()));
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldThrow_WhenDepartmentNotFound()
    {
        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync((Department)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateDepartmentAsync("1", new Department()));

        _repoMock.Verify();
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldThrow_WhenNameEmpty()
    {
        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync(new Department { Id = "1", Name = "Old" });

        var updated = new Department { Name = "" };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateDepartmentAsync("1", updated));

        _repoMock.Verify();
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldUpdate_WhenValid()
    {
        var existing = new Department { Id = "1", Name = "Marketing" };
        var updated = new Department { Name = "Sales" };

        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.UpdateDepartmentAsync("1", updated))
                 .ReturnsAsync(updated);

        var result = await _service.UpdateDepartmentAsync("1", updated);

        Assert.Equal("Sales", result.Name);

        _repoMock.Verify();
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldThrow_WhenIdEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.DeleteDepartmentAsync(""));
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldThrow_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync((Department)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeleteDepartmentAsync("1"));

        _repoMock.Verify();
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldDelete_WhenValid()
    {
        var dept = new Department { Id = "1", Name = "Legal" };

        _repoMock.Setup(r => r.GetDepartmentByIdAsync("1"))
                 .ReturnsAsync(dept);

        _repoMock.Setup(r => r.DeleteDepartmentAsync("1"))
                 .ReturnsAsync(true);

        var result = await _service.DeleteDepartmentAsync("1");

        Assert.True(result);

        _repoMock.Verify();
    }
}
