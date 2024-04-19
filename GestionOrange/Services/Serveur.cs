using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GestionOrange.Models;
using GestionOrange.Views;
using GestionOrange.ViewModels;

namespace GestionOrange.Services
{
    public class Serveur
    {
        private DatabaseContext _databaseContext;
        public Serveur() 
        {
            _databaseContext = new DatabaseContext();
        }
        public async void StartServeur()
        {
            try
            {
                int Port = 55000;
                TcpListener Serveur = new TcpListener(IPAddress.Any, Port);
                Serveur.Start();
                //await Shell.Current.DisplayAlert("Serveur connecté", "Connexion effectué", "OK");

                while (true)
                {
                    TcpClient client = await Serveur.AcceptTcpClientAsync();
                    if (client.Connected)
                    {
                        //await Shell.Current.DisplayAlert("Client connecté", "Connexion effectué", "OK");

                        if (await IsClientConnected(client))
                        {
                            await HandleClient(client);
                        }
                        else
                        {
                            //await Shell.Current.DisplayAlert("Client déconnecté", "Déconnexion effectué", "OK");
                        }
                    }
                    else
                    {
                        //await Shell.Current.DisplayAlert("Client déconnecté", "Déconnexion effectué", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Serveur Déconnecté", ex.Message, "OK");
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream FluxDonnees;
            int NbData;
            byte[] DataLues = new byte[50];
            string DataClient;
            string id, date, heure;
            try
            {
                FluxDonnees = client.GetStream();
                NbData = await FluxDonnees.ReadAsync(DataLues, 0, DataLues.Length);
                DataClient = System.Text.Encoding.ASCII.GetString(DataLues);

                ExtraireInformationsTrame(DataClient, out id, out date, out heure);
                var chambreList = await _databaseContext.GetAllAsync<AlerteModel>();


                var alerte = new AlerteModel
                {
                    Chambre_IdChambre = int.Parse(id),
                    DateAlerte = DateTime.Parse(date + " " + heure),
                 };
                await _databaseContext.AddItemAsync(alerte);

                await Shell.Current.DisplayAlert("Donneée Client", "N° de série : " + id + "\nDate : " + date + "\nHeure : " + heure, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Client déconnecté", ex.Message, "OK");
            }
            finally
            {
                // Libérer les ressources
                client.Close();
                //await Shell.Current.DisplayAlert("Client déconnecté", "Déconnexion effectué", "OK");
            }
        }

        //Vérifie si le client est toujours connecter
        private async Task<bool> IsClientConnected(TcpClient client)
        {
            try
            {
                // Essayer de lire des données à partir du flux réseau
                NetworkStream stream = client.GetStream();
                if (stream.CanRead)
                {
                    // Vérifier si des données sont disponibles
                    if (stream.DataAvailable)
                    {
                        // Lire un octet pour vérifier l'état de la connexion
                        byte[] buffer = new byte[1];
                        await stream.ReadAsync(buffer, 0, 1);
                    }
                }

                // Si aucune exception n'est levée, le client est toujours connecté
                return true;
            }
            catch (Exception)
            {
                // Une exception indique que la connexion a été perdue
                return false;
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
            date = trame.Substring(debutDate, finDate - debutDate);

            // Récupérer l'heure (caractères entre le "T" et la première ",")
            int debutHeure = finDate + 1; // Index du premier caractère après le "T"
            int finHeure = trame.IndexOf(','); // Index de la première ","
            heure = trame.Substring(debutHeure, finHeure - debutHeure);
        }




    }
}
