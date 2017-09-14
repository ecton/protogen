using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace SimpleExample.Models
{
    [Table("todos")]
    public class Todo
    {
        [Key]
        [Required]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("complete")]
        public bool Complete { get; set; }
        [Column("parent_id")]
        public long? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Todo Parent { get; set; }
        [Required]
        [Column("task")]
        public string Task { get; set; }
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<Todo> Children { get; set; }
    }
}
