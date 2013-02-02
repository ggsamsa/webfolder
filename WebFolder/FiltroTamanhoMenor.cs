using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroTamanhoMenor : FiltroTamanho
    {

        public FiltroTamanhoMenor(int i)
            : base(i)
        {
        }

        public override bool execute0(Url u)
        {
            if ((u.getSize()) < base.GetTamanho())
                return true;
            else
                return false;
        }
    }
}
