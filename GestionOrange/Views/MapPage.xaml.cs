using GestionOrange.Services;
using Microsoft.Maui.Controls.Maps;
using GestionOrange.Models;
using GestionOrange.ViewModels;
using Microsoft.Maui.Maps;

namespace GestionOrange.Views
{
    public partial class MapPage : ContentPage
    {
        private DatabaseContext _databaseContext;

        public MapPage()
        {
            InitializeComponent();
            _databaseContext = new DatabaseContext();
            CentrageMap(50.6365654, 3.0635282);
            AfficherChambre();
        }
        public void CentrageMap(double latitude, double longitude)
        {
            Location location = new Location(latitude, longitude);
            MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
            myMap.MoveToRegion(mapSpan);
        }

        public async void AfficherChambre()
        {
            var chambreList = await _databaseContext.GetAllAsync<ChambreModel>();
            if (chambreList.Count() > 0)
            {
                foreach (var chambre in chambreList)
                {
                    Pin pin = new Pin
                    {
                        Label = chambre.IdChambre.ToString(),
                        Address = "",
                        Type = PinType.Generic,
                        Location = new Location(chambre.Latitude, chambre.Longitude)
                    };
                    myMap.Pins.Add(pin);
                }
            }
        }
        public void SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (pickerSecteur.SelectedItem)
            {
                case "Lille":
                    CentrageMap(50.6365654, 3.0635282);
                    break;
                case "Douai":
                    CentrageMap(50.3675677, 3.0804641);
                    break;
                case "Lens":
                    CentrageMap(50.4291723, 2.8319805);
                    break;
                case "Arras":
                    CentrageMap(50.291048, 2.7772211);
                    break;
                default:
                    CentrageMap(50.5289671, 3.0883524);
                    break;
            }
        }
    }
}