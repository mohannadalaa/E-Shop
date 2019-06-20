using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TaskBackEnd.Models;

namespace TaskBackEnd.Controllers
{
    public class OrderItemsController : TableController<OrderItems>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<OrderItems>(context, Request);
        }

        // GET tables/OrderItems
        public IQueryable<OrderItems> GetAllOrderItems()
        {
            return Query(); 
        }

        // GET tables/OrderItems/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<OrderItems> GetOrderItems(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/OrderItems/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<OrderItems> PatchOrderItems(string id, Delta<OrderItems> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/OrderItems
        public async Task<IHttpActionResult> PostOrderItems(OrderItems item)
        {
            OrderItems current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/OrderItems/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteOrderItems(string id)
        {
             return DeleteAsync(id);
        }
    }
}
