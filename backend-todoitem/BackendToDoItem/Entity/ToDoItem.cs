using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendForToDoItem.Entity
{
    public class ToDoItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("createdTime")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        [BsonElement("done")]
        public bool Done { get; set; }
        [BsonElement("favorite")]
        public bool Favorite { get; set; }
        [BsonElement("children")]
        public List<ToDoItem> Children { get; set; }
    }
}
