using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Infrastructure.Identity;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.UnitTests.Infrastructure.Identity;

public class ScimUserServiceTests
{
    [Fact]
    public async Task CreateAsync_Persists_User_With_Active_Status()
    {
        await using var context = BuildContext();
        var service = new ScimUserService(context);

        var resource = new ScimUserResource
        {
            Email = "jane.doe@example.com",
            GivenName = "Jane",
            FamilyName = "Doe",
            UserName = "jane.doe",
            Active = true
        };

        var created = await service.CreateAsync(resource, CancellationToken.None);

        Assert.Equal("jane.doe", created.UserName);
        Assert.True(created.Active);
    }

    [Fact]
    public async Task PatchAsync_Updates_Active_Status()
    {
        await using var context = BuildContext();
        var service = new ScimUserService(context);

        var resource = new ScimUserResource
        {
            Email = "john.smith@example.com",
            GivenName = "John",
            FamilyName = "Smith",
            UserName = "john.smith",
            Active = true
        };

        var created = await service.CreateAsync(resource, CancellationToken.None);

        var patch = new System.Text.Json.Nodes.JsonObject
        {
            ["Operations"] = new System.Text.Json.Nodes.JsonArray
            {
                new System.Text.Json.Nodes.JsonObject
                {
                    ["op"] = "replace",
                    ["path"] = "active",
                    ["value"] = false
                }
            }
        };

        var updated = await service.PatchAsync(created.Id, patch, CancellationToken.None);

        Assert.NotNull(updated);
        Assert.False(updated!.Active);
    }

    private static ApplicationDbContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}