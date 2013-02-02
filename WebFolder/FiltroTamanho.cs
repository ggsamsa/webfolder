using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    abstract class FiltroTamanho : Filtro
    {
        int tamanho;

        public FiltroTamanho(int i)
            : base()
        {
            tamanho = i * 1024;// *1024;
        }

        protected int GetTamanho()
        {
            return tamanho;
        }

    }
}