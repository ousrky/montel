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
    public partial class urun_islemleri : Form
    {
        public urun_islemleri()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        string grup_turu = "";
        string kartela_turu = "";

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBoxEdit3.Text) || String.IsNullOrEmpty(textEdit1.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Ekleme işlemini yapmak istediğinizden emin misiniz?", "ATÖLYE SORUMLUSU EKLEME İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cevap == DialogResult.Yes)
                {
                    int sifir = 0;


                    if (comboBoxEdit3.SelectedIndex == 0)
                        grup_turu = "köşe takımı";
                    if (comboBoxEdit3.SelectedIndex == 1)
                        grup_turu = "tv ünitesi";
                    if (comboBoxEdit3.SelectedIndex == 2)
                        grup_turu = "tekli koltuk";

                    baglanti.Open();
                    OleDbCommand sorumlu_ekle = new OleDbCommand("insert into urunler(grup_turu,urun_cesidi,urun_uretim_ortalama_suresi,urun_uretim_sayisi,resim) values ('" + grup_turu + "','" + textEdit1.Text + "'," + sifir + "," + sifir + ",'"+ textEdit4.Text + "')", baglanti);
                    sorumlu_ekle.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarıyla eklenilmiştir.", "ÜRÜN EKLENİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textEdit1.Text = "";
                    textEdit4.Text = "";
                    comboBoxEdit3.Text = "";

                    OleDbCommand verileri_ekle = new OleDbCommand("SELECT grup_turu AS[GRUP TÜRÜ],urun_cesidi AS[ÜRÜN ÇEŞİDİ] from urunler", baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    baglanti.Close();

                }
            }
        }
        DialogResult cevap = new DialogResult();
        private void urun_islemleri_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(this.simpleButton10, "Resim yolunun jpg vb. uzatılı olacak şekilde Google görsellerden bulunuz.");
            toolTip1.SetToolTip(this.simpleButton9, "Resim yolunun jpg vb. uzatılı olacak şekilde Google görsellerden bulunuz.");

            comboBoxEdit3.Properties.Items.Add("Köşe Koltuk");
            comboBoxEdit3.Properties.Items.Add("TV Ünitesi");
            comboBoxEdit3.Properties.Items.Add("Tekli Koltuk");
            comboBoxEdit5.Properties.Items.Add("Köşe Koltuk");
            comboBoxEdit5.Properties.Items.Add("TV Ünitesi");
            comboBoxEdit5.Properties.Items.Add("Tekli Koltuk");
            comboBoxEdit6.Properties.Items.Add("Köşe Koltuk");
            comboBoxEdit6.Properties.Items.Add("TV Ünitesi");
            comboBoxEdit6.Properties.Items.Add("Tekli Koltuk");
            comboBoxEdit4.Properties.Items.Add("Deri");
            comboBoxEdit4.Properties.Items.Add("Kumaş");
            comboBoxEdit7.Properties.Items.Add("Deri");
            comboBoxEdit7.Properties.Items.Add("Kumaş");
            comboBoxEdit12.Properties.Items.Add("Deri");
            comboBoxEdit12.Properties.Items.Add("Kumaş");

           
        }

        private void comboBoxEdit5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit1.Properties.Items.Clear();
            comboBoxEdit1.Text = "";
            if (comboBoxEdit5.SelectedIndex == 0)
                grup_turu = "köşe takımı";
            if (comboBoxEdit5.SelectedIndex == 1)
                grup_turu = "tv ünitesi";
            if (comboBoxEdit5.SelectedIndex == 2)
                grup_turu = "tekli koltuk";

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from urunler where grup_turu='" + grup_turu + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit1.Properties.Items.Add(oku.GetValue(2).ToString());
            }
            baglanti.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Silme işlemini yapmak istediğinizden emin misiniz?", "SİLME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                baglanti.Open();
                OleDbCommand sorumlu_ekle = new OleDbCommand("delete from urunler where grup_turu='" + grup_turu + "' AND urun_cesidi='" + comboBoxEdit1.Text + "'", baglanti);
                sorumlu_ekle.ExecuteNonQuery();
                MessageBox.Show("Ürün  başarıyla silinmiştir.", "ÜRÜN SİLİNDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OleDbCommand verileri_ekle = new OleDbCommand("SELECT grup_turu AS[GRUP TÜRÜ],urun_cesidi AS[ÜRÜN ÇEŞİDİ] from urunler", baglanti);
                OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl1.DataSource = dt;

                comboBoxEdit1.Properties.Items.Clear();
                comboBoxEdit2.Properties.Items.Clear();
                comboBoxEdit5.Text = "";
                comboBoxEdit1.Text = "";

                baglanti.Close();
            }
        }

        private void comboBoxEdit6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Text = "";
            if (comboBoxEdit6.SelectedIndex == 0)
                grup_turu = "köşe takımı";
            if (comboBoxEdit6.SelectedIndex == 1)
                grup_turu = "tv ünitesi";
            if (comboBoxEdit6.SelectedIndex == 2)
                grup_turu = "tekli koltuk";

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from urunler where grup_turu='" + grup_turu + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
                comboBoxEdit2.Properties.Items.Add(oku.GetValue(2).ToString());

            baglanti.Close();
        }
        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textEdit6.Text = comboBoxEdit2.Text;

            baglanti.Open();
            OleDbCommand atolye_oku = new OleDbCommand("select * from urunler where grup_turu='" + grup_turu + "' AND urun_cesidi='"+comboBoxEdit2.Text+"'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            oku.Read();
            if (oku.HasRows)
            {
                pictureBox2.ImageLocation = oku.GetValue(5).ToString();
                textEdit8.Text= oku.GetValue(5).ToString();
            }
                baglanti.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBoxEdit6.Text) || String.IsNullOrEmpty(textEdit6.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Güncelleme işlemini yapmak istediğinizden emin misiniz?", "GÜNCELLEME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DialogResult.Yes == cevap)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_guncelle = new OleDbCommand("update urunler  set urun_cesidi='" + textEdit6.Text + "',resim='"+textEdit8.Text+"' where grup_turu='" + grup_turu + "' AND urun_cesidi='" + comboBoxEdit2.Text + "'", baglanti);
                    sorumlu_guncelle.ExecuteNonQuery();
                    MessageBox.Show("Ürün  başarıyla güncellenmiştir.", "SORUMLU BAŞARIYLA GÜNCELLENDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    OleDbCommand verileri_ekle = new OleDbCommand("SELECT grup_turu AS[GRUP TÜRÜ],urun_cesidi AS[ÜRÜN ÇEŞİDİ] from urunler", baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    /*  OleDbCommand satis_oku = new OleDbCommand("select * from urunler", baglanti);
                      OleDbDataReader oku = satis_oku.ExecuteReader();
                      while (oku.Read())
                      {
                          comboBoxEdit1.Properties.Items.Add(oku.GetValue(3).ToString());
                          comboBoxEdit2.Properties.Items.Add(oku.GetValue(3).ToString());
                      }*/
                    comboBoxEdit2.Text = "";
                    comboBoxEdit6.Text = "";
                    comboBoxEdit2.Properties.Items.Clear();

                    baglanti.Close();
                }
            }
        }

        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBoxEdit4.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(textEdit3.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Ekleme işlemini yapmak istediğinizden emin misiniz?", "KARTELA EKLEME İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cevap == DialogResult.Yes)
                {

                    if (comboBoxEdit4.SelectedIndex == 0)
                        kartela_turu = "deri";
                    if (comboBoxEdit4.SelectedIndex == 1)
                        kartela_turu = "kumaş";

                    baglanti.Open();
                    OleDbCommand kartela_ekle = new OleDbCommand("insert into kartela(kartela_turu,kartela_adi,kartela_rengi) values ('" + kartela_turu + "','" + textEdit2.Text + "','" + textEdit3.Text + "')", baglanti);
                    kartela_ekle.ExecuteNonQuery();
                    MessageBox.Show("Kartela başarıyla eklenilmiştir.", "KARTELA EKLENİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textEdit2.Text = "";
                    textEdit3.Text = "";
                    comboBoxEdit4.Text = "";

                    OleDbCommand verileri_ekle = new OleDbCommand("SELECT kartela_turu AS[KARTELA TÜRÜ],kartela_adi AS[KARTELA ADI],kartela_rengi AS [KARTELA RENGİ] from kartela", baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl2.DataSource = dt;

                    baglanti.Close();
                }
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Silme işlemini yapmak istediğinizden emin misiniz?", "SİLME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                baglanti.Open();
                OleDbCommand kartela_sil = new OleDbCommand("delete from kartela where kartela_turu='" + kartela_turu + "' AND kartela_adi='" + comboBoxEdit8.Text + "'AND kartela_rengi='" + comboBoxEdit9.Text + "'", baglanti);
                kartela_sil.ExecuteNonQuery();
                MessageBox.Show("Kartela  başarıyla silinmiştir.", "KARTELA SİLİNDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OleDbCommand verileri_ekle = new OleDbCommand("SELECT kartela_turu AS[KARTELA TÜRÜ],kartela_adi AS[KARTELA ADI],kartela_rengi AS [KARTELA RENGİ] from kartela", baglanti);
                OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl2.DataSource = dt;

                comboBoxEdit8.Properties.Items.Clear();
                comboBoxEdit9.Properties.Items.Clear();
                comboBoxEdit7.Text = "";
                comboBoxEdit8.Text = "";
                comboBoxEdit9.Text = "";

                baglanti.Close();
            }
        }

        private void comboBoxEdit7_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit8.Properties.Items.Clear();
            comboBoxEdit8.Text = "";
            if (comboBoxEdit7.SelectedIndex == 0)
                kartela_turu = "deri";
            if (comboBoxEdit7.SelectedIndex == 1)
                kartela_turu = "kumaş";

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from kartela where kartela_turu='" + kartela_turu + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit8.Properties.Items.Add(oku.GetValue(2).ToString());
            }
            baglanti.Close();
        }

        private void comboBoxEdit8_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit9.Properties.Items.Clear();
            comboBoxEdit9.Text = "";
           
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from kartela where kartela_adi='" + comboBoxEdit8.Text + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit9.Properties.Items.Add(oku.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void comboBoxEdit12_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit11.Properties.Items.Clear();
            comboBoxEdit11.Text = "";
            if (comboBoxEdit12.SelectedIndex == 0)
                kartela_turu = "deri";
            if (comboBoxEdit12.SelectedIndex == 1)
                kartela_turu = "kumaş";

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from kartela where kartela_turu='" + kartela_turu + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit11.Properties.Items.Add(oku.GetValue(2).ToString());
            }
            baglanti.Close();
        }

        private void comboBoxEdit11_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit10.Properties.Items.Clear();
            comboBoxEdit10.Text = "";

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();

            OleDbCommand atolye_oku = new OleDbCommand("select * from kartela where kartela_adi='" + comboBoxEdit11.Text + "'", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                comboBoxEdit10.Properties.Items.Add(oku.GetValue(3).ToString());
            }
            baglanti.Close();
            textEdit7.Text = comboBoxEdit11.Text;
        }

        private void comboBoxEdit10_SelectedIndexChanged(object sender, EventArgs e)
        {
            textEdit5.Text = comboBoxEdit10.Text;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBoxEdit12.Text) || String.IsNullOrEmpty(comboBoxEdit11.Text) || String.IsNullOrEmpty(textEdit7.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Güncelleme işlemini yapmak istediğinizden emin misiniz?", "GÜNCELLEME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DialogResult.Yes == cevap)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_guncelle = new OleDbCommand("update kartela  set kartela_adi='" + textEdit7.Text + "',kartela_rengi='"+ textEdit5.Text+"' where kartela_turu='" + kartela_turu + "' AND kartela_adi='" + comboBoxEdit11.Text + "'AND kartela_rengi='"+comboBoxEdit10.Text+"'", baglanti);
                    sorumlu_guncelle.ExecuteNonQuery();
                    MessageBox.Show("Ürün  başarıyla güncellenmiştir.", "SORUMLU BAŞARIYLA GÜNCELLENDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    OleDbCommand verileri_ekle = new OleDbCommand("SELECT kartela_turu AS[KARTELA TÜRÜ],kartela_adi AS[KARTELA ADI],kartela_rengi AS [KARTELA RENGİ] from kartela", baglanti);
                    OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl2.DataSource = dt;
                    
                    comboBoxEdit12.Text = "";
                    comboBoxEdit11.Text = "";
                    comboBoxEdit11.Properties.Items.Clear();
                    comboBoxEdit10.Text = "";
                    comboBoxEdit10.Properties.Items.Clear();
                    textEdit7.Text = "";
                    textEdit5.Text = "";


                    baglanti.Close();
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            yonetici_islemler yonetici = new yonetici_islemler();
            yonetici.Show();
            this.Hide();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gridControl2.Visible = false;
            gridControl1.Visible = true;
            baglanti.Open();
            OleDbCommand verileri_ekle = new OleDbCommand("SELECT grup_turu AS[GRUP TÜRÜ],urun_cesidi AS[ÜRÜN ÇEŞİDİ] from urunler", baglanti);
            OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;

            baglanti.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gridControl1.Visible = false;
            gridControl2.Visible = true;
            baglanti.Open();
            OleDbCommand verileri_ekle = new OleDbCommand("SELECT kartela_turu AS[KARTELA TÜRÜ],kartela_adi AS[KARTELA ÇEŞİDİ],kartela_rengi AS[KARTELA RENGİ] from kartela", baglanti);
            OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl2.DataSource = dt;

            baglanti.Close();
        }
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("chrome.exe", "https://www.google.com.tr/imghp?hl=en&tab=wi");
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void textEdit4_TextChanged(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = textEdit4.Text;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            progressBar1.Value = 0;
            timer1.Start();
          
        }
        int i = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i == 4)
            {
                timer1.Stop();
                i = 0;
                if (pictureBox1.PreferredSize.Width==16&& pictureBox1.PreferredSize.Height == 18)
                    MessageBox.Show("Resim yolunda bir sorun var lütfen kaydetmeden önce resim yolunu değiştirin.Resim ekranda göründükten sonra kaydetme işleminizi gerçekleştirin...", "RESİM YOLU HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                progressBar1.Value=i;
                i++;     
            }           
        }

        private void textEdit8_TextChanged(object sender, EventArgs e)
        {
                pictureBox2.ImageLocation = textEdit8.Text;
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                progressBar2.Value = 0;
                timer2.Start();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("chrome.exe", "https://www.google.com.tr/imghp?hl=en&tab=wi");
        }
        int j = 1;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (j == 4)
            {
                timer2.Stop();
                j = 0;
                if (pictureBox2.PreferredSize.Width == 16 && pictureBox2.PreferredSize.Height == 18 ||pictureBox2.Image==null)
                    MessageBox.Show("Resim yolunda bir sorun var lütfen kaydetmeden önce resim yolunu değiştirin.Resim ekranda göründükten sonra kaydetme işleminizi gerçekleştirin...", "RESİM YOLU HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                progressBar2.Value = j;
                j++;
            }
        }

      
    }
}
