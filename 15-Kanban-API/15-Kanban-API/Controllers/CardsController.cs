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
    public class CardsController : ApiController
    {
        private KanbanEntities db = new KanbanEntities();

        // GET: api/Cards
        public IQueryable<Card> GetCards()
        {
            return db.Cards;
        }

        // GET: api/Cards/5
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult GetCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {

                return NotFound();
            }
           //var cardModel = new CardModel();
           //cardModel.CardId = card.CardId;
           //cardModel.ListId = card.ListId;
           //cardModel.Name = card.Name;
           //cardModel.Text = card.Text;
           //cardModel.CreatedDate = card.CreatedDate;

            var x = Mapper.Map<CardModel>(card);            
            

            return Ok(x);
        }

        // PUT: api/Cards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCard(int id, Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.CardId)
            {
                return BadRequest();
            }

            db.Entry(card).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // POST: api/Cards
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult PostCard(CardModel cardModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Instantiate data Database Card object
            var dbCard = new Card();

            //Fill in Database Card object properties with cardModel properties
            //..
            dbCard.Name = cardModel.Name;
            dbCard.ListId = cardModel.ListId;
            dbCard.Text = cardModel.Text;
            dbCard.CreatedDate = cardModel.CreatedDate;
            dbCard.CardId = cardModel.CardId;
            
            //Save to Database
            db.Cards.Add(dbCard);

            //Update Database
            db.SaveChanges();
                     

            return CreatedAtRoute("DefaultApi", new { id = dbCard.CardId }, cardModel);
        }

        // DELETE: api/Cards/5
        [ResponseType(typeof(Card))]
        public IHttpActionResult DeleteCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.Cards.Count(e => e.CardId == id) > 0;
        }
    }
}