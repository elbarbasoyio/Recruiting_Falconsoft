using PedidosAPI.Controllers;
using PedidosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PedidosAPI.Test.UnitTest
{
    [TestClass]
    public class FilterControllerTest : TestBase
    {
        [TestMethod]
        public async Task BusquedaOKEnPedidos()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "Juan", FechaPedido = DateTime.Now, MontoPedido = 111 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "Pedro", FechaPedido = DateTime.Now, MontoPedido = 333 });
            await contexto.SaveChangesAsync();
            var contexto2 = ConstruirContext(nombreDB);

            var busqueda = "Juan";
            var controller = new FilterController(contexto2); 
            var resultado = await controller.FiltroBusquedaPedido(busqueda);
            Assert.AreEqual(1, resultado.TotalRegistros);
        }

        [TestMethod]
        public async Task BusquedaOKEnDetalles()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 100, PrecioTotal = 1000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto2", Cantidad = 20, PrecioUnitario = 100, PrecioTotal = 2000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto3", Cantidad = 30, PrecioUnitario = 100, PrecioTotal = 3000 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var busqueda = "Producto1";
            var controller = new FilterController(contexto2);
            var resultado = await controller.FiltroBusquedaDetalle(busqueda);
            Assert.AreEqual(1, resultado.TotalRegistros);
        }

        [TestMethod]
        public async Task BusquedaInexistenteEnPedidos()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba1", FechaPedido = DateTime.Now, MontoPedido = 111 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba2", FechaPedido = DateTime.Now, MontoPedido = 333 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var busqueda = "API";
            var controller = new FilterController(contexto2);
            var resultado = await controller.FiltroBusquedaPedido(busqueda);
            Assert.AreEqual(0, resultado.TotalRegistros);
        }

        [TestMethod]
        public async Task BusquedaInexisteEnDetalles()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 100, PrecioTotal = 1000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto2", Cantidad = 20, PrecioUnitario = 100, PrecioTotal = 2000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto3", Cantidad = 30, PrecioUnitario = 100, PrecioTotal = 3000 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var busqueda = "Gaseosa";
            var controller = new FilterController(contexto2);
            var resultado = await controller.FiltroBusquedaDetalle(busqueda);
            Assert.AreEqual(0, resultado.TotalRegistros);
        }

        [TestMethod]
        public async Task OrdenarPedidosOK()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba1", FechaPedido = DateTime.Now, MontoPedido = 111 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba2", FechaPedido = DateTime.Now, MontoPedido = 333 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba3", FechaPedido = DateTime.Now, MontoPedido = 9000 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba4", FechaPedido = DateTime.Now, MontoPedido = 54 });
            await contexto.SaveChangesAsync();
            var contexto2 = ConstruirContext(nombreDB);

            var col = "MontoPedido";
            var orden = "ASC";
            var controller = new FilterController(contexto2);
            var respuesta = await controller.FiltroOrdenarPedido(col, orden);
            List<Pedido> resultado = respuesta.Resultado.ToList();
            Assert.IsTrue(resultado[1].MontoPedido < resultado[2].MontoPedido);
        }

        [TestMethod]
        public async Task OrdenarDetallesOK()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 1000, PrecioTotal = 1000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto2", Cantidad = 20, PrecioUnitario = 7457, PrecioTotal = 4111 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto3", Cantidad = 30, PrecioUnitario = 474, PrecioTotal = 747417 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto4", Cantidad = 40, PrecioUnitario = 4, PrecioTotal = 5878 });
            await contexto.SaveChangesAsync();
            var contexto2 = ConstruirContext(nombreDB);

            var col = "PrecioTotal";
            var orden = "ASC";
            var controller = new FilterController(contexto2);
            var respuesta = await controller.FiltroOrdenarDetalle(col, orden);
            List<PedidoDetalle> resultado = respuesta.Resultado.ToList();
            Assert.IsTrue(resultado[1].PrecioTotal < resultado[2].PrecioTotal);
        }

        [TestMethod]
        public async Task OrdenarPedidosInexistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba1", FechaPedido = DateTime.Now, MontoPedido = 111 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba2", FechaPedido = DateTime.Now, MontoPedido = 333 });
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba3", FechaPedido = DateTime.Now, MontoPedido = 9000 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var col = "ColumnaInexistente";
            var orden = "DESC";
            var controller = new FilterController(contexto2);
            var respuesta = await controller.FiltroOrdenarPedido(col, orden);
            List<Pedido> resultado = respuesta.Resultado.ToList();
            Assert.IsTrue(resultado[1].ID < resultado[0].ID);
        }

        [TestMethod]
        public async Task OrdenarDetallesInexistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 1000, PrecioTotal = 1000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto2", Cantidad = 20, PrecioUnitario = 7457, PrecioTotal = 4111 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto3", Cantidad = 30, PrecioUnitario = 474, PrecioTotal = 747417 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto4", Cantidad = 40, PrecioUnitario = 4, PrecioTotal = 5878 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var col = "Sueldo";
            var orden = "DESC";
            var controller = new FilterController(contexto2);
            var respuesta = await controller.FiltroOrdenarDetalle(col, orden);
            List<PedidoDetalle> resultado = respuesta.Resultado.ToList();
            Assert.IsTrue(resultado[1].ID < resultado[2].ID);
        }
    }
}
