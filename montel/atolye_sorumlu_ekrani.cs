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
    public partial class atolye_sorumlu_ekrani : Form
    {
        public atolye_sorumlu_ekrani()
        {
            InitializeComponent();
        }
        int saniye = 0, saat = 0, dakika = 0, saniye_dikim = 0,dakika_dikim=0,saat_dikim=0, saniye_paket = 0, dakika_paket = 0, saat_paket = 0;
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=monteldb.accdb");
        DialogResult cevap = new DialogResult();
        private void atolye_sorumlu_ekrani_Load(object sender, EventArgs e)
        {// ürün hiç işlenmemişse 'başlangıç' eğer monteden cıkmışsa 'monte',eğer dikimden cıktıysa 'dikim' son olarak paketlemeden cıktıysa 'bitiş' olarak kaydedilecek...  
            listBoxControl1.Items.Add("Ürün NO" + "    TC NO " + "            Ürün Türü    "+"    Ürün Çeşidi ");
            listBoxControl2.Items.Add("Ürün NO" + "    TC NO " + "            Ürün Türü    "+"    Ürün Çeşidi ");
            listBoxControl3.Items.Add("Ürün NO" + "    TC NO " + "            Ürün Türü    "+"    Ürün Çeşidi ");

            groupControl1.BackColor = Color.Red;

            #region siparişleri listboxlara getiren kodlar
            baglanti.Open();
            OleDbCommand siparis_getir = new OleDbCommand("select * from siparisler where asama='başlangıç' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
            OleDbDataReader oku = siparis_getir.ExecuteReader();
            while (oku.Read())
            {
                OleDbCommand urun_cesidi_getir = new OleDbCommand("select * from urunler where id="+oku.GetValue(9).ToString(), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_cesidi_oku = urun_cesidi_getir.ExecuteReader();
                urun_cesidi_oku.Read();

                listBoxControl1.Items.Add( oku.GetValue(0).ToString() + "            " + oku.GetValue(1).ToString() + "   " + oku.GetValue(8).ToString()+"      "+urun_cesidi_oku.GetValue(2).ToString());
            }

            //tv ünitesini de sipariş verirken direk olarak monte diye ekleniyor fazla koda gerek kalmıyor...
            OleDbCommand dikim_getir = new OleDbCommand("select * from siparisler where asama='monte' AND (urun_turu='köşe takımı' OR urun_turu='tekli koltuk') order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
            OleDbDataReader dikim_oku = dikim_getir.ExecuteReader();
            while (dikim_oku.Read())
            {
                OleDbCommand urun_cesidi_getir = new OleDbCommand("select * from urunler where id=" + dikim_oku.GetValue(9).ToString(), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_cesidi_oku = urun_cesidi_getir.ExecuteReader();
                urun_cesidi_oku.Read();

                listBoxControl2.Items.Add( dikim_oku.GetValue(0).ToString() + "            " + dikim_oku.GetValue(1).ToString() + "   " + dikim_oku.GetValue(8).ToString() + "      " + urun_cesidi_oku.GetValue(2).ToString());
            }
            baglanti.Close();

            baglanti.Open();
            OleDbCommand paketleme_getir = new OleDbCommand("select * from siparisler where asama='paketleme' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
            OleDbDataReader paketleme_oku = paketleme_getir.ExecuteReader();
            while (paketleme_oku.Read())
            {
                OleDbCommand urun_cesidi_getir = new OleDbCommand("select * from urunler where id=" + paketleme_oku.GetValue(9).ToString(), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_cesidi_oku = urun_cesidi_getir.ExecuteReader();
                urun_cesidi_oku.Read();

                listBoxControl3.Items.Add( paketleme_oku.GetValue(0).ToString() + "            " + paketleme_oku.GetValue(1).ToString() + "   " + paketleme_oku.GetValue(8).ToString() + "      " + urun_cesidi_oku.GetValue(2).ToString());
            }

            baglanti.Close();
            #endregion

            if (listBoxControl1.Items.Count > 1)//monte kuyruğunda ürün var ise...
            {
                baglanti.Open();
                OleDbCommand monte_id_bul = new OleDbCommand("select * from siparisler where asama='başlangıç' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader id_oku = monte_id_bul.ExecuteReader();
                id_oku.Read();
                textEdit2.Text = id_oku.GetValue(8).ToString();
                urunid = id_oku.GetValue(0).ToString();

                uruncesidiid = id_oku.GetValue(9).ToString();
                OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id="+Convert.ToInt32(uruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                urun_id_oku.Read();
                labelControl7.Text =urun_id_oku.GetValue(2).ToString();
                baglanti.Close();
            }
            else
            {
                textEdit2.Text = "Kuyruk Boş";
                labelControl7.Text = "";
                simpleButton1.Enabled = false;
                simpleButton2.Enabled = false;
            }

            if (listBoxControl2.Items.Count > 1)//dikim kuyruğunda ürün var ise...
            {
                baglanti.Open();
                OleDbCommand dikim_id_bul = new OleDbCommand("select * from siparisler where asama='monte' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader id_dikim_oku = dikim_id_bul.ExecuteReader();
                id_dikim_oku.Read();
                textEdit1.Text = id_dikim_oku.GetValue(8).ToString();
                dikisurunid = id_dikim_oku.GetValue(0).ToString();

                dikimuruncesidiid = id_dikim_oku.GetValue(9).ToString();
                OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(dikimuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                urun_id_oku.Read();
                labelControl8.Text = urun_id_oku.GetValue(2).ToString();
                baglanti.Close();
            }
            else
            {
                textEdit1.Text = "Kuyruk Boş";
                labelControl8.Text = "";
                simpleButton4.Enabled = false;
                simpleButton5.Enabled = false;
            }
            if (listBoxControl3.Items.Count > 1)//monte kuyruğunda ürün var ise...
            {
                baglanti.Open();
                OleDbCommand monte_id_bul = new OleDbCommand("select * from siparisler where asama='paketleme' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader id_oku = monte_id_bul.ExecuteReader();
                id_oku.Read();
                textEdit3.Text = id_oku.GetValue(8).ToString();
                paketurunid = id_oku.GetValue(0).ToString();

                paketuruncesidiid = id_oku.GetValue(9).ToString();
                OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                urun_id_oku.Read();
                labelControl10.Text = urun_id_oku.GetValue(2).ToString();
                baglanti.Close();
            }
            else
            {
                textEdit3.Text = "Kuyruk Boş";
                labelControl10.Text = "";
                simpleButton6.Enabled = false;
                simpleButton7.Enabled = false;
            }

            simpleButton2.Enabled = false;
            simpleButton5.Enabled = false;
            simpleButton7.Enabled = false;
            listBoxControl1.Enabled = false;
            listBoxControl2.Enabled = false;
            textEdit2.Enabled = false;

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            stok_kontrolu frm = new stok_kontrolu();
            frm.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Monte işlemine başlamak istediğinizden emin misiniz?", "MONTE İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                timer1.Start();
                simpleButton1.Enabled = false;
                pictureEdit1.Visible = true;
                simpleButton2.Enabled = true;
                simpleButton8.Enabled = true;
            }
        }
        string uruncesidiid;
        string dikimuruncesidiid;
        string paketuruncesidiid;

        string urunid;
        string dikisurunid;



        string paketurunid;

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            simpleButton10.Enabled = false;
            simpleButton12.Enabled = false;
            simpleButton9.Enabled = false;
            simpleButton11.Enabled = true;
            simpleButton13.Enabled = true;
            simpleButton8.Enabled = true;
            simpleButton5.Enabled = true;
            simpleButton2.Enabled = true;
            simpleButton7.Enabled = true;
            timer2.Start();
            timer3.Start();
            timer1.Start();

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            simpleButton12.Enabled = false;
            simpleButton10.Enabled = false;
            simpleButton9.Enabled = false;
            simpleButton13.Enabled = true;
            simpleButton11.Enabled = true;
            simpleButton8.Enabled = true;
            simpleButton7.Enabled = true;
            simpleButton2.Enabled = true;
            simpleButton5.Enabled = true;
            timer2.Start();
            timer3.Start();
            timer1.Start();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            simpleButton10.Enabled = true;
            simpleButton12.Enabled = true;
            simpleButton9.Enabled = true;
            simpleButton8.Enabled = false;
            simpleButton11.Enabled = false;
            simpleButton13.Enabled = false;
            simpleButton5.Enabled = false;
            simpleButton2.Enabled = false;
            simpleButton7.Enabled = false;
            timer2.Stop();
            timer1.Stop();
            timer3.Stop();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            simpleButton12.Enabled = true;
            simpleButton10.Enabled = true;
            simpleButton9.Enabled = true;
            simpleButton13.Enabled = false;
            simpleButton11.Enabled = false;
            simpleButton8.Enabled = false;
            simpleButton7.Enabled = false;
            simpleButton2.Enabled = false;
            simpleButton5.Enabled = false;
            
            timer3.Stop();
            timer2.Stop();
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            #region süreyi labelda gösteren kodlar
            saniye_dikim++;
            if (saniye_dikim == 60)
            {
                saniye_dikim = 0;
                labelControl20.Text = saniye_dikim.ToString();
                dakika_dikim++;
                if (dakika_dikim == 60)
                {
                    dakika_dikim = 0;
                    saat_dikim++;
                    labelControl22.Text = saat_dikim.ToString();
                    labelControl21.Text = dakika_dikim.ToString();
                }
            }
            labelControl20.Text = saniye_dikim.ToString();
            labelControl21.Text = dakika_dikim.ToString();
            labelControl22.Text = saat_dikim.ToString();
            #endregion
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            #region süreyi labelda gösteren kodlar
            saniye_paket++;
            if (saniye_paket == 60)
            {
                saniye_paket = 0;
                labelControl26.Text = saniye_paket.ToString();
                dakika_paket++;
                if (dakika_paket == 60)
                {
                    dakika_paket = 0;
                    saat_paket++;
                    labelControl28.Text = saat_paket.ToString();
                    labelControl27.Text = dakika_paket.ToString();
                }
            }
            labelControl26.Text = saniye_paket.ToString();
            labelControl27.Text = dakika_paket.ToString();
            labelControl28.Text = saat_paket.ToString();
            #endregion
        }
        int sonuc = 0;

        private void listBoxControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            teslim_edilecek_urunler frm = new teslim_edilecek_urunler();
            frm.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            cevap = MessageBox.Show("Monte işlemini bitirmek istediğinizden emin misiniz?", "MONTE İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                #region stoktan hammadde düşme işlemleri
                baglanti.Open();
                if (textEdit2.Text=="tv ünitesi")
                {
                  int demir_miktari, civi, tahta, plastik;
                  OleDbCommand stoktan_demir_dus = new OleDbCommand("select * from stok where hammadde_adi='  demir'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                  OleDbDataReader stok_demir_oku = stoktan_demir_dus.ExecuteReader();
                    stok_demir_oku.Read();
                    demir_miktari =Convert.ToInt32(stok_demir_oku.GetValue(2));

                    OleDbCommand stoktan_civi_dus = new OleDbCommand("select * from stok where hammadde_adi='      çivi'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_civi_oku = stoktan_civi_dus.ExecuteReader();
                    stok_civi_oku.Read();
                    civi = Convert.ToInt32(stok_civi_oku.GetValue(2));

                    OleDbCommand stoktan_tahta_dus = new OleDbCommand("select * from stok where hammadde_adi='   tahta'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_tahta_oku = stoktan_tahta_dus.ExecuteReader();
                    stok_tahta_oku.Read();
                    tahta = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    OleDbCommand stoktan_plastik_dus = new OleDbCommand("select * from stok where hammadde_adi='plastik'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_plastik_oku = stoktan_plastik_dus.ExecuteReader();
                    stok_plastik_oku.Read();
                    plastik = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    demir_miktari -= 40;
                    civi -= 30;
                    tahta -= 60;
                    plastik -= 20;

                    OleDbCommand demir_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='"+demir_miktari+ "' where hammadde_adi='  demir'", baglanti);
                    demir_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand civi_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + civi + "' where hammadde_adi='      çivi'", baglanti);
                    civi_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand tahta_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='   tahta'", baglanti);
                    tahta_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand plastik_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='plastik'", baglanti);
                    plastik_stok_guncelle.ExecuteNonQuery();
                }      
                else if (textEdit2.Text == "köşe takımı")
                {
                    int demir_miktari, civi, tahta, plastik;
                    OleDbCommand stoktan_demir_dus = new OleDbCommand("select * from stok where hammadde_adi='  demir'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_demir_oku = stoktan_demir_dus.ExecuteReader();
                    stok_demir_oku.Read();
                    demir_miktari = Convert.ToInt32(stok_demir_oku.GetValue(2));

                    OleDbCommand stoktan_civi_dus = new OleDbCommand("select * from stok where hammadde_adi='      çivi'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_civi_oku = stoktan_civi_dus.ExecuteReader();
                    stok_civi_oku.Read();
                    civi = Convert.ToInt32(stok_civi_oku.GetValue(2));

                    OleDbCommand stoktan_tahta_dus = new OleDbCommand("select * from stok where hammadde_adi='   tahta'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_tahta_oku = stoktan_tahta_dus.ExecuteReader();
                    stok_tahta_oku.Read();
                    tahta = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    OleDbCommand stoktan_plastik_dus = new OleDbCommand("select * from stok where hammadde_adi='plastik'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_plastik_oku = stoktan_plastik_dus.ExecuteReader();
                    stok_plastik_oku.Read();
                    plastik = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    demir_miktari -= 30;
                    civi -= 30;
                    tahta -= 50;
                    plastik -=20;

                    OleDbCommand demir_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + demir_miktari + "' where hammadde_adi='  demir'", baglanti);
                    demir_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand civi_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + civi + "' where hammadde_adi='      çivi'", baglanti);
                    civi_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand tahta_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='   tahta'", baglanti);
                    tahta_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand plastik_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='plastik'", baglanti);
                    plastik_stok_guncelle.ExecuteNonQuery();
                }
                else if (textEdit2.Text == "tekli koltuk")
                {
                    int demir_miktari, civi, tahta, plastik;
                    OleDbCommand stoktan_demir_dus = new OleDbCommand("select * from stok where hammadde_adi='  demir'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_demir_oku = stoktan_demir_dus.ExecuteReader();
                    stok_demir_oku.Read();
                    demir_miktari = Convert.ToInt32(stok_demir_oku.GetValue(2));

                    OleDbCommand stoktan_civi_dus = new OleDbCommand("select * from stok where hammadde_adi='      çivi'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_civi_oku = stoktan_civi_dus.ExecuteReader();
                    stok_civi_oku.Read();
                    civi = Convert.ToInt32(stok_civi_oku.GetValue(2));

                    OleDbCommand stoktan_tahta_dus = new OleDbCommand("select * from stok where hammadde_adi='   tahta'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_tahta_oku = stoktan_tahta_dus.ExecuteReader();
                    stok_tahta_oku.Read();
                    tahta = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    OleDbCommand stoktan_plastik_dus = new OleDbCommand("select * from stok where hammadde_adi='plastik'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_plastik_oku = stoktan_plastik_dus.ExecuteReader();
                    stok_plastik_oku.Read();
                    plastik = Convert.ToInt32(stok_tahta_oku.GetValue(2));

                    demir_miktari -= 20;
                    civi -= 20;
                    tahta -= 40;
                    plastik -= 10;

                    OleDbCommand demir_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + demir_miktari + "' where hammadde_adi='  demir'", baglanti);
                    demir_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand civi_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + civi + "' where hammadde_adi='      çivi'", baglanti);
                    civi_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand tahta_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='   tahta'", baglanti);
                    tahta_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand plastik_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + tahta + "' where hammadde_adi='   tahta'", baglanti);
                    plastik_stok_guncelle.ExecuteNonQuery();
                }
                baglanti.Close();
                #endregion

                simpleButton8.Enabled = false;
                #region Süre Kaydetme İşlemi
                baglanti.Open();
                /* OleDbCommand urun_hesapla = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(urunid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                 OleDbDataReader urun_hesapla_oku = urun_hesapla.ExecuteReader();
                 urun_hesapla_oku.Read();
                 if (urun_hesapla_oku.HasRows)
                 {*/
                sonuc = saat * 3600 + dakika * 60 + saniye;

                OleDbCommand monte_sure_guncelleme = new OleDbCommand("update siparisler set monte_suresi=" + sonuc + " where id=" + urunid, baglanti);
                monte_sure_guncelleme.ExecuteNonQuery();
                // }
                baglanti.Close();
                #endregion

                if (textEdit2.Text == "köşe takımı" || textEdit2.Text == "tekli koltuk")
                {
                    simpleButton1.Enabled = true;
                    simpleButton2.Enabled = false;
                    pictureEdit1.Visible = false;

                    baglanti.Open();
                    string text = "update siparisler  set asama='monte' where id= " + urunid;
                    OleDbCommand monte_guncelleme = new OleDbCommand(text, baglanti);
                    monte_guncelleme.ExecuteNonQuery();
                    baglanti.Close();

                    listBoxControl2.Items.Add(listBoxControl1.Items[1]);
                    listBoxControl1.Items.RemoveAt(1);
                    if (listBoxControl1.Items.Count > 1)//başlangıç aşamasındaki ilk ürün varsa gelsin...
                    {
                        baglanti.Open();
                        OleDbCommand monte_id_bul = new OleDbCommand("select * from siparisler where asama='başlangıç' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader id_oku = monte_id_bul.ExecuteReader();
                        id_oku.Read();
                        textEdit2.Text = id_oku.GetValue(8).ToString();
                        urunid = id_oku.GetValue(0).ToString();

                        uruncesidiid = id_oku.GetValue(9).ToString();
                        OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(uruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                        urun_id_oku.Read();
                        labelControl7.Text = urun_id_oku.GetValue(2).ToString();
                        baglanti.Close();
                    }
                    else
                    {
                        simpleButton1.Enabled = false;
                        labelControl7.Text = "";
                        textEdit2.Text = "Kuyruk Boş";
                    }
                    if (textEdit1.Text == "Kuyruk Boş")
                    {
                        simpleButton4.Enabled = true;
                        if (listBoxControl2.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                        {
                            baglanti.Open();
                            OleDbCommand dikim_id_bul = new OleDbCommand("select * from siparisler where asama='monte' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                            OleDbDataReader id_oku = dikim_id_bul.ExecuteReader();
                            id_oku.Read();
                            textEdit1.Text = id_oku.GetValue(8).ToString();
                            dikisurunid = id_oku.GetValue(0).ToString();

                            dikimuruncesidiid = id_oku.GetValue(9).ToString();
                            OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(dikimuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                            OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                            urun_id_oku.Read();
                            labelControl8.Text = urun_id_oku.GetValue(2).ToString();
                            baglanti.Close();
                        }
                        else
                        {
                            simpleButton4.Enabled = false;
                            textEdit1.Text = "";
                            labelControl8.Text = "";
                        }
                    }                 
                }
                else
                {
                    simpleButton1.Enabled = true;
                    simpleButton2.Enabled = false;
                    simpleButton6.Enabled = true;
                    listBoxControl3.Items.Add(listBoxControl1.Items[1]);
                    listBoxControl1.Items.RemoveAt(1);
                    baglanti.Open();
                    string text = "update siparisler  set asama='paketleme' where id= " + urunid;
                    OleDbCommand paketleme_guncelleme = new OleDbCommand(text, baglanti);
                    paketleme_guncelleme.ExecuteNonQuery();
                    baglanti.Close();

                    if (listBoxControl1.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                    {
                        baglanti.Open();
                        OleDbCommand monte_id_bul = new OleDbCommand("select * from siparisler where asama='başlangıç' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader id_oku = monte_id_bul.ExecuteReader();
                        id_oku.Read();
                        textEdit2.Text = id_oku.GetValue(8).ToString();
                        urunid = id_oku.GetValue(0).ToString();

                        uruncesidiid = id_oku.GetValue(9).ToString();
                        OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(uruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                        urun_id_oku.Read();
                        labelControl7.Text = urun_id_oku.GetValue(2).ToString();
                        baglanti.Close();
                    }
                    else
                    {
                        simpleButton1.Enabled = false;
                        textEdit2.Text = "";
                    }
                    if (textEdit3.Text == "Kuyruk Boş")
                    {
                        simpleButton6.Enabled = true;
                        if (listBoxControl3.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                        {
                            baglanti.Open();
                            OleDbCommand dikim_id_bul = new OleDbCommand("select * from siparisler where asama='paketleme' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                            OleDbDataReader id_oku = dikim_id_bul.ExecuteReader();
                            id_oku.Read();
                            textEdit3.Text = id_oku.GetValue(8).ToString();
                            paketurunid = id_oku.GetValue(0).ToString();

                            paketuruncesidiid = id_oku.GetValue(9).ToString();
                            OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                            OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                            urun_id_oku.Read();
                            labelControl10.Text = urun_id_oku.GetValue(2).ToString();
                            baglanti.Close();
                        }
                        else
                        {
                            simpleButton6.Enabled = false;
                            labelControl10.Text = "";
                            textEdit3.Text = "Kuyruk Boş";
                        }
                    }
                }

                saniye = 0;
                saat = 0;
                dakika = 0;

                labelControl15.Text = saniye.ToString();
                labelControl14.Text = dakika.ToString();
                labelControl13.Text = saat.ToString();
            }
            else
                timer1.Start();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Dikim işlemine başlamak istediğinizden emin misiniz?", "DİKİM İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                timer2.Start();

                simpleButton4.Enabled = false;
                simpleButton5.Enabled = true;
                simpleButton11.Enabled = true;
                pictureEdit2.Visible = true;
                //baglanti.Open();
                //string text = "update siparisler  set asama='dikim' where id= " + dikisurunid;
                //OleDbCommand dikim_guncelleme = new OleDbCommand(text, baglanti);
                //dikim_guncelleme.ExecuteNonQuery();
                //baglanti.Close();

                /*  #region elimizdeki ürünlerin süresinin başlangıç tarihini kaydetme kodları               
                  baglanti.Open();
                  OleDbCommand baslangic_guncelle = new OleDbCommand("update siparisler set baslangic_tarih='" + DateTime.Now.ToShortDateString() + "' where id=" + dikisurunid, baglanti);
                  baslangic_guncelle.ExecuteNonQuery();
                  baglanti.Close(); 
                  #endregion   */

                simpleButton4.Enabled = false;
                simpleButton5.Enabled = true; 
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            cevap = MessageBox.Show("Dikim işlemini bitirmek istediğinizden emin misiniz?", "DİKİM İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (DialogResult.Yes == cevap)
            {

                #region stoktan hammadde düşme işlemleri

                baglanti.Open();
                
                if (textEdit1.Text == "köşe takımı")
                {
                    int deri_miktari, kumas, lastik;
                    OleDbCommand stoktan_deri_dus = new OleDbCommand("select * from stok where hammadde_adi='     deri'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_deri_oku = stoktan_deri_dus.ExecuteReader();
                    stok_deri_oku.Read();
                    deri_miktari = Convert.ToInt32(stok_deri_oku.GetValue(2));

                    OleDbCommand stoktan_kumas_dus = new OleDbCommand("select * from stok where hammadde_adi=' kumaş'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_kumas_oku = stoktan_kumas_dus.ExecuteReader();
                    stok_kumas_oku.Read();
                    kumas = Convert.ToInt32(stok_kumas_oku.GetValue(2));

                    OleDbCommand stoktan_lastik_dus = new OleDbCommand("select * from stok where hammadde_adi='   lastik'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_lastik_oku = stoktan_lastik_dus.ExecuteReader();
                    stok_lastik_oku.Read();
                    lastik = Convert.ToInt32(stok_lastik_oku.GetValue(2));

                    deri_miktari -= 50;
                    kumas -= 50;
                    lastik -= 30;
                
                    OleDbCommand deri_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + deri_miktari + "' where hammadde_adi='     deri'", baglanti);
                    deri_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand kumas_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + kumas + "' where hammadde_adi=' kumaş'", baglanti);
                    kumas_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand lastik_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + lastik + "' where hammadde_adi='   lastik'", baglanti);
                    lastik_stok_guncelle.ExecuteNonQuery();
                   
                }
                else if (textEdit1.Text == "tekli koltuk")
                {
                    int deri_miktari, kumas, lastik;
                    OleDbCommand stoktan_deri_dus = new OleDbCommand("select * from stok where hammadde_adi='     deri'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_deri_oku = stoktan_deri_dus.ExecuteReader();
                    stok_deri_oku.Read();
                    deri_miktari = Convert.ToInt32(stok_deri_oku.GetValue(2));

                    OleDbCommand stoktan_kumas_dus = new OleDbCommand("select * from stok where hammadde_adi=' kumaş'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_kumas_oku = stoktan_kumas_dus.ExecuteReader();
                    stok_kumas_oku.Read();
                    kumas = Convert.ToInt32(stok_kumas_oku.GetValue(2));

                    OleDbCommand stoktan_lastik_dus = new OleDbCommand("select * from stok where hammadde_adi='   lastik'", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader stok_lastik_oku = stoktan_lastik_dus.ExecuteReader();
                    stok_lastik_oku.Read();
                    lastik = Convert.ToInt32(stok_lastik_oku.GetValue(2));

                    deri_miktari -= 30;
                    kumas -= 30;
                    lastik -= 10;

                    OleDbCommand deri_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + deri_miktari + "' where hammadde_adi='     deri'", baglanti);
                    deri_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand kumas_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + kumas + "' where hammadde_adi=' kumaş'", baglanti);
                    kumas_stok_guncelle.ExecuteNonQuery();
                    OleDbCommand lastik_stok_guncelle = new OleDbCommand("update stok set hammadde_miktari='" + lastik + "' where hammadde_adi='   lastik'", baglanti);
                    lastik_stok_guncelle.ExecuteNonQuery();
                }
                baglanti.Close();
                #endregion
                #region Süre Kaydetme İşlemi
                baglanti.Open();
                /*OleDbCommand urun_hesapla = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(dikisurunid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_hesapla_oku = urun_hesapla.ExecuteReader();
                urun_hesapla_oku.Read();
                if (urun_hesapla_oku.HasRows)
                {*/
                    sonuc = saat_dikim * 3600 + dakika_dikim * 60 + saniye_dikim;

                    OleDbCommand dikim_sure_guncelleme = new OleDbCommand("update siparisler set dikim_suresi=" + sonuc + " where id=" + dikisurunid, baglanti);
                    dikim_sure_guncelleme.ExecuteNonQuery();
               // }
                baglanti.Close();
                #endregion

                simpleButton4.Enabled = true;
                simpleButton5.Enabled = false;
                simpleButton11.Enabled = false;
                pictureEdit2.Visible = false;
                listBoxControl3.Items.Add(listBoxControl2.Items[1]);
                listBoxControl2.Items.RemoveAt(1);

                baglanti.Open();
                string text = "update siparisler  set asama='paketleme' where id= " + dikisurunid;
                OleDbCommand dikim_guncelleme = new OleDbCommand(text, baglanti);
                dikim_guncelleme.ExecuteNonQuery();
                baglanti.Close();

                if (listBoxControl2.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                {
                    baglanti.Open();
                    OleDbCommand dikim_id_bul = new OleDbCommand("select * from siparisler where asama='monte' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader id_oku = dikim_id_bul.ExecuteReader();
                    id_oku.Read();
                    textEdit1.Text = id_oku.GetValue(8).ToString();
                    dikisurunid = id_oku.GetValue(0).ToString();

                    dikimuruncesidiid = id_oku.GetValue(9).ToString();
                    OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(dikimuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                    urun_id_oku.Read();
                    labelControl8.Text = urun_id_oku.GetValue(2).ToString();
                    baglanti.Close();
                }
                else
                {
                    simpleButton4.Enabled = false;
                    textEdit1.Text = "Kuyruk Boş";
                    labelControl8.Text = "";
                }
                if (textEdit3.Text == "Kuyruk Boş")
                {
                    simpleButton6.Enabled = true;
                    if (listBoxControl3.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                    {
                        baglanti.Open();
                        OleDbCommand dikim_id_bul = new OleDbCommand("select * from siparisler where asama='paketleme' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader id_oku = dikim_id_bul.ExecuteReader();
                        id_oku.Read();
                        textEdit3.Text = id_oku.GetValue(8).ToString();
                        paketurunid = id_oku.GetValue(0).ToString();

                        paketuruncesidiid = id_oku.GetValue(9).ToString();
                        OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                        OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                        urun_id_oku.Read();
                        labelControl10.Text = urun_id_oku.GetValue(2).ToString();
                        baglanti.Close();
                    }
                    else
                    {
                        labelControl10.Text = "";
                        simpleButton6.Enabled = false;
                        textEdit3.Text = "Kuyruk Boş";
                    }
                }


            saniye_dikim = 0;
            saat_dikim = 0;
            dakika_dikim = 0;
            labelControl22.Text = saniye_dikim.ToString();
            labelControl21.Text = dakika_dikim.ToString();
            labelControl20.Text = saat_dikim.ToString();
            }
            else
                timer2.Start();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            cevap = MessageBox.Show("Paketleme işlemini başlatmak istediğinizden emin misiniz?", "PAKETLEME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {
                timer3.Start();
                simpleButton7.Enabled = true;
                simpleButton13.Enabled = true;
                simpleButton6.Enabled = false;
                pictureEdit3.Visible = true;
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            cevap = MessageBox.Show("Paketleme işlemini bitirmek istediğinizden emin misiniz?", "PAKETLEME İŞLEMİ ONAYI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == cevap)
            {

                #region Süre Kaydetme İşlemi
                baglanti.Open();
               /* OleDbCommand urun_hesapla = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketurunid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_hesapla_oku = urun_hesapla.ExecuteReader();
                urun_hesapla_oku.Read();
                if (urun_hesapla_oku.HasRows)
                {*/
                    sonuc = saat_paket * 3600 + dakika_paket * 60 + saniye_paket;

                    OleDbCommand paket_guncelleme = new OleDbCommand("update siparisler set paket_suresi=" + sonuc + " where id=" + paketurunid, baglanti);
                    paket_guncelleme.ExecuteNonQuery();
              //  }
                baglanti.Close();
                #endregion

                #region Ürün Ortalama Süresini Hesaplama
                    double urun_ortalama_suresi=0,urun_uretim_sayisi=0,urun_sure_toplam;
                string id="";
                    baglanti.Open();
                OleDbCommand urun_id_getir = new OleDbCommand("select * from siparisler where id="+paketurunid, baglanti);
                OleDbDataReader urun_oku = urun_id_getir.ExecuteReader();
                urun_oku.Read();
                if (urun_oku.HasRows)
                {
                    OleDbCommand ortalama_sure_getir = new OleDbCommand("select * from urunler where id="+ urun_oku.GetValue(9), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader ortalama_oku = ortalama_sure_getir.ExecuteReader();
                    ortalama_oku.Read();
                    
                       urun_ortalama_suresi =Convert.ToInt32(ortalama_oku.GetValue(3));
                        urun_uretim_sayisi = Convert.ToInt32(ortalama_oku.GetValue(4));
                        id = urun_oku.GetValue(9).ToString();
                    
                    
                    urun_uretim_sayisi++;
                    int montesure= Convert.ToInt32(urun_oku.GetValue(12));
                    int dikimsure=Convert.ToInt32(urun_oku.GetValue(13));
                    int paketsure= Convert.ToInt32(urun_oku.GetValue(14));
                    urun_sure_toplam =(urun_ortalama_suresi+(montesure+ dikimsure+ paketsure))/urun_uretim_sayisi;
                    urun_sure_toplam = Math.Ceiling(urun_sure_toplam);
                    string textim = "update urunler set urun_uretim_ortalama_suresi=" + urun_sure_toplam + ",urun_uretim_sayisi=" + urun_uretim_sayisi + " where id=" + id;
                    OleDbCommand urun_uretim_sayisi_guncelle = new OleDbCommand(textim,baglanti);
                    urun_uretim_sayisi_guncelle.ExecuteNonQuery();
                 //   MessageBox.Show(urun_sure_toplam.ToString());

                }
                baglanti.Close();
                #endregion
                
                simpleButton6.Enabled = true;
                simpleButton7.Enabled = false;
                simpleButton13.Enabled = false;
                pictureEdit3.Visible = false;

                listBoxControl3.Items.RemoveAt(1);
                baglanti.Open();

                #region ürün paketleme bitince teslimat tablosuna ekleyip siparislerden silen kodlar
                OleDbCommand satis_oku = new OleDbCommand("select * from siparisler where id="+paketurunid, baglanti);
                OleDbDataReader teslimat_oku = satis_oku.ExecuteReader();
              
                if (teslimat_oku.Read())
                {
                    OleDbCommand teslimat_ekle = new OleDbCommand("insert into teslimat(tc_no,adi,soyadi,telefon_no,adres,urun_turu,urun_id) values ('" + teslimat_oku.GetValue(1) + "','" + teslimat_oku.GetValue(2) + "','" + teslimat_oku.GetValue(3) + "','" + teslimat_oku.GetValue(4) + "','"+ teslimat_oku.GetValue(5) + "','"+ teslimat_oku.GetValue(8) + "','"+ teslimat_oku.GetValue(0) + "')", baglanti);
                    teslimat_ekle.ExecuteNonQuery();

                   
                    OleDbCommand siparisi_sil = new OleDbCommand("delete * from siparisler where id="+paketurunid,baglanti);
                    siparisi_sil.ExecuteNonQuery();
                }
            
                #endregion

                string text = "update siparisler  set asama='teslimat' where id= " + paketurunid;
                OleDbCommand paketleme_guncelleme = new OleDbCommand(text, baglanti);
                paketleme_guncelleme.ExecuteNonQuery();
                baglanti.Close();
                if(listBoxControl3.Items.Count > 1)//ilk elemanı zaten ürün no tc no ürün türü diye ben eklemiştim...
                {
                    baglanti.Open();
                    OleDbCommand paket_id_bul = new OleDbCommand("select * from siparisler where asama='paketleme' order by id asc", baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader id_oku = paket_id_bul.ExecuteReader();
                    id_oku.Read();
                    textEdit3.Text = id_oku.GetValue(8).ToString();
                    paketurunid = id_oku.GetValue(0).ToString();

                    paketuruncesidiid = id_oku.GetValue(9).ToString();
                    OleDbCommand urunler_id_bul = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                    OleDbDataReader urun_id_oku = urunler_id_bul.ExecuteReader();
                    urun_id_oku.Read();
                    labelControl10.Text = urun_id_oku.GetValue(2).ToString();
                    baglanti.Close();
                }
                else
                {
                    labelControl10.Text = "";
                    simpleButton6.Enabled = false;
                    textEdit3.Text = "Kuyruk Boş";
                }


                saniye_paket = 0;
                saat_paket = 0;
                dakika_paket = 0;

                labelControl28.Text = saniye_paket.ToString();
                labelControl27.Text = saat_paket.ToString();
                labelControl26.Text = dakika_paket.ToString();

                /*baglanti.Open();
                OleDbCommand urun_hesapla = new OleDbCommand("select * from urunler where id=" + Convert.ToInt32(paketuruncesidiid), baglanti);  //ilk olarak başlangıç aşamasındaki ürünlerin gelmesi için
                OleDbDataReader urun_hesapla_oku = urun_hesapla.ExecuteReader();
                urun_hesapla_oku.Read();
                if (urun_hesapla_oku.HasRows)
                {

                }
                baglanti.Close();*/
            }
            else
                timer3.Start();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();

            simpleButton9.Enabled = true;
            simpleButton10.Enabled = true;
            simpleButton12.Enabled = true;
            simpleButton8.Enabled = false;
            simpleButton11.Enabled = false;
            simpleButton13.Enabled = false;
            simpleButton2.Enabled = false;
            simpleButton7.Enabled = false;
            simpleButton5.Enabled = false;
            
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            timer3.Start();

            simpleButton9.Enabled = false;
            simpleButton10.Enabled = false;
            simpleButton12.Enabled = false;
            simpleButton8.Enabled = true;
            simpleButton11.Enabled = true;
            simpleButton13.Enabled = true;
            simpleButton2.Enabled = true;
            simpleButton5.Enabled = true;
            simpleButton7.Enabled = true;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;
            if (saniye == 60)
            {
                saniye = 0;
                labelControl15.Text = saniye.ToString();
                dakika++;
                if (dakika == 60)
                {
                    dakika = 0;
                    saat++;
                    labelControl13.Text = saat.ToString();
                    labelControl14.Text = dakika.ToString();
                }
            }
            labelControl15.Text = saniye.ToString();
            labelControl14.Text = dakika.ToString();
            labelControl13.Text = saat.ToString();
        }
    }
}
