﻿using System.Collections.Generic;

namespace OctopusLabs.WordCounter.Core.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}