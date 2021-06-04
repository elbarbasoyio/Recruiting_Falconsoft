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
    public class PedidoControllerTest : TestBase
    {
        [TestMethod]
        public async Task ObtenerTodosLosPedidos()
        {

            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba1", FechaPedido = null, MontoPedido = 111});
            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba2", FechaPedido = null, MontoPedido = 333});
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var controller = new PedidoController(contexto2);
            var respuesta = await controller.GetPedidos();

            var resultado = respuesta.Count;
            Assert.AreEqual(2, resultado);
        }

        [TestMethod]
        public async Task ObtenerPedidosPorID()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba3", FechaPedido = null, MontoPedido = 1152});
            await contexto.SaveChangesAsync();
            var pedido = new Pedido();
            var contexto2 = ConstruirContext(nombreDB);
            var controller = new PedidoController(contexto2);
            var id = 1;
            //var respuesta = controller.GetPedidoByID(id);
            pedido = await controller.GetPedidoByID(id);

            var resultado = pedido.ID;
            Assert.AreEqual(id, resultado);
        }

        [TestMethod]
        public async Task AgregarPedido()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            var nuevoPedido = new Pedido() { Cliente = "ClientePrueba4", FechaPedido = DateTime.Now, MontoPedido = 9000 };
            var controller = new PedidoController(contexto);

            var respuesta = await controller.SavePedido(nuevoPedido);
            var resultado = respuesta as CreatedAtRouteResult;

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public async Task EditarPedido()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "Test_A", FechaPedido = DateTime.Now, MontoPedido = 1321 });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);
            var controller = new PedidoController(contexto2);

            var id = 1;
            var pedidoAct = new Pedido() { ID= id, Cliente = "Test_B", FechaPedido = DateTime.Now, MontoPedido = 1320 };
            var respuesta = await controller.EditPedido(id, pedidoAct);

            var context3 = ConstruirContext(nombreDB);
            var existe = await context3.Pedidos.AnyAsync(x => x.Cliente == "Test_B");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task BorrarPedidoOK()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);

            await contexto.Pedidos.AddAsync(new Pedido { Cliente = "ClientePrueba6", FechaPedido = null, MontoPedido = 9999 });
            await contexto.SaveChangesAsync();
            var controller = new PedidoController(contexto);

            var id = 1;
            var respuesta = await controller.DeletePedido(id);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(200, resultado.StatusCode);
        }

        [TestMethod]
        public async Task BorrarPedidoNoExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var controller = new PedidoController(contexto);

            var id = 1;
            var respuesta = await controller.DeletePedido(id);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(400, resultado.StatusCode);
        }
    }
}
