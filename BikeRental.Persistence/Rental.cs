using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRental.Persistence {
    public class Rental {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }
        [Required]
        public Bike Bike { get; set; }
        [Required]
        public DateTime Begin { get; set; }

        public DateTime? End { get; set; }
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal? Cost { get; set; }

        public bool Paid { get; set; }

        public decimal? CalculatePrice() {
            var duration = End - Begin;
            if (duration <= TimeSpan.FromMinutes(15)) {
                return (Cost = null);
            } else {
                return (Cost = Bike.PriceFirstHour + Bike.PriceFollowingHours * Math.Ceiling((decimal)(duration - TimeSpan.FromHours(1))?.TotalHours));
            }
        }
        [NotMapped]
        public bool Open => End == null || (End > DateTime.Now && Begin < DateTime.Now);

        public void Start() {
            if (Begin != default(DateTime)) {
                throw new InvalidOperationException("The rental has already begun.");
            }
            Begin = DateTime.Now;
        }
        public void Finish() {
            if (Begin == null || Begin > DateTime.Now) {
                throw new InvalidOperationException("The rental has not begun yet.");
            }
            if (!Open) {
                throw new InvalidOperationException("The rental has already ended.");
            }
            End = DateTime.Now;
            if (CalculatePrice() <= 0) {
                Paid = true;
            }
        }

        public void Pay() {
            if (Cost <= 0) {
                throw new InvalidOperationException("Nothing needs to be paid.");
            }
            Paid = true;
        }
    }
}