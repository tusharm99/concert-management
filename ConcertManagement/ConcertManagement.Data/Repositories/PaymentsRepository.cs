using ConcertManagement.Core.Entities;

namespace ConcertManagement.Data.Repositories
{
    public class PaymentsRepository : GenericRepository<Payment>, IPaymentsRepository
    {
        private readonly Entities.CmDbContext _dbContext;

        public PaymentsRepository(Entities.CmDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
