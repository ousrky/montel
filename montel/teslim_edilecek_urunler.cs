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
    public partial class teslim_edilecek_urunler : Form
    {
        public teslim_edilecek_urunler()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            atolye_sorumlu_ekrani frm = new atolye_sorumlu_ekrani();
            frm.Show();
        }

        private void teslim_edilecek_urunler_Load(object sender, EventArgs e)
        {
            OleDbCommand verileri_ekle = new OleDbCommand("SELECT  tc_no AS[TC NO],adi AS[ADI],soyadi AS[SOYADI],telefon_no AS[TELEFON NO],adres AS[ADRES],urun_turu AS[ÜRÜN TÜRÜ] from teslimat", baglanti);
            OleDbDataAdapter da = new OleDbDataAdapter(verileri_ekle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
            baglanti.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult mesaj = new DialogResult();
            mesaj = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "ÇIKIŞ İŞLEMİ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mesaj == DialogResult.Yes)
            {
                this.Hide();
            }
        }
    }
}
