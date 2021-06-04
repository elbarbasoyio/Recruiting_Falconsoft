using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public SampleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Sample>> GetPedidos()
        {
            var result = await context.Sample.ToListAsync();
            return result;
        }

        [HttpGet]
        [Route("{id}", Name = "GetSample")]
        public async Task<Sample> GetSampleByID(int id)
        {
            Sample sample = new Sample();
            if (await context.Sample.AnyAsync(s => s.ID == id.ToString()))
            {
                sample = await context.Sample.FirstOrDefaultAsync(s => s.ID == id.ToString());
            }
            return sample;
        }

        [HttpPut]
        [Route("EditSample/{id}")]
        public async Task<ActionResult> EditSample(string id, [FromBody] Sample sample)

        {
            try
            {
                if (await context.Sample.AnyAsync(s => s.ID == id))
                {
                    context.Entry(sample).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return CreatedAtRoute("GetSample", new { id = sample.ID }, sample);
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
        [Route("SaveSample")]
        public async Task<ActionResult> SaveSample([FromBody] Sample sample)
        {
            try
            {
                await context.Sample.AddAsync(sample);
                await context.SaveChangesAsync();
                return CreatedAtRoute("GetSample", new { id = sample.ID }, sample);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSample/{id}")]
        public async Task<ActionResult> DeleteSample(int id)
        {
            try
            {
                var sample = await context.Sample.FirstOrDefaultAsync(s => s.ID == id.ToString());
                if (sample != null)
                {
                    context.Sample.Remove(sample);
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
