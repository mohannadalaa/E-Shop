using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBackEnd.Models;

namespace TaskBackEnd.Controllers
{
    [MobileAppController]
    public class UsersController : ApiController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!db.Users.Any(u=>u.Name == user.Name))
            {
                db.Users.Add(new Models.User() { Name = user.Name , Id = Guid.NewGuid().ToString() });
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var returned = db.Users.FirstOrDefault(u => u.Name == user.Name);
            return Ok(returned);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUserByName([FromUri]string userName)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Name == userName);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}