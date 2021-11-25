using Domain.Interfaces;
using System;

namespace Domain.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }

        protected Entity()
        {
            Id = new Random().Next();
        }
    }
}