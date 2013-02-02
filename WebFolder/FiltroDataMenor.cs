using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroDataMenor : FiltroData
    {
        public FiltroDataMenor(DateTime d)
            : base(d)
        {
        }

        public override bool execute0(Url u)
        {
            if (base.GetDate().CompareTo(u.getDate()) > 0)
                return true;
            else
                return false;
        }
    }
}
