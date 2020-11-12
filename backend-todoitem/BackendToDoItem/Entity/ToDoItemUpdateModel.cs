using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendForToDoItem.Entity
{

    /// <summary>
    /// Partial Update Model
    /// </summary>
    public class ToDoItemUpdateModel
     {
        /// <summary>
        /// If not empty, database will update the value
        /// </summary>
        [BsonElement("description")]
         public string Description { get; set; }
         [BsonElement("done")]
         public bool? Done { get; set; }
         [BsonElement("favorite")]
         public bool? Favorite { get; set; }
     }
}
