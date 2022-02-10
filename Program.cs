using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace ApicovidUmbria

{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        //args[0]=consumerKey
        //args[1]=consumerSecret
        //args[2]=url
        //args[3]=jsonstring


        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 mioform = new Form1();
            string risposta = "Programma avviato";
            try
            {
                EseguiOperazioneAsync(args[0], args[1], args[2], args[3]); //esegue l'operazione di creazione tokene e poi invio post
            }
            catch
            {
                 risposta = "Errori parametri programma o errore connessione";
            }
            
            mioform.LabelText = risposta;
            Application.Run(mioform);


        }

       
        private static string Base64Encode(string plainText) //funzione ausiliaria per gestire la creazione del token dalle chiavi
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        static async Task<string> GeneraToken(string consumerSecret, string consumerKey) //funzione per la generazione del token
        {

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.regione.umbria.it/token"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Basic " + Base64Encode(consumerKey + ":" + consumerSecret));

                    request.Content = new StringContent("grant_type=client_credentials");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string contenuto = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(contenuto);
                    string token = obj.access_token;
                    using (StreamWriter writetext = new StreamWriter("token.txt"))
                    {

                        writetext.WriteLine(token);
                        return token;

                    }


                }
            }
        }

        static async Task<string> RisutaltoTest(string token, string url,string jsonstring) //funzione per l'esecuzione della post
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), url + "/risultato-test"))
                {
                    request.Headers.TryAddWithoutValidation("accept", "application/json");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
                    request.Content = new StringContent(jsonstring);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    string contenuto = await response.Content.ReadAsStringAsync();
                    using (StreamWriter writetext = new StreamWriter("risposta.txt"))
                    {

                        writetext.WriteLine(contenuto);
                        return contenuto;

                    }
                }
            }
        }

        private static async Task EseguiOperazioneAsync(string consumerKey,string consumerSecret,string url, string jsonstring) //funzione per garantire il sincronismo
        {
            string token = await GeneraToken(consumerSecret,consumerKey);
            Console.WriteLine(token);
            string risultato = await RisutaltoTest(token, url,jsonstring);
            Console.WriteLine(risultato);
            Application.ExitThread();


        }
    }




}
