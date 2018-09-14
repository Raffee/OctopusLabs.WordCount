using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OctopusLabs.WordCounter.Core.DataTransferObjects;
using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Services
{
    public class WordCounterService
    {
        private readonly ILogger _logger;
        private IWordCountRepository _wordCountRepository;

        private WordCounterService()
        {
            
        }

        public WordCounterService(IWordCountRepository wordCountRepository, ILoggerFactory loggerFactory)
        {
            _wordCountRepository = wordCountRepository;
            _logger = loggerFactory.CreateLogger("WordCounterService");
        }

        public void StoreWordCounts(List<KeyValuePair<string, int>> countedWords, int numberOfItemsToStore)
        {
            if (countedWords == null || !countedWords.Any())
            {
                return;
            }

            // Create an instance of the RSA algorithm class  
            var rsa = CryptographicFunctions.GetKeyFromContainer(WordCounterConstants.KeyContainerName);
            // Get the public keyy   
            var publicKey = rsa.ToXMLString(false); // false to get the public key    
            var someSalt = "somesalt";

            if (numberOfItemsToStore > countedWords.Count)
            {
                numberOfItemsToStore = countedWords.Count;
            }

            for (var i = 0; i < numberOfItemsToStore; i++)
            {
                var countedWord = countedWords[i];
                var keyAsBytes = Encoding.UTF32.GetBytes(countedWord.Key);
                var saltAsBytes = Encoding.UTF32.GetBytes(someSalt);
                var hashedWord = CryptographicFunctions.GenerateSaltedHash(keyAsBytes, saltAsBytes);
                var encryptedWord = CryptographicFunctions.Encrypt(publicKey, countedWord.Key);

                var wordCount = new WordCount
                {
                    Id = hashedWord,
                    EncryptedWord = encryptedWord,
                    Count = countedWord.Value
                };

                try
                {
                    AddOrUpdateWordCount(wordCount);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, null);
                }
            }
        }

        private void AddOrUpdateWordCount(WordCount wordCount)
        {
            var existingWordCount = _wordCountRepository.GetById(wordCount.Id);
            if (existingWordCount == null)
            {
                _wordCountRepository.Add(wordCount);
            }
            else
            {
                existingWordCount.Count += wordCount.Count;
                _wordCountRepository.Update(existingWordCount);
            }
        }

        public List<WordCountDto> GetAllCountedWords()
        {
            var wordCounts = _wordCountRepository.List();

            wordCounts.Sort
            (
                (kvp1, kvp2) => Comparer<int>.Default.Compare(kvp1.Count, kvp2.Count)
            );
            wordCounts.Reverse();

            return wordCounts;
        }
    }
}
