using Microsoft.Extensions.Logging;

using DbRepos;

namespace Services;

public class AdminServiceDb : IAdminService
{
    private readonly AdminDbRepos _repo;

    public AdminServiceDb(AdminDbRepos repo)
    {
        _repo = repo;
    }

    public Task SeedAsync(int nrItems)
    {
        return _repo.SeedAsync(nrItems);
    }
}
