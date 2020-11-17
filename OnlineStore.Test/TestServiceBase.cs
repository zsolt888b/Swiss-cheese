using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Test
{
    public class TestServiceBase : ServiceCollection
    {
        public TestServiceBase(bool useInMemoryDbContext = true)
        {
            if (useInMemoryDbContext)
                this.AddDbContext<OnlineStoreDbContext>(options => options.UseInMemoryDatabase(databaseName: "OnlineStoreDbTest"));

            this.AddLogging();
        }
    }
}
