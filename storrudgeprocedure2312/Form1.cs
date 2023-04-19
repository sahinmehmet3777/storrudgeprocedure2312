using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storrudgeprocedure2312
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connstring = "Server = DESKTOP-7EA7R1P;Initial Catalog=Northwind;Trusted_Connection=True";
        //"Server = DESKTOP-7EA7R1P;Initial Catalog=Northwind;Trusted_Connection=True"
        //Data Source=216-08\\SQLEXPRESS;Initial Catalog=Northwind;User ID=sa; Password=Fbu123456
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string komut = "";
                komut = "select * from Customers";
                
                #region Veri Listeleme

                using (SqlConnection baglanti = new SqlConnection()) // sql bağlantı nesnesi oluşturma
                { // disposable tanımlamadık, çünkü sql connection verilen hazır bir sınıf
                    baglanti.ConnectionString = connstring;
                    using (SqlCommand listcommand = new SqlCommand(komut, baglanti))
                    {
                        baglanti.Open();
                        using (DataTable datatable = new DataTable())
                        {
                            datatable.Columns.Add("Müşteri Adı");
                            datatable.Columns.Add("Firma Adı");
                            datatable.Columns.Add("Yetkili Kişi");
                            datatable.Columns.Add("Ülkesi");
                            using (SqlDataReader reader = listcommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // reader'ı satır bazında oku, satır eşitle, reader'daki bilgileri satır satır datatable'a ekledik
                                    DataRow row = datatable.NewRow(); // datatable'dan yeni bir satırı data row'a al
                                    row["Müşteri Adı"] = reader["CustomerID"];
                                    row["Firma Adı"] = reader["CompanyName"];
                                    row["Yetkili Kişi"] = reader["ContactName"];
                                    row["Ülkesi"] = reader["Country"];
                                    datatable.Rows.Add(row);
                                }
                                dataGridView1.DataSource = datatable;
                            }
                        }
                        baglanti.Close();
                    }

                    
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("Lütfen Müşteri bilgilerini giriniz.");
                textBox1.Select();
                return;
            }

            using (SqlConnection cn = new SqlConnection(connstring))
            {
                using (SqlCommand cmd = new SqlCommand("benimprosedurum", cn))
                {
                    cmd.CommandType= CommandType.StoredProcedure ;
                    cmd.Parameters.AddWithValue("@Action","Insert") ;
                    cmd.Parameters.AddWithValue("@customerID1", textBox1.Text) ;
                    cmd.Parameters.AddWithValue("@CompanyName", textBox2.Text) ;
                    cmd.Parameters.AddWithValue("@ContactName", textBox3.Text) ;
                    cmd.Parameters.AddWithValue("@Country", textBox4.Text) ;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }
            }
            Form1_Load(this, null);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Güncelle buton
            #region Veri güncelleme
            try
            {
                #region Veri Ekleme

                using (SqlConnection connection = new SqlConnection()) // sql bağlantı nesnesi oluşturma
                {

                    using (SqlCommand addcommand = new SqlCommand())
                    {
                        connection.ConnectionString = connstring;
                        connection.Open();
                        addcommand.Connection = connection;
                        addcommand.CommandType = CommandType.Text;
                        addcommand.CommandText = "update Customers set CustomerID = @customerID1 , CompanyName =@CompanyName ,ContactName = @ContactName ,Country=@Country where  CustomerID = @customerID1 ";
                        addcommand.Parameters.AddWithValue("@customerID1", textBox1.Text);
                        addcommand.Parameters.AddWithValue("@CompanyName", textBox2.Text);
                        addcommand.Parameters.AddWithValue("@ContactName", textBox3.Text);
                        addcommand.Parameters.AddWithValue("@Country", textBox4.Text);
                        bool character = false;//burda rakam varmı yokmu kontrol ediyoruz
                        for (int i = 0; i < (textBox1.Text).Length; i++)
                        {
                            if (char.IsLetter(Convert.ToChar((textBox1.Text).Substring(i, 1))))
                                character = true;
                            else
                            {
                                character = false;
                                break;
                            }
                        }
                        if (character)
                        {
                            addcommand.ExecuteNonQuery();
                            MessageBox.Show("Kayıt başarılı");
                        }
                        else
                            MessageBox.Show("Kayıt olmadı, çünkü Adı alanında numerik değer var.");

                        textBox1.Clear();
                        textBox2.Clear();
                        textBox1.Select();
                    }
                    connection.Close();
                }
                Form1_Load(this, null); // eklemeden sonra datagridview, tabloyu güncelleyip yeni değeri de gösterir
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //silme buton
            /*
             --delete from Employees where EmployeeID=@ID
            update Employees set FirstName = @FirstName , LastName = @LastName where  EmployeeID = @ID
             */
            try
            {
                #region Veri silme

                using (SqlConnection connection = new SqlConnection()) // sql bağlantı nesnesi oluşturma
                {

                    using (SqlCommand addcommand = new SqlCommand())
                    {
                        connection.ConnectionString = connstring;
                        connection.Open();
                        addcommand.Connection = connection;
                        addcommand.CommandType = CommandType.Text;
                        addcommand.CommandText = "delete from Customers where CustomerID = @customerID1";
                        addcommand.Parameters.AddWithValue("@customerID1", textBox1.Text);
                        addcommand.ExecuteNonQuery();

                        textBox1.Clear();
                        textBox2.Clear();
                        textBox1.Select();
                    }
                    connection.Close();
                }
                Form1_Load(this, null); // eklemeden sonra datagridview, tabloyu güncelleyip yeni değeri de gösterir
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Select();
            Form1_Load(this, null);
        }
    }
}
