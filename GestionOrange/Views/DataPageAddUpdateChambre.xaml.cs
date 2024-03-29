using GestionOrange.ViewModels;

namespace GestionOrange.Views;

public partial class DataPageAddUpdateChambre : ContentPage
{
    public DataPageAddUpdateChambre(DataPageAddUpdatesChambreViewModels viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}