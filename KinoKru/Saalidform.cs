using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace KinoKru
{
	public partial class Saalidform : Form
	{
        int i, j;
        Label[,] _arr;
        StreamWriter to_file;
        
        Button osta, kinni;
        bool ost = true;
        List<string> arr_pilet;
        TextBox Nimitxt, Emailtxt;
        int[] _tag;
        public Saalidform(int i_, int j_)
        {
            Nimitxt = new TextBox { Text ="Nimi",Location = new Point(10,320)};
            Emailtxt = new TextBox { Text="Email", Location = new Point(10, 350) };
            this.Text = "Ap_polo_kino";
            this.Controls.Add(Nimitxt);
            this.Controls.Add(Emailtxt);
            osta = new Button();
            osta.Text = "Osta";
            osta.Location = new Point(10, 380);
            this.Controls.Add(osta);
            osta.Click += Osta_Click;
            kinni = new Button();
            kinni.Text = "Kinni";
            kinni.Location = new Point(10, 410);
            this.Controls.Add(kinni);
            kinni.Click += Kinni_Click;
            _arr = new Label[i_, j_] ;
             
            this.Size = new Size(i_ * 100, j_ * 100);
            this.Text = "Ap_polo_kino";
            
            for (i = 0; i < i_; i++)
            {
                for (j = 0; j < j_; j++)
                {
                    _arr[i, j] = new Label();
                    _arr[i, j].Image = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\roh.png");
                    _arr[i, j].TextAlign= ContentAlignment.MiddleCenter;
                    _arr[i, j].Text = " Koht" + (j + 1);
                    _arr[i, j].Size = new Size(65, 65);
                    _arr[i, j].BorderStyle = BorderStyle.Fixed3D;
                    _arr[i, j].Location = new Point(j * 70 + 120, i * 70);
                    this.Controls.Add(_arr[i, j]);
                    _arr[i, j].Tag = new int[] { i, j };
					_arr[i, j].Click += Saalidform_Click1;
                }
            }
        }

		void Saalidform_Click1(object sender, EventArgs e)
		{
            var label = (Label)sender;
            var tag = (int[])label.Tag;
            if (_arr[tag[0], tag[1]].Text != "Kinni")
            {
                _arr[tag[0], tag[1]].Text = "Kinni";
                _arr[tag[0], tag[1]].Image = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\kol.jpg");

            }
            else
            {
                MessageBox.Show("Pilet rida:" + (tag[0] + 1) + " Koht:" + (tag[1] + 1) + " juba ostetud!");
            }
            _tag = tag;
        }

		
        
        private void Kinni_Click(object sender, EventArgs e)
        {
            string text = "";
            to_file = new StreamWriter("Kino.txt", false);
            
            to_file.Write(text);
            to_file.Close();
            this.Close();
        }
        void Pilet_saada()
        {
            string password = System.IO.File.ReadAllText(@"C:\Users\alisa\source\repos\KinoKru\password.txt");

            try
            {
                string adress = Emailtxt.Text;
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("alisa.krupenko18@gmail.com", password),
                    EnableSsl = true
                };
                mail.From = new MailAddress("alisa.krupenko18@gmail.com");
                mail.To.Add(adress);
                mail.Subject = Nimitxt.Text + " Rida " + (_tag[0] + 1).ToString() + " ja Koht " + (_tag[1] + 1).ToString();

                smtpClient.Send(mail);
                

            }
            catch (Exception)
            {
                MessageBox.Show("Viga");
            }

        }
        

        private void Insert_To_DataBase(int t, int i, int j)
        {
            
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opilane\source\repos\Krupenko\KinoKru\KinoKru\filmid.mdf;Integrated Security=True"); ;
            try
            {
                con.Open();
                MessageBox.Show("Andmebaas on avatud");
            }
            catch (Exception e)
            {
                MessageBox.Show("Andmebaasi avamiseks tekkis viga" + e.Message);
            }
            SqlCommand command = new SqlCommand("INSERT INTO Ostetud_Piletid(Id,Rida,Koht) VALUES(" + (t) + "," + (i + 1) + "," + (j + 1) + ")", con); ;
            
            command.ExecuteNonQuery();                  
            command.Dispose();
            con.Close();

        }

		private void Saalidform_Load(object sender, EventArgs e)
		{
            string text = "";
            StreamWriter to_file;
            if (!File.Exists("Kino.txt"))
            {
                to_file = new StreamWriter("Kino.txt", false);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        text += i + "," + j + ",false;";
                    }
                    text += "\n";
                }
                to_file.Write(text);
                to_file.Close();
            }
            StreamReader from_file = new StreamReader("Kino.txt", false);
            
            from_file.Close();

            
            
        }

		void Osta_Click(object sender, EventArgs e)
        {
            arr_pilet = new List<string>();
            if (ost == true)
            {
                var vastus = MessageBox.Show("Kas oled kindel?", "Appolo küsib", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (vastus == DialogResult.Yes)
                {
                    int t = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].Image == Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\kol.jpg"))
                            {
                                t++;
                                _arr[i, j].Image = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\pun.jpg");
                                //Сохранить каждый билет в файл
                                StreamWriter pilet = new StreamWriter("Pilet" + (t).ToString() + "Rida" + (i + 1).ToString() + "koht" + (j + 1).ToString() + ".txt");
                                //arr_pilet[t-1]="Pilet" + (t).ToString() + "Rida" + (i+1).ToString() + "koht" + (j+1).ToString() + ".txt";
                                arr_pilet.Add("Pilet" + (t).ToString() + "Rida" + (i + 1).ToString() + "koht" + (j + 1).ToString() + ".txt");
                                pilet.WriteLine("Pilet" + (t).ToString() + "Rida" + (i + 1).ToString() + "koht" + (j + 1).ToString());
                                pilet.Close();
                                Insert_To_DataBase(t, i, j);

                            }
                        }
                    }
                    //MessageBox.Show(arr_pilet.Count().ToString());
                    Pilet_saada();
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].Image == Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\kol.jpg"))
                            {
                                _arr[i, j].Text = " Koht" + (j + 1);
                                _arr[i, j].Image = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\roh.png"); ;
                                ost = false;
                            }
                        }
                    }
                }
            }
            else { MessageBox.Show("On vaja midagi valida!"); }
        }


        
    }
}
