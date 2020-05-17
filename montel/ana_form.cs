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
    public partial class ana_form : Form
    {
        public ana_form()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            textEdit2.Properties.PasswordChar = '*';
            comboBoxEdit1.Properties.Items.Add("Yönetici");
            comboBoxEdit1.Properties.Items.Add("Satış Danışmanı");
            comboBoxEdit1.Properties.Items.Add("Atölye Sorumlusu");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textEdit1.Text) || String.IsNullOrEmpty(textEdit2.Text) || String.IsNullOrEmpty(comboBoxEdit1.Text))
            {
                MessageBox.Show("Lütfen bilgilerinizi eksik bırakmayınız...", "EKSİK ALAN UYARISI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
               try
                {
                    baglanti.Open();
                    OleDbCommand giris_bilgi_oku = new OleDbCommand("select * from giris_bilgiler where kullanici_adi='" + textEdit1.Text + "'AND sifre='" + textEdit2.Text + "'", baglanti);
                    OleDbDataReader oku = giris_bilgi_oku.ExecuteReader();
                    oku.Read();
                    string mevki = oku.GetValue(3).ToString();
                    if (comboBoxEdit1.Text == "Yönetici" && comboBoxEdit1.Text==mevki)
                    {
                            yonetici_islemler frm = new yonetici_islemler();
                            frm.Show();
                            this.Hide();                    
                    }
                    else if (comboBoxEdit1.Text == "Satış Danışmanı" && comboBoxEdit1.Text == mevki)
                    {
                        satis_sorumlu_ekrani frm = new satis_sorumlu_ekrani();
                        frm.Show();
                        this.Hide();
                    }
                    else if (comboBoxEdit1.Text == "Atölye Sorumlusu" && comboBoxEdit1.Text == mevki)
                    {
                       atolye_sorumlu_ekrani frm = new atolye_sorumlu_ekrani();
                        frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Bu kullanıcı böyle bir yetkiye sahip değildir..","YANLIŞ YETKİ SEÇİMİ",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }
                    baglanti.Close();
                }


                catch (Exception)
                {
                   baglanti.Close();  // baglanti yanlış bilgi girildikten açık kalıyor..
                    MessageBox.Show("Giriş bilgileriniz hatalıdır...", "HATALI BİLGİLER", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox işaretli ise
            if (checkEdit1.Checked)
            {
                //karakteri göster.
                textEdit2.Properties.PasswordChar = '\0';
            }
            //değilse karakterlerin yerine * koy.
            else
            {
                textEdit2.Properties.PasswordChar = '*';
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(comboBoxEdit1.SelectedIndex==0)
            {
                pictureBox2.Image = null;
                pictureBox2.Image = montel.Properties.Resources.yonetici;
            }
            if (comboBoxEdit1.SelectedIndex == 1)
            {
                pictureBox2.Image = null;
                pictureBox2.Image = montel.Properties.Resources.satis;
            }
            if (comboBoxEdit1.SelectedIndex == 2)
            {
                pictureBox2.Image = null;
                pictureBox2.Image = montel.Properties.Resources.atolye;
            }
        }
    }
}

