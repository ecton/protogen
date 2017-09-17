using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompoundKey.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("msrp")]
        public double Msrp { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}
