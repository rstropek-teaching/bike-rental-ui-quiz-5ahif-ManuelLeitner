using BikeRental.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers {
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller {
        private readonly BikeContext context;

        public CustomersController(BikeContext context) {
            this.context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers(string lastName = null) {
            if (!string.IsNullOrWhiteSpace(lastName)) {
                return context.Customers.Where(c => c.LastName.Contains(lastName));
            }
            return context.Customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var customer = await context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            if (customer == null) {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != customer.Id) {
                return BadRequest();
            }

            context.Entry(customer).State = EntityState.Modified;

            try {
                await context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!CustomerExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var customer = await context.Customers.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null) {
                return NotFound();
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id) {
            return context.Customers.Any(e => e.Id == id);
        }
    }
}