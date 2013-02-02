using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    abstract class FiltroData : Filtro
    {
        DateTime data;

        public FiltroData(DateTime d)
            : base()
        {
            data = d;
        }

        protected DateTime GetDate()
        {
            return data;
        }

    }
}
