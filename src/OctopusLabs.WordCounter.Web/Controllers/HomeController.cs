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
        private readonly TextExtractionService _textExtractionService;

        private HomeController()
        {
        }

        public HomeController(IWordCountRepository wordCountRepository, ILoggerFactory loggerFactory)
        {
            _wordCounterService = new WordCounterService(wordCountRepository, loggerFactory);
            _textExtractionService = new TextExtractionService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string urlToSearch)
        {
            var wordCounts = await _textExtractionService.GetWordsFromUrlAsync(urlToSearch);
            _wordCounterService.StoreWordCounts(wordCounts, 100);

            var viewModel = new WordCounterViewModel
            {
                Url = urlToSearch,
                CountedWords = wordCounts,
                CountedWordsJson = Json(wordCounts)
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
