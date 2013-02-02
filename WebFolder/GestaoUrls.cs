using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace WebFolder
{
    class GestaoUrls
    {


        public void setUrls(Queue subUrls, int niv, Url u)
        {
            RegexOptions rxOpt = RegexOptions.Singleline |
                                RegexOptions.Compiled |
                                RegexOptions.IgnoreCase;
            Regex rImg, raHref, rHref, rJs, rFrame, rIFrame, rFlash;
            Match m;

            //Parse de <img src
            rImg = new Regex("<img[^>]*src=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rImg.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                Url tUrl = new Url(temp, u.getNivel() + 1);
                tUrl.setPastaDoPai(u.getPasta());
                tUrl.setDominio(u.getDominio());
                tUrl.setPai(u.getNome());
                if (!existeEmQurls(subUrls, tUrl))
                {
                    subUrls.Enqueue(tUrl);
                    u.addSubUrl(temp);
                }
            }
            if (u.getNivel() < niv || niv == -1)
            {//Parse de <a href= (outras páginas, pdfs, videos, etc)
                raHref = new Regex("<a[^>]*href=(\"|')(.*?)\\1[^>]*>(.*?)</a>", rxOpt);

                for (m = raHref.Match(u.getCode()); m.Success; m = m.NextMatch())
                {
                    string temp = m.Groups[2].ToString();
                    if (temp != "#")
                    {
                        Url tUrl = new Url(temp, u.getNivel() + 1);
                        tUrl.setPastaDoPai(u.getPasta());
                        tUrl.setDominio(u.getDominio());
                        tUrl.setPai(u.getNome());
                        if (!existeEmQurls(subUrls, tUrl))
                        {
                            subUrls.Enqueue(tUrl);
                            u.addSubUrl(temp);
                        }
                    }
                }
            }

            //Parse de <link (css, )
            rHref = new Regex("<link[^>]*href=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rHref.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                Url tUrl = new Url(temp, u.getNivel() + 1);
                tUrl.setPastaDoPai(u.getPasta());
                tUrl.setDominio(u.getDominio());
                tUrl.setPai(u.getNome());
                if (!existeEmQurls(subUrls, tUrl))
                {
                    subUrls.Enqueue(tUrl);
                    u.addSubUrl(temp);
                }
            }
            //Parse de localização de Javascript
            rJs = new Regex("<script[^>]*src=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rJs.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                Url tUrl = new Url(temp, u.getNivel() + 1);
                tUrl.setPastaDoPai(u.getPasta());
                tUrl.setDominio(u.getDominio());
                tUrl.setPai(u.getNome());
                if (!existeEmQurls(subUrls, tUrl))
                {
                    subUrls.Enqueue(tUrl);
                    u.addSubUrl(temp);
                }
            }
            //Parse de frames
            rFrame = new Regex("<frame[^>]*src=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rFrame.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                Url tUrl = new Url(temp, u.getNivel() + 1);
                tUrl.setPastaDoPai(u.getPasta());
                tUrl.setDominio(u.getDominio());
                tUrl.setPai(u.getNome());
                if (!existeEmQurls(subUrls, tUrl))
                {
                    subUrls.Enqueue(tUrl);
                    u.addSubUrl(temp);
                }
            }
            //Parse de frames
            rIFrame = new Regex("<iframe[^>]*src=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rIFrame.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                Url tUrl = new Url(temp, u.getNivel() + 1);
                tUrl.setPastaDoPai(u.getPasta());
                tUrl.setDominio(u.getDominio());
                tUrl.setPai(u.getNome());
                if (!existeEmQurls(subUrls, tUrl))
                {
                    subUrls.Enqueue(tUrl);
                    u.addSubUrl(temp);
                }
            }
            //flashes e afins
            rFlash = new Regex("<param[^>]*value=(\"|')(.*?)\\1[^>]*>", rxOpt);
            for (m = rFlash.Match(u.getCode()); m.Success; m = m.NextMatch())
            {
                string temp = m.Groups[2].ToString();
                string[] temp2 = temp.Split('/');
                string[] temp3 = temp2[temp2.Length - 1].Split('.');
                if (temp3.Length > 1)
                {
                    Url tUrl = new Url(temp, u.getNivel() + 1);
                    tUrl.setPastaDoPai(u.getPasta());
                    tUrl.setDominio(u.getDominio());
                    tUrl.setPai(u.getNome());
                    if (!existeEmQurls(subUrls, tUrl))
                    {
                        u.getSubUrls().Add(temp);
                        u.addSubUrl(temp);
                    } 
                }
            }
        }


        public void corrigirUrl(Url u)
        {
 
            bool rel = false;
            if (u.getNome().StartsWith("http://"))
            {
                string[] tem;
                tem = u.getNome().Split('/');
                u.setDominio(tem[2]);
            }
            
            if(u.getNome().StartsWith(".."))
            {
                u.setNome(corrigirRelativo(u));
                rel = true;
            }

            if (u.getNome().StartsWith("/"))
                u.setNome("http://" + u.getDominio() + "/" + u.getNome());

            if (u.getNome().IndexOf(u.getDominio()) == -1 && rel)
                u.setNome(u.getDominio() + "/"  + u.getNome());
            if (u.getNome().IndexOf(u.getDominio()) == -1 && !rel)
                u.setNome(u.getDominio() + "/" + u.getPastaDoPai() + u.getNome());
            //----------------------
            //tira espaços em branco
           //nome = nome.Replace(" ", "");
            //----------------------

            if (!u.getNome().StartsWith("http://"))
            {
                u.setNome("http://" + u.getNome());
            }


            if (u.getNome().EndsWith("/"))
            {
                string k = u.getNome();
                k = k.Remove(u.getNome().Length - 1);
                u.setNome(k);
            }   

            string[]sp = u.getNome().Split('/');
            string[] sf = sp[sp.Length-1].Split('.');
            /*
            if (sf.Length < 2 && sp.Length > 3)
            {
                MyWebRequest req = new MyWebRequest(u.getNome());
                u.setFicheiro(req.getFicheiro());
                u.setNome(u.getNome() + "/" + u.getFicheiro());
            }

            if (sp.Length < 4)
            {
                MyWebRequest req = new MyWebRequest(u.getNome());
                u.setFicheiro(req.getFicheiro());
                u.setNome(u.getNome() + "/" + u.getFicheiro());
            }
            */
            string pasta="";
            string [] temp2 = u.getNome().Split('/');
            for (int i = 3; i < temp2.Length - 1; i++)
                pasta += temp2[i] + "/";
            u.setPasta(pasta);
        }


        private bool existeEmQurls(Queue subUrls, Url u)
        {
            int i;
            Queue temp = new Queue();
            temp = (Queue)(subUrls.Clone());
            Url t = new Url();
            for (i = 0; i < subUrls.Count; i++)
            {
                t = (Url)(temp.Dequeue());
                if (t.getNome() == u.getNome())
                    return true;
            }
            return false;
        }
        
        private string corrigirRelativo(Url u)
        {
            //   ../../arq/img/sub/image.gif
            // www.dominio.com artigos/int/coisas/
            string total="";
            int cont=0;
            while (u.getNome().StartsWith(".."))
            {
                u.setNome(u.getNome().Remove(0, 3));
                cont++;  // arq/img/sub/image.gi
            }

            string [] tempppai;
            tempppai=u.getPastaDoPai().Split('/');
            int j = tempppai.Length-2;
            while (cont>0)
            {
                tempppai[j]= tempppai[j].Remove(0);
                j--;
                cont--;
            }
            // artigos em tempppai[0] e pastafilho

            for (int k = 0; k < tempppai.Length-1; k++)
                if (tempppai[k].Length > 0)
                    total += tempppai[k] + "/";

            total = total + u.getNome();

            return total;
        }

        public void corrigeLinksComCaracteresEspeciais(Url u, Queue q)
        {
            if (q.Count > 0)
            {
                Queue temp = new Queue();
                Url turl = new Url();
                StringCollection sc = new StringCollection();
                temp = (Queue)(q.Clone());
                int i = 0;

                while (i < temp.Count)
                {
                    turl = (Url)(temp.Dequeue());
                    if (turl.getNome().IndexOf('?') != -1)
                        sc.Add(turl.getNome());
                    i++;
                }

                i = sc.Count - 1;
                int j;
                string codigo;
                foreach (object obj in sc)
                {
                    j = u.getCode().IndexOf(sc[i]);
                    string ts;
                    ts = sc[i].Replace('?', '-');
                    if (j != -1)
                    {
                        codigo = u.getCode();
                        codigo = codigo.Remove(j, ts.Length);
                        u.setCode(codigo.Insert(j, ts));
                    }
                    i--;
                }
            }
        }

        public void corrigirLinksRelativos(Url u)
        {

            string[] s = u.getPasta().Split('/');
            string ts = "";
            int i = 0, j, k;
            k = s.Length;
            while (k > 1)
            {
                ts += "../";
                k--;
            }
            StringCollection temp = new StringCollection();
            temp = u.getSubUrls();
            string codigo, t ="", asd;
            i = temp.Count - 1;
            foreach (object obj in temp)
            {
                if (obj.ToString().StartsWith("/"))
                {
                    asd = obj.ToString();
                    asd = asd.Remove(0, 1);
                    j = u.getCode().IndexOf(obj.ToString());
                    t += ts + asd;
                    
                    if (j != -1)
                    {
                        codigo = u.getCode();
                        codigo = codigo.Remove(j, obj.ToString().Length);
                        u.setCode(codigo.Insert(j, t));
                    }
                }
                t = "";
            }

        }

        public void setContent(Url u)
        {
            MyWebRequest req = new MyWebRequest(u);
            u.setTipo(req.Gettype());
            u.setSize(req.Getsize());
            u.setDate(req.GetLastModified());
            u.setFicheiro();
        }


        }



    }

