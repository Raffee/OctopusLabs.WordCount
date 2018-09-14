using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OctopusLabs.WordCounter.Core.DataTransferObjects;
using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Infrastructure.Data
{
    public class WordCountRepository : IWordCountRepository
    {
        private readonly AppDbContext _dbContext;

        public WordCountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public WordCount GetById(byte[] id)
        {
            return _dbContext.Set<WordCount>().SingleOrDefault(e => e.Id.Equals(id));
        }

        public List<WordCountDto> List()
        {
            var wordCounts = _dbContext.Set<WordCount>().ToList();
            var rsa = CryptographicFunctions.GetKeyFromContainer(WordCounterConstants.KeyContainerName);

            return wordCounts.Select(wordCount => new WordCountDto
                {
                    Word = CryptographicFunctions.Decrypt(rsa.ToXMLString(true), wordCount.EncryptedWord),
                    Count = wordCount.Count
                })
                .ToList();
        }

        public WordCount Add(WordCount entity)
        {
            _dbContext.Set<WordCount>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(WordCount entity)
        {
            _dbContext.Set<WordCount>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(WordCount entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}