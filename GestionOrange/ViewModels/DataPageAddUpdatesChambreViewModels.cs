using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Services;

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
            string champsVides = string.Empty;

            if (string.IsNullOrEmpty(ChambreDetails.Longitude) ||
                string.IsNullOrEmpty(ChambreDetails.Latitude))
            {
                await Shell.Current.DisplayAlert("Champs manquants", "Veuillez remplir tous les champs", "OK");
                return;
            }

            bool success = ChambreDetails.IdChambre > 0 ?
            await _dbContext.UpdateItemAsync<ChambreModel>(ChambreDetails) :
            await _dbContext.AddItemAsync<ChambreModel>(ChambreDetails);

            string message = success ? "Enregistrement réussi" : "Quelque chose s'est mal passé lors de l'ajout ou de la mise à jour de la chambre";
            await Shell.Current.DisplayAlert("Information de la chambre ", message, "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
