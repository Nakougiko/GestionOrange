using SQLite;

namespace GestionOrange.Models
{
    [Table("Chambres")]
    public class ChambreModel
    {
        [PrimaryKey, AutoIncrement, Column("idChambre")]
        public int IdChambre { get; set; }
        
        [Column("latitude")]
        public string Latitude { get; set; }

        [Column("longitude")]
        public string Longitude { get; set; }

        [Column("Secteur_idSecteur")]
        public int Secteur_IdSecteur { get; set; }
    }
}
