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
        public ObservableCollection<TechnicienModel> Techniciens { get; set; } = new ObservableCollection<TechnicienModel>();
        public ObservableCollection<ChambreModel> Chambres { get; set; } = new ObservableCollection<ChambreModel>();

        private readonly DatabaseContext _dbContext;

        public DataListViewModels()
        {
            _dbContext = new DatabaseContext();
        }

        // Methode générique pour récupérer la liste des éléments de la base de données
        private async Task GetListAsync<T>(ObservableCollection<T> list) where T : class, new()
        {
            list.Clear();
            var items = await _dbContext.GetAllAsync<T>();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    if (item is TechnicienModel technicien)
                    {
                        technicien.NomTechnicien = FormatDonnee.ConvertToDisplayFormat(technicien.NomTechnicien);
                        technicien.PrenomTechnicien = FormatDonnee.ConvertToDisplayFormat(technicien.PrenomTechnicien);
                        technicien.NumeroTechnicien = FormatDonnee.ConvertNumeroToDisplayFormat(technicien.NumeroTechnicien);
                    }

                    list.Add(item);
                }
            }
        }

        [RelayCommand]
        public async Task GetTechnicienList()
        {
            await GetListAsync(Techniciens);
        }

        [RelayCommand]
        public async Task GetChambreList()
        {
            await GetListAsync(Chambres);
        }

        [RelayCommand]
        public async Task AddTechnicien()
        {
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateTechnicien));
        }

        [RelayCommand]
        public async Task AddChambre()
        {
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateChambre));
        }
        
        [RelayCommand]
        public async Task DisplayAction(TechnicienModel technicien)
        {
            var response = await Shell.Current.DisplayActionSheet("Sélectionner une option", "OK", null, "Modifier", "Supprimer");
            switch (response)
            {
                case "Modifier":
                    await EditTechnicien(technicien);
                    break;

                case "Supprimer":
                    await DeleteTechnicien(technicien);
                    break;
            }
        }

        [RelayCommand]
        public async Task EditTechnicien(TechnicienModel technicien)
        {
            var navParam = new Dictionary<string, object>
            {
                { "TechnicienDetails", technicien }
            };
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateTechnicien), navParam);
        }

        [RelayCommand]
        public async Task DeleteTechnicien(TechnicienModel technicien)
        {
            var verif = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer ce technicien ?", "Oui", "Non");
            if (!verif)
                return;

            var delResponse = await _dbContext.DeleteItemAsync<TechnicienModel>(technicien);
            if (delResponse)
            {
                await GetTechnicienList();
            }
        }

        [RelayCommand]
        public async Task DisplayActionChambre(ChambreModel chambre)
        {
            var response = await Shell.Current.DisplayActionSheet("Sélectionner une option", "OK", null, "Modifier", "Supprimer");
            switch (response)
            {
                case "Modifier":
                    await EditChambre(chambre);
                    break;

                case "Supprimer":
                    await DeleteChambre(chambre);
                    break;
            }
        }

        [RelayCommand]
        private async Task EditChambre(ChambreModel chambre)
        {
            var navParam = new Dictionary<string, object>
            {
                { "ChambreDetails", chambre }
            };
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateChambre), navParam);
        }

        [RelayCommand]
        private async Task DeleteChambre(ChambreModel chambre)
        {
            var verif = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cette chambre ?", "Oui", "Non");
            if (!verif)
                return;

            var delResponse = await _dbContext.DeleteItemAsync<ChambreModel>(chambre);
            if (delResponse)
            {
                await GetChambreList();
            }
        }
    }
}
