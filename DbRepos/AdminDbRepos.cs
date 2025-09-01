using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        // Use _dbContext and _encryptions to seed the database
        var seeder = new SeedGenerator();

        var creditcards = seeder.ItemsToList<CreditCardDbM>(nrItems);
        _dbContext.CreditCards.AddRange(creditcards);
        await _dbContext.SaveChangesAsync();
    }
    public AdminDbRepos(MainDbContext dbContext, Encryptions encryptions)
    {
        _dbContext = dbContext;
        _encryptions = encryptions;
    }
}
