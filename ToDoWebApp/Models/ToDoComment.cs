using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoWebApp.Models
{
    public class ToDoComment
    {
        public Guid Id { get; set; }
        public Guid ToDoGId { get; set; }
        public DateTime Created { get; set; }
        public String Comment { get; set; }
    }
}