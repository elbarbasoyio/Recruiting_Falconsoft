using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace WebAPITest
{
        public class TestBase
        {
            protected ApplicationDbContext ConstruirContext(string nombreDB)
            {
                var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(nombreDB).Options;

                var dbContext = new ApplicationDbContext(opciones);

                return dbContext;
            }

        }
}
