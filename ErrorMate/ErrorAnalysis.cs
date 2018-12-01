using Newtonsoft.Json.Linq;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ErrorMate
{
    class ErrorAnalysis
    {
        // Azure ubscription key.
        const string subscriptionKey = "3df569f2caff4366851e0991714b0d2d";

        // URL for Access to API
        const string uriBase = "https://australiaeast.api.cognitive.microsoft.com/vision/v2.0/recognizeText";

        // Google search subscription key and searchengine id
        const string googleApiKey = "AIzaSyCF-a0XK9h4uW28be5FkW20d5lHkXWW2kc";
        const string googleSearchEngineId = "010110896744742302390:ckh9sn3qtbc";

        // Google search language
        const CseResource.ListRequest.LrEnum lang = CseResource.ListRequest.LrEnum.LangEn;

        // Google search word exclusion
        private readonly static string[] EXCLUDE_WORDS = { "OK", "the" };

        /// <summary>
        /// Runs the collected image against Azure Cognative Services
        /// </summary>
        /// <param name="byteData">Byte data of bmp image.</param>
        //static void Main(string[] args)
        public static async Task<IList<Result>> RunBMP(byte[] byteData)
        {

            // Check to make sure the file/image is legit
            if (byteData != null && byteData.Length > 0)
            {
                // Make API call to review image.
                Console.WriteLine("\nPlease wait while we check for results.\n");

                CognativeServicesJsonResult jsonResults = await AnalyseImageError(byteData);
                IList<Result> results = GoogleSearch(jsonResults);

                return results;
            }
            else
            {
                Console.WriteLine("\nNo image supplied, or byte data empty");
                return null;
            }
        }

        /// <summary>
        /// Collates the collected text from the specified image and
        /// runs a search with Google (looking at Microsoft Support).
        /// Returns list of Google Search results.
        /// </summary>
        /// <param name="jsonResult">Result of the image using Azure Computer Vision.</param>
        static IList<Result> GoogleSearch(CognativeServicesJsonResult jsonResult)
        {
            try
            {
                var query = new StringBuilder();
                foreach (Line line in jsonResult.recognitionResult.lines)
                {
                    //Console.WriteLine(line.text.ToString());
                    var cleaned = Regex.Replace(line.text.ToString(), "\\b" + string.Join("\\b|\\b", EXCLUDE_WORDS) + "\\b", "");
                    query.Append(cleaned + " ");
                }
                var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = googleApiKey });
                var listRequest = customSearchService.Cse.List(query.ToString());
                //API types list here https://developers.google.com/custom-search/json-api/v1/reference/cse/list
                listRequest.Cx = googleSearchEngineId;
                listRequest.Lr = lang;

                Console.WriteLine("\nStarting google search.\n");
                IList<Result> results = new List<Result>();
                results = listRequest.Execute().Items;

                return results;
            }
            catch(Exception e)
            {
                Console.WriteLine("\n" + e.Message + ":" + e.InnerException);
                throw e;
            }     
        }

        /// <summary>
        /// Gets the text from the specified image file by using
        /// the Azure Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with text.</param>
        //static async Task<JsonResult> AnalyseImageError(string imageFilePath)
        static async Task<CognativeServicesJsonResult> AnalyseImageError(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameter, this can be Printed or Handwritten.
                string requestParameters = "mode=Printed";

                // Create URI for the API call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // We need two API calls here.
                // One call for image processing, and another call
                // to retrieve the text from the image.
                // operationLocation stores the API location to call to
                // retrieve the text.
                string operationLocation;

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // The first REST call starts the async process to analyze the
                    // written text in the image.
                    Console.WriteLine("\nSending image for analysis.\n");
                    response = await client.PostAsync(uri, content);
                }

                // The response contains the URI to retrieve the result of the process.
                if (response.IsSuccessStatusCode)
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else
                {
                    // Display the JSON error data.
                    string errorString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("\n\nResponse:\n{0}\n",
                        JToken.Parse(errorString).ToString());
                    var jsonError = JsonConvert.DeserializeObject<CognativeServicesJsonResult>(errorString);
                    return jsonError;
                }

                // The second REST call retrieves the text written in the image.
                //
                // Note: The response may not be immediately available. Handwriting
                // recognition is an async operation that can take a variable amount
                // of time depending on the length of the handwritten text. You may
                // need to wait or retry this operation.
                //
                // This example checks once per second for ten seconds.
                Console.WriteLine("\nReceiving analysis of image.\n");
                string contentString;
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    response = await client.GetAsync(operationLocation);
                    contentString = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

                if (i == 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
                {
                    Console.WriteLine("\nTimeout error in receiving analysis of image.\n");
                }

                var jsonResult = JsonConvert.DeserializeObject<CognativeServicesJsonResult>(contentString);

                // Display the full JSON response.
                //Console.WriteLine("\nResponse:\n\n{0}\n",
                //JToken.Parse(contentString).ToString());

                // Display only lines from JSON reponse.
                foreach (Line line in jsonResult.recognitionResult.lines)
                {
                    Console.WriteLine(line.text.ToString());
                }

                return jsonResult;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message + ":" + e.InnerException);
                throw e;
            }
        }
    }
}
