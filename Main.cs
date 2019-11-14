using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wox.Plugin.WDWNTunes
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            var currentSongInfo = GetCurrentSongInfo();

            return new List<Result>
            {
                new Result
                {
                    IcoPath = $"Images\\ntunes.png",
                    SubTitle = $"{currentSongInfo.Artist}",
                    Title = $"{currentSongInfo.Title}",
                    Action = _ => { return true; }
                }
            };
        }

        private NTunesCurrentTrack GetCurrentSongInfo()
        {
            using (var httpClient = new HttpClient())
            {
                var response = Task.Run(async () => await httpClient.GetStringAsync("http://fastpass.wdwnt.com/live365")).Result;
                var jsonObject = JObject.Parse(response);

                return new NTunesCurrentTrack
                {
                    Artist = (string)jsonObject["current-track"]["artist"],
                    Title = (string)jsonObject["current-track"]["title"],
                };
            }
        }
    }

    public class NTunesCurrentTrack
    {
        public string Artist { get; set; }
        public string Title { get; set; }
    }
}
