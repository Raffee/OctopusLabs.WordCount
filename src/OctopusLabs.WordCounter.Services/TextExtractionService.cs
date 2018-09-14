using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OctopusLabs.WordCounter.Services
{
    public class TextExtractionService
    {
        public async Task<string> GetTextFromUrlAsync(string url)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                var resultHtml = await content.ReadAsStringAsync();
                var resultTextOnly = ExtractText(resultHtml);
                var text2 = ExtractText2(resultHtml);

                return resultTextOnly;
            }
        }

        public async Task<List<KeyValuePair<string, int>>> GetWordsFromUrlAsync(string url)
        {
            var textFromUrl = await GetTextFromUrlAsync(url);
            var arrayOfWords = ConvertStringToArray(textFromUrl);
            var wordCounts = GetWordCountCollection(arrayOfWords);

            return wordCounts;
        }
        public static string ExtractText(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();
            foreach (var item in doc.DocumentNode.DescendantsAndSelf())
            {
                if (item.NodeType != HtmlNodeType.Text || string.IsNullOrWhiteSpace(item.InnerText.Trim())) continue;

                var innerText = item.InnerText.Trim();
                innerText = innerText.Replace("&nbsp;", "");
                innerText = innerText.Replace("&amp;", "&");
                chunks.Add(innerText);
            }
            return string.Join(" ", chunks);
        }

        public static string ExtractText2(string htmlString)
        {
            var h = new SautinSoft.HtmlToRtf();
            if (!h.OpenHtml(htmlString)) return string.Empty;
            var rtfString = h.ToText();
            return rtfString;
        }

        public static string[] ConvertStringToArray(string text)
        {
            return text.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?', '(', ')', '\'', '\"', '{', '}' });
        }

        public static List<KeyValuePair<string, int>> GetWordCountCollection(string[] arrayOfWords)
        {
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

            var summaryList = new List<KeyValuePair<string, int>>();
            summaryList.AddRange(wordCounts);
            summaryList.Sort
            (
                (kvp1, kvp2) => Comparer<int>.Default.Compare(kvp1.Value, kvp2.Value)
            );

            summaryList.Reverse();
            return summaryList;
        }

        private static bool IsValidWord(string word)
        {
            return !string.IsNullOrWhiteSpace(word);
        }
    }
}
