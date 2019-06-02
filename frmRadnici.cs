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

namespace Evidencija_Radnika_Pavlovic
{
    public partial class frmRadnici : Form
    {
        OleDbCommand komand;
        OleDbConnection konek;
        OleDbDataReader reader;
        public frmRadnici()
        {
            InitializeComponent();
            string putanja = Environment.CurrentDirectory;
            string[] putanjaBaze = putanja.Split(new string[] { "bin" }, StringSplitOptions.None);
            AppDomain.CurrentDomain.SetData("DataDirectory", putanjaBaze[0]);
            konek = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= |DataDirectory|\Evidencija radnika.mdb");

        }

        int[] dani = new int[31];
        int[] meseci = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRadnici_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'evidencija_radnikaDataSet.RADNIK' table. You can move, or remove it, as needed.
            this.rADNIKTableAdapter.Fill(this.evidencija_radnikaDataSet.RADNIK);
            for (int i = 0; i < 12; i++)
            {
                comboBox1.Items.Add(meseci[i]);
                comboBox3.Items.Add(meseci[i]);
            }
            for(int j = 0; j < 31; j++)
            {
                dani[j] = j + 1;

            }

            try
            {
                konek.Open();
                komand = new OleDbCommand("Select RadnikID from Radnik order by RadnikID ASC", konek);
                reader = komand.ExecuteReader();
                while(reader.Read())
                {
                    comboBox6.Items.Add(reader[0]);
                }

            }
            catch(System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska");
            }
            finally
            {
                komand.Dispose();
                reader.Close();
                konek.Close();
            }
            try
            {
                konek.Open();
                komand = new OleDbCommand("Select distinct KvalifikacijaID from Radnik order by KvalifikacijaID ASC", konek);
                reader = komand.ExecuteReader();
                while (reader.Read())
                {
                    comboBox5.Items.Add(reader[0]);
                }

            }
            catch (System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska");
            }
            finally
            {
                komand.Dispose();
                reader.Close();
                konek.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (Convert.ToInt32(comboBox1.SelectedItem.ToString()) % 2 == 1)
            {
                for (int j = 0; j < 31; j++)
                {
                    comboBox2.Items.Add(dani[j]);
                }
            }
            else if (Convert.ToInt32(comboBox1.SelectedItem.ToString()) % 2 == 0 && Convert.ToInt32(comboBox1.SelectedItem.ToString()) != 2)
            {
                for (int j = 0; j < 30; j++)
                {
                    comboBox2.Items.Add(dani[j]);
                }
            }
            else if (Convert.ToInt32(comboBox1.SelectedItem.ToString()) == 2)
            {
                for (int j = 0; j < 29; j++)
                {
                    comboBox2.Items.Add(dani[j]);
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            if (Convert.ToInt32(comboBox3.SelectedItem.ToString()) % 2 == 1)
            {
                for (int j = 0; j < 31; j++)
                {
                    comboBox4.Items.Add(dani[j]);
                }
            }
            else if (Convert.ToInt32(comboBox3.SelectedItem.ToString()) % 2 == 0 && Convert.ToInt32(comboBox3.SelectedItem.ToString()) != 2)
            {
                for (int j = 0; j < 30; j++)
                {
                    comboBox4.Items.Add(dani[j]);
                }
            }
            else if (Convert.ToInt32(comboBox3.SelectedItem.ToString()) == 2)
            {
                for (int j = 0; j < 29; j++)
                {
                    comboBox4.Items.Add(dani[j]);
                }
            }
        }

        public void Upisi()
        {
            string ime, prezime;
            int sifra;
            int darR, danZ, mesR, mesZ, kvalifikacija;
            string godR, godZ;

            if (comboBox6.Text == "")
            {
                MessageBox.Show("Sifra je obavezno polje");
                comboBox6.Focus();
                return;
            }
            if (maskedTextBox2.Text == "")
            {
                MessageBox.Show("Ime je obavezno polje");
                maskedTextBox2.Focus();
                return;
            }
            if (maskedTextBox3.Text == "")
            {
                MessageBox.Show("Prezime je obavezno polje");
                maskedTextBox3.Focus();
                return;
            }

            ime = maskedTextBox2.Text;
            prezime = maskedTextBox3.Text;
            sifra = Convert.ToInt32(comboBox6.Text);

            mesR = Convert.ToInt32(comboBox1.Text);
            mesZ = Convert.ToInt32(comboBox3.Text);
            darR = Convert.ToInt32(comboBox2.Text);
            danZ = Convert.ToInt32(comboBox4.Text);
            godR = maskedTextBox4.Text;
            godZ = maskedTextBox5.Text;
            kvalifikacija = Convert.ToInt32(comboBox5.Text);
            string datumR = mesR.ToString() + "/" + darR.ToString() + "/" + godR;
            string datumZ = mesZ.ToString() + "/" + danZ.ToString() + "/" + godZ;

            
                string insert = "Insert into [RADNIK]([RadnikID],[Ime],[Prezime],[Datum Rodjenja],[Datum Zaposlenja],[KvalifikacijaID])";
                string values = "values(@RadnikID, @Ime, @Prezime, @DatumR, @DatumZ, @KvalifikacijaID)";
                OleDbCommand komanda = new OleDbCommand(insert + values, konek);

                komanda.Parameters.AddWithValue("@RadnikID", sifra);
                komanda.Parameters.AddWithValue("@Ime", ime);
                komanda.Parameters.AddWithValue("@Prezime", prezime);
                komanda.Parameters.AddWithValue("@Datum Rodjenja", Convert.ToDateTime(datumR));
                komanda.Parameters.AddWithValue("@Datum Zaposlenja", Convert.ToDateTime(datumZ));
                komanda.Parameters.AddWithValue("KvalifikacijaID", kvalifikacija);
            try
            {
                konek.Open();
                komanda.ExecuteNonQuery();
                MessageBox.Show("Uspesno uneto");
            }
            catch(System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska!");
            }
            finally
            {
                komanda.Dispose();
                konek.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Upisi();

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            bool postoji = false;
            int id = Convert.ToInt32(comboBox6.Text);
            try
            {
                konek.Open();
                OleDbCommand citaj = new OleDbCommand("Select RadnikId from Radnik", konek);
                reader = citaj.ExecuteReader();
                while (reader.Read())
                {
                    if(reader[0].ToString() == id.ToString())
                    {
                        postoji = true;
                    }
                }
            }
            catch(System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska!");
            }
            finally{
                reader.Close();
                konek.Close();
            }
            if (postoji == false)
            {
                MessageBox.Show("Ne postoji takav radnik");
            }
            else
            {
                try
                {

                    konek.Open();
                    OleDbCommand brisi = new OleDbCommand("Delete from Radnik where RadnikID = @id", konek);
                    brisi.Parameters.Add("@id",id);
                    brisi.ExecuteNonQuery();
                    
                    
                }
                catch (System.Data.OleDb.OleDbException ex1)
                {
                    MessageBox.Show(ex1.ToString(), "Greska!");
                }
                finally
                {
                    MessageBox.Show("Uspesno obrisan radnik");
                    konek.Close();
                }
            }
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool postoji = false;
            int id = Convert.ToInt32(comboBox6.Text);

            try
            {
                konek.Open();
                OleDbCommand citaj = new OleDbCommand("Select RadnikId from Radnik", konek);
                reader = citaj.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() == id.ToString())
                    {
                        postoji = true;
                    }
                }
            }
            catch (System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska!");
            }
            finally
            {
                reader.Close();
                konek.Close();
            }
            if (postoji == false)
            {
                MessageBox.Show("Ne postoji takav radnik");
            }
            else
            {
                string ime, prezime;
                int sifra;
                int darR, danZ, mesR, mesZ, kvalifikacija;
                string godR, godZ;

                if (comboBox6.Text == "")
                {
                    MessageBox.Show("Sifra je obavezno polje");
                    comboBox6.Focus();
                    return;
                }
                if (maskedTextBox2.Text == "")
                {
                    MessageBox.Show("Ime je obavezno polje");
                    maskedTextBox2.Focus();
                    return;
                }
                if (maskedTextBox3.Text == "")
                {
                    MessageBox.Show("Prezime je obavezno polje");
                    maskedTextBox3.Focus();
                    return;
                }

                ime = maskedTextBox2.Text;
                prezime = maskedTextBox3.Text;
                sifra = Convert.ToInt32(comboBox6.Text);

                mesR = Convert.ToInt32(comboBox1.Text);
                mesZ = Convert.ToInt32(comboBox3.Text);
                darR = Convert.ToInt32(comboBox2.Text);
                danZ = Convert.ToInt32(comboBox4.Text);
                godR = maskedTextBox4.Text;
                godZ = maskedTextBox5.Text;
                kvalifikacija = Convert.ToInt32(comboBox5.Text);
                string datumR = mesR.ToString() + "/" + darR.ToString() + "/" + godR;
                string datumZ = mesZ.ToString() + "/" + danZ.ToString() + "/" + godZ;

                OleDbCommand komanda = new OleDbCommand("UPDATE [Radnik] SET [Ime]=@Ime, [Prezime]=@Prezime, [Datum Rodjenja]=@Datum_Rodjenja, [Datum Zaposlenja] = @Datum_Zaposlenja, [KvalifikacijaID] = @KvalifikacijaId WHERE [RadnikID] = @RadnikID", konek);
                
                komanda.Parameters.AddWithValue("@RadnikID", sifra);
                komanda.Parameters.AddWithValue("@Ime", ime);
                komanda.Parameters.AddWithValue("@Prezime", prezime);
                komanda.Parameters.AddWithValue("@Datum_Rodjenja", Convert.ToDateTime(datumR));
                komanda.Parameters.AddWithValue("@Datum_Zaposlenja", Convert.ToDateTime(datumZ));
                komanda.Parameters.AddWithValue("KvalifikacijaID", kvalifikacija);

                
                
                try
                {
                    konek.Open();
                    komanda.ExecuteNonQuery();
                    MessageBox.Show("Uspesno uneto");
                }
                catch (System.Data.OleDb.OleDbException ex1)
                {
                    MessageBox.Show(ex1.ToString(), "Greska!");
                }
                finally
                {
                    konek.Close();
                }
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool postoji = false;
            int id = Convert.ToInt32(comboBox6.Text);
            try
            {
                konek.Open();
                OleDbCommand citaj = new OleDbCommand("Select * from Radnik order by RadnikID ASC", konek);
                reader = citaj.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() == id.ToString())
                    {
                        postoji = true;
                        maskedTextBox2.Text = reader[1].ToString();
                        maskedTextBox3.Text = reader[2].ToString();
                        string godinaR = reader[3].ToString();
                        string godinaZ = reader[4].ToString();
                        comboBox5.Text = reader[5].ToString();

                        string[] par1 = godinaR.Split(new string[] {"/"}, StringSplitOptions.None);
                        comboBox1.Text = par1[0];
                        comboBox2.Text = par1[1];
                        maskedTextBox4.Text = par1[2];
                        string[] par2 = godinaZ.Split(new string[] { "/" }, StringSplitOptions.None);
                        comboBox3.Text = par2[0];
                        comboBox4.Text = par2[1];
                        maskedTextBox5.Text = par2[2];


                    }
                    
                }
            }
            catch (System.Data.OleDb.OleDbException ex1)
            {
                MessageBox.Show(ex1.ToString(), "Greska!");
            }
            finally
            {
                reader.Close();
                konek.Close();
            }
            if (postoji == false)
            {
                MessageBox.Show("Ne postoji takav radnik");
            }


        }
    }
    
}
