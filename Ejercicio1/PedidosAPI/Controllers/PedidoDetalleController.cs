using PedidosAPI.Authentication;
using PedidosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PedidosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class PedidoDetalleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PedidoDetalleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<List<PedidoDetalle>> GetDetallesPedidos()
        {
            var result = await context.PedidosDetalle.ToListAsync();
            return result;
        }

        [HttpGet]
        [Route("{id}", Name = "GetDetalle")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<PedidoDetalle> GetDetalleByID(int id)
        {
            PedidoDetalle det = new PedidoDetalle();
            if (await context.PedidosDetalle.AnyAsync(d => d.ID == id))
            {
                det = await context.PedidosDetalle.FirstOrDefaultAsync(det => det.ID == id);
            }
            return det;
        }

        [HttpPut]
        [Route("EditDetalle/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> EditDetalle(int id, [FromBody] PedidoDetalle detalle)
        {
            try
            {
                if (await context.PedidosDetalle.AnyAsync(pd => pd.ID == id))
                {
                    context.Entry(detalle).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return CreatedAtRoute("GetDetalle", new { id = detalle.ID }, detalle);
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
        [Route("SaveDetalle")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult> SaveDetallePedido([FromBody] PedidoDetalle detalle)
        {
            try
            {
                if (await context.Pedidos.AnyAsync(p => p.ID == detalle.Pedido))
                {
                    await context.PedidosDetalle.AddAsync(detalle);
                    await context.SaveChangesAsync();
                    return CreatedAtRoute("GetDetalle", new { id = detalle.ID }, detalle);
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

        [HttpDelete]
        [Route("DeleteDetalle/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> DeleteDetalle(int id)
        {
            try
            {
                var detalle = await context.PedidosDetalle.FirstOrDefaultAsync(d => d.ID == id);
                if (detalle != null)
                {
                    context.PedidosDetalle.Remove(detalle);
                    await context.SaveChangesAsync();
                    return Ok(id);
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

