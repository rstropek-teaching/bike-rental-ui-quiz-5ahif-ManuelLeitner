using BikeRental.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers {
    [Produces("application/json")]
    [Route("api/Rentals")]
    public class RentalsController : Controller {
        private readonly BikeContext context;

        public RentalsController(BikeContext context) {
            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetRentalsAsync([FromQuery]bool onlyOpen, [FromQuery]int? customerId) {
            Customer customer = null;
            if (customerId != null) {
                customer = await context.Customers.FindAsync(customerId);
                if (customer == null)
                    return NotFound();
            }

            return Ok(context.Rentals.Include(r => r.Customer)
                .Where(r => (r.Customer == customer || customerId == null) && !(onlyOpen && r.Paid)));
        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var rental = await context.Rentals.SingleOrDefaultAsync(m => m.Id == id);

            if (rental == null) {
                return NotFound();
            }

            return Ok(rental);
        }

        // PUT: api/Rentals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental([FromRoute] int id, [FromBody] Rental rental) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != rental.Id) {
                return BadRequest();
            }

            context.Entry(rental).State = EntityState.Modified;

            try {
                await context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RentalExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Rentals
        [HttpPost]
        public async Task<IActionResult> PostRental([FromBody] Rental rental) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            context.Rentals.Add(rental);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.Id }, rental);
        }

        // DELETE: api/Rentals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var rental = await context.Rentals.SingleOrDefaultAsync(m => m.Id == id);
            if (rental == null) {
                return NotFound();
            }

            context.Rentals.Remove(rental);
            await context.SaveChangesAsync();

            return Ok(rental);
        }

        private bool RentalExists(int id) {
            return context.Rentals.Any(e => e.Id == id);
        }

        [HttpGet("Start")]
        public async Task<IActionResult> StartAsync(int id) => await DoAsync(id, r => r.Start());
        [HttpGet("End")]
        public async Task<IActionResult> EndAsync(int id) => await DoAsync(id, r => r.Finish());
        [HttpGet("Pay")]
        public async Task<IActionResult> PayAsync(int id) => await DoAsync(id, r => r.Pay());

        private async Task<IActionResult> DoAsync(int id, Action<Rental> p) {
            var rental = await context.Rentals.FindAsync(id);
            if (rental == null) {
                return NotFound();
            }
            try {
                rental.Start();
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}