using System;
using System.Collections.Generic;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NorthWinds.Persistence.Entities;

namespace NorthWinds.Persistence.DBContexts;

public partial class NorthWindsReadOnlyContext : BaseContext, IUnitOfWork
{
    public NorthWindsReadOnlyContext()
    {
    }

    public NorthWindsReadOnlyContext(DbContextOptions<NorthWindsReadOnlyContext> options)
        : base(options)
    {       
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }
    
}
