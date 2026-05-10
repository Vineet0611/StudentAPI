using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository.Base
{
    public interface IUnitOfWork
    {

        int Commit();

        void Rollback();

        Task CommitAsync();

        Task RollbackAsync();
        Task BeginTransactionAsync();
        Task CommitAsyncWithTransactions();
    }
}
