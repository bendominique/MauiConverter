
using MVVMConversorMoedas.ViewModels;
namespace MVVMConversorMoedas
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModels();
        }

        
    }
}
