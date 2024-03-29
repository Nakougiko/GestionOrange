using GestionOrange.Views;

namespace GestionOrange
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DataPageAddUpdateTechnicien), typeof(DataPageAddUpdateTechnicien));
            Routing.RegisterRoute(nameof(DataPageAddUpdateChambre), typeof(DataPageAddUpdateChambre));
        }
    }
}
