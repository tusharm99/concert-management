using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class EventsRepository : GenericRepository<Event>, IEventsRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public EventsRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
