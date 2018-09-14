﻿using System.Collections.Generic;
using OctopusLabs.WordCounter.Core.DataTransferObjects;
using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Core.Interfaces
{
    public interface IWordCountRepository
    {
        WordCount GetById(byte[] id);
        List<WordCountDto> List();
        WordCount Add(WordCount entity);
        void Update(WordCount entity);
        void Delete(WordCount entity);
    }
}