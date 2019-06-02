using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization.Charting;

namespace Evidencija_Radnika_Pavlovic
{
    public partial class frmAnaliza : Form
    {
        OleDbCommand komand;
        OleDbConnection konek;
        OleDbDataReader reader;
        public frmAnaliza()
        {
            InitializeComponent();
            string putanja = Environment.CurrentDirectory;
            string[] putanjaBaze = putanja.Split(new string[] { "bin" }, StringSplitOptions.None);
            AppDomain.CurrentDomain.SetData("DataDirectory", putanjaBaze[0]);
            konek = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= |DataDirectory|\Evidencija radnika.mdb");
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string godina = maskedTextBox1.Text;
            string[] naziv = new string[13];
            bool ima = false;
            bool[] projekatZavrsen = new bool[13];
            int[] budzet = new int[13];
            int i = 0;
            try
            {
                konek.Open();
                komand = new OleDbCommand("Select DatumPocetka,Budzet,ProjekatZavrsen, Naziv from PROJEKAT ", konek);
                reader = komand.ExecuteReader();
                while (reader.Read())
                {
                    string datum = (Convert.ToDateTime(reader[0].ToString()).Year).ToString();
                    //string[] puta = datum.Split(new string[] { "/" }, StringSplitOptions.None);
                    if (godina == datum)
                    {
                        budzet[i] = Convert.ToInt32(reader[1].ToString());
                        projekatZavrsen[i] = Convert.ToBoolean(reader[2].ToString());
                        naziv[i] = reader[3].ToString();
                        ima = true;
                        Series series = chart1.Series.Add(naziv[i].ToString());
                        series.Points.Add(budzet[i]);
                    }
                    
                i++;
                }
                



            }
            catch(System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska!");
            }
            finally
            {
                konek.Close();
            }

        }

        private void frmAnaliza_Load(object sender, EventArgs e)
        {
            chart1.Palette = ChartColorPalette.Fire;
            chart1.Titles.Add("Budzet");
            
        }
    }
}
