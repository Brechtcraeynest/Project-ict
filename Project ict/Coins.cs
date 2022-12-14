using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ict
{
    internal class Coins
    {
        int coins;

        public string Toevoegen()
        {
            // zorg dat het commando coins.Toevoegen zorgt dat er een coin word bijgeteld.
            coins++;
            return coins.ToString();
        }
        public string Resetten()
        {
            // zorg dat het commando coins.Resetten coins op 0 zet.
            coins = 0;
            return coins.ToString();
        }
        public string Tonen()
        {
            // zorg dat het commando coins.Tonen het totaal terugstuurd.
            return $"{coins}";
        }
    }
}
