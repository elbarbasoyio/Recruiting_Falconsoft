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
    public class FilterController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private int pagina = 1;
        private int registros_por_pagina = 10;
        public FilterController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("BuscarPedido")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<PedidoPaginacion<Pedido>> FiltroBusquedaPedido(string buscar)
        {
            List<Pedido> _pedidos;
            PedidoPaginacion<Pedido> _pedidoPaginacion;
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            _pedidos = await context.Pedidos.ToListAsync();

            if (!string.IsNullOrEmpty(buscar))
            {
                foreach (var item in buscar.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _pedidos = _pedidos.Where(x => x.Cliente.Contains(item) ||
                                                   x.FechaPedido.Value.ToString("yyyy-MM-dd HH:mm:ss").Contains(item) ||
                                                   x.MontoPedido.Value.ToString().Contains(item))
                                                   .ToList();
                }
            }
            _TotalRegistros = _pedidos.Count();
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);
            _pedidos = _pedidos.Skip((pagina - 1) * registros_por_pagina)
                                            .Take(registros_por_pagina)
                                            .ToList();
            _pedidoPaginacion = new PedidoPaginacion<Pedido>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                BusquedaActual = buscar,
                OrdenActual = "",
                TipoOrdenActual = "",
                Resultado = _pedidos
            };
            return _pedidoPaginacion;
        }


        [HttpGet]
        [Route("BuscarDetalle")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<PedidoPaginacion<PedidoDetalle>> FiltroBusquedaDetalle(string buscar)
        {
            List<PedidoDetalle> _pedidosdetalle;
            PedidoPaginacion<PedidoDetalle> _pedidoPaginacion;
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            _pedidosdetalle = await context.PedidosDetalle.ToListAsync();

            if (!string.IsNullOrEmpty(buscar))
            {
                foreach (var item in buscar.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _pedidosdetalle = _pedidosdetalle.Where(x => x.Pedido.ToString().Contains(item) ||
                                                   x.Producto.Contains(item) ||
                                                   x.Cantidad.ToString().Contains(item) ||
                                                   x.PrecioUnitario.ToString().Contains(item) ||
                                                   x.PrecioTotal.ToString().Contains(item))
                                                   .ToList();
                }

            }
            _TotalRegistros = _pedidosdetalle.Count();
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);
            _pedidosdetalle = _pedidosdetalle.Skip((pagina - 1) * registros_por_pagina)
                                            .Take(registros_por_pagina)
                                            .ToList();
            _pedidoPaginacion = new PedidoPaginacion<PedidoDetalle>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                BusquedaActual = buscar,
                OrdenActual = "",
                TipoOrdenActual = "",
                Resultado = _pedidosdetalle
            };
            return _pedidoPaginacion;
        }

        [HttpGet]
        [Route("OrdenarPedido")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<PedidoPaginacion<Pedido>> FiltroOrdenarPedido(string columna, 
                                                                        string tipo_orden)
        {
            List<Pedido> _pedidos;
            PedidoPaginacion<Pedido> _pedidoPaginacion;
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            _pedidos = await context.Pedidos.ToListAsync();

            switch (columna)
            {
                case "ID":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidos = _pedidos.OrderByDescending(x => x.ID).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidos = _pedidos.OrderBy(x => x.ID).ToList();
                    break;

                case "Cliente":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidos = _pedidos.OrderByDescending(x => x.Cliente).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidos = _pedidos.OrderBy(x => x.Cliente).ToList();
                    break;

                case "FechaPedido":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidos = _pedidos.OrderByDescending(x => x.FechaPedido).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidos = _pedidos.OrderBy(x => x.FechaPedido).ToList();
                    break;

                case "MontoPedido":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidos = _pedidos.OrderByDescending(x => x.MontoPedido).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidos = _pedidos.OrderBy(x => x.MontoPedido).ToList();
                    break;

                default:
                    if (tipo_orden.ToLower() == "desc")
                        _pedidos = _pedidos.OrderByDescending(x => x.ID).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidos = _pedidos.OrderBy(x => x.ID).ToList();
                    break;
            }

            _TotalRegistros = _pedidos.Count();
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);
            _pedidos = _pedidos.Skip((pagina - 1) * registros_por_pagina)
                                            .Take(registros_por_pagina)
                                            .ToList();
            _pedidoPaginacion = new PedidoPaginacion<Pedido>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                BusquedaActual = "",
                OrdenActual = "",
                TipoOrdenActual = "",
                Resultado = _pedidos
            };
            return _pedidoPaginacion;
        }

        [HttpGet]
        [Route("OrdenarDetalle")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<PedidoPaginacion<PedidoDetalle>> FiltroOrdenarDetalle(string columna,
                                                                          string tipo_orden)
        {
            List<PedidoDetalle> _pedidosdetalle;
            PedidoPaginacion<PedidoDetalle> _pedidoPaginacion;
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            _pedidosdetalle = await context.PedidosDetalle.ToListAsync();

            switch (columna)
            {
                case "Pedido":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.Pedido).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.Pedido).ToList();
                    break;

                case "Producto":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.Producto).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.Producto).ToList();
                    break;

                case "Cantidad":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.Cantidad).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.Cantidad).ToList();
                    break;

                case "PrecioUnitario":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.PrecioUnitario).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.PrecioUnitario).ToList();
                    break;

                case "PrecioTotal":
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.PrecioTotal).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.PrecioTotal).ToList();
                    break;

                default:
                    if (tipo_orden.ToLower() == "desc")
                        _pedidosdetalle = _pedidosdetalle.OrderByDescending(x => x.Pedido).ToList();
                    else if (tipo_orden.ToLower() == "asc")
                        _pedidosdetalle = _pedidosdetalle.OrderBy(x => x.ID).ToList();
                    break;
            }

            _TotalRegistros = _pedidosdetalle.Count();
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);
            _pedidosdetalle = _pedidosdetalle.Skip((pagina - 1) * registros_por_pagina)
                                            .Take(registros_por_pagina)
                                            .ToList();
            _pedidoPaginacion = new PedidoPaginacion<PedidoDetalle>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                BusquedaActual = "",
                OrdenActual = "",
                TipoOrdenActual = "",
                Resultado = _pedidosdetalle
            };
            return _pedidoPaginacion;
        }
    }
}
