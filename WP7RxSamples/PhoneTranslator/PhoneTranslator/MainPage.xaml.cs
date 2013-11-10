using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using System.Collections.ObjectModel;
using PhoneTranslator.BingTranslatorService;

namespace PhoneTranslator
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        const string AppID = "1848A7A37C632E32B67492E22DA29125992A298C";
        string _sourceLanguage = "en";
        string[] _destLanguages = { "en", "de", "es", "fr", "it" };
        const string TranslatingText = "Translating...";
        const double PressDelay = 1.0;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           SetupTranslator();
        }

        private void SetupTranslator()
        {
            try
            {

                var results = new ObservableCollection<KeyValuePair<string, string>>();
                Translations.ItemsSource = results;

                IObservable<string> translationTexts =
                    (from keyup in Observable.FromEvent<KeyEventArgs>(Input, "KeyUp")
                     where !String.IsNullOrEmpty(Input.Text)
                     select Input.Text)
                    .Throttle(TimeSpan.FromSeconds(PressDelay))
                    .ObserveOnDispatcher()
                    .Do(_ => results.Clear())
                    ;


                var destLanguages = new string[] { "de", "es", "zh-CHT", "fr", "it", "ar", "ht", "he", "ja", "ko", "no", "ru", "th" };

                LanguageService svc = new BingTranslatorService.LanguageServiceClient();
                var svcObs = Observable.FromAsyncPattern<TranslateRequest, TranslateResponse>(svc.BeginTranslate, svc.EndTranslate);

                var query =
                    from lang in destLanguages.ToObservable()
                    from text in translationTexts
                    from res in svcObs.Invoke(new TranslateRequest(AppID, text, "en-us", lang))
                        .TakeUntil(translationTexts)
                    select new { Result = res.TranslateResult, TargetLanguage = lang };

                query.ObserveOnDispatcher().Subscribe(
                    trans => results.Add(new KeyValuePair<string, string>(trans.TargetLanguage, trans.Result)),
                    err => results.Add(new KeyValuePair<string, string>("Error", err.ToString())));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

        }

    }
}
