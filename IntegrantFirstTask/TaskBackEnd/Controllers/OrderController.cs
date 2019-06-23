using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TaskBackEnd.DataObjects;
using TaskBackEnd.Models;
using User = TaskBackEnd.Models.User;

namespace TaskBackEnd.Controllers
{
    public class OrderController : TableController<Order>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Order>(context, Request);
        }

        // GET tables/Order
        public IQueryable<Order> GetAllOrder()
        {
            return Query(); 
        }

        // GET tables/Order/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Order> GetOrder(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Order/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Order> PatchOrder(string id, Delta<Order> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Order
        public async Task<IHttpActionResult> PostOrder(OrderDTO item)
        {
            try
            {
                MobileServiceContext context = new MobileServiceContext();
                List<OrderItems> OI = new List<OrderItems>();
                //User InsertedUser = await InsertAsync<User>()
                for (int i = 0; i < item.OrderItems.Count; i++)
                {
                    OI.Add(new OrderItems());
                    OI[i].ItemID = item.OrderItems[i].ItemID;
                    OI[i].ItemCount = (int)item.OrderItems[i].ItemCount;
                    OI[i].Id = Guid.NewGuid().ToString();
                }

                Order order = new Order()
                {
                    User = context.Users.FirstOrDefault(u => u.Name == item.User.Name),
                    OrderItems = OI,
                    Id = Guid.NewGuid().ToString(),
                    SubmittedOnline = true
                };

                context.Orders.Add(order);
                context.SaveChanges();
                return Ok();


                //Order current = await InsertAsync(order);
                //return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }

        // DELETE tables/Order/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteOrder(string id)
        {
             return DeleteAsync(id);
        }
    }
}
