using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace WebFolder
{
    public partial class Proproj : Form
    {
        Projecto p = new Projecto();
        int i;
        String filtros;
        Definicoes def = new Definicoes();

        public Proproj()
        {
            InitializeComponent();
        }

        public Proproj(Projecto temp)
        {
            i = 1;
            p = temp;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
            p.setPasta(textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Proproj_Load(object sender, EventArgs e)
        {
            BD bd = new BD();
            def = bd.getDefinicoes();
            StringCollection sc = new StringCollection();
            sc = def.getExtensoes();
            foreach (string s in sc)
            {
                comboBox3.Items.Add(s);
            }
            numericUpDown2.Enabled = false;
            if (i == 1)
            {
                textBox1.Enabled = false;
                textBox1.Text = p.getNome();
                textBox2.Text = p.getUrlInicial();
                textBox3.Text = p.getPasta();
                if (p.getvisualizacaolocal() == 1)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }

                if (p.getNivel() == -1)
                {
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                }

                if (p.getNivel() == 0)
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = true;
                }

                if (p.getNivel() > 0)
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = true;
                    radioButton5.Checked = false;
                    numericUpDown2.Enabled = true;
                    numericUpDown2.Value = p.getNivel();
                }

                if (p.getImagensRemotas() == 1)
                    checkBox16.Checked = true;

                if (p.getFiltros() != null)
                    carregar_filtros(p.getFiltros());

                if (p.getAceitarRejeitar() == 1)
                {
                    radioButton7.Checked = false;
                    radioButton8.Checked = true;
                }
                else
                {
                    radioButton7.Checked = true;
                    radioButton8.Checked = false;
                }


            }

           numericUpDown1.Visible=false;
           dateTimePicker1.Visible=false;
           button7.Visible = false;
           label11.Visible = false;
           textBox4.Visible = false;
           comboBox3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s = "";
            if (comboBox1.SelectedIndex == 0)
            {
                s+="extensao ";
                if (comboBox2.SelectedIndex == 0)
                    s += "= ";
                else s += "!= ";

                s += textBox4.Text;
            }

            if (comboBox1.SelectedIndex == 1)
            {
                s += "tamanho ";
                if (comboBox2.SelectedIndex == 0)
                    s += "< ";
                else s += "> ";

                s += numericUpDown1.Value.ToString();
            }

            if (comboBox1.SelectedIndex == 2)
            {
                s += "data ";
                if (comboBox2.SelectedIndex == 0)
                    s += "> ";
                else s += "< ";

                DateTimeConverter d = new DateTimeConverter();
                string temp;
                temp = d.ConvertToString(dateTimePicker1.Value.Date);
                s += temp;
            }

            if (comboBox1.SelectedIndex == 3)
            {
                s += "nome ";
                s += "contem ";
                s += textBox4.Text;
            }

            listBox1.Items.Add(s);

            if (comboBox1.SelectedIndex == 0)
                if (textBox4.Text != null)
                    actualizar_combo_extensoes(textBox4.Text);
                
        }
        
        bool testalista(string s)
        {
            int i;
            int a = listBox1.SelectedItems.Count;
            for (i=0; i < a; i++)
                if (listBox1.Items.ToString() == s)
                    return false;
            return true;
        }
        
        public void usar_template()
        {
            groupBox6.Enabled = false;
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection col = new ListBox.SelectedIndexCollection(listBox1);
            ListBox.SelectedObjectCollection col2 = new ListBox.SelectedObjectCollection(listBox1);
            col = listBox1.SelectedIndices;
            col2 = listBox1.SelectedItems;

            string s = "";
            foreach (object obj in col2)
            {
                s += obj.ToString();
            }

            if (col.Count == 2  && !s.Contains(" E "))
            {
                listBox1.Items.Insert(col[0], col2[0].ToString() + " E " + col2[1].ToString());
                listBox1.Items.RemoveAt(col[0]);
                listBox1.Items.RemoveAt(col[0]);
            }
            else
            {
                MessageBox.Show("Seleccione dois Filtros Únicos!");
            }

            listBox1.ClearSelected();
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection indices = listBox1.SelectedIndices;
            int i;
            for (i= indices.Count; i > 0; i--)
                listBox1.Items.Remove(listBox1.Items[indices[i-1]]);
            listBox1.SelectedItems.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BD bd = new BD();
            if (i == 1)
            {
                Projecto p = new Projecto();
                p = constroiProjecto();
                bd.actualizarProjecto(p);
                this.Close();
            }
            else
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                    MessageBox.Show("Preencha todos os campos!");
                else
                {
                    Projecto p = new Projecto();
                    p = constroiProjecto();
                    bd.gravarProjecto(p);
                    this.Close();
                }
            }

            constroiDefinicoes();

            bd.gravarDefinicoes(def);

        }

        private void constroiDefinicoes()
        {
            string s = "";
            int i = 0;
            foreach (string x in comboBox3.Items)
            {
                s += x.ToString();
                if (i < comboBox3.Items.Count -1)
                    s += ", ";
                i++;
            }

            def.setExtensoes(s);
        }

        private Projecto constroiProjecto()
        {

            Projecto p = new Projecto();
            p.setNome(textBox1.Text);
            p.setUrlInicial(textBox2.Text);
            p.setNivel((int)(numericUpDown2.Value));
            p.setPasta(textBox3.Text);

            if (radioButton3.Checked)
                p.setNivel(-1);
            if (radioButton4.Checked)
                p.setNivel((int)(numericUpDown2.Value));
            if (radioButton5.Checked)
                p.setNivel(0);
            if (checkBox16.Checked) p.setImagensRemotas(1);
            else p.setImagensRemotas(0);

            if (radioButton1.Checked) p.setvisualizacaolocal(1);
            else p.setvisualizacaolocal(0);

            criar_string_filtros();
            p.setFiltros(filtros);
            if (radioButton7.Checked) p.setAceitarRejeitar(0);
            else p.setAceitarRejeitar(1);

            return p;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                numericUpDown2.Enabled = true;
            else
                numericUpDown2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mudar_opcoes_combos();
        }

        private void mudar_opcoes_combos()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox4.Visible = true;
                textBox4.Clear();
                comboBox3.Visible = true;
                button7.Visible = true;
                numericUpDown1.Visible = false;
                dateTimePicker1.Visible = false;
                label11.Visible = false;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("=");
                comboBox2.Items.Add("!=");
                comboBox2.SelectedIndex = 0;
            }

            if (comboBox1.SelectedIndex == 1)
            {
                textBox4.Visible = false;
                textBox4.Clear();
                comboBox3.Visible = false;
                button7.Visible = false;
                numericUpDown1.Visible = true;
                dateTimePicker1.Visible = false;
                label11.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("<");
                comboBox2.Items.Add(">");
                comboBox2.SelectedIndex = 0;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                textBox4.Visible = false;
                textBox4.Clear();
                comboBox3.Visible = false;
                button7.Visible = false;
                numericUpDown1.Visible = false;
                dateTimePicker1.Visible = true;
                label11.Visible = false;
                comboBox2.Items.Clear();
                comboBox2.Items.Add(">");
                comboBox2.Items.Add("<");
                comboBox2.SelectedIndex = 0;
            }

            if (comboBox1.SelectedIndex == 3)
            {
                textBox4.Visible = true;
                textBox4.Clear();
                comboBox3.Visible = false;
                button7.Visible = false;
                numericUpDown1.Visible = false;
                dateTimePicker1.Visible = false;
                label11.Visible = false;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("contem");
                comboBox2.SelectedIndex = 0;
            }

            


        }

        private void criar_string_filtros()
        {
            for (int i=0; i < listBox1.Items.Count; i++)
            {
                filtros += listBox1.Items[i].ToString();
                if (i < listBox1.Items.Count-1)
                    filtros += " | ";
            }
        }

        private void carregar_filtros(string stringfiltros)
        {
            string[] fonte = stringfiltros.Split('|');
            for (int i = 0; i < fonte.Length; i++)
            {
                if (i > 0)
                {
                    fonte[i] = fonte[i].Remove(0, 1);
                }
                listBox1.Items.Add(fonte[i]);
            }
        }

        private void actualizar_combo_extensoes(string s)
        {
            bool res=false;
            for (int i = 0; i < comboBox3.Items.Count; i++)
                if (s == comboBox3.Items[i].ToString())
                    res = true;
            if (res == false)
                comboBox3.Items.Add(s);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = comboBox3.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string s = textBox4.Text;
            int i = 0;
            int res=-1;
            foreach (object obj in comboBox3.Items)
            {
                if (obj.ToString() == s)
                    res = i;
                i++;
            }
            if (res >= 0)
                comboBox3.Items.Remove(comboBox3.Items[res]);
        }

    }
}