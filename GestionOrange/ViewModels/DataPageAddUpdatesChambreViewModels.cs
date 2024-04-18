using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GestionOrange.ViewModels
{
    [QueryProperty(nameof(ChambreDetails), "ChambreDetails")]
    public partial class DataPageAddUpdatesChambreViewModels : ObservableObject
    {
        [ObservableProperty]
        private ChambreModel _chambreDetails = new ChambreModel();
        private readonly DatabaseContext _dbContext;

        public DataPageAddUpdatesChambreViewModels(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        public async void AddUpdateChambre()
        {
            if (!ValidateChambreDetails())
                return;

            if (await ChambreExists(ChambreDetails.IdChambre, ChambreDetails.Longitude, ChambreDetails.Latitude, ChambreDetails.NumeroSerie))
            {
                await Shell.Current.DisplayAlert("Technicien existant", "Des données similaires ont été trouvées", "OK");
                return;
            }

            bool success = ChambreDetails.IdChambre > 0 ?
            await _dbContext.UpdateItemAsync<ChambreModel>(ChambreDetails) :
            await _dbContext.AddItemAsync<ChambreModel>(ChambreDetails);

            string message = success ? "Enregistrement réussi" : "Quelque chose s'est mal passé lors de l'ajout ou de la mise à jour de la chambre";
            await Shell.Current.DisplayAlert("Information de la chambre ", message, "OK");
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateChambreDetails()
        {
            if (string.IsNullOrEmpty(ChambreDetails.NumeroSerie))
            {
                Shell.Current.DisplayAlert("Champs manquants", "Veuillez remplir tous les champs", "OK");
                return false;
            }

            // Validation de l'adresse MAC
            Regex macRegex = new Regex(@"^([0-9A-F]{2}:){5}[0-9A-F]{2}$", RegexOptions.IgnoreCase);
            if (!macRegex.IsMatch(ChambreDetails.NumeroSerie))
            {
                Shell.Current.DisplayAlert("Numéro de série invalide", "L'adresse MAC doit être au format XX:XX:XX:XX:XX:XX", "OK");
                return false;
            }

            // Validation de la latitude
            if (ChambreDetails.Latitude < -90 || ChambreDetails.Latitude > 90)
            {
                Shell.Current.DisplayAlert("Latitude invalide", "La latitude doit être comprise entre -90 et 90", "OK");
                return false;
            }

            // Validation de la longitude
            if (ChambreDetails.Longitude < -180 || ChambreDetails.Longitude > 180)
            {
                Shell.Current.DisplayAlert("Longitude invalide", "La longitude doit être comprise entre -180 et 180", "OK");
                return false;
            }

            return true;
        }

        private async Task<bool> ChambreExists(int id, float longitude, float latitude, string numeroSerie)
        {
            var chambres = await _dbContext.GetAllAsync<ChambreModel>();
            return chambres.Any(c => c.IdChambre != id && ((c.NumeroSerie.Equals(numeroSerie)) || (c.Longitude.Equals(longitude) && c.Latitude.Equals(latitude))));
        }
    }
}
