using System;
using System.Collections.Generic;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NorthWinds.Persistence.Entities;

namespace NorthWinds.Persistence.DBContexts;

public partial class NorthWindsContext : BaseContext, IUnitOfWork
{
    public NorthWindsContext()
    {
    }

    public NorthWindsContext(DbContextOptions<NorthWindsContext> options)
        : base(options)
    {
    }
      
}
