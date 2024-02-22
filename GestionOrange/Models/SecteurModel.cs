using SQLite;

namespace GestionOrange.Models
{
    [Table("Secteur")]
    public class SecteurModel
    {
        [PrimaryKey, AutoIncrement, Column("idSecteur")]
        public int IdSecteur { get; set; }

        [Column("nom")]
        public string NomSecteur { get; set; }
    }
}
