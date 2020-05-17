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
    public partial class stok_kontrolu : Form
    {
        public stok_kontrolu()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        DialogResult cevap = new DialogResult();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text==""||textEdit1.Text=="")
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurunuz...","BOŞ ALAN UYARISI",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                cevap = MessageBox.Show("Hammadde eklemek istediğinizden emin misiniz?", "HAMMADDE  EKLEME İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (cevap == DialogResult.Yes)
                {
                    int stok=0;
                   
                    baglanti.Open();
                    OleDbCommand siparis_oku = new OleDbCommand("select * from siparis_stok where hammadde_adi='" + comboBoxEdit1.Text + "'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader siparis_okuma = siparis_oku.ExecuteReader();
                    siparis_okuma.Read();
                    if (siparis_okuma.HasRows)
                    {
                        stok =Convert.ToInt32(siparis_okuma.GetValue(2));

                        OleDbCommand hammadde_ekle = new OleDbCommand("update siparis_stok set hammadde_miktari=" + (Convert.ToInt32(textEdit1.Text) + stok) + " where hammadde_adi='"+ comboBoxEdit1.Text+ "'", baglanti);
                        hammadde_ekle.ExecuteNonQuery();
                      //  baglanti.Close();
                    }
                    else
                    {
                     //   baglanti.Open();
                        OleDbCommand hammadde_ekle = new OleDbCommand("insert into siparis_stok(hammadde_adi,hammadde_miktari) values ('" + comboBoxEdit1.Text + "','" + textEdit1.Text + "')", baglanti);
                        hammadde_ekle.ExecuteNonQuery();

                    }
                    baglanti.Close();
                    MessageBox.Show("Hammadde başarıyla eklenilmiştir.", "HAMMADDE EKLENİLDİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textEdit1.Text = "";
                        comboBoxEdit1.Text = "";
                    //listbox2 de göster...
                    
                   
                    baglanti.Open();
                    OleDbCommand siparis_id_bul = new OleDbCommand("select * from siparis_stok order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için

                    OleDbDataReader id_siparis_oku = siparis_id_bul.ExecuteReader();

                    listBoxControl2.Items.Clear();
                    listBoxControl2.Items.Add("Hammadde Adı" + "           Hammadde Miktarı");
                    while (id_siparis_oku.Read())
                    {
                        listBoxControl2.Items.Add(id_siparis_oku.GetValue(1)+ "           " + id_siparis_oku.GetValue(2));
                    }
                    baglanti.Close();
                    }
            }
        }

        private void stok_kontrolu_Load(object sender, EventArgs e)
        {
            listBoxControl1.Items.Add("Hammadde Adı"+"    Hammadde Miktarı");
            listBoxControl2.Items.Add("Hammadde Adı" + "           Hammadde Miktarı");

            baglanti.Open();
            OleDbCommand siparis_getir = new OleDbCommand("select * from siparis_stok order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
            OleDbDataReader siparis_oku = siparis_getir.ExecuteReader();
            while (siparis_oku.Read())
            {
                listBoxControl2.Items.Add(siparis_oku.GetValue(1).ToString() + "            " + siparis_oku.GetValue(2).ToString() );
            }
            baglanti.Close();
            baglanti.Open();
            OleDbCommand atolye_oku = new OleDbCommand("select * from stok", baglanti);
            OleDbDataReader oku = atolye_oku.ExecuteReader();
            while (oku.Read())
            {
                listBoxControl1.Items.Add(oku.GetValue(1).ToString()+"                     "+oku.GetValue(2));
                comboBoxEdit1.Properties.Items.Add(oku.GetValue(1).ToString());
            }
            baglanti.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                this.Hide();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            atolye_sorumlu_ekrani atolye = new atolye_sorumlu_ekrani();
            atolye.Show();
            this.Hide();
        }

     
    }
}
