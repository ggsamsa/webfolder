using System;
using System.Collections;
using System.Text;
using System.Collections.Specialized;



namespace WebFolder
{
    class Url
    {
        string nome, pageText, ficheiro, pasta ="", pastadopai;
        String tipo;
        string dominio;
        int nivel, tamanho;
        string pai;
        DateTime date;
        StringCollection subUrls = new StringCollection();

        public Url()
        {

        }

        public void setPai(string n)
        {
            pai = n;
        }

        public StringCollection getSubUrls()
        {
            return subUrls;
        }

        public string getPai()
        {
            return pai;
        }

        public Url(String n, int i)
        {
            nome = n;
            nivel = i;
        }

        public String getNome()
        {
            return nome;
        }

        public void setPasta(string p)
        {
            pasta = p;
        }

        public void setNome(string n)
        {
            nome = n;
        }

        public string getPastaDoPai()
        {
            return pastadopai;
        }

        public String gettipo()
        {
            return tipo;
        }

        public void setcontent()
        {
            GestaoUrls g = new GestaoUrls();
            g.setContent(this);


        }

        public void setUrls(Queue subUrls, int niv)
        {
            GestaoUrls g = new GestaoUrls();
            g.setUrls(subUrls, niv, this);
        }

        public void setBody()
        {
            MyWebRequest req = new MyWebRequest(nome);
            pageText = req.GetPage();
        }

        public string getCode()
        {
            return pageText;
        }

        public string getDominio()
        {
            return dominio;
        }

        public void setFicheiro()
        {
            string[] s;
            s = nome.Split('/');
            ficheiro = s[s.Length - 1];
        }

        public void setFicheiro(string s)
        {
            ficheiro = s;
        }

        public string getFicheiro()
        {
            return ficheiro;
        }

        public string getPasta()
        {
            return pasta;
        }

        public void corrigirUrl()
        {
            GestaoUrls g = new GestaoUrls();
            g.corrigirUrl(this);
        }

        public void setDominio(String s)
        {
            dominio = s;
        }

        public void setPastaDoPai(String s)
        {
            pastadopai = s;
        }

        public int getNivel()
        {
            return nivel;
        }

        public void setNivel(int i)
        {
            nivel = i;
        }


        public void transformarEmRelativo()
        {
            int i = 1;

            while (i != -1)
            {
                i = pageText.IndexOf(dominio);
                if (i != -1)
                    pageText = pageText.Remove(i-7, dominio.Length + 1+7);
            }
        }

        public void corrigeLinksComCaracteresEspeciais(Queue q)
        {
            GestaoUrls g = new GestaoUrls();
            g.corrigeLinksComCaracteresEspeciais(this, q);
        }

        public void corrigirLinksRelativos()
        {
            GestaoUrls g = new GestaoUrls();
            g.corrigirLinksRelativos(this);
        }

        public void setCode(string n)
        {
            pageText = n;
        }

        public void addSubUrl(string n)
        {
            subUrls.Add(n);
        }

        public int getSize()
        {
            return tamanho;
        }

        public DateTime getDate()
        {
            return date;
        }

        public void setDate(DateTime d)
        {
            date = d;
        }

        public void setTipo(string n)
        {
            tipo = n;
        }

        public void setSize(int i)
        {
            tamanho = i;
        }
    }
}
