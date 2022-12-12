using Microsoft.EntityFrameworkCore;
using mobpsycho.Models;

namespace Infraestructure
{

    public class SqlProductRepository
    {
        private readonly MobpsychoDbContext _context;

        public SqlProductRepository(MobpsychoDbContext context)
        {
            _context = context;
        }

        public async Task Add(Character character)
        {
            await _context.Characters.AddAsync(character);

            await _context.SaveChangesAsync();
        }

        public async Task<Character> Get(int characterId)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.IdCharacter == characterId);

            return character;
        }

    }


    //public interface IProductRepository
    //{
    //    Task Add(Product product);
    //    Task<Product> Get(string productId);
    //}

    //public class Product
    //{
    //    public string ProductId { get; set; }

    //    public string Name { get; set; }

    //    public decimal Price { get; set; }
    //}

}