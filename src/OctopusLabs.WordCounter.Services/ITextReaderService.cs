using System.Threading.Tasks;

namespace OctopusLabs.WordCounter.Services
{
    public interface ITextReaderService
    {
        Task<string> GetTextFromSource(string url);
    }
}