
//Models have the bussines logical, adding our money 
namespace MVVMConversorMoedas.Models
{

    //Convert Defaults
    public class RateTable
    {
        private readonly Dictionary<string, decimal>
            _toBRL = new()//We'are creating an dictionary where all the program will run throught it
            {

                ["BRL"] = 1.00m, // BRAZILIAN REAL Currency 
                ["USD"] = 5.29m, //USA Currency Dolar
                ["EUR"] = 6.29m,
                ["JPY"] = 0.039m,
                ["GBP"] = 7.29m,
            };
        //we'll create an class now for none another attribute can use to edit it (dictionary), but only read it
        public IReadOnlyDictionary<string, decimal>
            ToBRL => _toBRL;

        public IEnumerable<string> GetCurrencies() =>
            _toBRL.Keys.OrderBy(k => k);
        //with my public IEnumerable allows to return a collection of elements, working like a vector
        //<string> will return the elements from the collection where's a string
        //GetCurrencies -> method name which allows return the elements type string
        //_toBRL.Keys. -> acess the dictionary keys, with OrderBy(putting the k => k in alphabetic order)

        //----------------------------------------------------------------------------------------------------

        public bool Supports(string code) =>
            _toBRL.ContainsKey(code);
        //verify if the currency key it's supported, viewing if the currency are or not in our models and returning true or false

        public decimal Convert(decimal amount, string from, string to) // amount -> value wich the user insert, from -> the value wich as received to convert, and to-> wich currency will be converted
        {
            if (!Supports(from) || !Supports(to)) return 0m;
            // if the currency choices from from or from to was invalid return 0 if the currency's not supported

            if (from == to) return amount;
            //return the original value if the from and to is the same    

            var brl = amount * _toBRL[from];
            return brl / _toBRL[to];


        }


    }
}
