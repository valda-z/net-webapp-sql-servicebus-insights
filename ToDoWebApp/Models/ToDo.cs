using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoWebApp.Models
{
    [Table("ToDo")]
    public class ToDo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid GId { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime Created { get; set; }
    }
}