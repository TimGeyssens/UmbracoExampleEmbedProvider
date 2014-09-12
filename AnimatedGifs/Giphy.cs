using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Umbraco.Web.Media.EmbedProviders;

namespace AnimatedGifs
{
    public class Giphy : AbstractProvider
    {
        public override bool SupportsDimensions
        {
            get { return false; }
        }

        public override string GetMarkup(string url, int maxWidth, int maxHeight)
        {
            var u = new Uri(url);
            //Example url http://giphy.com/gifs/reactiongifs-2vA33ikUb0Qz6
            var id = u.Segments.Last().Split('-').Last();
            //using public api key atm
            var api = string.Format("http://api.giphy.com/v1/gifs/{0}?api_key=dc6zaTOxFJmzC", id);

            using(var apiClient = new HttpClient())
            {

                var dataFromAPI = apiClient.GetAsync(api).Result;

                if (dataFromAPI.IsSuccessStatusCode)
                {
                    var APIresult = dataFromAPI.Content.ReadAsStringAsync();

                    var result = JObject.Parse(APIresult.Result);

                    var embedUrl = result.SelectToken("data.images.original.url").Value<string>();

                    return string.Format("<img src=\"{0}\"/>", embedUrl);
                }
            }
            //fall back to a default 'fail' gif
            return string.Format("<iframe src=\"//giphy.com/embed/{0}\" width=\"{1}\" height=\"{2}\" frameBorder=\"0\" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>",
              "bJAi9R0WWOohO",250,153);
        }
    }
}
