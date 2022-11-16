using Essy.Tools.InputBox;
using System.Text;

namespace RC5_Infobez
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
        }
        public static byte wordCount = 32;  
        public static byte u = (byte) (wordCount / 8);
        public static double golden_ratio = ((1 + Math.Sqrt(5)) / 2);
        public static double nedo_P = ((Math.E - 2) * Math.Pow(2, wordCount));
        public static double nedo_Q = ((golden_ratio - 1) * Math.Pow(2, wordCount));
        public static byte rounds = 12;
        public static byte t = (byte) (2 * (rounds + 1));
        public static byte keySize = 16;
        public static byte c = (byte)(keySize % u > 0 ? keySize / u + 1 : keySize / u);
        public static ulong mod = (ulong)Math.Pow(2, wordCount);

        public static List<byte> keyBytes = new List<byte>();
        public static byte[] wordBytes = new byte[8];
        public static byte[] wordOutBytes = new byte[8];
        public static ulong[] L = new ulong[c];
        public static ulong[] S = new ulong[t];
        public ulong P = Math.Round(nedo_P) % 2 != 0 ? (ulong)Math.Round(nedo_P) : Math.Round(nedo_P) > nedo_P ? (ulong)Math.Round(nedo_P) - 1 : (ulong)Math.Round(nedo_P) + 1;
        public ulong Q = Math.Round(nedo_Q) % 2 != 0 ? (ulong)Math.Round(nedo_Q) : Math.Round(nedo_Q) > nedo_Q ? (ulong)Math.Round(nedo_Q) - 1 : (ulong)Math.Round(nedo_Q) + 1;
        private void init(byte WordCount, byte Rounds, byte KeySize)
        {
            wordCount = WordCount;
            u = (byte)(wordCount / 8);
            golden_ratio = ((1 + Math.Sqrt(5)) / 2);
            nedo_P = ((Math.E - 2) * Math.Pow(2, wordCount));
            nedo_Q = ((golden_ratio - 1) * Math.Pow(2, wordCount));
            rounds = Rounds;
            t = (byte)(2 * (rounds + 1));
            keySize = KeySize;
            c = (byte)(keySize % u > 0 ? keySize / u + 1 : keySize / u);
            mod = (ulong)Math.Pow(2, wordCount);

            keyBytes = new List<byte>();
            wordBytes = new byte[wordCount / 4];
            wordOutBytes = new byte[wordCount / 4];
            L = new ulong[c];
            S = new ulong[t];
            P = Math.Round(nedo_P) % 2 != 0 ? (ulong)Math.Round(nedo_P) : Math.Round(nedo_P) > nedo_P ? (ulong)Math.Round(nedo_P) - 1 : (ulong)Math.Round(nedo_P) + 1;
            Q = Math.Round(nedo_Q) % 2 != 0 ? (ulong)Math.Round(nedo_Q) : Math.Round(nedo_Q) > nedo_Q ? (ulong)Math.Round(nedo_Q) - 1 : (ulong)Math.Round(nedo_Q) + 1;
    }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            keyBytes.Clear();
            S = new ulong[t];
            L = new ulong[c];
            var item = textBox4.Text.Trim().Split();
            int maxind = item.Length;
            for (int ind = 0; ind < maxind; ind++)
            {
                keyBytes.Add(Convert.ToByte(item[ind], 16));
            }
            L[c - 1] = 0;
            for (int ind = keyBytes.Count - 1; ind >= 0; ind--)
            {
                L[ind / u] = ROL(L[ind / u], 8) + keyBytes[ind];
            }
            S[0] = P;
            for (int ind = 1; ind < t; ind++)
            {
                S[ind] = (S[ind - 1] + Q) % mod;
            }
            ulong x = 0, y = 0;
            int i = 0, j = 0;
            int n = 3 * Math.Max(t, c);
            for (int k = 0; k < n; k++)
            {
                x = S[i] = (ROL((S[i] + x + y) % mod, 3)) % mod;
                y = L[j] = (ROL((L[j] + x + y) % mod, (x + y) % mod)) % mod;
                i = (i + 1) % t;
                j = (j + 1) % c;
            }
            var astr = "";
            var bstr = "";
            var temp = textBox3.Text.Trim().Split();
            for (int ind = 0; ind < temp.Length; ind++)
            {
                if (ind < temp.Length / 2) astr += temp[ind].ToString();
                else bstr += temp[ind].ToString();
            }
            ulong a = Convert.ToUInt64("0x" + astr, 16);
            ulong b = Convert.ToUInt64("0x" + bstr, 16);
            a = (a + S[0]) % mod;
            b = (b + S[1]) % mod;
            for (int ind = 1; ind < rounds + 1; ind++)
            {
                a = ((ROL((a ^ b), b) + S[2 * ind])) % mod;
                b = ((ROL((b ^ a), a) + S[2 * ind + 1])) % mod;
            }
            label6.Text = a.ToString("X2") + " " + b.ToString("X2");
            var stra = label6.Text.Split()[0];
            var strb = label6.Text.Split()[1];
            for (int index = 0; index < stra.Length; index += 2)
            {
                var st = stra[index].ToString() + stra[index + 1].ToString();
                label8.Text += Convert.ToChar(Convert.ToInt32(st, 16));
            }
            for (int index = 0; index < strb.Length; index += 2)
            {
                var st = strb[index].ToString() + strb[index + 1].ToString();
                label8.Text += Convert.ToChar(Convert.ToInt32(st, 16));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            keyBytes.Clear();
            S = new ulong[t];
            L = new ulong[c];
            var item = textBox4.Text.Trim().Split();
            int maxind = item.Length;
            for (int ind = 0; ind < maxind; ind++)
            {
                keyBytes.Add(Convert.ToByte(item[ind], 16));
            }
            L[c - 1] = 0;
            for (int ind = keyBytes.Count - 1; ind >= 0; ind--)
            {
                L[ind / u] = ROL(L[ind / u], 8) + keyBytes[ind];
            }
            S[0] = P;
            for (int ind = 1; ind < t; ind++)
            {
                S[ind] = (S[ind - 1] + Q) % mod;
            }
            ulong x = 0, y = 0;
            int i = 0, j = 0;
            int n = 3 * Math.Max(t, c);
            for (int k = 0; k < n; k++)
            {
                x = S[i] = (ROL((S[i] + x + y) % mod, 3)) % mod;
                y = L[j] = (ROL((L[j] + x + y) % mod, (x + y) % mod)) % mod;
                i = (i + 1) % t;
                j = (j + 1) % c;
            }
            var astr = "";
            var bstr = "";
            var temp = textBox3.Text.Trim().Split();
            for (int ind = 0; ind < temp.Length; ind++)
            {
                if (ind < temp.Length / 2) astr += temp[ind].ToString();
                else bstr += temp[ind].ToString();
            }
            ulong a = Convert.ToUInt64("0x" + astr, 16);
            ulong b = Convert.ToUInt64("0x" + bstr, 16);

            for (int ind = rounds; ind > 0; ind--)
            {
                b = (ROR((b - S[2 * ind + 1]), a)) ^ a;
                a = (ROR((a - S[2 * ind]), b)) ^ b;
            }
            b = (b - S[1] + mod) % mod;
            a = (a - S[0] + mod) % mod;
            label6.Text = a.ToString("X2") + " " + b.ToString("X2");
            label8.Text = "";
            var stra = label6.Text.Split()[0];
            var strb = label6.Text.Split()[1];
            for (int index = 0; index < stra.Length; index += 2)
            {
                var st = stra[index].ToString() + stra[index + 1].ToString();
                label8.Text += Convert.ToChar(Convert.ToInt32(st, 16));
            }
            for (int index = 0; index < strb.Length; index += 2)
            {
                var st = strb[index].ToString() + strb[index + 1].ToString();
                label8.Text += Convert.ToChar(Convert.ToInt32(st, 16));
            }   
        }
        private ulong ROL(ulong a, ulong offset)
        {
            var bitarr = Convert.ToString((int)a, 2).ToCharArray();
            byte[] arr = new byte[wordCount];
            byte[] res = new byte[wordCount];
            for (int i = bitarr.Length - 1; i >= 0; i--) arr[wordCount - (bitarr.Length - i)] = byte.Parse(bitarr[i].ToString());
            offset %= wordCount;
            if (offset == 0) return a;
            Array.Copy(arr, (int)offset, res, 0, arr.Length - (int)offset);
            Array.Copy(arr, 0, res, arr.Length - (int)offset, (int)offset);
            string toConv = "";
            foreach (var item in res) toConv += item.ToString();
            ulong ret = Convert.ToUInt64(toConv, 2);
            return ret;
        }
        private ulong ROR(ulong a, ulong offset)
        {
            var bitarr = Convert.ToString((int)a, 2).ToCharArray();
            byte[] arr = new byte[wordCount];
            byte[] res = new byte[wordCount];
            for (int i = bitarr.Length - 1; i >= 0; i--) arr[wordCount - (bitarr.Length - i)] = byte.Parse(bitarr[i].ToString());
            offset %= wordCount;
            if (offset == 0) return a;
            Array.Copy(arr, arr.Length - (int)offset, res, 0, (int)offset);
            Array.Copy(arr, 0, res, (int)offset, arr.Length - (int)offset);
            string toConv = "";
            foreach (var item in res) toConv += item.ToString();
            ulong ret = Convert.ToUInt64(toConv, 2);
            return ret;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfForm infForm = new InfForm();
            infForm.Visible = true;
        }

        private void алгоритмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (System.Diagnostics.Process process = new())
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = "https://ru.wikipedia.org/wiki/RC5";
                process.Start();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            string filename = openFileDialog1.FileName;
            string fileText = File.ReadAllText(filename);
            if (fileText.Length == keySize)
            {
                textBox2.Text = fileText;
                foreach (var item in fileText) textBox4.Text += $"{((byte)item).ToString("X2")} ";
            }
            else if (fileText.Length == 3 * keySize - 1)
            {
                textBox2.Text = fileText;
                textBox4.Text = fileText;
            }
            else MessageBox.Show("Данная реализация алгоритма поддерживает только 16-символьный ASCII ключ, либо 16 байтов в 16-ричной сс");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            string filename = openFileDialog1.FileName;
            string fileText = File.ReadAllText(filename);
            if (fileText.Length == (int)wordCount / 4)
            {
                textBox1.Text = fileText;
                foreach (var item in fileText) textBox3.Text += $"{((byte)item).ToString("X2")} ";
            }
            else if(fileText.Length == ((int) wordCount / 2) + 1)
            {
                textBox1.Text = fileText;
                textBox3.Text = fileText;
            }
            else MessageBox.Show("Данная реализация алгоритма поддерживает только 8-символьное ASCII слово, либо 2 слова по 4 байта в 16-ричной сс");
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void выбратьРазмерБлокаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte WordCount = byte.Parse(InputBox.ShowInputBox("Введите размер блока: ", null, false));
            init(WordCount, rounds, keySize);
        }

        private void выбратьРазмерКлючаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte KeySize = byte.Parse(InputBox.ShowInputBox("Введите размер ключа: ", null, false));
            init(wordCount, rounds, KeySize);
        }

        private void выбратьКоличествоРаундовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte Rounds = byte.Parse(InputBox.ShowInputBox("Введите количество раундов: ", null, false));
            init(wordCount, Rounds, keySize);
        }
    }
}