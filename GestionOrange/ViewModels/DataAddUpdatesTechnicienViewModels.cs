using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Services;

namespace GestionOrange.ViewModels
{
    [QueryProperty(nameof(TechnicienDetails), "TechnicienDetails")]
    public partial class DataAddUpdatesTechnicienViewModels : ObservableObject
    {
        [ObservableProperty]
        private TechnicienModel _technicienDetails = new TechnicienModel();
        private readonly DatabaseContext _dbContext;

        public DataAddUpdatesTechnicienViewModels(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        public async void AddUpdateTechnicien()
        {
            string champsVides = string.Empty;

            if (string.IsNullOrEmpty(TechnicienDetails.NomTechnicien) ||
                string.IsNullOrEmpty(TechnicienDetails.PrenomTechnicien) ||
                string.IsNullOrEmpty(TechnicienDetails.NumeroTechnicien))
            {
                await Shell.Current.DisplayAlert("Champs manquants", "Veuillez remplir tous les champs", "OK");
                return;
            }

            bool success = TechnicienDetails.IdTechnicien > 0 ?
            await _dbContext.UpdateItemAsync<TechnicienModel>(TechnicienDetails) :
            await _dbContext.AddItemAsync<TechnicienModel>(TechnicienDetails);

            string message = success ? "Enregistrement réussi" : "Quelque chose s'est mal passé lors de l'ajout ou de la mise à jour du technicien";
            await Shell.Current.DisplayAlert("Information du technicien", message, "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}