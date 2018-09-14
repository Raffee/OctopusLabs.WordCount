using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Services;
using OctopusLabs.WordCounter.Web.ViewModels;

namespace OctopusLabs.WordCounter.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly WordCounterService _wordCounterService;
        private readonly TextSplitterService _textSplitterService;

        private HomeController()
        {
        }

        public HomeController(IWordCountRepository wordCountRepository, ILoggerFactory loggerFactory)
        {
            var webPageReaderService = new WebPageReaderService();
            _textSplitterService = new TextSplitterService(webPageReaderService);
            _wordCounterService = new WordCounterService(wordCountRepository, loggerFactory);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string urlToSearch)
        {
            try
            {
                var wordCounts = await _textSplitterService.GetWordsFromUrlAsync(urlToSearch, 100);
                _wordCounterService.StoreWordCounts(wordCounts, 100);

                var viewModel = new WordCounterViewModel
                {
                    Url = urlToSearch,
                    CountedWords = wordCounts,
                    CountedWordsJson = Json(wordCounts)
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ViewData["ErrorStack"] = e.StackTrace;
                ViewData["ErrorInner"] = e.InnerException?.Message;
                return View("Error");
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
