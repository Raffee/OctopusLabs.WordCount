using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OctopusLabs.WordCounter.Web.ViewModels
{
    public class WordCounterViewModel
    {
        public string Url { get; set; }
        public List<KeyValuePair<string, int>> CountedWords { get; set; }
        public JsonResult CountedWordsJson { get; set; }
    }
}
