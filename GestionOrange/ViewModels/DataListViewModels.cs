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


        private readonly DatabaseContext _dbContext;

        public DataListViewModels()
        {
            _dbContext = new DatabaseContext();
        }

        [RelayCommand]
        public async void GetTechnicienList()
        {
            var technicienList = await _dbContext.GetAllAsync<TechnicienModel>();

            if (technicienList.Any()) // Any vérifie si il y a au moins un élément dans la liste
            {
                technicienList = technicienList.OrderBy(t => t.NomTechnicien).ToList();
                Techniciens.Clear();

                foreach (var technicien in technicienList)
                {
                    Techniciens.Add(technicien);
                }

                TechniciensListForSearch.Clear();
                TechniciensListForSearch.AddRange(technicienList);
            }
        }

        [RelayCommand]
        public async void AddTechncien()
        {
            await Shell.Current.GoToAsync(nameof(DataPageAddUpdateTechnicien));
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
    }
}
