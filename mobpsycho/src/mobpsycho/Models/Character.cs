using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mobpsycho.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCharacter { get; set; }
        public string Name { get; set; }
        public string UrlImg { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Abilitie> Abilities { get; set; }
    }
    public class CharacterRequest
    {
        public int IdCharacter { get; set; }
        public string Name { get; set; }
        public string UrlImg { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CharacterDetails
    {
        public int IdCharacter { get; set; }
        public string Name { get; set; }
        public string UrlImg { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public List<AbilitieSimple > Abilitie { get; set; }
    }
}
