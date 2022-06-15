using NUnit.Framework;
using Tables.Services;
using Tables.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Tables.Tests;

public class DeskServiceTest
{

    private DbContextOptions<DataContext> _dbContextOptions;
    private IDeskService _deskService;
    private DataContext _context;

    [OneTimeSetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Tables1")
            .Options;
        _context = new DataContext(_dbContextOptions);
        _deskService = new DeskService(_context);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task TestAddDesk()
    {
        var location = new Location()
        {
            Name = "Kraków"
        };
        var desk = new Desk()
        {
            Capacity = 1,
            IsAvailiable = false,
            DeskLocation = location
        };

        var res = await _deskService.AddDesk(desk);
        var desks = await _deskService.GetDesks();
        // TestContext.Out.Write("Desks length:\t");
        // TestContext.Out.Write(desks.ToArray().Length);
        // TestContext.Out.WriteLine(desks.ToArray()[0].Id);
        Assert.NotNull(desks.Find(d => d.Capacity == 1));
    }

}