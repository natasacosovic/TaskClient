using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        static string GetText(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string text = responseStream.ReadToEnd();
            webresponse.Close();
            return text;
        }
        static void Main(string[] args)
        {
            string repeat;
            do
            {
                Console.WriteLine($"Izaberi nacin citanja (unesi 0, 1 ili 2):\n 0 iz fajla \n 1 iz baze \n 2 korisnik unosi tekst");
                int x;
                do
                {
                    int.TryParse(Console.ReadLine(), out x);
                } while (x > 2 || x < 0);

                string text = string.Empty;
                switch (x)
                {
                    case 0:
                        text = Program.GetText("https://localhost:44356/api/text/file");
                        Console.WriteLine(text);
                        break;
                    case 1:
                        text = Program.GetText("https://localhost:44356/api/text/db");
                        Console.WriteLine(text);
                        break;
                    case 2:
                        Console.WriteLine("Unesite zeljeni tekst");
                        text = Console.ReadLine();
                        text = JsonConvert.SerializeObject(text);
                        break;
                    default:
                        break;
                }
                Console.WriteLine($"Da li zelite da prebrojite reci u tekstu? [y/n]");

                string answer = Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create("https://localhost:44356/api/text");
                    webrequest.Method = "POST";
                    webrequest.ContentType = "application/json";
                    using (StreamWriter writer = new StreamWriter(webrequest.GetRequestStream()))
                    {
                        writer.Write(text);
                    }
                    HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                    Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                    string text2 = responseStream.ReadToEnd();
                    webresponse.Close();
                    Console.WriteLine($"rezultat je: {text2}");
                }
                Console.WriteLine("Probaj ponovo[y/n]?");
                repeat = Console.ReadLine();
            } while (repeat == "y" || repeat == "Y");
        }
    }
}
