using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PedidosAPI.Models
{
    public class PedidoDetalle
    {
        [Key]
        public int ID { get; set; }
        public int Pedido { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal? PrecioUnitario {get; set;} 
        public decimal? PrecioTotal { get; set; }
    }
}
