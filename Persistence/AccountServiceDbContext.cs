using ApplicationServices.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class AccountServiceDbContext : DbContext, IAccountServiceDbContext
    {
        public AccountServiceDbContext(DbContextOptions<AccountServiceDbContext> options)
            : base(options)
        {
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnSavingChanges();

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public override int SaveChanges()
        {
            OnSavingChanges();

            return base.SaveChanges();
        }

        private void OnSavingChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Entity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateAdded = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        entry.Entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountServiceDbContext).Assembly);
            //makes sure that all items returned are not deleted items
            modelBuilder.ApplyGlobalFilters<bool>("IsDeleted", false);

            // Map entities to tables  
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Customer>().ToTable("Customers");

            // Configure Primary Keys  
            modelBuilder.Entity<Account>().HasKey(ug => ug.Id);
            modelBuilder.Entity<Transaction>().HasKey(u => u.Id);
            modelBuilder.Entity<Customer>().HasKey(u => u.Id);

            // Configure indexes  
            modelBuilder.Entity<Account>().HasIndex(p => p.CustomerId).IsUnique(false);
            modelBuilder.Entity<Transaction>().HasIndex(u => u.AccountId).IsUnique(false);
            modelBuilder.Entity<Transaction>().HasIndex(u => u.TransactionReference).IsUnique(true);
            modelBuilder.Entity<Customer>().HasIndex(u => u.Email).IsUnique();

            // Configure columns  
            modelBuilder.Entity<Customer>().Property(ug => ug.Id).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Customer>().Property(ug => ug.Email).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Customer>().Property(ug => ug.Firstname).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Customer>().Property(ug => ug.Image).HasColumnType("varchar(100)").IsRequired(false);
            modelBuilder.Entity<Customer>().Property(ug => ug.Lastname).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Customer>().Property(ug => ug.PhoneNo).HasColumnType("varchar(50)").IsRequired();

            modelBuilder.Entity<Account>().Property(u => u.Id).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Account>().Property(u => u.AccountNo).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Account>().Property(u => u.AvailableBalance).HasColumnType("decimal(18,2)").IsRequired();
            modelBuilder.Entity<Account>().Property(u => u.LedgerBalance).HasColumnType("decimal(18,2)").IsRequired();
            modelBuilder.Entity<Account>().Property(u => u.CustomerId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Account>().Property(u => u.AccountType).HasColumnType("int").IsRequired();

            modelBuilder.Entity<Transaction>().Property(ug => ug.Id).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.AccountId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.Amount).HasColumnType("decimal(18,2)").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.Narration).HasColumnType("varchar(100)").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.TransactionReference).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Transaction>().Property(ug => ug.TransactionType).HasColumnType("int").IsRequired();


            // Configure relationships  
            modelBuilder.Entity<Account>().HasOne<Customer>().WithMany().HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.CustomerId).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Customer");
        }
    }
}