using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.Persistence {
    public class Bike {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfLastService { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PriceFirstHour { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PriceFollowingHours { get; set; }
        [Required]
        public Category Category { get; set; }
        public IEnumerable<Rental> Rentals { get; set; }
    }

    public class Category {

        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}