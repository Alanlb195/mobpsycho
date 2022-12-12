using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mobpsycho.Models
{
    public class Abilitie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Id Autoincrement
        public int IdAbilitie { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdCharacter { get; set; }
        [ForeignKey("IdCharacter")]
        public Character Character { get; set; }
    }

    public class AbilitieRequest
    {
        public int IdAbilitie { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdCharacter { get; set; }
    }

    public class AbilitieSimple {
        public int IdAbilitie { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
