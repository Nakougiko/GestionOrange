using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GestionOrange.Models;

namespace GestionOrange.Services
{
    public class Serveur
    {
        private readonly DatabaseContext _databaseContext;
        private readonly SMS _sms;

        // Constructeur pour initialiser le contexte de base de données
        public Serveur()
        {
            _databaseContext = new DatabaseContext();
            _sms = new SMS();
        }

        // Méthode principale pour démarrer le serveur
        public async void StartServeur()
        {
            try
            {
                int Port = 55000; // Port sur lequel le serveur écoutera les connexions
                TcpListener Serveur = new TcpListener(IPAddress.Any, Port); // Initialiser le TcpListener pour écouter sur toutes les interfaces réseau
                Serveur.Start(); // Démarrer le serveur

                // Boucle infinie pour accepter et gérer les connexions des clients
                while (true)
                {
                    TcpClient client = await Serveur.AcceptTcpClientAsync(); // Attendre qu'un client se connecte
                    if (client.Connected) // Vérifier si le client est connecté
                    {
                        await HandleClient(client); // Gérer la connexion du client de manière asynchrone
                    }
                }
            }
            catch (Exception ex)
            {
                // Afficher un message en cas d'exception lors du démarrage ou de l'écoute du serveur
                await Shell.Current.DisplayAlert("Serveur Déconnecté", ex.Message, "OK");
            }
        }

        // Méthode pour gérer la connexion d'un client
        private async Task HandleClient(TcpClient client)
        {
            NetworkStream FluxDonnees; // Flux de données pour communiquer avec le client
            int NbData; // Nombre d'octets lus depuis le flux de données
            byte[] DataLues = new byte[50]; // Buffer pour stocker les données lues
            string DataClient; // Chaîne de données reçue du client
            string id, date, heure; // Variables pour stocker les informations extraites de la trame

            try
            {
                // Obtenir le flux de données du client
                FluxDonnees = client.GetStream();
                // Lire les données du flux
                NbData = await FluxDonnees.ReadAsync(DataLues, 0, DataLues.Length);
                // Convertir les données lues en chaîne de caractères
                DataClient = Encoding.ASCII.GetString(DataLues, 0, NbData);

                // Extraire les informations de la trame reçue
                ExtraireInformationsTrame(DataClient, out id, out date, out heure);




                // Créer un nouvel objet AlerteModel avec les informations extraites
                var alerte = new AlerteModel
                {
                    Chambre_IdChambre = int.Parse(id), // Convertir l'ID de la chambre en entier
                    DateAlerte = DateTime.Parse($"{date} {heure}"), // Combiner la date et l'heure en un seul DateTime
                };

                // Enregistrer l'alerte dans la base de données
                await _databaseContext.AddItemAsync(alerte);
                _sms.sendSms();
                // Afficher les données du client
                await Shell.Current.DisplayAlert("Donnée Client", $"N° de série : {id}\nDate : {date}\nHeure : {heure}", "OK");
            }
            catch (Exception ex)
            {
                // Afficher un message en cas d'exception lors de la gestion du client
                await Shell.Current.DisplayAlert("Client déconnecté", ex.Message, "OK");
            }
            finally
            {
                // Fermer la connexion avec le client
                client.Close();
            }
        }

        // Méthode pour extraire les informations de la trame
        private void ExtraireInformationsTrame(string trame, out string id, out string date, out string heure)
        {
            // Séparer la trame en éléments en utilisant les délimiteurs ":" et ","
            string[] elements = trame.Split(':', ',');

            // Récupérer l'ID (premiers caractères avant le premier ":")
            id = elements[0].Trim();

            // Récupérer la date (caractères entre le premier ":" et le "T")
            int debutDate = trame.IndexOf(':') + 1; // Index du premier caractère après le ":"
            int finDate = trame.IndexOf('T'); // Index du caractère "T"
            date = trame.Substring(debutDate, finDate - debutDate).Trim();

            // Récupérer l'heure (caractères entre le "T" et la première ",")
            int debutHeure = finDate + 1; // Index du premier caractère après le "T"
            int finHeure = trame.IndexOf(','); // Index de la première ","
            heure = trame.Substring(debutHeure, finHeure - debutHeure).Trim();
        }
    }
}
