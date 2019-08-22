using System;
using System.Net;

namespace ProjetoAPP
{
    public class ConnWithPHP
    {
        // Envia e recebe as variáveis do Server via http
        public string Conexao(string sei, string variaveis)
        {
            String url = "http://petprojeto.000webhostapp.com/app.php" + sei + variaveis;
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
            httpWReq.Method = "GET";
            httpWReq.ContentType = "application/json"; // xml;
            try
            {
                var httpResponse = (HttpWebResponse)httpWReq.GetResponse();

                WebClient client = new WebClient();
                string downloadString = client.DownloadString("http://petprojeto.000webhostapp.com/app.php" + sei + "");
                return downloadString;
            }
            catch (Exception)
            {
                return "Algo não está correto!";
            }
        }
    }
}

