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
    public partial class yonetici_islemler : Form
    {
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        public yonetici_islemler()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            satis_sorumlusu_islemleri frm = new satis_sorumlusu_islemleri();
            frm.Show();
            this.Hide();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?","ÇIKIŞ İŞLEMİ",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(mesaj==DialogResult.Yes)
            {
                Application.Exit();
            }
          
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            atolye_sorumlusu_islemler frm = new atolye_sorumlusu_islemler();
            frm.Show();
            this.Hide();
        }
        DialogResult cevap = new DialogResult();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Hammadde eklemek istediğinizden emin misiniz?", "HAMMADDE  EKLEME İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            int fire;
            if (cevap == DialogResult.Yes)
            {
                baglanti.Open();
                OleDbCommand siparis_getir = new OleDbCommand("select * from siparis_stok", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader siparis_oku = siparis_getir.ExecuteReader();
                while (siparis_oku.Read())
                {
                    OleDbCommand stok_getir = new OleDbCommand("select * from stok where hammadde_adi='"+siparis_oku.GetValue(1).ToString()+"'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_oku = stok_getir.ExecuteReader();
                    stok_oku.Read();
                    if (stok_oku.HasRows)
                    {
                        fire = Convert.ToInt32(stok_oku.GetValue(2)) + Convert.ToInt32(siparis_oku.GetValue(2))- Convert.ToInt32(siparis_oku.GetValue(2)) * 3 / 100;
                        OleDbCommand hammadde_ekle = new OleDbCommand("update stok set hammadde_miktari='" + fire + "' where hammadde_adi='" + siparis_oku.GetValue(1).ToString() + "'", baglanti);
                        hammadde_ekle.ExecuteNonQuery();
                    }
                }
                listBoxControl1.Items.Clear();
                listBoxControl1.Items.Add("Hammadde Adı" + "           Hammadde Miktarı");
                OleDbCommand stok_sil = new OleDbCommand("Delete * from siparis_stok ",baglanti);
                stok_sil.ExecuteNonQuery();
                baglanti.Close();
            }
        }

        private void yonetici_islemler_Load(object sender, EventArgs e)
        {
            listBoxControl1.Items.Clear();
            listBoxControl1.Items.Add("Hammadde Adı" + "           Hammadde Miktarı");

            baglanti.Open();
            OleDbCommand siparis_getir = new OleDbCommand("select * from siparis_stok ", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
            OleDbDataReader siparis_oku = siparis_getir.ExecuteReader();
            while (siparis_oku.Read())
            {
                listBoxControl1.Items.Add(siparis_oku.GetValue(1).ToString() + "                         " + siparis_oku.GetValue(2).ToString());
            }
            baglanti.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            urun_islemleri urn = new urun_islemleri();
            urn.Show();
            this.Hide();
        }
    }
}
