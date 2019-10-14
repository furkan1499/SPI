using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPI
{
    public partial class Form1 : Form
    {
        private double data;

        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;//webcam isminde tanımladığımız değişken bilgisayara kaç kamera bağlıysa onları tutan bir dizi. 
        private VideoCaptureDevice cam;//cam ise bizim kullanacağımız aygıt.

        private void Form1_Load(object sender, EventArgs e)
        {
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);//webcam dizisine mevcut kameraları dolduruyoruz.
            foreach (FilterInfo videocapturedevice in webcam)
            {
                comboBox1.Items.Add(videocapturedevice.Name);//kameraları combobox a dolduruyoruz.
            }
            comboBox1.SelectedIndex = 0; //Comboboxtaki ilk index numaralı kameranın ekranda görünmesi için
            //---------
            textBox1.ReadOnly = true;                    //textBox1'i yalnızca okunabilir şekilde ayarla
            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
                comboBox2.Items.Add(port);               //Seri portları comboBox1'e ekleme

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived); //DataReceived eventini oluşturma

        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data = double.Parse(serialPort1.ReadLine());                      //Veriyi al
            this.Invoke(new EventHandler(displayData_event));
        }
        private void displayData_event(object sender, EventArgs e)
        {                       //Gelen değeri ProgressBar'ın değerine eşitle
            textBox1.Text = data.ToString();
          
            if (0.00<data && data < 100.00)
            {
                Button2_Click_1(sender, e);
                Button4_Click_1(sender, e);
            }
        }
        private void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;
        }

     

       

    

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        { //For kapatılırken kamera açıksa kapatıyoruz.
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
            cam.Start();

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {

            if (cam.IsRunning) //kamera açıksa kapatıyoruz.
            {
                cam.Stop();
            }
            
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image.Save(@"C:\Users\furkan\Desktop\a");
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    pictureBox1.Image.Save(saveFileDialog1.FileName);//Picturebox'taki görüntüyü kaydediyoruz.
            //}
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox2.Text;  //ComboBox1'de seçili nesneyi port ismine ata
                serialPort1.BaudRate = 9600;            //BaudRate 9600 olarak ayarla
                serialPort1.Open();                     //Seri portu aç
               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");    //Hata mesajı göster
            }
        }
    }
}
