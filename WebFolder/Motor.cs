using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.ComponentModel;
using System.Threading;

namespace WebFolder
{
    class Motor
    {
        Queue qUrls = new Queue();
        Queue temp = new Queue();
        RichTextBox log;
        int fNivel, actualizar = 0;
        string pasta;
        FiltroTipoDiferente ftd;
        FiltroTipoIgual fti;
        FiltroTamanhoMenor ftme;
        FiltroTamanhoMaior ftma;
        FiltroNome fn;
        Filtro filtros;
        FiltroOu actual;
        FiltroDataMenor fdme;
        FiltroDataMaior fdma;
        Projecto p = new Projecto();
        int x = 0, contfil =0, geral = 1;
        Label l1, l2, l3, l4;


        public Motor(Projecto temp, RichTextBox r, int act, Label la1, Label la2, Label la3, Label la4)
        {
            log = r;
            r.Text = "Motor criado\n";
            pasta = temp.getPasta();
            fNivel = temp.getNivel();
            p = temp;
            criarfiltro(p.getFiltros());
            actualizar = act;
            l1 = la1;
            l2 = la2;
            l3 = la3;
            l4 = la4;
            l1.Text = "0";
            l2.Text = "0";
            l3.Text = "0";
            l4.Text = "0";
            l1.Visible = true;
            l2.Visible = true;
            l3.Visible = true;
            l4.Visible = true;
        }

        public void run(Url url)
        {
            if (geral == 0)
                qUrls.Clear();

            x++;
            l1.Text = x.ToString();
            l2.Text = temp.Count.ToString(); ;
            l3.Text = qUrls.Count.ToString();
            l4.Text = contfil.ToString();

            if (url.getNome().Contains("#"))
                if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
            
            url.corrigirUrl();
            
            log.Text += url.getNome() + "\n";

            string[] conta = url.getNome().Split(':');
            if (conta.Length > 2)
            {
                log.Text += "-> Rejeitado;\n";
                if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
            }

            try
            {
                log.Text += "Info: ";
                url.setcontent();
                log.Text += "OK ";
            }
            catch 
            {
                log.Text += "-> Rejeitado;\n";
                if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
            }

            string[] domproj = p.getUrlInicial().Split('/');
            string dominioprojecto = "";
           // for (int t = 1; t < domproj.Length - 2; t++)
            dominioprojecto = domproj[2];

            if (url.getNome().Contains(dominioprojecto))
            {
                if (url.gettipo().Contains("html"))
                {
                    log.Text += "A processar: ";
                    url.setBody();
                    log.Text += ".";
                    url.setUrls(qUrls, fNivel);
                    log.Text += ".";
                    url.corrigeLinksComCaracteresEspeciais(qUrls);
                    log.Text += ".";
                    url.corrigirLinksRelativos();
                    log.Text += "OK ";
                    if (p.getvisualizacaolocal() == 0)
                    {
                        log.Text += "A converter links:";
                        url.transformarEmRelativo();
                        log.Text += "OK ";
                    }
                }

                if (filtros != null)
                    log.Text += "Filtragem: ";

                if (actualizar == 1)
                {
                    if (p.getData().CompareTo(url.getDate()) < 0)
                    {
                        if (p.getAceitarRejeitar() == 0)
                        {
                            if (filtros != null)
                            {
                                if (filtros.execute(url))
                                {
                                    log.Text += "Rejeitado\n";
                                    contfil++;
                                    if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                                }
                                else
                                {
                                    log.Text += "Aceite ";
                                    continuar(url);
                                }
                            }
                            else continuar(url);
                        }
                        else
                        {
                            if (filtros != null)
                            {
                                if (filtros.execute(url))
                                {
                                    log.Text += "Aceite ";
                                    continuar(url);
                                }
                                else
                                {
                                    log.Text += "Rejeitado\n";
                                    contfil++;
                                    if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                                }
                            }
                            else continuar(url);
                        }
                    }
                    else
                    {
                        log.Text += "Rejeitado\n";
                        if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                    }
                }
                else
                {
                    if (p.getAceitarRejeitar() == 0)
                    {
                        if (filtros != null)
                        {

                            if (filtros.execute(url))
                            {
                                log.Text += "Rejeitado\n";
                                if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                            }
                            else
                            {
                                log.Text += "Aceite ";
                                continuar(url);
                            }
                        }
                        else continuar(url);
                    }
                    else
                    {
                        if (filtros != null)
                        {
                            if (filtros.execute(url))
                            {
                                log.Text += "Aceite ";
                                continuar(url);
                            }
                            else
                            {
                                log.Text += "Rejeitado\n";
                                if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                            }
                        }
                        else continuar(url);
                    }
                }
            }
            else
            {
                if (url.gettipo() == "css" || url.gettipo() == "javascript" || (p.getImagensRemotas() == 1 && (url.gettipo() == "jpeg" || url.gettipo() == "gif" || url.gettipo() == "jpg" || url.gettipo() == "png" || url.gettipo() == "bmp")))
                {
                    continuar(url);
                }
                else
                {
                    log.Text += "Fora do domínio\n";
                    if (qUrls.Count > 0) run((Url)(qUrls.Dequeue()));
                }
            }
        }


        private void continuar(Url url)
        {
            log.Text += "A gravar: ";
            if (Gravar(url))
            {
                log.Text += "OK\n";
            }
            else
                log.Text += "Não gravou \n";
            temp.Enqueue(url);
        if (qUrls.Count > 0)
        {
            run((Url)(qUrls.Dequeue()));
        }
        else log.Text += "FIM\n";
        }

