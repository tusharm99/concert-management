using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class TicketTypesRepository : GenericRepository<TicketType>, ITicketTypesRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public TicketTypesRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
