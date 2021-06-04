using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PedidosAPI.Controllers;
using PedidosAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidosAPI.Test.UnitTest
{
    [TestClass]
    public class PedidoDetalleControllerTest : TestBase
    {
        [TestMethod]
        public async Task ObtenerTodosLosDetalles()
        {

            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 100, PrecioTotal = 1000});
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto2", Cantidad = 20, PrecioUnitario = 200, PrecioTotal = 2000});
            await contexto.SaveChangesAsync();
            var contexto2 = ConstruirContext(nombreDB);

            var controller = new PedidoDetalleController(contexto2);
            var respuesta = await controller.GetDetallesPedidos();
            var detalles = respuesta.Count;
            Assert.AreEqual(2, detalles);
        }

        [TestMethod]
        public async Task ObtenerDetallePorIDNoExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var controller = new PedidoDetalleController(contexto);
            var respuesta = await controller.GetDetalleByID(15);
            Assert.AreEqual(0, respuesta.ID);
        }

        [TestMethod]
        public async Task ObtenerDetallePorIDExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle {Pedido = 1, Producto = "Producto1", Cantidad = 10, PrecioUnitario = 100, PrecioTotal = 1000 });
            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle {Pedido = 2, Producto = "Producto2", Cantidad = 11, PrecioUnitario = 200, PrecioTotal = 1000 });
            await contexto.SaveChangesAsync();
            var controller = new PedidoDetalleController(contexto); 
            var id = 1;
            var respuesta = await controller.GetDetalleByID(id);
            Assert.AreEqual(1, respuesta.ID);
        }

        [TestMethod]
        public async Task AgregarDetalle()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            var nuevoPedido = new Pedido() { Cliente = "ClientePrueba4", FechaPedido = DateTime.Now, MontoPedido = 9000 };
            var controller = new PedidoController(contexto);
            await controller.SavePedido(nuevoPedido);

            var nuevoDetalle = new PedidoDetalle() { Pedido = 1, Producto = "Producto77", Cantidad = 45, PrecioUnitario = 55, PrecioTotal = 21545};
            var controller2 = new PedidoDetalleController(contexto);
            var respuesta = await controller2.SaveDetallePedido(nuevoDetalle);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task EditarDetalle()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle {Pedido = 1, Producto = "Producto444", Cantidad = 33, PrecioUnitario = 44, PrecioTotal = 555});
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);
            var controller = new PedidoDetalleController(contexto2);

            var id = 1;
            var detalleAct = new PedidoDetalle() { ID = id, Pedido = 1, Producto = "NombreProducto", Cantidad = 33, PrecioUnitario = 44, PrecioTotal = 555};
            var respuesta = await controller.EditDetalle(id, detalleAct);

            var context3 = ConstruirContext(nombreDB);
            var existe = await context3.PedidosDetalle.AnyAsync(x => x.Producto == "NombreProducto");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task BorrarDetalleOK()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.PedidosDetalle.AddAsync(new PedidoDetalle { Pedido = 1, Producto = "Producto101", Cantidad = 1, PrecioUnitario = 10, PrecioTotal = 10});
            await contexto.SaveChangesAsync();
            var controller = new PedidoDetalleController(contexto);

            var id = 1;
            var respuesta = await controller.DeleteDetalle(id);
            var resultado = respuesta as OkObjectResult;
            Assert.AreEqual(200, resultado.StatusCode);
        }

        [TestMethod]
        public async Task BorrarDetalleNoExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var controller = new PedidoDetalleController(contexto);

            var id = 1;
            var respuesta = await controller .DeleteDetalle(id);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(400, resultado.StatusCode);
        }
    }
}
