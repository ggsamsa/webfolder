using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroNome : Filtro
    {
        string parte;

        public FiltroNome(string p)
            : base()
        {
            parte = p;
        }

        public override bool execute0(Url u)
        {
            if (u.getNome().Contains(parte))
                return true;
            else
                return false;
        }
    }
}
