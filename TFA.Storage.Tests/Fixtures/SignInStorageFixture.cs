using TFA.Storage.Entities;

namespace TFA.Storage.Tests.Fixtures;

public class SignInStorageFixture(StorageTestFixture storageTestFixture) : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
    
        await  base.InitializeAsync();
        
        await using var dbContext = storageTestFixture.GetDbContext();
        await dbContext.Users.AddRangeAsync(new User
        {
            UserId = Guid.Parse("846090D7-0EBF-43C4-92B4-2A1577B0F15E"),
            Login = "test user",
            Salt = [1],
            PasswordHash = [2]
        }, new User
        {
            UserId = Guid.Parse("78A9EA1D-A02B-437E-B903-A5A3B9151792"),
            Login = "another user",
            Salt = [1],
            PasswordHash = [2]
        });
        await dbContext.SaveChangesAsync();
    }
}