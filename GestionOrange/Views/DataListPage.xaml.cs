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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.GetTechnicienList();
        }
    }

}
