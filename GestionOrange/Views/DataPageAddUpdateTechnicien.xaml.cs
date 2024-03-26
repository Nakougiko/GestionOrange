using GestionOrange.ViewModels;

namespace GestionOrange.Views;

public partial class DataPageAddUpdateTechnicien : ContentPage
{
	public DataPageAddUpdateTechnicien(DataAddUpdatesTechnicienViewModels viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}