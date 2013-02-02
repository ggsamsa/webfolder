using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Data;
using System.IO;

namespace WebFolder
{
    class BD
    {
        string fprojectos = "projectos.xml";
        string sprojectos = "projectos.xsd";

        public BD()
        {
        }

        public StringCollection lerProjectos()
        {
            StringCollection col = new StringCollection();
            DataSet ds = new DataSet();

            FileStream finschema = new FileStream(sprojectos, FileMode.Open, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();

            FileStream findata = new FileStream(fprojectos, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();
            
            foreach (DataRow dr in ds.Tables["Projecto"].Rows)
            {
                col.Add((string)(dr["nome"]) + ";" + (string)(dr["data"]) + ";" + (string)(dr["ficheiro"]));
            }

            return col;
        }

        public void gravarProjecto(Projecto p)
        {
            string file, schema;
            file = getFicheiro(p.getNome());
            if (file.Length == 0)
            {
                file = p.getNome() + ".xml";
                schema = file.Remove(file.Length - 4) + ".xsd";
                File.Copy("exemplo.xml", file);
                

            }
            else { schema = file.Remove(file.Length - 4) + ".xsd"; }
            DataSet ds = new DataSet();

            FileStream finschema = new FileStream("exemplo.xsd", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();
            FileStream findata = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();
            
            foreach (DataRow dr in ds.Tables["projecto"].Rows)
            {
                dr["nome"] = p.getNome();
                dr["data"] = DateTime.Now;
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["url"] = p.getUrlInicial();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["pasta"] = p.getPasta();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["niveis"] = p.getNivel();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["visualizacaolocal"] = p.getvisualizacaolocal();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["imagensremotas"] = p.getImagensRemotas();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["aceitarrejeitar"] = p.getAceitarRejeitar();
            }

            foreach (DataRow dr in ds.Tables["Projecto"].Rows)
            {
                dr["filtros"] = p.getFiltros();
            }

            FileStream fout = new FileStream(file, FileMode.OpenOrCreate,
                    FileAccess.Write, FileShare.ReadWrite);
            //Only write the Xml data to the stream
            ds.WriteXml(fout);
            //Close the Stream
            fout.Close();

            criarProjecto(p);

        }

        public string getFicheiro(string nome)
        {
            StringCollection col = new StringCollection();
            col = lerProjectos();
            String [] projectos;
            foreach (string s in col)
            {
                projectos = s.Split(';');
                if (nome == projectos[0])
                    return projectos[2];
            }
            return "";
        }

        public void criarProjecto(Projecto p)
        {
            DataSet ds = new DataSet();
            FileStream finschema = new FileStream("projectos.xsd", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();
            FileStream findata = new FileStream("projectos.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();

            DataRow newrow = ds.Tables["Projecto"].NewRow();
            newrow["nome"] = p.getNome();
            newrow["data"] = DateTime.Now;
            newrow["ficheiro"] = p.getNome() + ".xml";
            ds.Tables["Projecto"].Rows.Add(newrow);
            FileStream fout = new FileStream("projectos.xml", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            ds.WriteXml(fout);
            fout.Close();
        }

        public Projecto lerProjecto(string n)
        {
            Projecto p = new Projecto();
            DataSet ds = new DataSet();
            FileStream finschema = new FileStream("exemplo.xsd", FileMode.Open, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();

            FileStream findata = new FileStream(n + ".xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();

            foreach (DataRow dr in ds.Tables["Projecto"].Rows)
            {
                p.setNome((string)(dr["nome"]));
                p.setFiltros((string)(dr["Filtros"]));
                DateTimeConverter d = new DateTimeConverter();
                DateTime da = new DateTime();
                da = (DateTime)(d.ConvertFromString((string)(dr[1])));
                p.setData(da);
            }

            foreach (DataRow dr in ds.Tables["propriedades"].Rows)
            {
                p.setPasta((string)(dr["pasta"]));
                p.setUrlInicial((string)(dr["url"]));
                p.setvisualizacaolocal(Convert.ToInt32(dr["visualizacaolocal"]));
                p.setNivel(Convert.ToInt32(dr["niveis"]));
                p.setImagensRemotas(Convert.ToInt32(dr["imagensremotas"]));
                p.setAceitarRejeitar(Convert.ToInt32(dr["aceitarrejeitar"]));
            }

            return p;
        }

        public void removerProjecto(string n)
        {
            DataSet ds = new DataSet();
            FileStream finschema = new FileStream("projectos.xsd", FileMode.Open, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();

            FileStream findata = new FileStream("projectos.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();
            int i = 0;
            foreach (DataRow dr in ds.Tables["projecto"].Rows)
            {
                if (dr["nome"].ToString() == n)
                {
                    ds.Tables["projecto"].Rows.Remove(dr);
                    dr.Delete();
                    break;
                }
                i++;
            }
            
            FileStream fout = new FileStream("projectos.xml", FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
            ds.WriteXml(fout);
            fout.Close();

            File.Delete(n + ".xml");
        }

        public void actualizarProjecto(Projecto p)
        {
            string file, schema;
            file = getFicheiro(p.getNome());
            if (file.Length == 0)
            {
                file = p.getNome() + ".xml";
                schema = file.Remove(file.Length - 4) + ".xsd";
                File.Copy("exemplo.xml", file);


            }
            else { schema = file.Remove(file.Length - 4) + ".xsd"; }
            DataSet ds = new DataSet();

            FileStream finschema = new FileStream("exemplo.xsd", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();
            FileStream findata = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();

            foreach (DataRow dr in ds.Tables["projecto"].Rows)
            {
                dr["nome"] = p.getNome();
                dr["data"] = DateTime.Now;
                dr["Filtros"] = p.getFiltros();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["url"] = p.getUrlInicial();
            }

            foreach (DataRow dr in ds.Tables["Propriedades"].Rows)
            {
                dr["pasta"] = p.getPasta();
                dr["visualizacaolocal"] = p.getvisualizacaolocal();
                dr["niveis"] = p.getNivel();
                dr["imagensremotas"] = p.getImagensRemotas();
                dr["aceitarrejeitar"] = p.getAceitarRejeitar();
            }

            FileStream fout = new FileStream(file, FileMode.Truncate,
                    FileAccess.Write, FileShare.ReadWrite);
            //Only write the Xml data to the stream
            ds.WriteXml(fout);
            //Close the Stream
            fout.Close();
        }


        public Definicoes getDefinicoes()
        {
            DataSet ds = new DataSet();
            Definicoes def = new Definicoes();

            FileStream finschema = new FileStream("definicoes.xsd", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();
            FileStream findata = new FileStream("definicoes.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();

            foreach (DataRow dr in ds.Tables["definicoes"].Rows)
            {
                def.setExtensoes((string)(dr["extensoes"]));
            }

            return def;
        }

        public void gravarDefinicoes(Definicoes def)
        {
            DataSet ds = new DataSet();
            string s="";
            StringCollection sc = new StringCollection();
            sc = def.getExtensoes();
            int i = 0;
            foreach (string x in sc)
            {
                s += x;
                if (i < sc.Count - 1)
                    s += ", ";
                i++;
            }
            FileStream finschema = new FileStream("definicoes.xsd", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            ds.ReadXmlSchema(finschema);
            finschema.Close();
            FileStream findata = new FileStream("definicoes.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            ds.ReadXml(findata);
            findata.Close();

            foreach (DataRow dr in ds.Tables["definicoes"].Rows)
            {
                dr["extensoes"] = s;
            }
            FileStream fout = new FileStream("definicoes.xml", FileMode.Truncate,
                   FileAccess.Write, FileShare.ReadWrite);
            ds.WriteXml(fout);
            fout.Close();

        }


    }
}
