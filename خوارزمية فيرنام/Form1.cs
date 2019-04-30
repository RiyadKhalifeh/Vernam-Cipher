using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace خوارزمية_فيرنام
{
   
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
        }
        public static int Mod(int a, int n)
        {
            return a - (int)Math.Floor((double)a / n) * n;
        }


        string RandKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz01234567ً89|أبتثجحخدذرزسشصضطظعغفقكلمنهويائءةلإؤى\\,.؟{}!@#$%^&*)(/-+<،>~:;?'=_";
        private string RandomString(int Size)
        {
            Random random = new Random();
            string RandomKey = "";
            for (int i = 0; i < Size; i++)
            {
                RandomKey += RandKey[random.Next(0, RandKey.Length)];
            }
            return RandomKey;
        }

        byte[] convertToBytes(string s)
        {
            byte[] result = new byte[(s.Length + 7) / 8];

            int i = 0;
            int j = 0;
            foreach (char c in s)
            {
                result[i] <<= 1;
                if (c == '1')
                    result[i] |= 1;
                j++;
                if (j == 8)
                {
                    i++;
                    j = 0;
                }
            }
            return result;
        }


        string binary;
        void EnDec()
        {
            string planText = textBoxFrom.Text;
            string KeyText = textBoxKey.Text;

            for (int i = 0; i < planText.Length; i++)
            {
                int PlanIndex = alphabets.IndexOf(planText[i]);
                int KeyIndex = alphabets.IndexOf(KeyText[i]);
                binary = "";
                char[] asciiplan = Convert.ToString(PlanIndex, 2).PadLeft(8, '0').ToCharArray();
                char[] asciiKey = Convert.ToString(KeyIndex, 2).PadLeft(8, '0').ToCharArray();
                for (int j = 0; j < 8; j++)
                {
                    if (asciiplan[j] == '1' && asciiKey[j] == '1')
                        binary += "0";
                    else if (asciiplan[j] == '0' && asciiKey[j] == '0')
                        binary += "0";
                    else if (asciiplan[j] == '0' && asciiKey[j] == '1')
                        binary += "1";
                    else if (asciiplan[j] == '1' && asciiKey[j] == '0')
                        binary += "1";
                }
                int @decimal = Convert.ToInt32(binary, 2);
                textBoxTo.Text += alphabets[@decimal % alphabets.Length];
            }
        }
        

        void EncryptOrDecrypt(string text, string key, bool cipher)
        {
            textBoxTo.Clear();
            string Cipher = "";
            string Text = "";

            if (cipher)
                Text = text;
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (File.ReadAllText(openFileDialog1.FileName).Length > textBoxKey.Text.Length)
                    {
                        MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                        return;
                    }
                    else
                        Text = File.ReadAllText(openFileDialog1.FileName);
                }
                else
                    return;
            }

            for (int c = 0; c < Text.Length; c++)
            {
                Cipher += (char)(Text[c] ^ key[c]);
            }


            if (cipher)
            {
                saveFileDialog1.Filter = "Text File | *.txt";
                saveFileDialog1.InitialDirectory = @"D:\فيرنام";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, Cipher);
                    MessageBox.Show("تم حفظ الشيفرة في المكان المحدد بنجاح", "عملية ناجحة");
                }
            }
            else
                textBoxTo.Text = Cipher;
        }


        byte[] Key, Msg;
        public void EnDeCrypt(string Message, string Userkey, bool Cipher)
        {
            textBoxTo.Clear();
            string txtCipher = string.Empty;
            ASCIIEncoding asc = new ASCIIEncoding();
            Key = asc.GetBytes(Userkey);
            if (Cipher)
                Msg = asc.GetBytes(Message);
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (File.ReadAllBytes(openFileDialog1.FileName).Length > textBoxKey.Text.Length)
                    {
                        MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                        return;
                    }
                    else
                        Msg = File.ReadAllBytes(openFileDialog1.FileName);
                }
                else
                    return;
            }

            byte[] Text = Msg;
            //
            for (int x = 0; x < Msg.Length; x++)
            {
                Text[x] = (byte)(Msg[x] ^ Key[x]);
            }

            txtCipher = Encoding.Default.GetString(Text);  //تحويل من أري بايتس إلى سترينج

            if (Cipher)
            {
                saveFileDialog1.Filter = "Text File | *.txt";
                saveFileDialog1.InitialDirectory = @"D:\فيرنام";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog1.FileName, Text);
                    MessageBox.Show("تم حفظ الشيفرة في المكان المحدد بنجاح", "عملية ناجحة");
                }
            }
            else
                textBoxTo.Text = txtCipher;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text == string.Empty || textBoxKey.Text == string.Empty)
                MessageBox.Show("أحد الحقول المطلوبة فارغ", "حقل مطلوب");
            else
                if (textBoxFrom.Text.Length > textBoxKey.Text.Length)
                    MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                else
                {
                    EncryptOrDecrypt(textBoxFrom.Text, textBoxKey.Text, true);
                }
        }

        private void buttonDeXor_Click(object sender, EventArgs e)
        {
            if (textBoxKey.Text == string.Empty)
                MessageBox.Show("حقل المفتاح فارغ", "حقل مطلوب");
            else
                EncryptOrDecrypt("", textBoxKey.Text, false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxFrom.Clear();
            textBoxKey.Clear();
            textBoxTo.Clear();
            textBox1.Clear();
            // EncryptOrDecrypt(textBoxFrom.Text, textBoxKey.Text, false);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label1.Text = textBoxFrom.Text.Length.ToString();
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            label2.Text = textBoxKey.Text.Length.ToString();
        }
        private void textBoxTo_TextChanged(object sender, EventArgs e)
        {
            labelTo.Text = textBoxTo.Text.Length.ToString();
        }






        private void button2_Click(object sender, EventArgs e)
        {
            //textBox3.Text = EnDeCrypt(textBox2.Text);
            Process(textBoxTo.Text, "A1");
            //  MessageBox.Show( EncryptOrDecrypt(textBox2.Text,"A1"));
            //string ka = textBox4.Text;
            //for (int c = 0; c < textBox2.Text.Length; c++)
            //{
            //    textBox3.Text += Convert.ToString((char)(textBox2.Text[c] ^ ka[c % ka.Length]));

            //}
        }


        public static string key = "sd";
        //int charValue = Convert.ToInt32(key);
        public static string EncryptDecrypt(string textToEncrypt)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ Convert.ToInt32(key));
                outSb.Append(c);
            }
            return outSb.ToString();
        }
        //string EncryptOrDecrypt(string text, string key)
        //{
        //    var result = new StringBuilder();

        //    for (int c = 0; c < text.Length; c++)
        //        result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

        //    return result.ToString();
        //}
        //public static string Encrypt(string Input, string Key)
        //{
        //    return Process(Input, Key);
        //}


        private void Process(string Input, string Key)
        {
            //if (Input.Length != Key.Length)
            //{  

            //    throw new ArgumentException("Key is not the same length as the input string");
            //}
            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            byte[] KeyArray = Encoding.GetBytes(Key);
            byte[] OutputArray = new byte[InputArray.Length];
            for (int x = 0; x < InputArray.Length; ++x)
            {

                OutputArray[x] = (byte)(InputArray[x] ^ Key[x % KeyArray.Length]);
                textBoxKey.Text += Encoding.GetString(OutputArray, x, 1);
            }
            // textBox2.Text = Encoding.GetString(OutputArray);
            //return Encoding.GetString(OutputArray);
        }

        private string GenerateKey(int KeyLength)
        {
            int Lbound = 48;
            int Ubound = 122;
            int k;
            string strKey = string.Empty;
            Random rnd = new Random();


            for (int x = 0; x < KeyLength; x++)
            {
                // This section here is what generates a random key with the help of
                // NextDouble method from the Random() class which produces a random
                // float values between 0 and 1
                k = Convert.ToInt32(((Ubound - Lbound) + 1) * rnd.NextDouble() + Lbound);
                // The cast the resulting binary value to its corresponding character value
                strKey = strKey + (char)k;
            }
            return strKey;
        }


        public void incryptimage()
        {
            byte[] chKey, chMsg;

            ASCIIEncoding asc = new ASCIIEncoding();
            chKey = asc.GetBytes("12abcR@#$%^&*isdfghiuytiwسسيبنتاع34yad3");
            Image Img = Image.FromFile(@"D:\DSC00262.JPG");
            chMsg = imageToByteArray(Img);

            // long jpegByteSize;
            // using (var ms = new MemoryStream()) // estimatedLength can be original fileLength
            //{
            //  Img.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg); // save image to stream in Jpeg format
            //  jpegByteSize = ms.Length;
            // }
            //new FileInfo("").Length;
            Byte[] FileBytes = File.ReadAllBytes(@"D:\#.txt");
            byte[] newimage = FileBytes;  //= new byte[999999999];
            byte[] Nategimage = FileBytes;//= new byte[999999999];

            for (int i = 0; i < FileBytes.Length; i++)
            {
                // 'k' gets the character exclusion (^) of each character in the
                // message and key in the same character index position
                newimage[i] = (byte)(FileBytes[i] ^ chKey[i % chKey.Length]);
            }
            File.WriteAllBytes(@"D:\RiyadCi", newimage);

            for (int i = 0; i < newimage.Length; i++)
            {
                // 'k' gets the character exclusion (^) of each character in the
                // message and key in the same character index position
                Nategimage[i] = (byte)(newimage[i] ^ chKey[i % chKey.Length]);
            }
            File.WriteAllBytes(@"D:\RiyadFileDe.txt", Nategimage);
            byte[] R = File.ReadAllBytes(@"D:\RiyadFileDe.txt");
            string hsh = Encoding.Default.GetString(R);
            textBoxKey.Text = hsh;
            MessageBox.Show(hsh);

            //MemoryStream mss = new MemoryStream(Nategimage);
            //Image returnImage = Image.FromStream(mss, false); returnImage.Save(@"D:\e.JPG");
            //pictureBox3.Image = returnImage;


        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Ana();
        }

        private void buttonGenrateKey_Click(object sender, EventArgs e)
        {
            try
            {
                int number;
                bool Isnumber = int.TryParse(textBox1.Text, out number);
                if (Isnumber)
                {
                    textBoxKey.Clear();
                    textBoxKey.Text = RandomString(Convert.ToInt32(textBox1.Text));
                }
                else
                    MessageBox.Show("أدخل قيمة عددية فقط");
            }
            catch
            { }
        }
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefgنhijklmnopمqrstuvwيxyz0123456789ً|\\,.؟{}!@#$%^&*\")(/-+<،>~:;?'=_أبتثجحخدذرزسشصضطظعغفقكلهوآائءةلإؤى";//"ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789|\\,.؟{}!@#$%^&*)(/-+<،>~:;?'=_أبتثجحخدذرزسشصضطظعغفقكلمنهويائءةلإؤى\r\n\t\v\f\b\a\"\0";
        private void buttonAddMin_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text == string.Empty || textBoxKey.Text == string.Empty)
                MessageBox.Show("أحد الحقول المطلوبة فارغ", "حقل مطلوب");
            else
                if (textBoxFrom.Text.Length > textBoxKey.Text.Length)
                    MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                else
                {
                    textBoxTo.Clear();
                    for (int i = 0; i < textBoxFrom.Text.Length; i++)
                        textBoxTo.Text += alphabets[Mod(alphabets.IndexOf(textBoxFrom.Text[i]) + alphabets.IndexOf(textBoxKey.Text[i]), alphabets.Length)];
                }
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text == string.Empty || textBoxKey.Text == string.Empty)
                MessageBox.Show("أحد الحقول المطلوبة فارغ", "حقل مطلوب");
            else
                if (textBoxFrom.Text.Length > textBoxKey.Text.Length)
                    MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                else
                {
                    textBoxTo.Clear();
                    for (int i = 0; i < textBoxFrom.Text.Length; i++)
                        textBoxTo.Text += alphabets[Mod(alphabets.IndexOf(textBoxFrom.Text[i]) - alphabets.IndexOf(textBoxKey.Text[i]), alphabets.Length)];
                }
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            textBoxFrom.RightToLeft = RightToLeft.Yes;
            textBoxKey.RightToLeft = RightToLeft.Yes;
            textBoxTo.RightToLeft = RightToLeft.Yes;
            textBoxFrom.Focus();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            textBoxFrom.RightToLeft = RightToLeft.No;
            textBoxKey.RightToLeft = RightToLeft.No;
            textBoxTo.RightToLeft = RightToLeft.No;
            textBoxFrom.Focus();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show(EnDeCrypt(textBoxFrom.Text));
            //EnDeCrypt(textBoxFrom.Text);
        }


        private void button2_Click_2(object sender, EventArgs e)
        {

            // folderBrowserDialog1.ShowDialog();

            //  timer1.Tick += new EventHandler(timer1_Tick);

            // MessageBox.Show(DateTime.Now.Second.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Random random = new Random();
            //textBoxKey.Text += DateTime.Now.Millisecond.ToString();
            ///*
            //int a = 0;
            //if (DateTime.Now.Second == 5  && DateTime.Now.Millisecond == 5)
            //{
            //    a = random.Next(0, 10);
            //    textBoxKey.Text += a.ToString();*/

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void buttonEnDec_Click(object sender, EventArgs e)
        {
            if (textBoxFrom.Text == string.Empty || textBoxKey.Text == string.Empty)
                MessageBox.Show("أحد الحقول المطلوبة فارغ", "حقل مطلوب");
            else
                if (textBoxFrom.Text.Length > textBoxKey.Text.Length)
                    MessageBox.Show("المفتاح يجب أن يكون بطول الرسالة", "المفتاح قصير");
                else
                    EnDec();

        }
    }








}

