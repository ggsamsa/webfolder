using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{

    abstract class Filtro
    {
        protected Filtro next;

        public bool execute(Url u)
        {
            bool res = execute0(u);
            if ((res == false) && next != null)
                res = next.execute(u);
            return res;
        }

        abstract public bool execute0(Url u);

        public void setNext(Filtro f)
        {
            next = f;
        }

        public Filtro dnext()
        {
            return next;
        }
    }
}

