using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using CalcLib.BusinessLogic.Interfaces;
using CalcLib.Data;
using Newtonsoft.Json.Linq;

namespace CalcLib.BusinessLogic.Implementation
{
    public class DataProvider : IDataProvider
    {
        private readonly Settings _settings;
        public DataProvider()
        {
            ISettingsReader settingsReader = new SettingsReader();
            _settings = settingsReader.GetSettings();
        }
        public List<Team> GetAllTeams()
        {
            if (_settings?.AllTeamUrl == null)
                return new List<Team>();

            var url = _settings.AllTeamUrl;
            var responsetext = GetResponseText(url);

            try
            {
                var json = JObject.Parse(responsetext);
                var resultSets = json["resultSets"];
                var rowSet = resultSets[0]["rowSet"] as JArray;
                var teams = new List<Team>();
                if (rowSet != null)
                {
                    foreach (var item in rowSet)
                    {
                        long id = (long) item[1];
                        Team team = GetTeam(id);

                    }
                    return teams;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new List<Team>();
        }

        private string GetResponseText(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Accept = "application/json";
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", ": gzip, deflate, br");
            string responseText;
            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                using (var responsechar = new StreamReader(response.GetResponseStream()))
                {
                    responseText = responsechar.ReadToEnd();
                    Debug.Write(responseText);
                }

                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return responseText;
        }

        public Team GetTeam(long id)
        {
            if (_settings?.TeamUrl == null)
                return null;
            try
            {
                var url = string.Format(_settings.TeamUrl, id);
                var response = GetResponseText(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new Team();
        }
    }
}