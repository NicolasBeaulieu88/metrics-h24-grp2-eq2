using MetricsAPI_LOG680;
using MetricsAPI_LOG680.Repositories;
using MetricsAPI_LOG680.Services;
using Microsoft.EntityFrameworkCore;

namespace MetricsAPI_LOG680_Tests;

[TestFixture]
public class SnapshotServiceTests
{
    private DateTime _date;
    private ISnapshotService _snapshotService;
    private ISnapshotRepository _snapshotRepository;

    [SetUp]
    public void Setup()
    {
        _date = DateTime.UtcNow;
        var dbContextMock = new Mock<ApiDbContext>(new DbContextOptions<ApiDbContext>());
        _snapshotRepository = new SnapshotRepository(dbContextMock.Object);
        _snapshotService = new SnapshotService(_snapshotRepository);
    }

    [Test]
    public void CreateSnapshotWithValidValuesTest()
    {
        var backlogItems = 10;
        var snapshotData = _snapshotService.CreateSnapshot(backlogItems, 0, 0, 0, 0, _date);

        // Assert
        Assert.That(snapshotData.Backlog_items, Is.EqualTo(backlogItems));
        Assert.That(snapshotData.A_faire_items, Is.EqualTo(0));
        Assert.That(snapshotData.En_cours_items, Is.EqualTo(0));
        Assert.That(snapshotData.Revue_items, Is.EqualTo(0));
        Assert.That(snapshotData.Terminee_items, Is.EqualTo(0));
        Assert.That(snapshotData.Total_items, Is.EqualTo(backlogItems));
        Assert.That(snapshotData.Timestamps, Is.EqualTo(_date));
    }

    [Test]
    public void CreateSnapshotWithRandomValidValuesTest()
    {
        var rnd = new Random();
        var backlogItems = rnd.Next(10);
        var aFaireItems = rnd.Next(10);
        var enCoursItems = rnd.Next(10);
        var revueItems = rnd.Next(10);
        var termineeItems = rnd.Next(10);
        var snapshotData = _snapshotService.CreateSnapshot(backlogItems, aFaireItems, enCoursItems, revueItems, termineeItems, _date);

        // Verif
        var totalItems = backlogItems + aFaireItems + enCoursItems + revueItems + termineeItems;

        // Assert
        Assert.That(snapshotData.Backlog_items, Is.EqualTo(backlogItems));
        Assert.That(snapshotData.A_faire_items, Is.EqualTo(aFaireItems));
        Assert.That(snapshotData.En_cours_items, Is.EqualTo(enCoursItems));
        Assert.That(snapshotData.Revue_items, Is.EqualTo(revueItems));
        Assert.That(snapshotData.Terminee_items, Is.EqualTo(termineeItems));
        Assert.That(snapshotData.Total_items, Is.EqualTo(totalItems));
        Assert.That(snapshotData.Timestamps, Is.EqualTo(_date));
    }
}
