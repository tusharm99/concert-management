using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class TicketsRepository : GenericRepository<Ticket>, ITicketsRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public TicketsRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
