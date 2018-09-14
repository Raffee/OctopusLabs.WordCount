using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLabs.WordCounter.Services
{
    public class WebPageReaderService : ITextReaderService
    {
        public async Task<string> GetTextFromSource(string url)
        {
            if(!IsValidUrl(url))
                throw new ArgumentException("Provided string is not a valid URL");

            var client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new HttpRequestException("Unable to access provided url");
            }
            using (var content = response.Content)
            {
                var resultHtml = await content.ReadAsStringAsync();
                var resultTextOnly = ExtractTextWithSautinSoft(resultHtml);

                return resultTextOnly;
            }
        }
        private static string ExtractText(string html)
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

        private static string ExtractTextWithSautinSoft(string htmlString)
        {
            var h = new SautinSoft.HtmlToRtf();
            if (!h.OpenHtml(htmlString)) return string.Empty;
            var rtfString = h.ToText();
            return rtfString;
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri) && null != uri;
        }
    }
}
