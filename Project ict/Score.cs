using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ict
{
    internal class Score
    {
        int score;

        public string Toevoegen()
        {
            // zorg dat het commando score.Toevoegen zorgt dat er score word bijgeteld.
            score++;
            return score.ToString();
        }
        public string Resetten()
        {
            // zorg dat het commando score.Resetten score op 0 zet.
            score = 0;
            return score.ToString();
        }
        public string Tonen()
        {
            // zorg dat het commando score.Tonen het totaal terugstuurd.
            return $"{score}";
        }
    }
}
