using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightApplication1.BingTranslatorService
{
    public partial class LanguageServiceClient
    {
        //public IAsyncResult BeginTranslate(object values, AsyncCallback callback, object state)
        //{
        //    var input = values as ServiceParams;
        //    return ((SilverlightApplication1.BingTranslatorService.LanguageService)(this)).BeginTranslate(input.AppID, input.Text, input.sourceLanguage, input.targetLanguage, callback, "");
        //}
        //public void TranslateAsync(object values)
        //{
        //    var input = values as ServiceParams;
        //    this.TranslateAsync(input.AppID, input.Text, input.sourceLanguage, input.targetLanguage);
        //}

    }
    // AppID, text, sourceLanguage, targetLanguage
    internal class ServiceParams
    {
        public string AppID { get; set; }
        public string Text { get; set; }
        public string sourceLanguage { get; set; }
        public string targetLanguage { get; set; }
    }

}
