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

        //Define the characters that should bemarked out and used as word separators.
        //There's probably a better way of doing this.
        private readonly char[] wordSeparators = new char[]
        {
            ' ', ',', '.', ';', ':', '!', '?', '(', ')', '\'', '\"', '{', '©',
            '}', '&', '+', '*', '/', '\\', '`', '~', '@', '#', '$', '%', '^', '[', ']', '<', '>', '|'
        };

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

        public string[] ConvertStringToArray(string text)
        {
            text = text.Replace("\r\n", " ");
            text = text.Replace(" - ", " ");
            text = text.Replace(" _ ", " ");
            text = text.Replace("&gt;", " ");
            text = text.Replace("&lt;", " ");
            text = text.Replace("&amp;", " ");
            text = text.Replace("&nbsp;", " ");
            text = text.Replace("&copy;", " ");
            return text.Split(wordSeparators);
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

                var loweredWord = word.Trim().ToLower();
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
