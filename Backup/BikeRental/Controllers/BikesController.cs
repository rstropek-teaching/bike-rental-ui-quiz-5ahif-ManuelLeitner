using BikeRental.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers {
    [Produces("application/json")]
    [Route("api/Bikes")]
    public class BikesController : Controller {
        private readonly BikeContext _context;

        public BikesController(BikeContext context) {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Bike> GetBikes([FromQuery]bool mustBeAvailable, [FromQuery]string property) {
            var bikes = _context.Bikes.Where(b => !b.Rentals.Any(r => r.Open) || !mustBeAvailable);
            if (!string.IsNullOrWhiteSpace(property))
                bikes = bikes.OrderBy(property);
            return bikes;
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBike([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var bike = await _context.Bikes.SingleOrDefaultAsync(m => m.Id == id);

            if (bike == null) {
                return NotFound();
            }

            return Ok(bike);
        }

        // PUT: api/Bikes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike([FromRoute] int id, [FromBody] Bike bike) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != bike.Id) {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!BikeExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bikes
        [HttpPost]
        public async Task<IActionResult> PostBike([FromBody] Bike bike) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var bike = await _context.Bikes.SingleOrDefaultAsync(m => m.Id == id);
            if (bike == null) {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return Ok(bike);
        }

        private bool BikeExists(int id) {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}