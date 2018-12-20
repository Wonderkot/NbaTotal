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
        public Dictionary<long, string> GetAllTeams()
        {
            if (_settings?.AllTeamUrl == null)
                return new Dictionary<long, string>();

            var url = _settings.AllTeamUrl;
            var responsetext = GetResponseText(url);

            try
            {
                var json = JObject.Parse(responsetext);
                var resultSets = json["resultSets"];
                var rowSet = resultSets[0]["rowSet"] as JArray;
                var teams = new Dictionary<long, string>();
                if (rowSet != null)
                {
                    foreach (var item in rowSet)
                    {
                        var value = item[4].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            long id = (long)item[1];
                            teams.Add(id, value);
                        }
                    }
                    return teams;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new Dictionary<long, string>();
        }

        private string GetResponseText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/json";
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", ": gzip, deflate, br");
            //request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";
            request.Referer = "http://stats.nba.com/scores/";

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
                var json = JObject.Parse(response);
                var resultSets = json["resultSets"];
                var rowSet = resultSets[0]["rowSet"] as JArray;
                if (rowSet != null)
                {
                    var pts = rowSet[0][27].Value<double>();
                    var plusMinus = rowSet[0][28].Value<double>();
                    var team = new Team
                    {
                        Id = id,
                        PlusMinus = plusMinus,
                        Pts = pts
                    };
                    return team;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new Team();
        }
    }
}