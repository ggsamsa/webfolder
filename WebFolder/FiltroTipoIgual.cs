using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroTipoIgual : FiltroTipo
    {

        public FiltroTipoIgual(string s)
            : base(s)
        {
        }

        public override bool execute0(Url u)
        {
            if ((u.gettipo()) == base.GetTipo())
                return true;
            else
                return false;
        }

    }
}
