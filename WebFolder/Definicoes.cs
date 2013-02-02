using System;
using System.Collections.Specialized;
using System.Text;

namespace WebFolder
{
    class Definicoes
    {
        StringCollection extensoes = new StringCollection();

        public Definicoes()
        {
 
        }


        public void setExtensoes(string n)
        {
            extensoes.Clear();
            string[] ss = n.Split(',');
            foreach (string s in ss)
            {
                string x = s.Trim();
                extensoes.Add(x);
            }
        }

        public StringCollection getExtensoes()
        {
            return extensoes;
        }
    }
}
