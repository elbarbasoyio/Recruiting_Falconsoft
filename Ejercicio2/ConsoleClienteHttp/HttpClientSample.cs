using ConsoleClienteHttp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientSample
{
    public class Program
    {
        public static void Main(string[] args)
        {

            SaveToFile();

            string _photos = File.ReadAllText("C:\\ProyectosCode\\FilePhoto.json");
            List<Photo> photos = JsonConvert.DeserializeObject<List<Photo>>(_photos);

            var pepe = ";";




            //CASO A: ARCHIVO DE CASI 1.5 MB

            //string url = "https://jsonplaceholder.typicode.com/photos";   //caso liviano
            ////string url = "https://raw.githubusercontent.com/zemirco/sf-city-lots-json/master/citylots.json"; //caso pesado
            //HttpClient client = new HttpClient();
            //var httpResponse = await client.GetAsync(url);

            //if (httpResponse.IsSuccessStatusCode)
            //{
            //    //parte 1: grabamos el archivo en nuestra carpeta
            //    var content = await httpResponse.Content.ReadAsStringAsync();
            //    //string archivo = JsonSerializer.Serialize(content);
            //    string archivo = System.Text.Json.JsonSerializer.Serialize(content);
            //    File.WriteAllText("photo.json", archivo);


            //    //    ////parte 2: lo subimos como un objeto que ya podemos usar
            //    //    //List<Photo> photos = JsonSerializer.Deserialize<List<Photo>>(content);
            //}

            //********************************

            ////string url = "https://raw.githubusercontent.com/zemirco/sf-city-lots-json/master/citylots.json";
            //string url = "https://jsonplaceholder.typicode.com/photos";
            //FileStream fileStream = File.Create("C:\\ProyectosCode\\photoconflush.json");
            //using (var httpClient = new HttpClient())
            //{
            //    //esto funcionaaaa lo guarda al archivo :) 

            //    var stream = await httpClient.GetStreamAsync(url);
            //    await stream.CopyToAsync(fileStream);
            //}


            //string url = "https://raw.githubusercontent.com/zemirco/sf-city-lots-json/master/citylots.json";
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpWebRequest.Method = WebRequestMethods.Http.Get;
            //httpWebRequest.Accept = "application/json; charset=utf-8";
            //string file;
            //var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var sr = new StreamReader(response.GetResponseStream()))
            //{
            //    file = sr.ReadToEnd();
            //}

            //File.WriteAllText("yyy.json", file);





            //deserializacion

            //string path = @"C:\\ProyectosCode\\photo.json";
            //if (File.Exists(path))
            //{
            //    String JSONtxt = File.ReadAllText(path);
            //    var photos = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Photo>>(JSONtxt);
            //}






            //metodo 2
            //using (StreamReader reader = new StreamReader("C:\\ProyectosCode\\photo.json"))
            //{
            //    //carga el json en un string
            //    var json = reader.ReadToEndAsync();
            //    string jsFile = AppDomain.CurrentDomain.BaseDirectory + json;
            //    var verdura = "sds";
            //}
            //string json = await File.ReadAllTextAsync("C:\\ProyectosCode\\photo.json");
            //Stream stream = new MemoryStream();
            //var aux = stream.ReadAsync(json);
            //string pepe = "fdsfsfsfsfd";

            //Photo photo = JsonSerializer.DeserializeAsync<Photo>(json);


        }

        public static void SaveToFile()
        {
            string url = "https://jsonplaceholder.typicode.com/photos";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
            }

            File.WriteAllText("C:\\ProyectosCode\\FilePhoto.json", file);

        }

    }
}
