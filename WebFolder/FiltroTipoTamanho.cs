using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    class FiltroTipoTamanho : Filtro
    {
        string tipo;
        int tamanho;
        int comp;
        //comp == 0 -> <
        //comp == 1 -> >
        public FiltroTipoTamanho(string ti, int tam, int c)
        {
            tipo = ti;
            tamanho = tam;
            comp = c;
        }

        public override bool execute0(Url u)
        {
            if(comp == 0)
            {
                if(u.gettipo() == tipo && u.getSize() < tamanho)
                    return true;
                else
                    return false;
            }
            if(comp == 1)
            {
                if(u.gettipo() == tipo && u.getSize() >tamanho)
                    return true;
                else
                    return false;

            }
            return false;
        }
    }
}
