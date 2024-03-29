using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Views;
using GestionOrange.Services;
using System.Collections.ObjectModel;

namespace GestionOrange.ViewModels
{
    public partial class DataListViewModels : ObservableObject
    {
        public static List<TechnicienModel> TechniciensListForSearch { get; private set; } = new List<TechnicienModel>();
        public ObservableCollection<TechnicienModel> Techniciens { get; set; } = new ObservableCollection<TechnicienModel>();

        public static List<ChambreModel> ChambresListForSearch { get; private set; } = new List<ChambreModel>();
        public ObservableCollection<ChambreModel> Chambres { get; set; } = new ObservableCollection<ChambreModel>();

        private readonly DatabaseContext _dbContext;

        public DataListViewModels()
        {
            _dbContext = new DatabaseContext();
        }

        [RelayCommand]
        public async void GetTechnicienList()
        {
            Techniciens.Clear();
            var technicienList = await _dbContext.GetAllAsync<TechnicienModel>();

            if (technicienList.Any()) // Any vérifie si il y a au moins un élément dans la liste
            {
                technicienList = technicienList.OrderBy(t => t.NomTechnicien).ToList();

                foreach (var technicien in technicienList)
                {
                    Techniciens.Add(technicien);
                }

                TechniciensListForSearch.Clear();
                TechniciensListForSearch.AddRange(technicienList);
            }
        }

        [RelayCommand]
        public async void GetChambreList()
        {
            Chambres.Clear();
            var chambreList = await _dbContext.GetAllAsync<ChambreModel>();

            if (chambreList.Any()) // Any vérifie si il y a au moins un élément dans la liste
            {
                chambreList = chambreList.OrderBy(c => c.IdChambre).ToList();

                foreach (var chambre in chambreList)
                {
                    Chambres.Add(chambre);
                }

                ChambresListForSearch.Clear();
                ChambresListForSearch.AddRange(chambreList);
            }
        }

        [RelayCommand]
        public async void AddTechnicien()
        {
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateTechnicien));
        }

        [RelayCommand]
        public async void AddChambre()
        {
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateChambre));
        }

        [RelayCommand]
        public async void DeleteTechnicien(TechnicienModel technicien)
        {
            var verif = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer ce technicien ?", "Oui", "Non");
            if (!verif)
                return;
            
            var delResponse = await _dbContext.DeleteItemAsync<TechnicienModel>(technicien);
            if (delResponse)
            {
                GetTechnicienList();
            }
        }

        [RelayCommand]
        public async void EditTechnicien(TechnicienModel technicien)
        {
            var navParam = new Dictionary<string, object>
            {
                { "TechnicienDetails", technicien }
            };
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateTechnicien), navParam);
        }

        
        [RelayCommand]
        public async void DisplayAction(TechnicienModel technicien)
        {
            var response = await Shell.Current.DisplayActionSheet("Sélectionner une option", "OK", null, "Modifier", "Supprimer");
            switch (response)
            {
                case "Modifier":
                    EditTechnicien(technicien);
                    break;

                case "Supprimer":
                    DeleteTechnicien(technicien);
                    break;
            }
        }

        [RelayCommand]
        public async void DisplayActionChambre(ChambreModel chambre)
        {
            var response = await Shell.Current.DisplayActionSheet("Sélectionner une option", "OK", null, "Modifier", "Supprimer");
            switch (response)
            {
                case "Modifier":
                    EditChambre(chambre);
                    break;

                case "Supprimer":
                    DeleteChambre(chambre);
                    break;
            }
        }

        [RelayCommand]
        private async void DeleteChambre(ChambreModel chambre)
        {
            var verif = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cette chambre ?", "Oui", "Non");
            if (!verif)
                return;

            var delResponse = await _dbContext.DeleteItemAsync<ChambreModel>(chambre);
            if (delResponse)
            {
                GetChambreList();
            }
        }


        [RelayCommand]
        private async void EditChambre(ChambreModel chambre)
        {
            var navParam = new Dictionary<string, object>
            {
                { "ChambreDetails", chambre }
            };
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateChambre), navParam);
        }
    }
}
