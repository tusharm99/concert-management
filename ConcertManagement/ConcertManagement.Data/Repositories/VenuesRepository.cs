using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class VenuesRepository : GenericRepository<Venue>, IVenuesRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public VenuesRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
