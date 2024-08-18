using System;
using UnityEngine;

namespace Source
{
    [Serializable]
    public class UniqueEvent
    {
        public EventStruct Event;
        public string Id { get; }

        public UniqueEvent(string type, string data)
        {
            Event.Type = type;
            Event.Data = data;
            Id = Generate();
        }

        private string Generate() => 
            Guid.NewGuid() + Event.Type + Event.Data;
    }
}