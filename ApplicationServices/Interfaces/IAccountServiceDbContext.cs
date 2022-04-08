using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface IAccountServiceDbContext
    {
        DbSet<Account> Accounts { get; set; }  
        DbSet<Customer> Customers { get; set; }
        DbSet<Transaction> Transactions { get; set; } 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        int SaveChanges();
    }
}
