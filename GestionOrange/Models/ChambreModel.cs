using SQLite;

namespace GestionOrange.Models
{
    [Table("Chambres")]
    public class ChambreModel
    {
        [PrimaryKey, AutoIncrement, Column("idChambre")]
        public int IdChambre { get; set; }

        [Column("numeroSerie")]
        public string? NumeroSerie { get; set; }

        [Column("latitude")]
        public float Latitude { get; set; }

        [Column("longitude")]
        public float Longitude { get; set; }

        [Column("Secteur_idSecteur")]
        public int Secteur_IdSecteur { get; set; }
    }
}
