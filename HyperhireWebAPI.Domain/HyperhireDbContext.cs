using HyperhireWebAPI.Domain.Common;
using HyperhireWebAPI.Domain.Entities;
using HyperhireWebAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace HyperhireWebAPI.Domain;

public partial class HyperhireDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _dbContextTransaction;
    public HyperhireDbContext()
    {
    }

    public HyperhireDbContext(DbContextOptions<HyperhireDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }
    public DbSet<OrderDetail> OrderDetail { get; set; }
    public DbSet<ProductDetail> ProductDetail { get; set; }
    public DbSet<User> User { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedBy ??= "Admin";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.Now;
                    entry.Entity.LastModifiedBy ??= "Admin";
                    break;
            }
        }
        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedBy = "Admin";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.Now;
                    entry.Entity.LastModifiedBy = "Admin";
                    break;
            }
        }
        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
        .ToArray();

        var result = base.SaveChanges();

        return result;
    }

    public IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, string? lockName = null)
    {
        _dbContextTransaction = Database.BeginTransaction(isolationLevel);
        return _dbContextTransaction;
    }

    public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, string? lockName = null, CancellationToken cancellationToken = default)
    {
        _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _dbContextTransaction;
    }

    public void CommitTransaction()
    {
        _dbContextTransaction?.Commit();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContextTransaction != null)
            await _dbContextTransaction.CommitAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //All Decimals will have 18,6 Range
        foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
        modelBuilder.Entity<ProductDetail>().ToTable("ProductDetail");
        modelBuilder.Entity<User>().ToTable("User");

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = Guid.Parse("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"),
                Name = "Amazing View",
                CategoryNameId = "amazing_view",
                Icon = "url icon",
                IsActive = true,
                Created = DateTime.Now,
                CreatedBy = "Admin"
            }
        );
        modelBuilder.Entity<ProductDetail>().HasData(
            new ProductDetail
            {
                Id = Guid.NewGuid(),
                Name = "Centana Penthouse 3BR_180sqm Panoramic CityVIEW",
                CategoryId = Guid.Parse("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"),
                OriginalPrice = 10,
                Price = 7,
                MaxNight = 10,
                MaxGuests = 10,
                ImgUrl = ["img url 1", "img url 2"],
                Location = "Quận 2, Vietnam",
                Decription = "PENHOUSE 180smq 3 with super large PANORAMA BALCONY at CENTANA District 2 HCMC",
                Created = DateTime.Now,
                CreatedBy = "Admin"
            }
        );

        modelBuilder.Entity<ProductDetail>().HasData(
            new ProductDetail
            {
                Id = Guid.NewGuid(),
                Name = "VILLA VENITY Sky",
                CategoryId = Guid.Parse("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"),
                OriginalPrice = 8,
                Price = 6,
                MaxNight = 8,
                MaxGuests = 8,
                ImgUrl = ["img url 1", "img url 2"],
                Location = "Thành phố Nha Trang, Vietnam",
                Decription = "The Mirror Villa is luxurious all the way and features everything you can expect from a smart, upscale property of 21st century. It impresses with utilizing contemporary and distinctive materials, finishing with the utmost attention to details and quality, innovative technologies and high-end appliances.",
                Created = DateTime.Now,
                CreatedBy = "Admin"
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                UserName =  "123456789",
                Password =  "123456",
                Created = DateTime.Now,
                CreatedBy = "Admin"
            }
        );

        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
