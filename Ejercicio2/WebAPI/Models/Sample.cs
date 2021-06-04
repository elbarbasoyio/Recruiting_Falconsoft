using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Sample
    {
        [Key]
        public string ID { get; set; }
        public string Content { get; set; }
        public int Qty { get; set; }
    }
}
