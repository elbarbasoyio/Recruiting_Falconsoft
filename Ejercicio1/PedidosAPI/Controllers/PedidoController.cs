using PedidosAPI.Authentication;
using PedidosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PedidosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PedidoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<List<Pedido>> GetPedidos()
        {
            var result = await context.Pedidos.ToListAsync();
            return result;
        }

        [HttpGet]
        [Route("{id}", Name = "GetPedido")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Pedido> GetPedidoByID(int id)
        {
            Pedido ped = new Pedido();
            if (await context.Pedidos.AnyAsync(p => p.ID == id))
            {
                ped = await context.Pedidos.FirstOrDefaultAsync(ped => ped.ID == id);
            }
            return ped;
        }

        [HttpPut]
        [Route("EditPedido/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> EditPedido(int id, [FromBody] Pedido pedido)

        {
            try
            {
                if (await context.Pedidos.AnyAsync(p => p.ID == id))
                {
                    context.Entry(pedido).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return CreatedAtRoute("GetPedido", new { id = pedido.ID }, pedido);
                }
                    else
                    {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SavePedido")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult> SavePedido([FromBody] Pedido pedido)
        {
            try
            {
                await context.Pedidos.AddAsync(pedido);
                await context.SaveChangesAsync();
                return CreatedAtRoute("GetPedido", new {id = pedido.ID}, pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePedido/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> DeletePedido(int id)
        {
            try
            {
                var pedido = await context.Pedidos.FirstOrDefaultAsync(ped => ped.ID == id);
                if(pedido != null)
                {
                    var detalles = await context.PedidosDetalle.Where(d => d.Pedido == pedido.ID).ToListAsync();
                    foreach(PedidoDetalle det in detalles)
                    {
                        context.PedidosDetalle.Remove(det);
                        await context.SaveChangesAsync();
                    }
                    context.Pedidos.Remove(pedido);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else 
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
