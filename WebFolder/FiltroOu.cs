using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroOu : Filtro
    {
        Filtro next2;

        public override bool execute0(Url u)
        {
            if (next != null)
            {
                if (next.execute(u) || next2.execute(u))
                    return true;
                else
                    return false;
            }
            else
            {
                if (next2.execute(u))
                    return true;
                else
                    return false;
            }

        }

        public void setNext2(Filtro f)
        {
            next2 = f;
        }

        public Filtro dnext2()
        {
            return next2;
        }
    }
}
