using GestionOrange.Models;
using SQLite;

namespace GestionOrange.Services
{
    public class DatabaseContext
    {
        private const string DbName = "ProjetOrange.db3";
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

        private SQLiteAsyncConnection _connection = null!;
        private SQLiteAsyncConnection Database =>
            (_connection ??= new SQLiteAsyncConnection(DbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));

        public DatabaseContext()
        {
            Task.Run(async () => await FillSecteurTableAsync());
        }

        private async Task<bool> FillSecteurTableAsync()
        {
            await CreateTableIfNotExists<SecteurModel>();

            var secteurs = await GetAllAsync<SecteurModel>();
            if (secteurs.Any())
            {
                return false;
            }

            var defaultSecteurs = new List<SecteurModel>
            {
                new SecteurModel { IdSecteur=1, NomSecteur = "Artois" },
                new SecteurModel { IdSecteur=2, NomSecteur = "Hainaut" },
                new SecteurModel { IdSecteur=3, NomSecteur = "Métropole" },
                new SecteurModel { IdSecteur=4, NomSecteur = "Opale" }
            };
            
            foreach (var secteur in defaultSecteurs)
            {
                await AddItemAsync<SecteurModel>(secteur);
            }

            return true;
        }

        private async Task CreateTableIfNotExists<TTable>() where TTable : class, new()
        {
            await Database.CreateTableAsync<TTable>();
        }

        // Récupère la table spécifiée
        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return Database.Table<TTable>();
        }

        // Récupère tous les éléments de la table spécifiée
        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }

        // Exécute une action sur la table spécifiée
        private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await action();
        }

        // Ajoute un nouvel élément à la table spécifiée
        public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);
        }

        // Met à jour un élément dans la table spécifiée
        public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.UpdateAsync(item) > 0;
        }

        // Supprime un élément de la table spécifiée
        public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync(item) > 0;
        }

        // Supprime un élément par clé de la table spécifiée
        public async Task<bool> DeleteItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync<TTable>(primaryKey) > 0;
        }

        // Libère les ressources de connexion à la base de données
        public async ValueTask DisposeAsync() => await _connection.CloseAsync();
    }
}