using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucas.Model
{
    internal class Expression
    {
        public string word;
        public decimal? value;
        public List<string> references;
        public Expression(string w, decimal? v)
        {
            word = w;
            value = v;
            references = new List<string>();
        }
    }
}
