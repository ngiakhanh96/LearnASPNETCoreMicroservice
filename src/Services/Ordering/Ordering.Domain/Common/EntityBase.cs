﻿using System;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase<TId>
    {
        public TId Id { get; protected set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
