using System;
using System.Collections.Generic;
using System.Text;

namespace MSDev.Work.Models
{
    class BingTranslateResponse
    {
        public string ResultSMT { get; set; }
        public string TargetLanguage { get; set; }
        public string ResultNMT { get; set; }
        public string SourceLanguage { get; set; }
        public string Text { get; set; }

    }
}
