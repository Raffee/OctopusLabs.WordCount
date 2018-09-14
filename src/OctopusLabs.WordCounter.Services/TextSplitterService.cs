using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OctopusLabs.WordCounter.Services
{
    public class TextSplitterService
    {
        private readonly ITextReaderService _textReader;

        private TextSplitterService()
        {
        }

        public TextSplitterService(ITextReaderService textReaderService)
        {
            _textReader = textReaderService;
        }

        public async Task<List<KeyValuePair<string, int>>> GetWordsFromUrlAsync(string url, int numberOfItemsToGet)
        {
            var textFromUrl = await _textReader.GetTextFromSource(url);
            var arrayOfWords = ConvertStringToArray(textFromUrl);
            var wordCounts = GetWordCountCollection(arrayOfWords, numberOfItemsToGet);

            return wordCounts;
        }

        public static string[] ConvertStringToArray(string text)
        {
            return text.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?', '(', ')', '\'', '\"', '{', '}' });
        }

        public static List<KeyValuePair<string, int>> GetWordCountCollection(string[] arrayOfWords, int numberOfItemsToGet)
        {
            var orderedCountedWords = new List<KeyValuePair<string, int>>();
            if (arrayOfWords == null || arrayOfWords.Length == 0)
                return orderedCountedWords;

            if (numberOfItemsToGet > arrayOfWords.Length)
            {
                numberOfItemsToGet = arrayOfWords.Length;
            }

            var wordCounts = new Dictionary<string, int>();
            foreach (var word in arrayOfWords)
            {
                if (!IsValidWord(word)) continue;

                var loweredWord = word.ToLower();
                if (wordCounts.ContainsKey(loweredWord))
                {
                    var currentCount = (int)(wordCounts[loweredWord]);
                    wordCounts[loweredWord] = ++currentCount;
                }
                else
                {
                    wordCounts.Add(loweredWord, 1);
                }
            }

            orderedCountedWords.AddRange(wordCounts);
            orderedCountedWords.Sort
            (
                (kvp1, kvp2) => Comparer<int>.Default.Compare(kvp1.Value, kvp2.Value)
            );

            orderedCountedWords.Reverse();
            return orderedCountedWords.Take(numberOfItemsToGet).ToList();
        }

        private static bool IsValidWord(string word)
        {
            return !string.IsNullOrWhiteSpace(word);
        }
    }
}
