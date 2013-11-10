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
using SilverlightApplication1.Observables;
using System.Collections.ObjectModel;
using SilverlightApplication1.BingTranslatorService;

namespace SilverlightApplication1.Views
{
    /// <summary>
    /// Sample translator using the Bing translation service.
    /// See http://www.minddriven.de/index.php/technology/dot-net/rx-bing-translate-example for details.
    /// </summary>
    public partial class ReactiveTranslator : Page
    {
        public ReactiveTranslator()
        {
            InitializeComponent();
        }

        const string AppID = "1848A7A37C632E32B67492E22DA29125992A298C";
        string _sourceLanguage = "en";
        string[] _destLanguages = { "en", "de", "es", "fr", "it" };
        const string TranslatingText = "Translating...";
        const double PressDelay = .25;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetupCombo();
            SetupTranslator();
        }

        private void SetupTranslator()
        {
            try
            {

                var results = new ObservableCollection<string>();
                Translations.ItemsSource = results;

                IObservable<string> translationTexts =
                    (from keyup in Observable.FromEvent<KeyEventArgs>(txtTranslation, "KeyUp")
                     where !String.IsNullOrEmpty(txtTranslation.Text)
                     select txtTranslation.Text)
                    .Throttle(TimeSpan.FromSeconds(PressDelay))
                    .ObserveOnDispatcher()
                    .Do(_ => results.Clear())
                    ;

                //LanguageService svc = new LanguageServiceClient();
                //var translatorSvc = Observable.FromAsyncPattern<TranslateRequest,TranslateResponse>(svc.BeginTranslate, svc.EndTranslate);

                var destLanguages = new string[] { "de", "es", "zh-CHT", "fr", "it" , "ar", "ht", "he", "ja", "ko", "no", "ru", "th"};

                var query =
                    from lang in destLanguages.ToObservable()
                    from source in translationTexts
                    let svc = new LanguageServiceClient() as LanguageService
                    from res in Observable.FromAsyncPattern<TranslateRequest, TranslateResponse>(svc.BeginTranslate, svc.EndTranslate)
                            ((new TranslateRequest(AppID, source, "en-us", lang)))
                        .TakeUntil(translationTexts) 
                    select new { Result = res, TargetLanguage = lang };

                query.ObserveOnDispatcher().Subscribe(
                    trans => results.Add(trans.TargetLanguage + ": " + trans.Result.TranslateResult),
                    err => System.Diagnostics.Debug.WriteLine(err));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

        }

        private void SetupCombo()
        {
            var comboChanged = Observable.FromEvent<SelectionChangedEventArgs>(cbLanguage, "SelectionChanged");
            comboChanged.Subscribe(_ => _sourceLanguage = _destLanguages[cbLanguage.SelectedIndex]);
        }
    }
}
