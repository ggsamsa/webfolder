using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroTipoDiferente : FiltroTipo
    {

        public FiltroTipoDiferente(string s)
            : base(s)
        {
        }

        public override bool execute0(Url u)
        {
            if ((u.gettipo()) != base.GetTipo())
                return true;
            else
                return false;
        }

    }
}
