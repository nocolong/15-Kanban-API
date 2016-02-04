using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using _15_Kanban_API;
using _15_Kanban_API.Models;
using AutoMapper;

namespace _15_Kanban_API.Controllers
{
    public class ListsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/Lists
        public IEnumerable<ListModel> GetLists()
        {
            return Mapper.Map<IEnumerable<ListModel>>(db.Lists);
        }
        [Route("api/lists/{listId}/cards")]
        public IEnumerable<CardModel> GetCardsForList(int listId)
        {
            var cards = db.Cards.Where(c => c.ListId == listId);

            return Mapper.Map<IEnumerable<CardModel>>(cards);
        }

        // GET: api/Lists/5
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult GetList(int id)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ListModel>(list));
        }

        // PUT: api/Lists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutList(int id, ListModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.ListId)
            {
                return BadRequest();
            }

            
            var dbList = db.Lists.Find(id);

            dbList.Update(list);
            db.Entry(dbList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Lists
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult PostList(ListModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbList = new List(list);

            list.CreatedDate = dbList.CreatedDate;
            list.ListId = dbList.ListId;

            db.Lists.Add(dbList);
            db.SaveChanges();


            return CreatedAtRoute("DefaultApi", new { id = dbList.ListId }, list);
        }

        // DELETE: api/Lists/5
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult DeleteList(int id)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            db.Lists.Remove(list);
            db.SaveChanges();

            return Ok(Mapper.Map<ListModel>(list));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListExists(int id)
        {
            return db.Lists.Count(e => e.ListId == id) > 0;
        }
    }
}