using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Memory.Win64;
using Memory.Utils;
using System.IO;
using System.Net.NetworkInformation;
using static Testicles.ConsoleH;
using System.Windows.Forms.VisualStyles;
using System.Media;
using System.Reflection;

namespace Testicles
{
    public partial class Form1 : Form
    {
        private SoundPlayer soundPlayer;

        private void LoadEmbeddedAudio(string resourceName)
        {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
                using (Stream resourceStream = assembly.GetManifestResourceStream(fullResourceName))
                {
                        soundPlayer.Stream = resourceStream;
                        soundPlayer.PlayLooping();
                }
            }

        public Form1()
        {
            InitializeComponent();
            soundPlayer = new SoundPlayer();
            
        }

        //mp addresses
        ulong baseAddrFOW, baseAddrDBG, baseAddrAT, targetAddrTS, targetAddrTD;
        ulong StartGame;

        //sp addresses
        ulong baseAddrROC, baseAddrIC, baseAddrNOCB, baseAddrIW, baseAddrIT, baseAddrFA, baseAddrFNC;
        int i = 0;
        bool music = true;
        MemoryHelper64 helper;
        






        private void Form1_Load(object sender, EventArgs e)
        {

            //ConsoleHelper.AllocConsole();
            //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            //Console.WriteLine("[AdamHook DEBUG] Loaded");





            Process p = Process.GetProcessesByName("hoi4").FirstOrDefault();
            if (p == null)
            {
                label14.Text = "Hoi4 Not Connected: Restart";
                //Console.WriteLine("[AdamHook DEBUG] CANNOT CONNECT TO HOI4. RESTART PROGRAM");
                LoadEmbeddedAudio("heaven.wav");
                return;

            }
            label14.Text = "Hoi4 Connected";
            LoadEmbeddedAudio("purgatory.wav");
            Console.WriteLine("[AdamHook DEBUG] HOI4 CONNECTED.");
            Console.WriteLine("[AdamHook DEBUG] Updated for version 1.13.4");
            helper = new MemoryHelper64(p);

            

            // works in MP

            //Tag Switch
            ulong baseAddrTS = helper.GetBaseAddress(0x2C87850); 
            int[] offsetTS = { 0x4A0 };
            targetAddrTS = MemoryUtils.OffsetCalculator(helper, baseAddrTS, offsetTS);

            //Tdebug
            ulong baseAddrTD = helper.GetBaseAddress(0x2C87860);
            int[] offsetTD = { 0x78 };
            targetAddrTD = MemoryUtils.OffsetCalculator(helper, baseAddrTD, offsetTD);


            //FOW
            baseAddrFOW = helper.GetBaseAddress(0x2AABDDA);

            //AllowTraits
            baseAddrAT = helper.GetBaseAddress(0x2AABDB8);

            //Debug
            baseAddrDBG = helper.GetBaseAddress(0x2C8732C);




            //StartGame = helper.GetBaseAddress(0xEBD230);



            // works in sp
            
            //research on click
            baseAddrROC = helper.GetBaseAddress(0x2AABDB6);

            //instant construction
            baseAddrIC = helper.GetBaseAddress(0x2AABDC9);

            //no diplo
            baseAddrNOCB = helper.GetBaseAddress(0x2AABDB7);

            //instant wargoal
            baseAddrIW = helper.GetBaseAddress(0x2AABDE4);

            //instant training
            baseAddrIT = helper.GetBaseAddress(0x2AABDCC);

            //focus autocomplete
            baseAddrFA = helper.GetBaseAddress(0x2AABDCD);

            //focus nochecks
            baseAddrFNC = helper.GetBaseAddress(0x2AABDD2);

            timer1.Start();
        }

      


        //Refresh to see new memory value
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            i++;
            if(i == 100)
            { Console.WriteLine("[AdamHook DEBUG] LOOP x100"); i = 0; }
            
            // FOW
            if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "0")
            {
                label4.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "1")
            {
                label4.Text = "On";
            }

