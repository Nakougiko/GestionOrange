using SQLite;

namespace GestionOrange.Models
{
    [Table("Alertes")]
    public class AlerteModel
    {
        [PrimaryKey, AutoIncrement, Column("idAlerte")]
        public int IdAlerte { get; set; }

        [Column("date")]
        public DateTime DateAlerte { get; set;}

        [Column("Chambre_idChambre")]
        public int Chambre_IdChambre { get; set; }

        [Column("Technicien_idTechnicien")]
        public int Technicien_IdTechnicien { get; set; }

    }
}
