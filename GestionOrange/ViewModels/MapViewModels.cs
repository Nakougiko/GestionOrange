using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using GestionOrange.Models;
using GestionOrange.Services;

namespace GestionOrange.ViewModels
{
    /*public class MapViewModels
    {
        private Microsoft.Maui.Controls.Maps.Map _map;

        public MapViewModels(Microsoft.Maui.Controls.Maps.Map map)
        {
            _map = map;
        }
        public void InitializeMap(Microsoft.Maui.Controls.Maps.Map map)
        {
            _map = map;
        }

        private DatabaseContext _databaseContext;
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
        
    }*/
}
