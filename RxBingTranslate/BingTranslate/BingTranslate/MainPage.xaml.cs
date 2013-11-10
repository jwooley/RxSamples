using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace BingTranslate
{
    public partial class MainPage
    {
        #region Private Fields

        private string[] _languages = { "en", "de", "es", "fr", "it" };
        private string _sourceLanguage = "en";
        private const string TranslatingText = "Translating...";
        private const double PressDelay = 1.0;

        #endregion

        #region Public Methods

        public MainPage()
        {
            InitializeComponent();
            //Observable.Context = SynchronizationContexts.CurrentDispatcher;

            SetUpComboBox();

            // 2 possible ways to process the events here:
            SetUpTranslationService();              // just initiates the translation events independent from each other
            //SetUpTranslationServiceWithJoin();      // joins the translation events after return by Bing
        }

        #endregion

        #region Private Helper Methods

        private void SetUpComboBox()
        {
            var comboChanged = Observable.FromEventPattern<SelectionChangedEventArgs>(cbLanguage, "SelectionChanged");
            comboChanged.Subscribe(_ => _sourceLanguage = _languages[cbLanguage.SelectedIndex]);
        }

        /// <summary>
        /// Sets up asynchronous translation of a given input term into some languages.
        /// The translation is written to the UI unsychronized. That means when one translation comes
        /// back, it is immediately put out.
        /// </summary>
        private void SetUpTranslationService()
        {
            // benefits of Rx:
            // - for the supplier (e.g. API designer):
            //   * no declaring event and delegate
            //   * no spatial separation of async method and event that's fired on return
            //     => event-based programming: only connected by naming convention!
            // - for the client (e.g. API user):
            //   * no spatial separation of subscribing to an event and starting the operation
            //   * no order dependencies of subscribing to an event and calling the method
            // => less implicit dependencies, less code to write
            // => translation incl. output can be done in just 1 line of code:
            //BingService.Translate("Hello world", "en", "de").Subscribe(val => Debug.WriteLine(val.GetTranslatedTerm()));


            // get throttled key events
            IObservable<string> translationTexts =
                (from keyup in Observable.FromEventPattern<KeyEventArgs>(txtTranslation, "KeyUp")
                 select txtTranslation.Text)
                .Throttle(TimeSpan.FromSeconds(PressDelay));
            translationTexts.Subscribe(
                _ =>
                {
                    txtEnglish.Text = TranslatingText;
                    txtGerman.Text = TranslatingText;
                    txtSpanish.Text = TranslatingText;
                    txtFrench.Text = TranslatingText;
                    txtItalian.Text = TranslatingText;
                }
                );

            // set up all translation services
            var englishTranslations =
                from text in translationTexts
                from englishResults in BingService.Translate(text, _sourceLanguage, "en")
                                                  .TakeUntil(translationTexts)
                select englishResults;

            var germanTranslations =
                from text in translationTexts
                from germanResults in BingService.Translate(text, _sourceLanguage, "de")
                                                 .TakeUntil(translationTexts)
                select germanResults;

            var spanishTranslations =
                from text in translationTexts
                from spanishResults in BingService.Translate(text, _sourceLanguage, "es")
                                                  .TakeUntil(translationTexts)
                select spanishResults;

            var frenchTranslations =
                from text in translationTexts
                from frenchResults in BingService.Translate(text, _sourceLanguage, "fr")
                                                 .TakeUntil(translationTexts)
                select frenchResults;

            var italianTranslations =
                from text in translationTexts
                from italianResults in BingService.Translate(text, _sourceLanguage, "it")
                                                  .TakeUntil(translationTexts)
                select italianResults;

            // get and evaluate translation results
            englishTranslations.Subscribe(transl => txtEnglish.Text = transl.GetTranslatedTerm(),
                                          exc => txtEnglish.Text = exc.Message);
            germanTranslations.Subscribe(transl => txtGerman.Text = transl.GetTranslatedTerm(),
                                         exc => txtGerman.Text = exc.Message);
            spanishTranslations.Subscribe(transl => txtSpanish.Text = transl.GetTranslatedTerm(),
                                          exc => txtSpanish.Text = exc.Message);
            frenchTranslations.Subscribe(transl => txtFrench.Text = transl.GetTranslatedTerm(),
                                         exc => txtFrench.Text = exc.Message);
            italianTranslations.Subscribe(transl => txtItalian.Text = transl.GetTranslatedTerm(),
                                          exc => txtItalian.Text = exc.Message);
        }

        /// <summary>
        /// Sets up asynchronous translation of a given input term into some languages.
        /// The translation is written to the UI sychronized. That means the translation
        /// is first put out, when ALL translation requests come back.
        /// </summary>
        private void SetUpTranslationServiceWithJoin()
        {
            // get throttled key events
            IObservable<string> translationTexts =
                (from keyup in Observable.FromEvent<KeyEventArgs>(txtTranslation, "KeyUp")
                 select txtTranslation.Text)
                .Throttle(TimeSpan.FromSeconds(PressDelay));
            translationTexts.Subscribe(
                _ =>
                {
                    txtEnglish.Text = TranslatingText;
                    txtGerman.Text = TranslatingText;
                    txtSpanish.Text = TranslatingText;
                    txtFrench.Text = TranslatingText;
                    txtItalian.Text = TranslatingText;
                }
                );

            // set up synchronized translation (returns when all service requests return)
            var translations =
                from text in translationTexts
                let english = BingService.Translate(text, _sourceLanguage, "en")
                let german = BingService.Translate(text, _sourceLanguage, "de")
                let spanish = BingService.Translate(text, _sourceLanguage, "es")
                let french = BingService.Translate(text, _sourceLanguage, "fr")
                let italian = BingService.Translate(text, _sourceLanguage, "it")

                from results in Observable.Join(english.And(german).And(spanish).And(french)
                                                .Then(
                                                    (enTrans, deTrans, esTrans, frTrans)
                                                    => new
                                                    {
                                                        English = enTrans,
                                                        German = deTrans,
                                                        Spanish = esTrans,
                                                        French = frTrans
                                                    }))
                                          .TakeUntil(translationTexts)
                select results;

            // get and evaluate translation results
            translations.Subscribe(
                result =>
                {
                    txtEnglish.Text = result.English.GetTranslatedTerm();
                    txtGerman.Text = result.German.GetTranslatedTerm();
                    txtSpanish.Text = result.Spanish.GetTranslatedTerm();
                    txtFrench.Text = result.French.GetTranslatedTerm();
                },
                exc =>
                {
                    txtEnglish.Text = exc.Message;
                    txtGerman.Text = "";
                    txtSpanish.Text = "";
                    txtFrench.Text = "";
                    txtItalian.Text = "";
                }
            );
        }

        #endregion
    }
}
