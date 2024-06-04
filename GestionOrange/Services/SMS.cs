using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;

namespace GestionOrange.Services
{
    public class SMS
    {
        private readonly DatabaseContext _databaseContext;

        // Constructeur pour initialiser le contexte de base de données
        public SMS()
        {
            _databaseContext = new DatabaseContext();
        }
        public static string HashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public void sendSms()
        {
            String AK = "3ef60de4dc70e757";
            String AS = "d79a0c781375a5bf93860f1abfdff005";
            String CK = "51eb137d0d6af213e832525b97f839ae";

            //Paramètres de la méthode appellée
            String ServiceName = "sms-bn654768-1";
            String METHOD = "POST";
            String QUERY = "https://eu.api.ovh.com/1.0/sms/" + ServiceName + "/jobs";
            String BODY = @"{ ""charset"": ""UTF-8"", ""receivers"": [ ""+33783228225"" ], ""message"": ""Test SMS OVH"", ""priority"": ""high"",  ""senderForResponse"": true, ""sender"": ""Alarme Orange""}";

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            String TSTAMP = (unixTimestamp).ToString();


            String signature = "$1$" + HashSHA1(AS + "+" + CK + "+" + METHOD + "+" + QUERY + "+" + BODY + "+" + TSTAMP);
            //Console.WriteLine(signature);

            //Création de la requete
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(QUERY);
            req.Method = METHOD;
            req.ContentType = "application/json";
            req.Headers.Add("X-Ovh-Application:" + AK);
            req.Headers.Add("X-Ovh-Consumer:" + CK);
            req.Headers.Add("X-Ovh-Signature:" + signature);
            req.Headers.Add("X-Ovh-Timestamp:" + TSTAMP);

            //Ecriture des paramètres BODY
            using (System.IO.Stream s = req.GetRequestStream())
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                    sw.Write(BODY);
            }

            try
            {
                //Récupération du résultat de l'appel
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)req.GetResponse();
                String[] l = null;
                using (var respStream = myHttpWebResponse.GetResponseStream())
                {
                    var reader = new StreamReader(respStream);
                    String result = reader.ReadToEnd().Trim();
                    //Console.WriteLine(result);

                }
                myHttpWebResponse.Close();

            }
            catch (WebException e)
            {
                /*Console.WriteLine("Error : ");
                Console.WriteLine("Error : ");
                using (WebResponse response = e.Response)
                using (Stream data = ((HttpWebResponse)response).GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }*/
            }
        }
    }
}
