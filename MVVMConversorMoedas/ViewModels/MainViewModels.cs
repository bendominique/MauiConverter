using MVVMConversorMoedas.Models;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MVVMConversorMoedas.ViewModels
{
    public class MainViewModels : INotifyPropertyChanged //INotifyPropertyChanged causes an auto update and notify the view where was changed 
    {
        public event PropertyChangedEventHandler? PropertyChanged; //PropertyChanged's the event which will appears when an propriety changes, the ? means the prropertychangedhandler could be nnull

        void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(name));

        private readonly RateTable _rates = new(); //connect to models

        public IList<string> Currencies { get; } // here take all the currencies we have in Curriencies

        string? _amountText;

        public string? AmountText
        {
            get => _amountText;
            set
            {
                if (value == _amountText) return; //if the value's equals the _amountText just return a value


                _amountText = value; //here my _amountText receive value and will start an OnPropertyChanged() and my ConvertCommand will be a command and could execute an change
                OnPropertyChanged();
                ((Command)ConvertCommand).ChangeCanExecute();

            }
        }

        string _from = "JPY"; //here will run like an initdefaults and when the application starts will show the JPY from first value, but could be changed

        public string From
        {
            get => _from; //here will take the value and if the user want to stand with the value the program just run the application
            set
            {
                if (_from == value) return;
                _from = value;
                OnPropertyChanged();
                ((Command)ConvertCommand).ChangeCanExecute();
            }
        }

        string _to = "BRL";
        public string To
        {
            get => _to; //here will take the value and if the user want to stand with the value the program just run the application
            set
            {
                if (_to == value) return;
                _to = value;
                OnPropertyChanged();
                ((Command)ConvertCommand).ChangeCanExecute();
            }
        }

        string _resultText = "--";

        public string ResultText
        {
            get => _resultText;
            set //if the value will be different, the set goes attribute the new value.
            {
                if (_resultText != value)
                { _resultText = value;
                    OnPropertyChanged(); }
            }
        }

        public ICommand ConvertCommand { get; }
        public ICommand SwapCommand { get; }
        public ICommand ClearCommand { get; }

        readonly CultureInfo _en = new("en");

        public MainViewModels()
        {
            Currencies = _rates.GetCurrencies().ToList();

            ConvertCommand = new Command(DoConvert, CanConvert);
            SwapCommand = new Command(DoSwap);
            ClearCommand = new Command(DoClear);
        }
        //here w
        public void DoConvert()
        {

            if (!TryParseAmount(AmountText, out var amount))
            {
                ResultText = "Invalid Value";
                return;
                //if the value is out from the allowed something like a letter or a negative value shows invalid 
            }
            if (!_rates.Supports(From) || !_rates.Supports(To))
            {
                ResultText = "Invalid currency";
                return;
            }

            var result = _rates.Convert(amount, From, To); //Takes my converts defaults from models
            ResultText = string.Format(_en, "{0:N2} {1} = {2:N2} {3}",
                amount, From, result, To);

            //0:N2 currency default of conversion
            //0 quantity which i want to convert
            //2:N2 result of the count
            //3 and the currency

        }

        public bool CanConvert()
        {
            if (string.IsNullOrEmpty(AmountText)) return false;
            return TryParseAmount(AmountText, out _); //try validate the text entrt
        }


        void DoSwap()
        {
            (From, To) = (To, From);
            ResultText = "--";//change the place
        }
        void DoClear()
        {
            AmountText = string.Empty;
            ResultText = "--";//Clean
        }
        
    bool TryParseAmount(string? text, out decimal amount)

        {
            amount = 0m;
            if (string.IsNullOrEmpty(text)) return false;

            var s = text.Trim(); //Trim excludes the space before the user insert a value
            if (!decimal.TryParse(s, NumberStyles.Number, _en,
                out amount)) return true;

            s = s.Replace(".", ",");
            return decimal.TryParse(s, NumberStyles.Number, _en, out amount);
        }



    }
}
    

