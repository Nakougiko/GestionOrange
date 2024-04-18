using GestionOrange.ViewModels;

namespace GestionOrange.Views
{
    public partial class DataListPage : ContentPage
    {
        private readonly DataListViewModels _viewModel;

        public DataListPage(DataListViewModels viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.GetTechnicienList();
            await _viewModel.GetChambreList();
        }
    }

}
