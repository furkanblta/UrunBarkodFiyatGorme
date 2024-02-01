using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {



        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelUrunFiyat.Text = "";
            labelUrunAd.Text = "";
            labelUrunBarkod.Text = " ";
            txtBarkod.Clear();
           this.ActiveControl= txtBarkod;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string barkodNumarasi = txtBarkod.Text.Trim();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL sorgusunu parametre ile oluştur
                string sqlQuery = "SELECT ITEM_SALE_PRICE, i.ITEM_NAME, b.BARCODE_NUMBER " +
                                  "FROM ITEMS i " +
                                  "JOIN BARCODES b ON i.ITEM_NUMBER = b.ITEM_NUMBER " +
                                  "WHERE b.BARCODE_NUMBER = @BarkodNumarasi AND STATUS=1";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Parametre ekleyerek SQL sorgusunu güvenli hale getir
                    command.Parameters.AddWithValue("@BarkodNumarasi", barkodNumarasi);

                    // Sorguyu çalıştır ve SqlDataReader ile sonuçları oku
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal fiyat = reader.GetDecimal(0);
                            string urunAdi = reader.GetString(1);
                            long barkod = reader.GetInt64(2);

                            labelUrunFiyat.Text = $"{fiyat} TL";
                            labelUrunAd.Text = urunAdi;
                            labelUrunBarkod.Text = barkod.ToString();
                            txtBarkod.Clear();
                            this.ActiveControl = txtBarkod;
                            txtBarkod.Focus();
                        }
                        else
                        {
                            connection.Close();
                            connection.Open();
                            // İlk sorgu sonuç vermediği durumda diğer tabloyu sorgula
                            sqlQuery = "SELECT BARCODE_NUMBER, ITEM_NAME, ITEM_SALE_PRICE FROM ITEM_UNKNOWN " +
                                       "WHERE BARCODE_NUMBER = @BarkodNumarasi";

                            using (SqlCommand secondCommand = new SqlCommand(sqlQuery, connection))
                            {
                                secondCommand.Parameters.AddWithValue("@BarkodNumarasi", barkodNumarasi);

                                using (SqlDataReader secondReader = secondCommand.ExecuteReader())
                                {
                                    
                                    if (secondReader.Read())
                                    {
                                        decimal fiyat = secondReader.GetDecimal(2);
                                        string urunAdi = secondReader.GetString(1);
                                        long barkod = secondReader.GetInt64(0);


                                        labelUrunFiyat.Text = $"{fiyat} TL";
                                        labelUrunAd.Text = urunAdi;
                                        labelUrunBarkod.Text = barkod.ToString();
                                        txtBarkod.Clear();
                                    }
                                    else
                                    {
                                        
                                       
                                        //MessageBox.Show("Ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        labelUrunFiyat.Text = "";
                                        labelUrunAd.Text = "Ürün Bulunamadı";
                                        labelUrunBarkod.Text = txtBarkod.Text;
                                        txtBarkod.Clear();
                                        //MessageBox.Show("Ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string barkodNumarasi = txtBarkod.Text.Trim();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL sorgusunu parametre ile oluştur
                    string sqlQuery = "SELECT ITEM_SALE_PRICE, i.ITEM_NAME, b.BARCODE_NUMBER " +
                                      "FROM ITEMS i " +
                                      "JOIN BARCODES b ON i.ITEM_NUMBER = b.ITEM_NUMBER " +
                                      "WHERE b.BARCODE_NUMBER = @BarkodNumarasi AND STATUS=1";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Parametre ekleyerek SQL sorgusunu güvenli hale getir
                        command.Parameters.AddWithValue("@BarkodNumarasi", barkodNumarasi);

                        // Sorguyu çalıştır ve SqlDataReader ile sonuçları oku
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal fiyat = reader.GetDecimal(0);
                                string urunAdi = reader.GetString(1);
                                long barkod = reader.GetInt64(2);

                                labelUrunFiyat.Text = $"{fiyat} TL";
                                labelUrunAd.Text = urunAdi;
                                labelUrunBarkod.Text = barkod.ToString();
                                txtBarkod.Clear();
                            }
                            else
                            {
                                connection.Close();
                                connection.Open();
                                // İlk sorgu sonuç vermediği durumda diğer tabloyu sorgula
                                sqlQuery = "SELECT BARCODE_NUMBER, ITEM_NAME, ITEM_SALE_PRICE FROM ITEM_UNKNOWN " +
                                           "WHERE BARCODE_NUMBER = @BarkodNumarasi";

                                using (SqlCommand secondCommand = new SqlCommand(sqlQuery, connection))
                                {
                                    secondCommand.Parameters.AddWithValue("@BarkodNumarasi", barkodNumarasi);

                                    using (SqlDataReader secondReader = secondCommand.ExecuteReader())
                                    {
                                        if (secondReader.Read())
                                        {
                                            decimal fiyat = secondReader.GetDecimal(2);
                                            string urunAdi = secondReader.GetString(1);
                                            long barkod = secondReader.GetInt64(0);

                                            labelUrunFiyat.Text = $"{fiyat} TL";
                                            labelUrunAd.Text = urunAdi;
                                            labelUrunBarkod.Text = barkod.ToString();
                                            txtBarkod.Clear();
                                        }
                                        else
                                        {

                                            //MessageBox.Show("Ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            labelUrunFiyat.Text = "";
                                            labelUrunAd.Text = "Ürün Bulunamadı";
                                            labelUrunBarkod.Text = txtBarkod.Text;
                                            txtBarkod.Clear();

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
    }

