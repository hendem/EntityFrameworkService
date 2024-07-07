using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthWinds.Persistence.DBContexts;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace EntityFrameworkService_tests
{
    public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    //services.Replace(
                    //    ServiceDescriptor.Scoped(_ =>
                    //        _cartContextFactory.GetCartRepository()));

                    //var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IHelloWorldDependency));
                    //services.Remove(descriptor);


                    //services.AddHttpClient<IHelloWorldDependency, HelloWorldDependency>((client) =>
                    //{
                    //    client.BaseAddress = new System.Uri("http://localhost");
                    //});

                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                     typeof(DbContextOptions<NorthWindsContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    descriptor = services.SingleOrDefault(
                       d => d.ServiceType ==
                    typeof(DbContextOptions<NorthWindsReadOnlyContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                   
                    var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
                    var connection = new SqliteConnection(connectionStringBuilder.ToString());
                    connection.Open();                   
                    services.AddDbContext<NorthWindsContext>(options =>
                         options.UseSqlite(connection));
                    services.AddDbContext<NorthWindsReadOnlyContext>(options =>
                        options.UseSqlite(connection));

                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<NorthWindsContext>();
                        dbContext.Database.EnsureCreated();

                        var readOnlyDbContext = scope.ServiceProvider.GetRequiredService<NorthWindsReadOnlyContext>();
                        readOnlyDbContext.Database.EnsureCreated();
                    }

                });

        }
   

    }
}
