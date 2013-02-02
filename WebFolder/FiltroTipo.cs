using System;
using System.Collections.Generic;
using System.Text;
//using WebFolder;

namespace WebFolder
{
    abstract class FiltroTipo : Filtro
    {
        String tipo;

        public FiltroTipo(String s)
        {
            tipo = s;
            /*
            if (s == "jpeg" || s == "gif" || s == "bmp" || s == "png")
                tipo = "image/" + s;
            if (s == "pdf")
                tipo = "application/" + s;
            if (s == "wmv" || s == "mpeg" || s == "avi" || s == "ram" || s == "mov")
                tipo = "video/" + s;*/
        }

        protected string GetTipo()
        {
            return tipo;
        }
    }
}