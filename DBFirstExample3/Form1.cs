using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBFirstExample3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NorthwindEntities db = new NorthwindEntities();

        private void button1_Click(object sender, EventArgs e)
        {
            //#region
            // fiyatı 20 ile 50 arasında olan ürünlerin ID,adi,Fiyatı, Stok Miktarı ve Kategorisini getiren sorgu

            #region LINQ To SQL
            var result = from p in db.Products
                         where p.UnitPrice > 20 && p.UnitPrice < 50
                         orderby p.UnitPrice descending
                         select new
                         {
                             UrunID = p.ProductID,
                             UrunAdi = p.ProductName,
                             Fiyat = p.UnitPrice,
                             Stok = p.UnitsInStock,
                             Kategori = p.Category.CategoryID
                         };
            dataGridView1.DataSource = result.ToList();
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            // Siparişler tablosundan Müşteri adı, Çalışan adı soyadı, SiparişID, Sipariş Tarihi ve kargo şirket adı
            #region LINQ To SQL
            var sonuc = from o in db.Orders
                        select new
                        {
                            MüsteriAdi = o.Customer.CompanyName,
                            CalisanAdiSoyadi = o.Employee.FirstName + " " + o.Employee.LastName,
                            SiparisID = o.OrderID,
                            SiparisTarihi = o.OrderDate,
                            KargoSirketi = o.Shipper.CompanyName,
                        };

            dataGridView1.DataSource = sonuc.ToList();
            #endregion

            #region Linq to Entity
            dataGridView1.DataSource = db.Orders.Select(o => new
            {
                MusteriSirketAdi = o.Customer.CompanyName,
                CalisanAdiSoyadi = o.Employee.FirstName + " " + o.Employee.LastName,
                SiparisID = o.OrderID,
                SiparisTarihi = o.OrderDate,
                KargoSirketi = o.Shipper.CompanyName,
            }).ToList();
            #endregion




        }

        private void button3_Click(object sender, EventArgs e)
        {
            #region LINQ to SQL

            // CompanyName içerisinde restaurant geçen müşteriler
            var sonuc = from c in db.Customers
                        where c.CompanyName.Contains("restaurant")
                        select c;
            dataGridView1.DataSource = sonuc.ToList();

            #endregion

            #region Linq to Entity
            dataGridView1.DataSource = db.Customers.Where(x => x.CompanyName.Contains("restaurant")).ToList();
            #endregion

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Kategorisi Beverages olan ve ürün adı: Kola, Fiyatı:5.00, Stok Adedi 500 olan ürün ekleme
            /* 1.yol 
            #region
            int kategoriID = db.Categories.FirstOrDefaul(x => x.CategoryName == "beverages").CategoryID;

            Product urun = new Product();
            urun.ProductName = "Kola";
            urun.UnitPrice = 5;
            urun.UnitsInStock = 500;
            urun.CategoryID = kategoriID;
            db.Products.Add(urun);
            #endregion */
            
            /* 2.yol

            
            db.Products.Add(new Product
            {
                ProductName = "Kola",
                UnitPrice = 5,
                UnitsInStock = 500,
                CategoryID = kategoriID
   

             db.Categories.FirstOrDefault(x => x.CategoryID == "beverages")}); */
            //3.YOL
            db.Categories.FirstOrDefault(x => x.CategoryName == "beverages").Products.Add(new Product
            {
                ProductName = "Kola2",
                UnitPrice = 5,
                UnitsInStock = 500
            });

            db.SaveChanges();
            dataGridView1.DataSource = db.Products.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Çalışanların adını, soyadını doğum tarihi ve yaşını getiren sorgu
            #region Linq To SQL
            var sonuc = from E in db.Employees
                        select new
                        {
                            AdiSoyadi = E.FirstName + " " + E.LastName,
                            DogumTarihi = E.BirthDate,
                            Yas = SqlFunctions.DateDiff("Year", E.BirthDate, DateTime.Now)
                        };
            dataGridView1.DataSource = sonuc.ToList();
            #endregion

            #region Linq to Entity
            dataGridView1.DataSource = db.Employees.Select(x => new
            {
                AdiSoyadi = x.FirstName + " " + x.LastName,
                DogumTarihi = x.BirthDate,
                Yas = SqlFunctions.DateDiff("YEAR" + x.BirthDate, DateTime.Now)
            }).ToList();
               
            #endregion
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Kategorilerine göre stotakti ürün sayısını veren sorgu
            #region Linq To SQL

            var sonuc = from p in db.Products 
                        select new
                        {
                            KategoriAdı=p.Category.CategoryName,
                            StokSayısı=p.UnitsInStock)
                        }
                        dataGridView1.DataSource = sonuc.ToList();
            #endregion

        }
    }
}
