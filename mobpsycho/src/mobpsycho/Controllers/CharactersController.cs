using System;
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
    public class CharactersController : ControllerBase
    {
        private readonly MobpsychoDbContext _context;
        private readonly IMapper _mapper;

        public CharactersController(MobpsychoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Obtiene todos los personajes
        /// </summary>
        // GET: api/Characters
        [HttpGet]
        public async Task<ActionResult<Response>> GetCharacters()
        {
            try
            {
                var data = await _context.Characters.ToListAsync();

                var response = _mapper.Map<List<CharacterRequest>>(data);

                if(data.Count > 0)
                {
                    return Ok(new Response(true, "Todos los personajes correctamente obtenidos", response));
                }
                return NotFound(new Response(false, "al parecer no hay personajes"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response (false,"Ocurrió el siguiente error", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un personaje específico y sus habilidades asociadas
        /// </summary>
        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            try
            {
                CharacterDetails characterDetail = new CharacterDetails();
                using (var db = new MobpsychoDbContext())
                {
                    characterDetail = (from d in db.Characters
                                       where d.IdCharacter == id
                                       select new CharacterDetails
                                       {
                                           IdCharacter = d.IdCharacter,
                                           Name = d.Name,
                                           UrlImg = d.UrlImg,
                                           Age = d.Age,
                                           BirthDate = d.BirthDate,

                                           Abilitie = (
                                                from a in db.Abilities
                                                where a.IdCharacter == d.IdCharacter
                                                select new AbilitieSimple
                                                {
                                                    IdAbilitie = a.IdAbilitie,
                                                    Name = a.Name,
                                                    Description = a.Description,
                                                }
                                           ).ToList()

                                       }).FirstOrDefault();
                }
                return Ok(new Response(true, "Objeto complejo con LINQ", characterDetail));
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un personaje específico
        /// </summary>
        /// <param name="id"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> PutCharacter(int id, CharacterRequest request)
        {
            if (id != request.IdCharacter)
            {
                return BadRequest(new Response(false, "Los id no coinciden"));
            }

            Character characterModified = _mapper.Map<Character>(request);

            _context.Entry(characterModified).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
                {
                    return NotFound(new Response(false, "El personaje a modificar no existe"));
                }
                else
                {
                    throw;
                }
            }

            return Ok(new Response(true, "Personaje correctamente actualizado"));
        }

        /// <summary>
        /// Inserta un nuevo personaje
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        // POST: api/Characters
        [HttpPost]
        public async Task<ActionResult<Response>> PostCharacter(CharacterRequest request)
        {
            try
            {
                // Auto Mapper: asignación dinámica de atributos de una clase a otra
                Character character = _mapper.Map<Character>(request);
            
                _context.Characters.Add(character);

                await _context.SaveChangesAsync();

                return Ok(new Response(true, "Peronaje creado"));

            } catch (Exception ex)
            {
                return BadRequest(new Response(false, "Ocurrió un error" + ex));
            }
        }

        /// <summary>
        /// Elimina un personaje
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound(new Response(false, "Personaje no encontrado"));
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return Ok(new Response(false, "Personaje Correctamente eliminado"));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.IdCharacter == id);
        }
    }
}
