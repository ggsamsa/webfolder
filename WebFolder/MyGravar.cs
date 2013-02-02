using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace WebFolder
{
    class MyGravar
    {
        bool resultado = false;
        string directorio;
        public MyGravar(Url u, string dir)
        {
            directorio = dir + "/";
            directorio = directorio.Replace('/', '\\');
            if (u.gettipo().Contains("html"))
                resultado = gravahtml(u);
            else resultado = gravaoutro(u);
        }

        private bool gravahtml(Url u)
        {
            try
            {

                string path = "";
                byte[] stream = new System.Text.UTF8Encoding(true).GetBytes(u.getCode());
                FileStream fp;
                if ((u.getPasta() != null) && (u.getPasta().Length > 0))
                {
                    criaestrutura(u.getDominio() + "/" + u.getPasta());
                    path = u.getDominio() + "\\" + u.getPasta().Replace('/', '\\') + "\\" + u.getFicheiro();
                    path = path.Replace('?', '-');
                    fp = File.Create(directorio + path);
                    fp.Write(stream, 0, stream.Length);
                    if (fp != null) resultado = true;
                    fp.Close();
                }
                else
                {
                    criaestrutura(u.getDominio());
                    fp = File.Create(directorio + u.getDominio() + "\\" + u.getFicheiro());
                    fp.Write(stream, 0, stream.Length);
                    if (fp != null) resultado = true;
                    fp.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool getResultado()
        {
            return resultado;
        }

        private bool gravaoutro(Url u)
        {
            try
            {
                WebClient Client = new WebClient();
                string path = "";

                if (u.getPasta().Length > 0)
                {
                    criaestrutura(u.getDominio() + "/" + u.getPasta());
                    path = u.getDominio() + "\\" + u.getPasta().Replace('/', '\\') + "\\" + u.getFicheiro();
                    Client.DownloadFile(u.getNome(), directorio + path);
                    resultado = true;

                }
                else
                {
                    criaestrutura(u.getDominio());
                    Client.DownloadFile(u.getNome(), directorio + u.getDominio() + "\\" + u.getFicheiro());
                    resultado = true;
                }
                return resultado;
            }
            catch
            {
                return false;
            }
        }

        private void criaestrutura(String pasta)
        {
            string[] s;
            DirectoryInfo dp;
            int i;

            s = pasta.Split('/');
            dp = Directory.CreateDirectory(directorio + s[0]);
            if (s.Length >= 1)
                {
                    for (i = 1; i < s.Length-1; i++)
                    {
                        if(s[i].Length>0)
                            dp = dp.CreateSubdirectory(s[i]);
                    }
                }           
        }

    }
}
