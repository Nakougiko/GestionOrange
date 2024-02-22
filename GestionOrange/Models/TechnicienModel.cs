using SQLite;

namespace GestionOrange.Models
{
    [Table("Technicien")]
    public class TechnicienModel
    {

        [PrimaryKey, AutoIncrement, Column("idTechnicien")]
        public int IdTechnicien { get; set; }

        [Column("nom")]
        public string NomTechnicien { get; set; }

        [Column("prenom")]
        public string PrenomTechnicien { get; set; }

        [Column("numeroTelephone")]
        public string NumeroTechnicien { get; set; }

        [Column("astreinte")]
        public bool Astreinte { get; set; }
    }
}

