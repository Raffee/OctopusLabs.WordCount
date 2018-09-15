using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Services;
using OctopusLabs.WordCounter.Web.ViewModels;

namespace OctopusLabs.WordCounter.Web.Api
{
    [Produces("application/json")]
    [Route("api/WebWordCounter")]
    public class WebWordCounterApiController : Controller
    {
        private readonly WordCounterService _wordCounterService;
        private readonly TextSplitterService _textSplitterService;

        private WebWordCounterApiController()
        {
            
        }

        public WebWordCounterApiController(IWordCountRepository wordCountRepository, ILoggerFactory loggerFactory)
        {
            var webPageReaderService = new WebPageReaderService();
            _textSplitterService = new TextSplitterService(webPageReaderService);
            _wordCounterService = new WordCounterService(wordCountRepository, loggerFactory);
        }

        [HttpGet]
        public async Task<JsonResult> GetTextFromUrlAsync(string urlToSearch)
        {
            var wordCounts = await _textSplitterService.GetWordsFromUrlAsync(urlToSearch, 100);
            _wordCounterService.StoreWordCounts(wordCounts, 100);
            
            return new JsonResult(wordCounts);
        }

        [HttpDelete]
        public void DeleteAllWords()
        {
            _wordCounterService.DeleteAllCountedWords();
        }
    }
}