using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionOrange.Models;
using GestionOrange.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestionOrange.ViewModels
{
    [QueryProperty(nameof(TechnicienDetails), "TechnicienDetails")]
    public partial class DataAddUpdatesTechnicienViewModels : ObservableObject
    {
        [ObservableProperty]
        private TechnicienModel _technicienDetails = new TechnicienModel();

        private readonly DatabaseContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<SecteurModel> _secteurs = new ObservableCollection<SecteurModel>();

        [ObservableProperty]
        private SecteurModel _selectedSecteur;

        public DataAddUpdatesTechnicienViewModels(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            ChargerSecteur();
        }

        [RelayCommand]
        public async Task AddUpdateTechnicien()
        {
            if (!ValidateTechnicienDetails())
                return;

            // Formater les données du technicien
            TechnicienDetails.NomTechnicien = FormatDonnee.ConvertToDatabaseFormat(TechnicienDetails.NomTechnicien);
            TechnicienDetails.PrenomTechnicien = FormatDonnee.ConvertToDatabaseFormat(TechnicienDetails.PrenomTechnicien);
            TechnicienDetails.NumeroTechnicien = FormatDonnee.ConvertNumeroToDatabaseFormat(TechnicienDetails.NumeroTechnicien);

            // Vérifier si le technicien existe déjà
            if (await TechnicienExists(TechnicienDetails.IdTechnicien, TechnicienDetails.NomTechnicien, TechnicienDetails.PrenomTechnicien, TechnicienDetails.NumeroTechnicien))
            {
                await Shell.Current.DisplayAlert("Technicien existant", "Des données similaires ont été trouvées", "OK");
                return;
            }

            // Ajouter ou mettre à jour le technicien
            bool success = TechnicienDetails.IdTechnicien > 0
                ? await _dbContext.UpdateItemAsync<TechnicienModel>(TechnicienDetails)
                : await _dbContext.AddItemAsync<TechnicienModel>(TechnicienDetails);

            string message = success ? "Enregistrement réussi" : "Quelque chose s'est mal passé lors de l'ajout ou de la mise à jour du technicien";
            await Shell.Current.DisplayAlert("Information du technicien", message, "OK");
            await Shell.Current.GoToAsync("..");
        }

        // Méthode pour charger les secteurs
        private async void ChargerSecteur()
        {
            var secteurs = await _dbContext.GetAllAsync<SecteurModel>();
            Secteurs = new ObservableCollection<SecteurModel>(secteurs);
            if (TechnicienDetails.Secteur_IdSecteur != 0)
            {
                SelectedSecteur = Secteurs.FirstOrDefault(s => s.IdSecteur == TechnicienDetails.Secteur_IdSecteur);
            }
        }

        partial void OnSelectedSecteurChanged(SecteurModel value)
        {
            if (value != null)
            {
                TechnicienDetails.Secteur_IdSecteur = value.IdSecteur;
            }
        }

        // Valide les détails du technicien
        private bool ValidateTechnicienDetails()
        {
            // Vérifie si les champs sont vides
            if (string.IsNullOrWhiteSpace(TechnicienDetails.NomTechnicien) ||
                string.IsNullOrWhiteSpace(TechnicienDetails.PrenomTechnicien) ||
                string.IsNullOrWhiteSpace(TechnicienDetails.NumeroTechnicien))
            {
                Shell.Current.DisplayAlert("Champs manquants", "Veuillez remplir tous les champs", "OK");
                return false;
            }

            if (SelectedSecteur == null)
            {
                Shell.Current.DisplayAlert("Secteur manquant", "Veuillez sélectionner un secteur", "OK");
                return false;
            }

            // Vérifie la validité du format du nom et du prénom
            string namePattern = @"^[a-zA-ZÀ-ÿÉé]+([ '-]?[a-zA-ZÀ-ÿÉé]+)*$";
            if (!Regex.IsMatch(TechnicienDetails.PrenomTechnicien, namePattern) ||
                !Regex.IsMatch(TechnicienDetails.NomTechnicien, namePattern))
            {
                Shell.Current.DisplayAlert("Champs invalide", "Veuillez vérifier si les champs prénoms et nom sont correct", "OK");
                return false;
            }

            // Vérifie la validité du format du numéro de téléphone
            string phonePattern = @"^0\d\s\d{2}\s\d{2}\s\d{2}\s\d{2}$";
            if (!Regex.IsMatch(TechnicienDetails.NumeroTechnicien, phonePattern) || TechnicienDetails.NumeroTechnicien.Length < 14)
            {
                Shell.Current.DisplayAlert("Champs invalide", "Veuillez vérifier le numéro de téléphone", "OK");
                return false;
            }

            return true;
        }

        // Vérifie si un technicien existe déjà avec des détails similaires
        private async Task<bool> TechnicienExists(int id, string? nom, string? prenom, string? telephone)
        {
            var techniciens = await _dbContext.GetAllAsync<TechnicienModel>();
            return techniciens.Any(t => t.IdTechnicien != id &&
                ((t.NomTechnicien!.Equals(nom) && t.PrenomTechnicien!.Equals(prenom)) || t.NumeroTechnicien!.Equals(telephone)));
        }
    }
}
