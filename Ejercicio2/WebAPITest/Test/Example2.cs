using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPITest.Test
{
    [TestClass]
    public class Example2 : TestBase
    {
        [TestMethod]
        public async Task Ejercicio3()
        {
            //Crear una rutina que ejecute un loop con 10 threads paralelas utilizando como input por
            //ejemplo, una lista tipo del item 1, donde cada iteración debe sumar el Qty del objeto Sample
            //para retornar la suma como resultado.

            //La iteración de dicho código en paralelo debe ser cancelable.
            //Se pide simular una cancelación y simular otro tipo de exceptions dentro del loop.
            //El llamador de la rutina debe ser capaz de recibir el resultado de la ejecución y las
            //excepciones generadas por la misma y poder distinguir los tipos de exception para darles
            //diferente tratamiento a cada uno.

            List<Sample> ListaSample = new();
            var nombreDB = Guid.NewGuid().ToString();
            WebAPI.ApplicationDbContext contexto = ConstruirContext(nombreDB);

            for (int i = 1; i < 500001; i++)
            {
                await contexto.Sample.AddAsync(new Sample { ID = i.ToString(), Content = ("content_" + i.ToString()), Qty = i });
            }
            await contexto.SaveChangesAsync();
            ListaSample.AddRange(contexto.Sample);

            try 
            {
                EjecucionEnParalelo(ListaSample);
            }
            catch (AggregateException ex) 
            {
                List<Exception> IgnoredExceptions = new();
                // This is where you can choose which exceptions to handle.
                foreach (var exception in ex.Flatten().InnerExceptions)
                {
                    if (exception is ArgumentException)
                        Console.WriteLine(exception.Message);
                    else
                        IgnoredExceptions.Add(exception);
                }
                if (IgnoredExceptions.Count > 0) throw new AggregateException(IgnoredExceptions);
            }
        }

        public long EjecucionEnParalelo(List<Sample> lista) 
        {
            long resultado = 0;
            ConcurrentQueue<Exception> exceptions = new();
            ParallelOptions options = new();
            options.MaxDegreeOfParallelism = 10;           
            Parallel.ForEach<Sample, long>(lista, options,
                                            () => 0,
                                            (j, loop, subtotal) =>
                                            {
                                                if (j.Qty > 3 && j.Qty < 6)    /// exception con 4-5 PERO SALTA EN EL $, NO SIGUE CON EL 5 !!!
                                                {
                                                    throw new ArgumentException("ERROR: En este caso, Qty = " + j.Qty.ToString());
                                                }

                                                subtotal += j.Qty;
                                                return subtotal;
                                            },
                                            (val) => Interlocked.Add(ref resultado, val)
                                          );
            
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
            return resultado;
        }

    }
}
