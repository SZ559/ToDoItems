using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendForToDoItem.Entity;
using BackendForToDoItem.Config;

namespace BackendForToDoItem.Service
{
    public class ToDoItemService : ITodoItemService
    {
        private MongoClient _client;
        private readonly IMongoCollection<ToDoItem> _collection;

        public ToDoItemService(MongoDbConfig config)
        {
            _client = new MongoClient(config.ConnectionString);
            _collection = _client.GetDatabase(config.Database).GetCollection<ToDoItem>(config.Collection);
        }

        public async Task<List<ToDoItem>> GetAllToDoItemsAsync()
        {
            return await _collection.Find(r => true).ToListAsync();
        }

        public async Task<ToDoItem> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ToDoItem>> QueryToDoItemAsync(string description, bool? done)
        {
            if (String.IsNullOrEmpty(description) && done == null)
            {
                return await GetAllToDoItemsAsync();
            }

            if (String.IsNullOrEmpty(description))
            {
                return await _collection.Find<ToDoItem>(r => r.Done == done).ToListAsync();
            }

            if (done == null)
            {
                return await _collection.Find<ToDoItem>(r => r.Description == description).ToListAsync();
            }

            return await _collection.Find<ToDoItem>(r => r.Description == description && r.Done == done).ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(r => r.Id == id);
        }

        public async Task UpsertAsync(ToDoItem item)
        {
            var opt = new ReplaceOptions { IsUpsert = true };
            await _collection.ReplaceOneAsync(t => t.Id.Equals(item.Id), item, opt);
        }

        //public async Task<ToDoItem> UpdateAsync(string id, ToDoItemUpdateModel updatedModel)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return null;
        //    }
        //    var itemToBeUpdated = await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();

        //    if (itemToBeUpdated == null)
        //    {
        //        return null;
        //    }

        //    if (!string.IsNullOrEmpty(updatedModel.Description))
        //    {
        //        itemToBeUpdated.Description = updatedModel.Description;
        //    }

        //    if (updatedModel.Favorite != null)
        //    {
        //        itemToBeUpdated.Favorite = updatedModel.Favorite.Value;
        //    }

        //    if (updatedModel.Done != null)
        //    {
        //        itemToBeUpdated.Done = updatedModel.Done.Value;
        //    }
        //    _collection.FindOneAndReplace(r => r.Id == itemToBeUpdated.Id, itemToBeUpdated);

        //    return itemToBeUpdated;
        //}
    }

    public interface ITodoItemService
    {
        Task<List<ToDoItem>> GetAllToDoItemsAsync();
        Task<ToDoItem> GetAsync(string id);
        Task DeleteAsync(string id);
        Task UpsertAsync(ToDoItem item);
        Task<List<ToDoItem>> QueryToDoItemAsync(string description, bool? done);
        //Task<ToDoItem> UpdateAsync(string id, ToDoItemUpdateModel updatedModel);
    }
}
