using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool success = false;
            
            if (TechnicienDetails.IdTechnicien > 0)
            {
                success = await _dbContext.UpdateItemAsync<TechnicienModel>(TechnicienDetails);
            }
            else
            {
                success = await _dbContext.AddItemAsync<TechnicienModel>(TechnicienDetails);
            }

            if (success)
            {
                await Shell.Current.DisplayAlert("Info sur le technicien enregistré", "Enregistrement réussi", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Quelque chose s'est mal passé lors de l'ajout ou de la mise à jour du technicien", "OK");
            }
        }
    }
}