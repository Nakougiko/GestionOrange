using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using GestionOrange.Models;
using GestionOrange.Services;

namespace GestionOrange.Services
{
    /*public class Maps
    {
        private DatabaseContext _databaseContext;
        private Microsoft.Maui.Controls.Maps.Map _map;

        public Maps(Microsoft.Maui.Controls.Maps.Map map)
        {
            this._map = map;
        }
        


        public void CentrageMap(float latitude, float longitude)
        {
            Location location = new Location(latitude, longitude);
            MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
            _map.MoveToRegion(mapSpan);
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
                    _map.Pins.Add(pin);
                }
            }
        }
        public void SelcetedIndexChanged(object sender, EventArgs e)
        {
            /*switch (pickerSecteur.SelectedItem)
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
    }*/
}
