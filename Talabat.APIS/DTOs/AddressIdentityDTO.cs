﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOs
{
    public class AddressIdentityDTO
    {

        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
