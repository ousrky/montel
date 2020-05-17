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
namespace montel
{
    public partial class satis_sorumlusu_islemleri : Form
    {
        public satis_sorumlusu_islemleri()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        DialogResult cevap = new DialogResult();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            bool kontrol = false;
            try
            {
                if (String.IsNullOrEmpty(textEdit1.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(textEdit3.Text) || String.IsNullOrEmpty(textEdit4.Text))
                {
                    MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cevap = MessageBox.Show("Ekleme işlemini yapmak istediğinizden emin misiniz?", "SATIŞ SORUMLUSU EKLEME İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (cevap == DialogResult.Yes)
                    {
                        baglanti.Open();
                        OleDbCommand satis_oku = new OleDbCommand("select * from satis_sorumlusu", baglanti);
                        OleDbDataReader oku = satis_oku.ExecuteReader();
                        while (oku.Read())
                        {
                            if (oku.GetValue(3).ToString() == textEdit3.Text)
                                kontrol = true;
                        }

                        if (kontrol == false)
                        {
                            OleDbCommand sorumlu_ekle = new OleDbCommand("insert into satis_sorumlusu(adi,soyadi,kullanici_adi,sifre) values ('" + textEdit1.Text + "','" + textEdit2.Text + "','" + textEdit3.Text + "','" + textEdit4.Text + "')", baglanti);
                            sorumlu_ekle.ExecuteNonQuery();
                            OleDbCommand giris_ekle = new OleDbCommand("insert into giris_bilgiler(kullanici_adi,sifre,yetki) values ('" + textEdit3.Text + "','" + textEdit4.Text + "','Satış Danışmanı')", baglanti);
                            giris_ekle.ExecuteNonQuery();
                            MessageBox.Show("Satış sorumlusu başarıyla eklenilmiştir.", "SORUMLU EKLENİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            OleDbCommand verileri_ekle = new OleDbCommand("SELECT adi AS[ADI],soyadi AS[SOYADI],kullanici_adi AS[KULLANICI ADI],sifre AS[ŞİFRE] from satis_sorumlusu", baglanti);
                            OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gridControl1.DataSource = dt;

                            comboBoxEdit1.Properties.Items.Clear();
                            comboBoxEdit2.Properties.Items.Clear();

                            OleDbCommand satis_oku_tekrar = new OleDbCommand("select * from satis_sorumlusu", baglanti);
                            OleDbDataReader oku_tekrar = satis_oku_tekrar.ExecuteReader();
                            while (oku_tekrar.Read())
                            {
                                comboBoxEdit1.Properties.Items.Add(oku_tekrar.GetValue(3).ToString());
                                comboBoxEdit2.Properties.Items.Add(oku_tekrar.GetValue(3).ToString());
                            }

                            textEdit1.Text = "";
                            textEdit2.Text = "";
                            textEdit3.Text = "";
                            textEdit4.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten kayıtlıdır.Lütfen farklı bir kullanıcı adı deneyiniz.","SORUMLU KULLANICI ADI KAYITLI !!!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            kontrol = false;
                        }
                        baglanti.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ekleme işlemi yapılamadı.Lütfen tekrar deneyin.", "BAŞARISIZ EKLEME İŞLEMİ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            yonetici_islemler frm = new yonetici_islemler();
            frm.Show();
            this.Hide();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void satis_sorumlusu_islemleri_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand satis_oku = new OleDbCommand("select * from satis_sorumlusu", baglanti);
            OleDbDataReader oku = satis_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit1.Properties.Items.Add(oku.GetValue(3).ToString());
                comboBoxEdit2.Properties.Items.Add(oku.GetValue(3).ToString());
            }
            OleDbCommand verileri_ekle = new OleDbCommand("SELECT adi AS[ADI],soyadi AS[SOYADI],kullanici_adi AS[KULLANICI ADI],sifre AS[ŞİFRE] from satis_sorumlusu", baglanti);
            OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
            baglanti.Close();
            // gridControl1.Enabled = false;
           // gridControl1.AllowRestoreSelectionAndFocusedRow= gridCo;
            gridControl1.AllowDrop = false;
           // gridControl1.
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Silme işlemini yapmak istediğinizden emin misiniz?", "SİLME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes==cevap)
            {
            baglanti.Open();
            OleDbCommand sorumlu_ekle = new OleDbCommand("delete from satis_sorumlusu where kullanici_adi='"+comboBoxEdit1.Text+"'", baglanti);
            sorumlu_ekle.ExecuteNonQuery();
                OleDbCommand giris_sil = new OleDbCommand("delete from giris_bilgiler where kullanici_adi='" + comboBoxEdit1.Text + "'", baglanti);
                giris_sil.ExecuteNonQuery();
                MessageBox.Show("Satış sorumlusu başarıyla silinmiştir.", "SORUMLU SİLİNDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OleDbCommand verileri_ekle = new OleDbCommand("SELECT adi AS[ADI],soyadi AS[SOYADI],kullanici_adi AS[KULLANICI ADI],sifre AS[ŞİFRE] from satis_sorumlusu", baglanti);
                OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl1.DataSource = dt;

                comboBoxEdit1.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.Clear();
                comboBoxEdit1.Text = "";
                OleDbCommand satis_oku = new OleDbCommand("select * from satis_sorumlusu", baglanti);
                OleDbDataReader oku = satis_oku.ExecuteReader();
                while (oku.Read())
                {
                    comboBoxEdit1.Properties.Items.Add(oku.GetValue(3).ToString());
                    comboBoxEdit2.Properties.Items.Add(oku.GetValue(3).ToString());
                }
                baglanti.Close();
            }
           
        }

        private void comboBoxEdit2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textEdit5.Text) || String.IsNullOrEmpty(textEdit6.Text) || String.IsNullOrEmpty(textEdit7.Text) || String.IsNullOrEmpty(textEdit8.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Güncelleme işlemini yapmak istediğinizden emin misiniz?", "GÜNCELLEME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DialogResult.Yes == cevap)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_guncelle = new OleDbCommand("update satis_sorumlusu  set adi='" + textEdit5.Text + "',soyadi='" + textEdit6.Text + "',kullanici_adi='" + textEdit7.Text + "',sifre='" + textEdit8.Text + "' where kullanici_adi='" + comboBoxEdit2.Text + "'", baglanti);
                    sorumlu_guncelle.ExecuteNonQuery();
                    OleDbCommand giris_guncelle = new OleDbCommand("update giris_bilgiler  set kullanici_adi='" + textEdit7.Text + "',sifre='" + textEdit8.Text + "' where kullanici_adi='" + comboBoxEdit2.Text + "'", baglanti);
                    giris_guncelle.ExecuteNonQuery();
                    MessageBox.Show("Satış sorumlusu başarıyla güncellenmiştir.", "SORUMLU BAŞARIYLA GÜNCELLENDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baglanti.Close();

                    comboBoxEdit2.Text = "";
                    comboBoxEdit1.Properties.Items.Clear();
                    comboBoxEdit2.Properties.Items.Clear();
                    textEdit5.Text = "";
                    textEdit6.Text = "";
                    textEdit7.Text = "";
                    textEdit8.Text = "";

                    baglanti.Open();
                    OleDbCommand verileri_ekle = new OleDbCommand("SELECT adi AS[ADI],soyadi AS[SOYADI],kullanici_adi AS[KULLANICI ADI],sifre AS[ŞİFRE] from satis_sorumlusu", baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    OleDbCommand satis_oku = new OleDbCommand("select * from satis_sorumlusu", baglanti);
                    OleDbDataReader oku = satis_oku.ExecuteReader();
                    while (oku.Read())
                    {
                        comboBoxEdit1.Properties.Items.Add(oku.GetValue(3).ToString());
                        comboBoxEdit2.Properties.Items.Add(oku.GetValue(3).ToString());
                    }
                    baglanti.Close();
                }
            }
        }

        private void comboBoxEdit2_SelectedValueChanged(object sender, EventArgs e)
        {

            
            
        }

        private void comboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
            baglanti.Open();

            OleDbCommand satis_oku = new OleDbCommand("select * from satis_sorumlusu where kullanici_adi='" + comboBoxEdit2.Text + "'", baglanti);
            OleDbDataReader oku = satis_oku.ExecuteReader();
            while (oku.Read())
            {
                textEdit5.Text = oku.GetValue(1).ToString();
                textEdit6.Text = oku.GetValue(2).ToString();
                textEdit7.Text = oku.GetValue(3).ToString();
                textEdit8.Text = oku.GetValue(4).ToString();
            }

            baglanti.Close();
        }

      
    }
}
