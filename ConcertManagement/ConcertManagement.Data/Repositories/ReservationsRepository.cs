using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class ReservationsRepository : GenericRepository<Reservation>, IReservationsRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public ReservationsRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
