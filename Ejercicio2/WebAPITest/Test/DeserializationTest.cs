using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPITest.Test
{
    [TestClass]
    public class DeserializationTest : TestBase
    {
        [TestMethod]
        public async Task HttpCall_and_Deserialization()
        {
            HttpClient client = new HttpClient();




        }
    }
}
