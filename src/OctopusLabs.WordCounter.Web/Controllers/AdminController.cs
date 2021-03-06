﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Services;

namespace OctopusLabs.WordCounter.Web.Controllers
{
    public class AdminController : Controller
    {
        private WordCounterService _service;

        private AdminController()
        {
            
        }

        public AdminController(IWordCountRepository repository, ILoggerFactory loggerFactory)
        {
            _service = new WordCounterService(repository, loggerFactory);
        }

        // GET: test
        public ActionResult Index()
        {
            try
            {
                var countedWords = _service.GetAllCountedWords();
                return View(countedWords);
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ViewData["ErrorStack"] = e.StackTrace;
                ViewData["ErrorInner"] = e.InnerException?.Message;
                return View("Error");

            }
        }

        public ActionResult Delete()
        {
            try
            {
                _service.DeleteAllCountedWords();
                return View("Index");
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ViewData["ErrorStack"] = e.StackTrace;
                ViewData["ErrorInner"] = e.InnerException?.Message;
                return View("Error");

            }
        }
    }
}