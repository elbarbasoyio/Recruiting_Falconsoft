using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;


namespace WebAPITest.Test
{
    [TestClass]
    public class ListsMatchingTest : TestBase
    {
        [TestMethod]
        public async Task ListsMatching()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var nombreDB2 = Guid.NewGuid().ToString();
            var contexto2 = ConstruirContext(nombreDB2);
            List<Sample> Lista_A = new List<Sample>();
            List<Sample> Lista_B = new List<Sample>();
            List<Sample> ListaResult = new List<Sample>();

            //en este ejemplo los elementos incluidos en ambas listas son los registros de ID 499.990 al 500.000 (11 registros)
            for (int i = 1; i < 500001; i++)
            {
                await contexto.Sample.AddAsync(new Sample { ID = i.ToString(), Content = ("content_" + i.ToString()), Qty = i }); 
            }
            await contexto.SaveChangesAsync();
            Lista_A.AddRange(contexto.Sample);

            for (int i = 499990; i < 999991; i++)
            {
                await contexto2.Sample.AddAsync(new Sample { ID = i.ToString(), Content = ("content_" + i.ToString()), Qty = i });
            }
            await contexto2.SaveChangesAsync();
            Lista_B.AddRange(contexto2.Sample);


            var result = from a in Lista_A
                         join b in Lista_B on a.ID equals b.ID
                         where a.Content == b.Content && a.Qty == b.Qty
                         select a;

            ListaResult = result.ToList();
            Assert.AreEqual(11, ListaResult.Count);
        }
    }
}
