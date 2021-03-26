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

namespace KinoKru
{
	public partial class Form1 : Form
	{
        SqlCommand command;
        Label kir;
        DateTimePicker dtp,aeg;
        Button bsearch, v,update;
        TextBox search, login, pass,nimi,pilet,kirjeldus;
        int[] read_list;
        int[] kohad_list;
        PictureBox filmpic ;
        ListBox saalide_list;
        public int i = 0, j = 0;
        DataGridView dgv;
        DataSet sds;
        Image[] images = new Image[5];
        SqlDataAdapter sAdapter;
        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alisa\Source\Repos\KinoKru\KinoKru\tables.mdf;Integrated Security=True");
        public Form1()
        {
            
            Label log = new Label { Text = "Login", Location = new Point(10,50) ,Size = new Size(35,20)};
            Label par = new Label { Text = "Parool", Location = new Point(10,80),Size = new Size(40,20) };
            this.Controls.Add(par);
            this.Controls.Add(log);
            login = new TextBox { Location = new Point(50, 50) };
            pass = new TextBox { Location = new Point(50, 80) };
            this.Controls.Add(login);
            this.Controls.Add(pass);
            this.Text = "Ap_polo_kino";
            v = new Button { Text = "Logi sisse", Location = new Point(50, 100) };
            this.Controls.Add(v);
			v.Click += V_Click;
        }

		private void V_Click(object sender, EventArgs e)//логин и пароль из таблицы login
		{
            connection.Open();
            string userid = login.Text;
            string password = pass.Text;
            SqlCommand cmd = new SqlCommand("select userid,password from login where userid='" + login.Text + "'and password='" + pass.Text + "'", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            sds = new DataSet();
            

            sAdapter = new SqlDataAdapter("SELECT * FROM Filmid ", connection);

            sAdapter.Fill(sds, "Filmid");

            connection.Close();
            dgv = new DataGridView { ReadOnly = true, BackgroundColor = Color.White };
            
            dgv.Size = new Size(400, 200);
            dgv.DataSource = sds.Tables["Filmid"];
            dgv.RowHeaderMouseClick += Dgv_RowHeaderMouseClick;
            da.Fill(dt);
            if (dt.Rows.Count > 0)//Проверка пароля и логина
            {
                this.Size = new Size(500,400);
                this.Controls.Clear();
                nimi = new TextBox {Location= new Point(50,30) ,Text="Filmi nimi"};
                kirjeldus = new TextBox { Location = new Point(50, 120) ,Text="Kirjeldus"};
                aeg = new DateTimePicker { Location = new Point(50, 60) };
                pilet = new TextBox { Location = new Point(50, 90) ,Text="Pileti hind"};
                dgv.Location = new Point(30, 150);
                update = new Button { Location= new Point(150,120),Text="Lisa"};
                update.Click += Update_Click;
                this.Controls.Add(update);
                this.Controls.Add(nimi);
                this.Controls.Add(aeg);
                this.Controls.Add(pilet);
                this.Controls.Add(kirjeldus);
                this.Controls.Add(dgv);
            }
            else
            {
                this.Controls.Clear();
                bsearch = new Button { Location = new Point(130, 15), Size = new Size(30, 30), Image = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\search.png") };
                search = new TextBox { Location = new Point(30, 20) };
                filmpic = new PictureBox { Location = new Point(600, 50), Size = new Size(500, 500) };
                this.Controls.Add(search);
                this.Controls.Add(bsearch);
                dtp = new DateTimePicker { Location = new Point(170, 25) };
                this.Controls.Add(dtp);
                InitializeComponent();
                bsearch.Click += Bsearch_Click;
                dtp.ValueChanged += Dtp_ValueChanged;
                dgv.Location = new Point(30, 50);
                this.Controls.Add(dgv);
                saalide_list = new ListBox { Size = new Size(100, 50) };
                images[1] = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\Seven.jpg");
                images[2] = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\ovg.jpg");
                images[3] = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\ltl.png");
                images[4] = Image.FromFile(@"C:\Users\alisa\source\repos\KinoKru\KinoKru\img\tc.jpg");
                saalide_list.Location = new Point(20, 330);
                connection.Open();
                sAdapter = new SqlDataAdapter("SELECT * FROM Saalid", connection);
                dgv.Columns["Id"].Visible = false;
                dgv.Columns["Kirjeldus"].Visible = false;
                this.Controls.Add(filmpic);
                kir = new Label {Location = new Point(150,300),Size = new Size(400,300)};
                this.Controls.Add(kir);
                DataTable saalid_table = new DataTable();
                sAdapter.Fill(saalid_table);
                foreach (DataRow row in saalid_table.Rows)
                {
                    saalide_list.Items.Add(row["Saalinimetus"]);
                }
                read_list = new int[saalid_table.Rows.Count];
                kohad_list = new int[saalid_table.Rows.Count];
                int a = 0;
                foreach (DataRow row in saalid_table.Rows)
                {
                    read_list[a] = (int)row["Read"];
                    kohad_list[a] = (int)row["Kohad"];
                    a++;
                }
            }
            connection.Close();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (nimi.Text != "" && pilet.Text != "" && kirjeldus.Text != "" )
            {
                try
                {
                    command = new SqlCommand("INSERT INTO Filmid(Nimi,Aeg,Pilet,Kirjeldus) values(@nimi,@kirjeldus,@pilet)", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@nimi", nimi.Text);
                    command.Parameters.AddWithValue("@kirjeldus", kirjeldus.Text);
                    command.Parameters.AddWithValue("@pilet", pilet.Text);
                    command.Parameters.AddWithValue("@aeg", aeg.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    
                    MessageBox.Show("Andmed on lisatud");
                }
                catch (Exception)
                {

                    MessageBox.Show("Viga lisamisega");
                }
            }
            else
            {
                MessageBox.Show("Viga");
            }
        }

        private void Dtp_ValueChanged(object sender, EventArgs e) //поиск в таблице по времени сеанса
		{
            DateTime searchValue = dtp.Value;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool valueResult = false;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value != null && row.Cells[i].Value.Equals(searchValue))
                        {
                            int rowIndex = row.Index;
                            dgv.Rows[rowIndex].Selected = true;
                            valueResult = true;
                            break;
                        }
                    }

                }
                if (!valueResult)
                {
                    MessageBox.Show("Film aega " + dtp.Value, "Ei leitud");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

		private void Bsearch_Click(object sender, EventArgs e) //поиск в таблице по названию фильма
		{
            string searchValue = search.Text;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool valueResult = false;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().Equals(searchValue))
                        {
                            int rowIndex = row.Index;
                            dgv.Rows[rowIndex].Selected = true;
                            valueResult = true;
                            break;
                        }
                    }

                }
                if (!valueResult)
                {
                    MessageBox.Show("Film nimega "+search.Text, "Ei leitud");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

		private void Dgv_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
            int p = 0;
            p = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[0].Value);
            filmpic.Image = images[p];
            kir.Text = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
            Label lbl = new Label { Text="Vali saal", Location = new Point(20,300), Size  = new Size(50,20)};
            
            this.Controls.Add(lbl);
            
            this.Controls.Add(saalide_list);
            saalide_list.SelectedIndexChanged += Saalide_list_SelectedIndexChanged;
        }

		private void Saalide_list_SelectedIndexChanged(object sender, EventArgs e)
		{


            i = read_list[saalide_list.SelectedIndex];
            j = kohad_list[saalide_list.SelectedIndex];
            Saalidform saalid = new Saalidform(i, j);
            saalid.Show();
        }
	}
}
