using System;
using System.Collections.Generic;
using System.Text;

namespace WebFolder
{
    public class  Projecto
    {
        String nome, pasta, ficheiro, urlInicial, filtros;
        int nivel;
        int dominio, imagensremotas, visualizacaolocal, aceitarrejeitar;
        DateTime data;

        public Projecto()
        { }

        public string getNome()
        {
            return nome;
        }

        public void setNome(string n)
        {
            nome = n;
        }

        public string getPasta()
        {
            return pasta;
        }

        public void setPasta(string p)
        {
            pasta = p;
        }

        public string getFicheiro()
        {
            return ficheiro;
        }

        public void setFicheiro(string f)
        {
            ficheiro = f;
        }

        public int getNivel()
        {
            return nivel;
        }

        public void setNivel(int i)
        {
            nivel =i;
        }

        public int getMsmDominio()
        {
            return dominio;
        }

        public void setMsmDominio(int d)
        {
            dominio = d;
        }
        public string getUrlInicial()
        {
            return urlInicial;
        }

        public void setUrlInicial(string u)
        {
            urlInicial = u;
        }

        public DateTime getData()
        {
            return data;
        }

        public void setData(DateTime d)
        {
            data = d;
        }

        public void setImagensRemotas(int k)
        {
            imagensremotas = k;
        }

        public int getImagensRemotas()
        {
            return imagensremotas;
        }

        public void setvisualizacaolocal(int v)
        {
            visualizacaolocal = v;
        }

        public int getvisualizacaolocal()
        {
            return visualizacaolocal;
        }

        public void setFiltros(string f)
        {
            filtros = f;
        }

        public string getFiltros()
        {
            if (filtros == null) return "0";
            else return filtros;
        }

        public void setAceitarRejeitar(int ar)
        {
            aceitarrejeitar = ar;
        }

        public int getAceitarRejeitar()
        {
            return aceitarrejeitar;
        }
    }
}
