using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsSharp
{
    public partial class Translator : Form
    {
        public Translator()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            SetupObservable();
        }

        const double PressDelay = 1.0;
        HttpClient client = new HttpClient();
        List<IDisposable> subscriptions = new List<IDisposable>();

        private void SetupObservable()
        {
            var translations = new BindingList<string>();
            lstResults.DataSource = translations;

            IObservable<string> translationTexts =
                (from keyup in Observable.FromEventPattern<KeyEventArgs>(txtInput, "KeyUp")
                 where !string.IsNullOrEmpty(txtInput.Text)
                 select txtInput.Text)
                .Throttle(TimeSpan.FromSeconds(PressDelay))
                .ObserveOn(txtInput)
                .Do(_ => translations.Clear())
                ;

            // See https://api.cognitive.microsofttranslator.com/languages?api-version=3.0
            // For full list of supported languages
            var destLanguages = new string[] { "de", "es", "zh-CHT", "fr", "it", "ar", "ht", "he", "ja", "ko", "no", "ru", "th", "tlh-Latn" };

            var query =
                from lang in destLanguages.ToObservable()
                from text in translationTexts
                from res in Observable.FromAsync(() => TranslateAsync(text, lang))
                    .TakeUntil(translationTexts)
                select new { Result = res, TargetLanguage = lang };

            subscriptions.Add(
                query.ObserveOn(txtInput)
                .Subscribe(
                    trans => translations.Add($"{trans.TargetLanguage}: {trans.Result}"),
                    err => translations.Add($"Error: {err}")));
        }

        async Task<string> TranslateAsync(string textToTranslate, string targetLanguage)
        {
            // Input and output languages are defined as parameters.
            string route = $"/translate?api-version=3.0&from=en&to={targetLanguage}";
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                JArray trans = JArray.Parse(result);
                
                return ((dynamic)trans[0]).translations[0].text;
            }
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                subscriptions.ForEach(s => s?.Dispose());
                client?.Dispose();
            }
            base.Dispose(disposing);
        }

        // See the Bing translator service quickstart to get your own subscription key
        // https://docs.microsoft.com/en-us/azure/cognitive-services/translator/quickstart-translator?tabs=csharp 
        #region secrets
        private static readonly string subscriptionKey = "UseTheLinkAboveToGetYourKey";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "eastus2";

        #endregion
    }


}
