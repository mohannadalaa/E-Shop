using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TaskBackEnd.DataObjects;
using TaskBackEnd.Models;

namespace TaskBackEnd.Controllers
{
    public class UserController : TableController<Models.User>
    {
        MobileServiceContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Models.User>(context, Request);
        }

        // GET tables/User
        public IQueryable<Models.User> GetAllUser()
        {
            return Query(); 
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Models.User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Models.User> PatchUser(string id, Delta<Models.User> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostUser(Models.User item)
        {
            if(!context.Users.Any(u=>u.Name == item.Name))
            {
                Models.User current = await base.InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }   
            else
            return NotFound();
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
