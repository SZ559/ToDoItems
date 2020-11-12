using BackendForToDoItem.Entity;
using BackendForToDoItem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BackendForToDoItem.Controllers
{
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoItemController : ControllerBase
    {
        private ITodoItemService _service;

        public ToDoItemController(ITodoItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get the To Do Item by id
        /// </summary>
        /// <param name="id">The id of the To Do Item.</param>
        /// <returns></returns>
        /// <response code="200">Successfully get all To Do Item with id</response>
        /// <response code="404">Cannot find the To Do Item with enetered id.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetAsync([Required] string id)
        {
            //if (string.IsNullOrEmpty(id))
            //    return BadRequest(new Dictionary<string, string>() { { "message", "Id is required in the To Do Item" } });
            var item = await _service.GetAsync(id);
            if (item == null)
                return NotFound(new Dictionary<string, string>() { { "message", $"Can't find {id}" } });
            return Ok(item);
        }

        /// <summary>
        /// Query To Do Items by description and done
        /// </summary>
        /// <param name="description">The descripion of the To Do Item; Can be empty.</param>
        /// <param name="done">The status of the To Do Item. Can be empty.</param>
        /// <returns></returns>
        /// <response code="200">Successfully get all To Do Item with specific description and status</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<List<ToDoItem>>> QueryToDoItem(string description, bool? done)
        {
            var list = await _service.QueryToDoItemAsync(description, done);
            return Ok(list);
        }

        /// <summary>
        /// Delete To Do Item by id
        /// </summary>
        /// <param name="id">Id of To Do Item to be deleted</param>
        /// <returns></returns>
        /// <response code="204">Successfully delete the To Do Item with id.</response>
        /// <response code="404">Cannot find the To Do Item with enetered id.</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([Required] string id)
        {
            /// <response code="400">If id is empty</response>
            //if (string.IsNullOrEmpty(id))
            //    return BadRequest(new Dictionary<string, string>() { { "message", "Id is required" } });
            var itemToBeDeleted = await _service.GetAsync(id);
            if (itemToBeDeleted == null)
                return NotFound(new Dictionary<string, string>() { { "message", $"Can't find {id}" } });

            await _service.DeleteAsync(id);
            return StatusCode(204);
        }


        /// <summary>
        /// If To Do Item already existed, update the To Do Item. Otherwise, create a new To Do Item.
        /// </summary>
        /// <param name="item">To Do Item needed to be created.</param>
        /// <returns></returns>
        /// <response code="200">Successfully upsert the To Do Item.</response>
        /// <response code="400">The Id of the to do item is null or empty.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult<ToDoItem>> UpsertAsync(ToDoItem item)
        {
            if (string.IsNullOrEmpty(item.Id))
                return BadRequest(new Dictionary<string, string>() { { "message", "Id is required in the To Do Item" } });
            await _service.UpsertAsync(item);
            var model = await _service.GetAsync(item.Id);
            if (model == null)
                return new ObjectResult(500) { Value = "Internal Error Occurs" };
            return Ok(model);
        }



        // The original API desgined by teacher in the backend class. But not applicable for the frontend To Do Item project. Just keep it for later use.
        
        ///// <summary>
        ///// Update To Do Item.
        ///// </summary>
        ///// <param name="id">Id of To Do Item to be updated</param>
        ///// <param name="updateModel">The information of Updated To Do Item.</param>
        ///// <returns></returns>
        ///// <response code="200">Successfully update To Do Item.</response>
        ///// <response code="404">Cannot find the To Do Item with entered id.</response>   
        //[ProducesResponseType(StatusCodes.Status200OK)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPatch("{id}")]
        //public async Task<ActionResult<ToDoItem>> UpdateAsync(
        //    [Required] string id,
        //    [Required] ToDoItemUpdateModel updateModel)
        //{
        //    /// <response code="400">Id is empty</response>
        //    //if (string.IsNullOrEmpty(id))
        //    //    return BadRequest(new Dictionary<string, string>() { { "message", "Id is required" } });

        //    var modelInDb = await _service.GetAsync(id);
        //    if (modelInDb == null)
        //        return NotFound(new Dictionary<string, string>() { { "message", $"Can't find {id}" } });

        //    var updated = await _service.UpdateAsync(id, updateModel);
        //    return Ok(updated);
        //}
        /// <summary>
        /// If To Do Item already existed, update the To Do Item. Otherwise, create a new To Do Item.
        /// </summary>
        /// <param name="item">To Do Item needed to be created.</param>
        /// <returns></returns>
        /// <response code="200">Successfully upsert the To Do Item.</response>
        /// <response code="400">The Id of the to do item is null or empty.</response>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpPut]
        //public async Task<ActionResult<ToDoItem>> UpsertAsync(ToDoItem item)
        //{
        //    //if (string.IsNullOrEmpty(item.Id))
        //    //    return BadRequest(new Dictionary<string, string>() { { "message", "Id is required in the To Do List" } });
        //    await _service.UpsertAsync(item);
        //    var model = await _service.GetAsync(item.Id);
        //    if (model == null)
        //        return new ObjectResult(500) { Value = "Internal Error Occurs" };
        //    return Ok(model);
        //}
    }
}
