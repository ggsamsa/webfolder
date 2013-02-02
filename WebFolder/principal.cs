using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Threading;

namespace WebFolder
{
    public partial class principal : Form
    {
        
        
        StringCollection col = new StringCollection();
        StringCollection qProjectos = new StringCollection();
        Thread t;
        Motor m;

        public principal()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sobre s = new Sobre();
            s.ShowDialog();
        }

        private void novoPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proproj temp = new Proproj();
            temp.ShowDialog();
            actualizarListaDeProjectos();
            listView1.Invalidate();
        }

        private void propriedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mostraPropriedades();
        }

        private void novaTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proproj p = new Proproj();
            p.usar_template();
            p.ShowDialog();
        }

        private void principal_Load(object sender, EventArgs e)
        {
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            actualizarListaDeProjectos();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                listView3.Items.Add(listView1.SelectedItems[0].Text);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            t = new Thread(new ThreadStart(run));
         
            if (toolStripButton1.Checked == true)
            {
                toolStripButton1.Checked = false;
                toolStripButton1.Text = "Começar"; 
            }
            else
            {
                if (listView3.Items.Count > 0)
                {
                    toolStripButton1.Checked = true;
                    toolStripButton1.Text = "Parar";
                    t.Start();
                }
                else MessageBox.Show("Queue Vazia");
            }
        }

        private void run()
        {

            Projecto p = new Projecto();
            BD bd = new BD();
            p = bd.lerProjecto(listView3.Items[0].Text);
            if (toolStripButton4.Checked == true)
                m = new Motor(p, log, 1, label5, label6, label7, label8);
            else
                m = new Motor(p, log, 0, label5, label6, label7, label8);
            Url u = new Url();
            u.setNome(p.getUrlInicial());
            m.run(u);
            passaAoProximoProjecto();
        }

        private void actualizarListaDeProjectos()
        {
            BD bd = new BD();
            col = bd.lerProjectos();
            string[] ss;
            int i = 0;
            listView1.Items.Clear();
            foreach (string s in col)
            {
                ss = s.Split(';');
                listView1.Items.Add(ss[0]);
                listView1.Items[i].SubItems.Add(ss[1]);
                i++;
            }
            listView1.Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (t != null)
            {
                ThreadState s = new ThreadState();
                s = t.ThreadState;
                if (s.ToString() == "Running")
                    t.Suspend();
                else if (s.ToString() == "Suspended")
                    t.Resume();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if ((MessageBox.Show("Tem a certeza que quer remover \"" + listView1.SelectedItems[0].Text + "\" ?", "Remoção!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    BD bd = new BD();
                    bd.removerProjecto(listView1.SelectedItems[0].Text);
                    actualizarListaDeProjectos();
                    listView1.Invalidate();
                }
            }
            else MessageBox.Show("Seleccione o projecto a remover!");
        }

        private void Propriedades_Click(object sender, EventArgs e)
        {
            mostraPropriedades();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                listView3.SelectedItems[0].Remove();
            }
        }

        private void mostraPropriedades()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                BD bd = new BD();
                Projecto temp = new Projecto();
                temp = bd.lerProjecto(listView1.SelectedItems[0].Text);
                Proproj prop = new Proproj(temp);
                prop.ShowDialog();
            }
        }

        private void passaAoProximoProjecto()
        {

            for (int i = 0; i < listView3.Items.Count; i++)
            {
                if (i + 1 < listView3.Items.Count)
                {
                    listView3.Items[i] = (ListViewItem)(listView3.Items[i + 1].Clone());
                    listView3.Items[i + 1].Remove();
                }

                else if (listView3.Items.Count == 1)
                    listView3.Items[0].Remove();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            mostraPropriedades();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void log_TextChanged(object sender, EventArgs e)
        {
           // log.Cursor = (int)(log.TextLength);
            log.SelectionStart = log.TextLength;
            log.ScrollToCaret();
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton1.Checked == false)
                m.setparar(0);
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apagarProjectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if ((MessageBox.Show("Tem a certeza que quer remover \"" + listView1.SelectedItems[0].Text + "\" ?", "Remoção!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    BD bd = new BD();
                    bd.removerProjecto(listView1.SelectedItems[0].Text);
                    actualizarListaDeProjectos();
                    listView1.Invalidate();
                }
            }
            else MessageBox.Show("Seleccione o projecto a remover!");
        }
    }
}