using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ErrorMate
{
    public class MsSupportJsonResult
    {
        public Metrics metrics { get; set; }
        public Details details { get; set; }
        public int _ts { get; set; }

        public static MsSupportJsonResult Retrieve(string url)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            var scriptBlocks = doc.DocumentNode.SelectNodes("//script").ToList();
            string scriptblock = scriptBlocks[10].InnerHtml.ToString();
            // Get rid of the javascript function around JSON
            Regex regex = new Regex("(?s){\\s+\"metrics\"(.*?)\"_ts\":\\s+0\\s+}");
            Match jsonMatch = regex.Match(scriptblock);
            if (jsonMatch.Success)
            {
                Console.WriteLine(jsonMatch.Value);
                MsSupportJsonResult jsonResult = JsonConvert.DeserializeObject<MsSupportJsonResult>(jsonMatch.Value);
                return jsonResult;
            }
            return null;
        }
    }

    public class Metrics
    {
        public object[] daily { get; set; }
        public Aggregate aggregate { get; set; }
    }

    public class Aggregate
    {
        public float bounceRate { get; set; }
        public float agentIngressRate { get; set; }
        public float surveySatisfaction { get; set; }
        public float pageImplicitSuccessRate { get; set; }
        public float pageImplicitFailureRate { get; set; }
        public string locale { get; set; }
        public DateTime date { get; set; }
        public int pageViews { get; set; }
        public int noBounce { get; set; }
        public int surveyYes { get; set; }
        public int surveyNo { get; set; }
        public object[] surveyComments { get; set; }
        public int agentIngress { get; set; }
        public float pageAvgDwellTimeMs { get; set; }
    }

    public class Details
    {
        public string subType { get; set; }
        public string heading { get; set; }
        public string description { get; set; }
        public Body[] body { get; set; }
        public string urltitle { get; set; }
        public bool autotitle { get; set; }
        public string[] keywords { get; set; }
        public string[] keywordsLower { get; set; }
        public object[] os { get; set; }
        public string[] products { get; set; }
        public bool noIndex { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string locale { get; set; }
        public string title { get; set; }
        public string titleLower { get; set; }
        public bool published { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime publishedOn { get; set; }
        public int version { get; set; }
        public string eolProject { get; set; }
        public bool isInternalContent { get; set; }
        public string[] supportAreaPaths { get; set; }
        public Supportareapathnode[] supportAreaPathNodes { get; set; }
        public Primarysupportareapath[] primarySupportAreaPath { get; set; }
    }

    public class Body
    {
        public Meta meta { get; set; }
        public string title { get; set; }
        public string[] content { get; set; }
    }

    public class Meta
    {
        public string type { get; set; }
        public object[] products { get; set; }
        public object[] supportAreaPaths { get; set; }
        public bool isInternalContent { get; set; }
    }

    public class Supportareapathnode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public object[] tree { get; set; }
    }

    public class Primarysupportareapath
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public object[] tree { get; set; }
    }

}
