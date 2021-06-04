using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PedidosAPI.Models
{
    public class Pedido
    {
        [Key]
        public int ID { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaPedido { get; set; }
        public decimal? MontoPedido { get; set; }
    }
}
