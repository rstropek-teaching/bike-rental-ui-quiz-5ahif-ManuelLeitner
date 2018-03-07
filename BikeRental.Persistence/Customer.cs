using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRental.Persistence {
    public class Customer {

        public int Id { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(75)]
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(75)]
        public string Street { get; set; }
        [Required]
        [MaxLength(75)]
        public string Town { get; set; }
        [Required]
        [MaxLength(10)]
        public string HouseNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }
    }

    public enum Gender {
        Male = 'm', Female = 'f', Unknown = '-'
    }
}