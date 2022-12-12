using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mobpsycho.Models;
using mobpsycho.Models.Response;

namespace mobpsycho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilitiesController : ControllerBase
    {
        private readonly MobpsychoDbContext _context;
        private readonly IMapper _mapper;

        // Constructor
        public AbilitiesController(MobpsychoDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las habilidades
        /// </summary>
        /// <returns></returns>
        // GET: api/Abilities
        [HttpGet]
        public async Task<ActionResult<Response>> GetAbilities()
        {
            try
            {
                var lista = await _context.Abilities.ToListAsync();

                if(lista.Count > 0) {

                    var request = _mapper.Map<List<AbilitieRequest>>(lista);

                    // regresa la response con un objeto character null
                    return Ok(new Response(true, "Lista de habilidades conseguida", request));
                }
                return NotFound(new Response(false, "No hay habilidades en la base de datos"));

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene una habilidad específica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Abilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetAbilitie(int id)
        {

            //Primero que nada, veré si la habilidad existe para que si no, regrese not found

           var abilitie = await _context.Abilities.FindAsync(id);

            if (abilitie == null)
            {
                return NotFound(new Response(false, "No se encontró la habilidad"));
            }

            var request = _mapper.Map<AbilitieRequest>(abilitie);

            return Ok(new Response(true, "Habilidad encontrada", request));

        }

        /// <summary>
        /// Actualiza una habilidad específica
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Abilities
        ///     
        ///     {
        ///        "idAbilitie": 1
        ///        "name": "New name",
        ///        "description": "New Description",
        ///        "idCharacter": 2
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="abilitie"></param>
        /// <returns></returns>
        // PUT: api/Abilities/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> PutAbilitie(int id, AbilitieRequest request)
        {
            if (id != request.IdAbilitie)
            {
                return BadRequest(new Response(false,"Los id no coinciden"));
            }

            Abilitie abilitie = _mapper.Map<Abilitie>(request);

            _context.Entry(abilitie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbilitieExists(id))
                {
                    return NotFound(new Response(false, "Esta habilidad no existe"));
                }
                else
                {
                    throw;
                }
            }

            return Ok(new Response(true,"Habilidad correctamente modificada"));
        }

        /// <summary>
        /// Inserta una nueva habilidad
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Abilities
        ///     
        ///     {
        ///        "name": "Atomic Punch",
        ///        "description": "A blow that makes you see little stars",
        ///        "idCharacter": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="abilitie"></param>
        /// <returns></returns>
        // POST: api/Abilities
        [HttpPost]
        public async Task<ActionResult<Response>> PostAbilitie([FromBody] AbilitieRequest request)
        {
            try
            {
                Abilitie abilitie = _mapper.Map<Abilitie>(request);

                _context.Abilities.Add(abilitie);

                await _context.SaveChangesAsync();

                return Ok(new Response(true, "Habilidad correctamente creada"));

            }
            catch (Exception exception)
            {
                //return CreatedAtAction("GetAbilitie", new { id = ab.IdAbilitie }, ab);
                return BadRequest(new Response(false, "Hubo un error: " + exception.Message));
            }
        }

        /// <summary>
        /// Elimina una habilidad, recibe idAbilitie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Abilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbilitie(int id)
        {
            var abilitie = await _context.Abilities.FindAsync(id);
            if (abilitie == null)
            {
                return NotFound();
            }

            _context.Abilities.Remove(abilitie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbilitieExists(int id)
        {
            return _context.Abilities.Any(e => e.IdAbilitie == id);
        }
    }
}
