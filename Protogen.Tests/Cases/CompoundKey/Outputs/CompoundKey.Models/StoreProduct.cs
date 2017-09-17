using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompoundKey.Models
{
    [Table("store_products")]
    public class StoreProduct
    {
        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        [Required]
        [Column("store_id")]
        public long StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        public virtual Store Store { get; set; }
        [Required]
        [Column("price")]
        public double Price { get; set; }
    }
}
