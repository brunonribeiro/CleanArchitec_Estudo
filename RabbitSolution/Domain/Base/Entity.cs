using Domain.Interfaces;
using System;

namespace Domain.Base
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}