            // Allow Traits
            if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "0")
            {
                label5.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "1")
            {
                label5.Text = "On";
            }

            // Debug
            if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "0")
            {
                label6.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "1")
            {
                label6.Text = "On";
            }

            // Tdebug
            if (helper.ReadMemory<Int32>(targetAddrTD).ToString() == "0")
            {
                label12.Text = "Off";
            }
            else if (helper.ReadMemory<Int32>(targetAddrTD).ToString() == "1")
            {
                label12.Text = "On";
            }

            //Tagswitch
            label1.Text = helper.ReadMemory<Int32>(targetAddrTS).ToString();

            // Research on click
            if (helper.ReadMemory<Byte>(baseAddrROC).ToString() == "0")
            {
                label30.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrROC).ToString() == "1")
            {
                label30.Text = "On";
            }

            // instant construction
            if (helper.ReadMemory<Byte>(baseAddrIC).ToString() == "0")
            {
                label18.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrIC).ToString() == "1")
            {
                label18.Text = "On";
            }

            // no diplo
            if (helper.ReadMemory<Byte>(baseAddrNOCB).ToString() == "0")
            {
                label20.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrNOCB).ToString() == "1")
            {
                label20.Text = "On";
            }

            // instant wargoal
            if (helper.ReadMemory<Byte>(baseAddrIW).ToString() == "0")
            {
                label22.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrIW).ToString() == "1")
            {
                label22.Text = "On";
            }

            // instant training
            if (helper.ReadMemory<Byte>(baseAddrIT).ToString() == "0")
            {
                label24.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrIT).ToString() == "1")
            {
                label24.Text = "On";
            }
            
            // focus auto
            if (helper.ReadMemory<Byte>(baseAddrFA).ToString() == "0")
            {
                label26.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrFA).ToString() == "1")
            {
                label26.Text = "On";
            }
            
            // focus nochecks
            if (helper.ReadMemory<Byte>(baseAddrFNC).ToString() == "0")
            {
                label28.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrFNC).ToString() == "1")
            {
                label28.Text = "On";
            }


        }

        //BUTTONS
        private void button1_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTS, Int32.Parse(textBox1.Text));
            //Console.WriteLine("[AdamHook DEBUG] 0x2C71168 4Byte (INT32) + Offset 0x4A0 | 4Byte Value set to " + textBox1.Text);
        }  
        private void button7_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("1"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2C70C4C | Byte Value set to 1 ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("0"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2A95DCA | Byte Value set to 0 ");
            
        }

        private void button23_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrROC, Byte.Parse("1"));
        }

        private void button22_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrROC, Byte.Parse("0"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIC, Byte.Parse("1"));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIC, Byte.Parse("0"));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrNOCB, Byte.Parse("1"));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrNOCB, Byte.Parse("0"));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIW, Byte.Parse("1"));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIW, Byte.Parse("0"));
        }

        private void button17_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIT, Byte.Parse("1"));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrIT, Byte.Parse("0"));
        }

        private void button19_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFA, Byte.Parse("1"));
        }

        private void button18_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFA, Byte.Parse("0"));
        }

        private void button21_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFNC, Byte.Parse("1"));
        }

        private void button20_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFNC, Byte.Parse("0"));
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

            music = !music;

            if (music == true)
            {
                soundPlayer.PlayLooping();
            }
            else if (music == false)
            {
                soundPlayer.Stop();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("0"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2A95DA8 | Byte Value set to 0 ");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("0"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2C70C4C | Byte Value set to 0 ");
        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("1"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2A95DA8 | Byte Value set to 1 ");
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTD, Int32.Parse("1"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2C70C4C | Byte Value set to 1 ");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTD, Int32.Parse("0"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2C70C4C | Byte Value set to 1 ");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //DO NOT USE THIS!
            helper.WriteMemory<Byte>(StartGame, Byte.Parse("40"));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.com/invite/tZMQwYdJjq");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("1"));
            //Console.WriteLine("[AdamHook DEBUG] 0x2A95DCA | Byte Value set to 1 ");
        } 

        //Youtube Link
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/@AdamDX1337");
        }
    }
}









