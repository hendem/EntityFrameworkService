using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
