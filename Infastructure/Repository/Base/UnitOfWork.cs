using Infastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dataContext.Database.BeginTransactionAsync();
        }

        public int Commit()
        {
            return _dataContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task CommitAsyncWithTransactions()
        {
            await _dataContext.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
            else
                _dataContext.Dispose();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
            else
                await _dataContext.DisposeAsync();
        }
    }
}