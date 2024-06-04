using GestionOrange.Views;
using GestionOrange.Services;

namespace GestionOrange
{
    public partial class AppShell : Shell
    {
        private readonly Serveur _serveur = new Serveur();
        private readonly SMS _sms = new SMS();

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DataPageAddUpdateTechnicien), typeof(DataPageAddUpdateTechnicien));
            Routing.RegisterRoute(nameof(DataPageAddUpdateChambre), typeof(DataPageAddUpdateChambre));
            _serveur.StartServeur();
            //_sms.sendSms();
        }
    }
}
