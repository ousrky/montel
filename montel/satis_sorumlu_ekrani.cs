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
    public partial class satis_sorumlu_ekrani : Form
    {
        public satis_sorumlu_ekrani()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        DialogResult cevap = new DialogResult();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textEdit1.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(textEdit3.Text) || String.IsNullOrEmpty(textEdit4.Text) || String.IsNullOrEmpty(richTextBox1.Text) || String.IsNullOrEmpty(comboBoxEdit1.Text) || String.IsNullOrEmpty(comboBoxEdit2.Text) || String.IsNullOrEmpty(comboBoxEdit3.Text) || String.IsNullOrEmpty(textEdit5.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                baglanti.Open();
                int ortalama_suresi=0,urun_kosetakimi_sayac = 0;
                OleDbCommand siparis_sayi_oku = new OleDbCommand("SELECT *  FROM siparisler where asama='başlangıç'", baglanti);
                OleDbDataReader siparis_top_sayi_oku = siparis_sayi_oku.ExecuteReader();
                while(siparis_top_sayi_oku.Read())
                {
                    urun_kosetakimi_sayac++;
                }
                OleDbCommand ortalama_sure_oku = new OleDbCommand("SELECT *  FROM urunler where urun_cesidi='"+comboBoxEdit1.Text+"'", baglanti);
                OleDbDataReader ort_sure_okuma = ortalama_sure_oku.ExecuteReader();
                ort_sure_okuma.Read();
                if (ort_sure_okuma.HasRows)
                {
                    ortalama_suresi =Convert.ToInt32( ort_sure_okuma.GetValue(3));
                }
                
                cevap = MessageBox.Show("Kuyruktaki ürün sayısı için bekleme süresi "+ (ortalama_suresi+urun_kosetakimi_sayac) + "' saniye süreli "+textEdit5.Text+" "+kartela_turu+" " +comboBoxEdit1.Text+" köşe koltuğun kartela türü "+comboBoxEdit2.Text+" ve kartela rengi "+comboBoxEdit3.Text+" olan siparişi vermek istediğinizden emin misiniz?", "KÖŞE KOLTUK SİPARİŞ İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                baglanti.Close();
                if (cevap == DialogResult.Yes)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_ekle = new OleDbCommand("insert into siparisler(tc_no,adi,soyadi,telefon_no,adres,kartela_id,kartela_turu,urun_turu,urun_id,asama) values ('" + textEdit1.Text + "','" + textEdit2.Text + "','" + textEdit3.Text + "','" + textEdit4.Text + "','"+richTextBox1.Text+ "','"+kartela_id+"','"+kartela_turu+ "','köşe takımı','"+urun_id+"','başlangıç')", baglanti);
                    sorumlu_ekle.ExecuteNonQuery();
                    MessageBox.Show("Sipariş başarıyla verilmiştir.", "SİPARİŞ VERİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baglanti.Close();
                }
            }          
        }

        private void satis_sorumlu_ekrani_Load(object sender, EventArgs e)
        {
            baglanti.Open();
             OleDbCommand kose_koltuk_oku = new OleDbCommand("SELECT * FROM urunler where grup_turu='köşe takımı'", baglanti);
             OleDbDataReader kose_koltuk_okuma = kose_koltuk_oku.ExecuteReader();
             while (kose_koltuk_okuma.Read())
             {
                comboBoxEdit1.Properties.Items.Add(kose_koltuk_okuma.GetValue(2).ToString());
             }
            OleDbCommand tv_unite_oku = new OleDbCommand("SELECT * FROM urunler where grup_turu='tv ünitesi'", baglanti);
            OleDbDataReader tv_unite_okuma = tv_unite_oku.ExecuteReader();
            while (tv_unite_okuma.Read())
            {
                comboBoxEdit4.Properties.Items.Add(tv_unite_okuma.GetValue(2).ToString());
            }
            OleDbCommand berjer_oku = new OleDbCommand("SELECT * FROM urunler where grup_turu='tekli koltuk'", baglanti);
            OleDbDataReader berjer_okuma = berjer_oku.ExecuteReader();
            while (berjer_okuma.Read())
            {
                comboBoxEdit5.Properties.Items.Add(berjer_okuma.GetValue(2).ToString());
            }
            baglanti.Close();
        }
        string kartela_id,kartela_turu,urun_id;
        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit3.Properties.Items.Clear();
            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT * FROM kartela where kartela_adi='"+comboBoxEdit2.Text+"'", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            while (kartela_okuma.Read())
            {
                comboBoxEdit3.Properties.Items.Add(kartela_okuma.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Text = "";
            kartela_turu = "deri";

            baglanti.Open();
                OleDbCommand kartela_oku = new OleDbCommand("SELECT DISTINCT kartela_adi FROM kartela where kartela_turu='deri'", baglanti);
                OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
                while (kartela_okuma.Read())
                {
                    comboBoxEdit2.Properties.Items.Add(kartela_okuma.GetValue(0).ToString());
                }
            baglanti.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Text = "";
            kartela_turu = "kumaş";

            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT DISTINCT kartela_adi FROM kartela where kartela_turu='kumaş'", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            while (kartela_okuma.Read())
            {
                comboBoxEdit2.Properties.Items.Add(kartela_okuma.GetValue(0).ToString());
            }
            baglanti.Close();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT *  FROM urunler where grup_turu='köşe takımı' AND urun_cesidi='"+comboBoxEdit1.Text+"'", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            kartela_okuma.Read();
            if (kartela_okuma.HasRows)
            {
               urun_id= kartela_okuma.GetValue(0).ToString();
                pictureBox1.ImageLocation = kartela_okuma.GetValue(5).ToString();
                pictureBox1.SizeMode= PictureBoxSizeMode.StretchImage;
            }
            baglanti.Close();
        }

        private void comboBoxEdit4_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT *  FROM urunler where grup_turu='tv ünitesi' AND urun_cesidi='" + comboBoxEdit4.Text + "'  ", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            kartela_okuma.Read();
            if (kartela_okuma.HasRows)
            {
                urun_id = kartela_okuma.GetValue(0).ToString();
                pictureBox2.ImageLocation = kartela_okuma.GetValue(5).ToString();
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            baglanti.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textEdit1.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(textEdit3.Text) || String.IsNullOrEmpty(textEdit4.Text) || String.IsNullOrEmpty(richTextBox1.Text) || String.IsNullOrEmpty(comboBoxEdit4.Text) || String.IsNullOrEmpty(textEdit6.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {

                baglanti.Open();
                int ortalama_suresi = 0, urun_tvunite_sayac = 0;
                OleDbCommand siparis_sayi_oku = new OleDbCommand("SELECT *  FROM siparisler where asama='başlangıç'", baglanti);
                OleDbDataReader siparis_top_sayi_oku = siparis_sayi_oku.ExecuteReader();
                while (siparis_top_sayi_oku.Read())
                {
                    urun_tvunite_sayac++;
                }
                OleDbCommand ortalama_sure_oku = new OleDbCommand("SELECT *  FROM urunler where urun_cesidi='" + comboBoxEdit4.Text + "'", baglanti);
                OleDbDataReader ort_sure_okuma = ortalama_sure_oku.ExecuteReader();
                ort_sure_okuma.Read();
                if (ort_sure_okuma.HasRows)
                {
                    ortalama_suresi = Convert.ToInt32(ort_sure_okuma.GetValue(3));
                }
                cevap = MessageBox.Show("Kuyruktaki ürün sayısı için bekleme süresi " + (ortalama_suresi + urun_tvunite_sayac) + " saniye süreli "+textEdit6.Text+" adet "+comboBoxEdit4.Text+" tv ünitesi siparişini vermek istediğinizden emin misiniz?", "TV ÜNİTE SİPARİŞ İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                baglanti.Close();
                if (cevap == DialogResult.Yes)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_ekle = new OleDbCommand("insert into siparisler(tc_no,adi,soyadi,telefon_no,adres,urun_turu,urun_id,asama,dikim_suresi) values ('" + textEdit1.Text + "','" + textEdit2.Text + "','" + textEdit3.Text + "','" + textEdit4.Text + "','" + richTextBox1.Text + "','tv ünitesi','" + urun_id + "','monte','0')", baglanti);
                    sorumlu_ekle.ExecuteNonQuery();
                    MessageBox.Show("Sipariş başarıyla verilmiştir.", "SİPARİŞ VERİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baglanti.Close();
                }
            }
        }

        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar);
        }

        private void textEdit3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textEdit1.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(textEdit3.Text) || String.IsNullOrEmpty(textEdit4.Text) || String.IsNullOrEmpty(richTextBox1.Text) || String.IsNullOrEmpty(comboBoxEdit5.Text) || String.IsNullOrEmpty(textEdit7.Text))
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                baglanti.Open();
                int ortalama_suresi = 0, urun_berjer_sayac = 0;
                OleDbCommand siparis_sayi_oku = new OleDbCommand("SELECT *  FROM siparisler where asama='başlangıç'", baglanti);
                OleDbDataReader siparis_top_sayi_oku = siparis_sayi_oku.ExecuteReader();
                while (siparis_top_sayi_oku.Read())
                {
                    urun_berjer_sayac++;
                }
                OleDbCommand ortalama_sure_oku = new OleDbCommand("SELECT *  FROM urunler where urun_cesidi='" + comboBoxEdit5.Text + "'", baglanti);
                OleDbDataReader ort_sure_okuma = ortalama_sure_oku.ExecuteReader();
                ort_sure_okuma.Read();
                if (ort_sure_okuma.HasRows)
                {
                    ortalama_suresi = Convert.ToInt32(ort_sure_okuma.GetValue(3));
                }
                cevap = MessageBox.Show("Kuyruktaki ürün sayısı için bekleme süresi   " + (ortalama_suresi+urun_berjer_sayac) + " saniye olan "+textEdit7.Text+" adet "+comboBoxEdit5.Text+" tekli koltuk siparişini vermek istediğinizden emin misiniz?", "TV ÜNİTE SİPARİŞ İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                baglanti.Close();
                if (cevap == DialogResult.Yes)
                {
                    baglanti.Open();
                    OleDbCommand sorumlu_ekle = new OleDbCommand("insert into siparisler(tc_no,adi,soyadi,telefon_no,adres,urun_turu,urun_id,asama) values ('" + textEdit1.Text + "','" + textEdit2.Text + "','" + textEdit3.Text + "','" + textEdit4.Text + "','" + richTextBox1.Text + "','tekli koltuk','" + urun_id + "','başlangıç')", baglanti);
                    sorumlu_ekle.ExecuteNonQuery();
                    MessageBox.Show("Sipariş başarıyla verilmiştir.", "SİPARİŞ VERİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);                
                    baglanti.Close();
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ana_form ana = new ana_form();
            ana.Show();
            this.Hide();
        }

        private void comboBoxEdit5_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT *  FROM urunler where grup_turu='tekli koltuk' AND urun_cesidi='" + comboBoxEdit5.Text + "'", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            kartela_okuma.Read();
            if (kartela_okuma.HasRows)
            {
                urun_id = kartela_okuma.GetValue(0).ToString();
                pictureBox3.ImageLocation = kartela_okuma.GetValue(5).ToString();
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            baglanti.Close();
        }

        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand kartela_oku = new OleDbCommand("SELECT * FROM kartela where kartela_adi='" + comboBoxEdit2.Text + "' AND kartela_rengi='"+comboBoxEdit3.Text+"'", baglanti);
            OleDbDataReader kartela_okuma = kartela_oku.ExecuteReader();
            kartela_okuma.Read();
            if (kartela_okuma.HasRows)
            {
            kartela_id = kartela_okuma.GetValue(0).ToString();
            }
            baglanti.Close();

        }
    }
}
