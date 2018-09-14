using System.Collections.Generic;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Core.Entities
{
    public class WordCount : BaseEntity<byte[]>
    {
        public WordCount()
        {
        }
        
        public byte[] EncryptedWord { get; set; }

        public int Count { get; set; }
    }
}