using System;
using System.Collections.Generic;
using System.Linq;
using BingTranslate.BingTranslate.Service;

namespace BingTranslate
{
    /// <summary>
    /// This class connects to Bing and translates a term.
    /// </summary>
    public static class BingService
    {
        #region Constants

        /// <summary>
        /// The AppId identifies your application at the Bing online service.
        /// Before running the application, you have to get your free AppId
        /// here: http://www.bing.com/developers/createapp.aspx
        /// </summary>
        private const string AppId = "1848A7A37C632E32B67492E22DA29125992A298C";

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the Bing translation service and returns an IObservable, 
        /// to which clients can subscribe.
        /// </summary>
        public static IObservable<TranslationResponse> Translate(
            this string text, string sourceLanguage, string targetLanguage)
        {
            // async subject holds just one value that is returned by the async Bing operation
            var subject = new AsyncSubject<TranslationResponse>();
            var service = new LiveSearchPortTypeClient();

            // connect SearchCompleted event to the subject's Observer
            service.SearchCompleted +=
                (sender, e) =>
                {
                    if (e.Cancelled)
                        subject.OnCompleted();
                    else if (e.Error != null)
                        subject.OnError(e.Error);
                    else
                        subject.OnNext(e.Result.Translation);
                };

            // set up Bing search request
            var request = new SearchRequest
            {
                AppId = AppId,
                Translation = new TranslationRequest
                {
                    SourceLanguage = sourceLanguage,
                    TargetLanguage = targetLanguage
                },
                Query = text,
                Sources = new[] { SourceType.Translation }
            };

            // start translation via Bing
            service.SearchAsync(request);

            // return the subject's Observable
            return subject.AsObservable();
        }

        #endregion
    }

    // Little helper just to get the translated term.
    internal static class TranslationResponseEx
    {
        public static string GetTranslatedTerm(this TranslationResponse response)
        {
            return (response != null && response.Results.Length > 0) ? response.Results[0].TranslatedTerm : "-";
        }
    }
}