        private Queue getQUrls()
        {
            return temp;
        }

        private bool Gravar(Url u)
        {
            MyGravar g = new MyGravar(u, pasta);
            return g.getResultado();
        }


        private void criarfiltro(string lfiltros)
        {
            lfiltros = lfiltros.Replace(" E ", "&");

            string[] fil = lfiltros.Split('|');

            for (int i = 0; i < fil.Length; i++)
            {
                if (fil[i].Contains("&"))
                {
                    string[] separar = fil[i].Split('&');

                    int res = procura_filtros(separar[0]);
                    if (res == 1)
                        inserir_na_lista(ftd);
                    if (res == 2)
                        inserir_na_lista(fti);
                    if (res == 3)
                        inserir_na_lista(ftme);
                    if (res == 4)
                        inserir_na_lista(ftma);
                    if (res == 5)
                        inserir_na_lista(fn);
                    if (res == 6)
                        inserir_na_lista(fdme);
                    if (res == 7)
                        inserir_na_lista(fdma);

                    res = procura_filtros(separar[1]);
                    if (res == 1)
                        inserir_na_lista(ftd);
                    if (res == 2)
                        inserir_na_lista(fti);
                    if (res == 3)
                        inserir_na_lista(ftme);
                    if (res == 4)
                        inserir_na_lista(ftma);
                    if (res == 5)
                        inserir_na_lista(fn);
                    if (res == 6)
                        inserir_na_lista(fdme);
                    if (res == 7)
                        inserir_na_lista(fdma);
                }
                else
                {
                    int res = procura_filtros(fil[i]);
                    if (res == 1)
                        inserir_na_lista_ou(ftd);
                    if (res == 2)
                        inserir_na_lista_ou(fti);
                    if (res == 3)
                        inserir_na_lista_ou(ftme);
                    if (res == 4)
                        inserir_na_lista_ou(ftma);
                    if (res == 5)
                        inserir_na_lista_ou(fn);
                    if (res == 6)
                        inserir_na_lista_ou(fdme);
                    if (res == 7)
                        inserir_na_lista_ou(fdma);
                }
            }
            //Url u = new Url();
            //bool resul = filtros.execute(u);
            //textBox2.Text = resul.ToString();
        }

        private void inserir_na_lista(FiltroTamanhoMenor f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroTamanhoMaior f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroTipoDiferente f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroTipoIgual f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroNome f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroDataMenor f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }

        private void inserir_na_lista(FiltroDataMaior f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                actual.dnext2().setNext(f);
            }
        }
        private int procura_filtros(string fonte)
        {
            if (fonte.Contains("!="))
            {
                string[] valor = fonte.Split('=');
                valor[1] = valor[1].Trim();
                ftd = new FiltroTipoDiferente(valor[1]);
                return 1;
            }
            else if (fonte.Contains("="))
            {
                string[] valor = fonte.Split('=');
                valor[1] = valor[1].Trim();
                fti = new FiltroTipoIgual(valor[1]);
                return 2;
            }
            else if (fonte.Contains("<") && !fonte.Contains("-"))
            {
                string[] valor = fonte.Split('<');
                valor[1] = valor[1].Trim();
                int tam = Convert.ToInt32(valor[1]);
                ftme = new FiltroTamanhoMenor(tam);
                return 3;
            }
            else if (fonte.Contains(">") && !fonte.Contains("-"))
            {
                string[] valor = fonte.Split('>');
                valor[1] = valor[1].Trim();
                int tam = Convert.ToInt32(valor[1]);
                ftma = new FiltroTamanhoMaior(tam);
                return 4;
            }
            else if (fonte.Contains("contem"))
            {
                fonte = fonte.Replace("contem", "%");
                string[] valor = fonte.Split('%');
                valor[1] = valor[1].Trim();
                string pesquisa = valor[1];
                fn = new FiltroNome(pesquisa);
                return 5;

            }
            else if (fonte.Contains("-") && fonte.Contains("<"))
            {
                DateTimeConverter conv = new DateTimeConverter();
                DateTime d = new DateTime();
                string[] partes = fonte.Split('<');
                partes[1] = partes[1].Trim();
                fonte = partes[1];
                d = (DateTime)(conv.ConvertFromString(fonte));
                fdme = new FiltroDataMenor(d);
                return 6;

            }

            else if (fonte.Contains("-") && fonte.Contains(">"))
            {
                DateTimeConverter conv = new DateTimeConverter();
                DateTime d = new DateTime();
                string[] partes = fonte.Split('>');
                partes[1] = partes[1].Trim();
                fonte = partes[1];
                d = (DateTime)(conv.ConvertFromString(fonte));
                fdma = new FiltroDataMaior(d);
                return 7;

            }
            else return 0;
        }


        private void inserir_na_lista_ou(FiltroTipoDiferente f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroTipoIgual f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroTamanhoMenor f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroTamanhoMaior f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroNome f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroDataMenor f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        private void inserir_na_lista_ou(FiltroDataMaior f)
        {
            if (filtros == null)
            {
                FiltroOu ou = new FiltroOu();
                filtros = ou;
                ou.setNext2(f);
                actual = ou;
            }
            else
            {
                FiltroOu ou = new FiltroOu();
                actual.setNext(ou);
                ou.setNext2(f);
                actual = ou;
            }
        }

        public void setparar(int i)
        {
            geral = i;
        }


    }
}
