using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CalcLib.BusinessLogic.Interfaces;
using CalcLib.Data;
using Newtonsoft.Json.Linq;

namespace CalcLib.BusinessLogic.Implementation
{
    public class DataProvider : IDataProvider
    {
        private readonly Settings _settings;
        private static readonly HttpClient Client = new HttpClient();
        public DataProvider()
        {
            ISettingsReader settingsReader = new SettingsReader();
            _settings = settingsReader.GetSettings();
        }
        public async Task<List<Team>> GetAllTeams()
        {
            if (_settings.AllTeamUrl == null)
                return new List<Team>();

            var url = _settings.AllTeamUrl;
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");

            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //var request = (HttpWebRequest)WebRequest.Create(url);

            //var response = (HttpWebResponse)request.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            try
            {
                var responseString = await Client.GetStringAsync(url);

                var json = JObject.Parse(responseString);

                var token = json["resultSets"]["0"]["rowSet"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return new List<Team>();
        }

        private bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        public Team GetTeam(long id)
        {
            return null;
        }
    }
}