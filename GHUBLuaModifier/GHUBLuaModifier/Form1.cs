using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;

namespace GHUBLuaModifier
{
    public partial class Form1 : Form
    {
        public class Weapons
        {
            // assault rifles
            //sledge/thatch
            public float L85A2;
            //hibana, iq, mayby other
            public float AR33;
            //ash, and other
            public float G36C;
            //ash
            public float R4C;
            //don't remember
            public float A556XI;
            //twitch
            public float F2;
            //ace, other
            public float AK12;
            //IQ, other
            public float AUGA2;
            //don't remember
            public float COMMANDO552;
            //jager I think
            public float CARBINEC416;
            //buck
            public float C8SFW;
            //don't remember
            public float MK17CQB;
            //don't remember
            public float PARA308;
            //don't remember
            public float TYPE89;
            //will add rest later
            public float C7E;
            public float M762;
            public float V308;
            public float SPEAR308;
            public float AR1550;
            public float M4;
            public float AK74M;
            public float ARX200;
            public float F90;
            public float COMMANDO9;
            public float SC3000K;

        }

        KeyboardHook hook = new KeyboardHook();

        #region BoringStuff

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        private string workingPath;
        private string configPath;
        private string scriptPath;


        private Thread thread;
        private Thread thread2;

        private Dictionary<String, String> myDictionary = new Dictionary<string, string>();

        private Weapons myWeapons;
        private Panel selectedPanel;

        private float selectedValue;

        public Form1()
        {
            InitializeComponent();

            hook.KeyPressed +=
            new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey(GHUBLuaModifier.ModifierKeys.Control | GHUBLuaModifier.ModifierKeys.Alt,
                Keys.F12);
        }

        private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            UpdateRapidFire();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            selectedPanel = r4cPanel;
            myWeapons = new Weapons();

            thread = new Thread(new ThreadStart(StartApp));
            thread.Start();
            thread2 = new Thread(new ThreadStart(loadWeapons));


        }

        private void StartApp()
        {
            if (!File.Exists("localdata.txt"))
            {
                var localFile = File.Create("localdata.txt");
                localFile.Close();
                this.Invoke(new MethodInvoker(() => DisableModifiers()));
                
            }
            int a = 1;
            while (a != 0)
            {
                AttemptLoadFiles();
                a = AttemptLoadFiles();
                System.Threading.Thread.Sleep(100);
            }
            if(a == 0)
            {
                
                thread2.Start();
                this.Invoke(new MethodInvoker(() => UpdateScriptName()));
                this.Invoke(new MethodInvoker(() => CheckRapidFire()));
            }
            
        }

        private void DisableModifiers()
        {
            r4cButton.Enabled = false;
            f2Button.Enabled = false;
            l85Button.Enabled = false;
            commandoButton.Enabled = false;
            type89Button.Enabled = false;
            ar33Button.Enabled = false;
            g36cButton.Enabled = false;
            a556xiButton.Enabled = false;
            ak12Button.Enabled = false;
            auga2Button.Enabled = false;
            carbineButton.Enabled = false;
            c8sfwButton.Enabled = false;
            mk17Button.Enabled = false;
            paraButton.Enabled = false;
            c7eButton.Enabled = false;
            m762Button.Enabled = false;
            v308Button.Enabled = false;
            spearButton.Enabled = false;
            ar15Button.Enabled = false;
            m4Button.Enabled = false;
            rapidFireButton.Enabled = false;
        }

        private void EnableModifiers()
        {
            r4cButton.Enabled = true;
            f2Button.Enabled = true;
            l85Button.Enabled = true;
            commandoButton.Enabled = true;
            type89Button.Enabled = true;
            ar33Button.Enabled = true;
            g36cButton.Enabled = true;
            a556xiButton.Enabled = true;
            ak12Button.Enabled = true;
            auga2Button.Enabled = true;
            carbineButton.Enabled = true;
            c8sfwButton.Enabled = true;
            mk17Button.Enabled = true;
            paraButton.Enabled = true;
            c7eButton.Enabled = true;
            m762Button.Enabled = true;
            v308Button.Enabled = true;
            spearButton.Enabled = true;
            ar15Button.Enabled = true;
            m4Button.Enabled = true;
            rapidFireButton.Enabled = true;
        }

        private void UpdateScriptName()
        {
            if (myDictionary.ContainsKey("name"))
            {

                scriptName.Text = myDictionary["name"].ToString();
            }
        }

        private void loadWeapons()
        {
            var text = File.ReadAllText("weapondata.json");
            //Console.WriteLine(text);
            Dictionary<string, float> localDict = JsonConvert.DeserializeObject<Dictionary<string, float>>(text);

            this.Invoke(new MethodInvoker(() => myWeapons.R4C = localDict["R4C"]));
            this.Invoke(new MethodInvoker(() => myWeapons.F2 = localDict["F2"]));
            this.Invoke(new MethodInvoker(() => myWeapons.L85A2 = localDict["L85A2"]));
            this.Invoke(new MethodInvoker(() => myWeapons.COMMANDO552 = localDict["COMMANDO552"]));
            this.Invoke(new MethodInvoker(() => myWeapons.TYPE89 = localDict["TYPE89"]));

            this.Invoke(new MethodInvoker(() => myWeapons.AR33 = localDict["AR33"]));
            this.Invoke(new MethodInvoker(() => myWeapons.G36C = localDict["G36C"]));
            this.Invoke(new MethodInvoker(() => myWeapons.A556XI = localDict["A556XI"]));
            this.Invoke(new MethodInvoker(() => myWeapons.AK12 = localDict["AK12"]));
            this.Invoke(new MethodInvoker(() => myWeapons.AUGA2 = localDict["AUGA2"]));

            this.Invoke(new MethodInvoker(() => myWeapons.CARBINEC416 = localDict["CARBINEC416"]));
            this.Invoke(new MethodInvoker(() => myWeapons.C8SFW = localDict["C8SFW"]));
            this.Invoke(new MethodInvoker(() => myWeapons.MK17CQB = localDict["MK17CQB"]));
            this.Invoke(new MethodInvoker(() => myWeapons.PARA308 = localDict["PARA308"]));
            this.Invoke(new MethodInvoker(() => myWeapons.C7E = localDict["C7E"]));

            this.Invoke(new MethodInvoker(() => myWeapons.M762 = localDict["M762"]));
            this.Invoke(new MethodInvoker(() => myWeapons.V308 = localDict["V308"]));
            this.Invoke(new MethodInvoker(() => myWeapons.SPEAR308 = localDict["SPEAR308"]));
            this.Invoke(new MethodInvoker(() => myWeapons.AR1550 = localDict["AR1550"]));
            this.Invoke(new MethodInvoker(() => myWeapons.M4 = localDict["M4"]));





            myWeapons.R4C = myWeapons.R4C * 10;
            myWeapons.F2 = myWeapons.F2 * 10;
            myWeapons.L85A2 = myWeapons.L85A2 * 10;
            myWeapons.COMMANDO552 = myWeapons.COMMANDO552 * 10;
            myWeapons.TYPE89 = myWeapons.TYPE89 * 10;

            myWeapons.AR33 = myWeapons.AR33 * 10;
            myWeapons.G36C = myWeapons.G36C * 10;
            myWeapons.A556XI = myWeapons.A556XI * 10;
            myWeapons.AK12 = myWeapons.AK12 * 10;
            myWeapons.AUGA2 = myWeapons.AUGA2 * 10;

            myWeapons.CARBINEC416 = myWeapons.CARBINEC416 * 10;
            myWeapons.C8SFW = myWeapons.C8SFW * 10;
            myWeapons.MK17CQB = myWeapons.MK17CQB * 10;
            myWeapons.PARA308 = myWeapons.PARA308 * 10;
            myWeapons.C7E = myWeapons.C7E * 10;

            myWeapons.M762 = myWeapons.M762 * 10;
            myWeapons.V308 = myWeapons.V308 * 10;
            myWeapons.SPEAR308 = myWeapons.SPEAR308 * 10;
            myWeapons.AR1550 = myWeapons.AR1550 * 10;
            myWeapons.M4 = myWeapons.M4 * 10;

            this.Invoke(new MethodInvoker(() => r4cTrackbar.Value = Convert.ToInt32(myWeapons.R4C)));
            this.Invoke(new MethodInvoker(() => f2Trackbar.Value = Convert.ToInt32(myWeapons.F2)));
            this.Invoke(new MethodInvoker(() => l85Trackerbar.Value = Convert.ToInt32(myWeapons.L85A2)));
            this.Invoke(new MethodInvoker(() => commandoTrackbar.Value = Convert.ToInt32(myWeapons.COMMANDO552)));
            this.Invoke(new MethodInvoker(() => type89Trackbar.Value = Convert.ToInt32(myWeapons.TYPE89)));

            this.Invoke(new MethodInvoker(() => ar33Trackbar.Value = Convert.ToInt32(myWeapons.AR33)));
            this.Invoke(new MethodInvoker(() => g36cTrackbar.Value = Convert.ToInt32(myWeapons.G36C)));
            this.Invoke(new MethodInvoker(() => a556xiTrackbar.Value = Convert.ToInt32(myWeapons.A556XI)));
            this.Invoke(new MethodInvoker(() => ak12Trackbar.Value = Convert.ToInt32(myWeapons.AK12)));
            this.Invoke(new MethodInvoker(() => auga2Trackbar.Value = Convert.ToInt32(myWeapons.AUGA2)));

            this.Invoke(new MethodInvoker(() => carbineTrackbar.Value = Convert.ToInt32(myWeapons.CARBINEC416)));
            this.Invoke(new MethodInvoker(() => c8sfwTrackbar.Value = Convert.ToInt32(myWeapons.C8SFW)));
            this.Invoke(new MethodInvoker(() => mk17Trackbar.Value = Convert.ToInt32(myWeapons.MK17CQB)));
            this.Invoke(new MethodInvoker(() => paraTrackbar.Value = Convert.ToInt32(myWeapons.PARA308)));
            this.Invoke(new MethodInvoker(() => c7eTrackbar.Value = Convert.ToInt32(myWeapons.C7E)));

            this.Invoke(new MethodInvoker(() => m762Trackbar.Value = Convert.ToInt32(myWeapons.M762)));
            this.Invoke(new MethodInvoker(() => v308Trackbar.Value = Convert.ToInt32(myWeapons.V308)));
            this.Invoke(new MethodInvoker(() => spearTrackbar.Value = Convert.ToInt32(myWeapons.SPEAR308)));
            this.Invoke(new MethodInvoker(() => ar15Trackbar.Value = Convert.ToInt32(myWeapons.AR1550)));
            this.Invoke(new MethodInvoker(() => m4Trackbar.Value = Convert.ToInt32(myWeapons.M4)));

            /*
             * 
            string none;
            Console.WriteLine("boop " + myDictionary.TryGetValue("name", out none));
            Console.WriteLine("boop " + myDictionary.TryGetValue("description", out none));
            */
            //Console.WriteLine(myDictionary["name"]);
        }

        private int AttemptLoadFiles()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("localdata.txt");
            try
            {
                int counter = 0;
                string line;
                
                while ((line = file.ReadLine()) != null)
                {
                    if(counter == 0)
                    {
                        configPath = line;
                    }
                    else if(counter == 1)
                    {
                        scriptPath = line;
                    }
                    counter++;
                }

                //Console.WriteLine(configPath);
                //Console.WriteLine(scriptPath);
                file.Close();
                this.Invoke(new MethodInvoker(() => EnableModifiers()));
                SetConnectedStatus();
                
            }

            catch (Exception exc)
            {
                Console.WriteLine(exc);
                
                this.Invoke(new MethodInvoker(() => currentStatus.Text = "No saved data, please select a folder"));
                this.Invoke(new MethodInvoker(() => currentStatus.ForeColor = Color.Red));
                return 1;
            }
            finally
            {
                file.Close();
            }

            return 0;
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        #region WindowMods

        private void CloseButton_Click(object sender, EventArgs e)
        {
            
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            if (thread2.IsAlive)
            {
                thread2.Abort();
            }
            
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        private void findFolder_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            
            
            if (browseFolders.ShowDialog() == DialogResult.OK)
            {
                workingPath = browseFolders.SelectedPath;
                configPath = workingPath + @"\config.json";
                scriptPath = workingPath + @"\script.lua";
                string[] lines = { configPath, scriptPath };
                if (File.Exists("localdata.txt"))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("localdata.txt"))
                    {
                        foreach (string line in lines)
                        {
                            file.WriteLine(line);
                        }
                    }
                    SetConnectedStatus();
                    UpdateScriptName();
                }
                else
                {
                    File.Create("localdata.txt");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("localdata.txt"))
                    {
                        foreach (string line in lines)
                        {
                            file.WriteLine(line);
                        }
                    }
                    SetConnectedStatus();
                    UpdateScriptName();
                }


            }
            else
            {
                currentStatus.Text = "Something happened. Please try again.";
                currentStatus.ForeColor = Color.Red;
            }
        }

        private void SetConnectedStatus()
        {
            LoadJson();

            this.Invoke(new MethodInvoker(() => currentStatus.Text = "Connected"));
            this.Invoke(new MethodInvoker(() => currentStatus.ForeColor = Color.Green));
            if (File.Exists("weapondata.json"))
            {
                if (File.ReadAllText("weapondata.json") != null)
                {
                    this.Invoke(new MethodInvoker(() => loadWeapons()));
                    
                }
                else
                {
                    this.Invoke(new MethodInvoker(() => SaveWeapons()));
                }
            }
            else
            {
                this.Invoke(new MethodInvoker(() => SaveWeapons()));
            }

        }

        private void LoadJson()
        {


            
            var text = File.ReadAllText(configPath);
            Console.WriteLine(text);
            myDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            /*
             * 
            string none;
            Console.WriteLine("boop " + myDictionary.TryGetValue("name", out none));
            Console.WriteLine("boop " + myDictionary.TryGetValue("description", out none));
            */
            Console.WriteLine(myDictionary["name"]);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // save all weapon data to a json file
            // on a separate thread of course
            //thread3 = new Thread(new ThreadStart(SaveWeapons));
            //thread3.Start();
            SaveWeapons();
        }

        private void SaveWeapons()
        {
            Dictionary<String, float> weaponValues = new Dictionary<string, float>();
            //unfortunately have to do this for ALL guns
            weaponValues.Add("R4C", r4cTrackbar.Value / 10f);
            weaponValues.Add("F2", f2Trackbar.Value / 10f);
            weaponValues.Add("L85A2", l85Trackerbar.Value / 10f);
            weaponValues.Add("COMMANDO552", commandoTrackbar.Value / 10f);
            weaponValues.Add("TYPE89", type89Trackbar.Value / 10f);

            weaponValues.Add("AR33", ar33Trackbar.Value / 10f);
            weaponValues.Add("G36C", g36cTrackbar.Value / 10f);
            weaponValues.Add("A556XI", a556xiTrackbar.Value / 10f);
            weaponValues.Add("AK12", ak12Trackbar.Value / 10f);
            weaponValues.Add("AUGA2", type89Trackbar.Value / 10f);

            weaponValues.Add("CARBINEC416", carbineTrackbar.Value / 10f);
            weaponValues.Add("C8SFW", c8sfwTrackbar.Value / 10f);
            weaponValues.Add("MK17CQB", mk17Trackbar.Value / 10f);
            weaponValues.Add("PARA308", paraTrackbar.Value / 10f);
            weaponValues.Add("C7E", c7eTrackbar.Value / 10f);

            weaponValues.Add("M762", m762Trackbar.Value / 10f);
            weaponValues.Add("V308", v308Trackbar.Value / 10f);
            weaponValues.Add("SPEAR308", spearTrackbar.Value / 10f);
            weaponValues.Add("AR1550", ar15Trackbar.Value / 10f);
            weaponValues.Add("M4", m4Trackbar.Value / 10f);

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText("weapondata.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, weaponValues);
            }
        }

        private void ResetPanels()
        {
            selectedPanel.BackColor = Color.FromArgb(0, 14, 30);
        }

        #region TrackBarChanged

        private void r4cTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = r4cTrackbar.Value / 10f;
            r4cValueLabel.Text = tempInt.ToString();
            myWeapons.R4C = tempInt * 10;
            
        }

        private void f2Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = f2Trackbar.Value / 10f;
            f2ValueLabel.Text = tempInt.ToString();
            myWeapons.F2 = tempInt * 10;
        }

        private void l85Trackerbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = l85Trackerbar.Value / 10f;
            l85ValueLabel.Text = tempInt.ToString();
            myWeapons.L85A2 = tempInt * 10;
        }

        private void commandoTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;

            tempInt = commandoTrackbar.Value / 10f;
            commandoValueLabel.Text = tempInt.ToString();
            myWeapons.COMMANDO552 = tempInt * 10;

        }

        private void type89Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = type89Trackbar.Value / 10f;
            type89ValueLabel.Text = tempInt.ToString();
            myWeapons.TYPE89 = tempInt * 10;
        }


        private void ar33Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = ar33Trackbar.Value / 10f;
            ar33ValueLabel.Text = tempInt.ToString();
            myWeapons.AR33 = tempInt * 10;
        }

        private void g36cTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = g36cTrackbar.Value / 10f;
            g36cValueLabel.Text = tempInt.ToString();
            myWeapons.G36C = tempInt * 10;
        }

        private void a556xiTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = a556xiTrackbar.Value / 10f;
            a556xiValueLabel.Text = tempInt.ToString();
            myWeapons.A556XI = tempInt * 10;
        }

        private void ak12Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = ak12Trackbar.Value / 10f;
            ak12ValueLabel.Text = tempInt.ToString();
            myWeapons.AK12 = tempInt * 10;
        }

        private void auga2Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = auga2Trackbar.Value / 10f;
            auga2ValueLabel.Text = tempInt.ToString();
            myWeapons.AUGA2 = tempInt * 10;
        }

        private void carbineTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = carbineTrackbar.Value / 10f;
            carbineValueLabel.Text = tempInt.ToString();
            myWeapons.CARBINEC416 = tempInt * 10;
        }

        private void c85sfwTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = c8sfwTrackbar.Value / 10f;
            c85sfwValueLabel.Text = tempInt.ToString();
            myWeapons.C8SFW = tempInt * 10;
        }

        private void mk17Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = mk17Trackbar.Value / 10f;
            mk17ValueLabel.Text = tempInt.ToString();
            myWeapons.MK17CQB = tempInt * 10;
        }

        private void paraTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = paraTrackbar.Value / 10f;
            paraValueLabel.Text = tempInt.ToString();
            myWeapons.PARA308 = tempInt * 10;
        }

        private void c7eTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = c7eTrackbar.Value / 10f;
            c7eValueLabel.Text = tempInt.ToString();
            myWeapons.C7E = tempInt * 10;
        }

        private void m762Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = m762Trackbar.Value / 10f;
            m762ValueLabel.Text = tempInt.ToString();
            myWeapons.M762 = tempInt * 10;
        }

        private void v308Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = v308Trackbar.Value / 10f;
            v308ValueLabel.Text = tempInt.ToString();
            myWeapons.V308 = tempInt * 10;
        }

        private void spearTrackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = spearTrackbar.Value / 10f;
            spearValueLabel.Text = tempInt.ToString();
            myWeapons.SPEAR308 = tempInt * 10;
        }

        private void ar15Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = ar15Trackbar.Value / 10f;
            ar15ValueLabel.Text = tempInt.ToString();
            myWeapons.AR1550 = tempInt * 10;
        }

        private void m4Trackbar_ValueChanged(object sender, EventArgs e)
        {
            float tempInt;
            tempInt = m4Trackbar.Value / 10f;
            m4ValueLabel.Text = tempInt.ToString();
            myWeapons.M4 = tempInt * 10;
        }



        #endregion


        #region WeaponButtonsPressed


        private void r4cButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            r4cPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.R4C / 10;
            selectedPanel = r4cPanel;
            ChangeScript();
        }

        private void f2Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            f2Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.F2 / 10;
            selectedPanel = f2Panel;
            ChangeScript();
        }

        private void l85Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            l85Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.L85A2 / 10;
            selectedPanel = l85Panel;
            ChangeScript();
        }

        private void commandoButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            commandoPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.COMMANDO552 / 10;
            selectedPanel = commandoPanel;
            ChangeScript();
        }

        private void type89Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            type89Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.TYPE89 / 10;
            selectedPanel = type89Panel;
            ChangeScript();
        }

        private void ar33Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            ar33Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.AR33 / 10;
            selectedPanel = ar33Panel;
            ChangeScript();
        }

        private void g36cButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            g36cPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.G36C / 10;
            selectedPanel = g36cPanel;
            ChangeScript();
        }

        private void a556xiButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            a556xiPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.A556XI / 10;
            selectedPanel = a556xiPanel;
            ChangeScript();
        }

        private void ak12Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            ak12Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.AK12 / 10;
            selectedPanel = ak12Panel;
            ChangeScript();
        }

        private void auga2Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            auga2Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.AUGA2 / 10;
            selectedPanel = auga2Panel;
            ChangeScript();
        }

        private void carbineButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            carbinePanel.BackColor = Color.Blue;
            selectedValue = myWeapons.CARBINEC416 / 10;
            selectedPanel = carbinePanel;
            ChangeScript();
        }

        private void c8sfwButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            c8sfwPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.C8SFW / 10;
            selectedPanel = c8sfwPanel;
            ChangeScript();
        }

        private void mk17Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            mk17cqbPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.MK17CQB / 10;
            selectedPanel = mk17cqbPanel;
            ChangeScript();
        }

        private void paraButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            paraPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.PARA308 / 10;
            selectedPanel = paraPanel;
            ChangeScript();
        }

        private void c7eButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            c7ePanel.BackColor = Color.Blue;
            selectedValue = myWeapons.C7E / 10;
            selectedPanel = c7ePanel;
            ChangeScript();
        }

        private void m762Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            m762Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.M762 / 10;
            selectedPanel = m762Panel;
            ChangeScript();
        }

        private void v308Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            v308Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.V308 / 10;
            selectedPanel = v308Panel;
            ChangeScript();
        }

        private void spearButton_Click(object sender, EventArgs e)
        {
            ResetPanels();
            spearPanel.BackColor = Color.Blue;
            selectedValue = myWeapons.SPEAR308 / 10;
            selectedPanel = spearPanel;
            ChangeScript();
        }

        private void ar15Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            ar15Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.AR1550 / 10;
            selectedPanel = ar15Panel;
            ChangeScript();
        }

        private void m4Button_Click(object sender, EventArgs e)
        {
            ResetPanels();
            m4Panel.BackColor = Color.Blue;
            selectedValue = myWeapons.M4 / 10;
            selectedPanel = m4Panel;
            ChangeScript();
        }

        #endregion

        private void ChangeScript()
        {
            //possibly gonna do a rapid fire section
            try
            {
                int tempVar = 0;
                using (var input = File.OpenText(scriptPath))
                using (var output = new StreamWriter("script.lua"))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        if (line.Contains("Sleep"))
                        {
                            if (tempVar == 0)
                            {
                                string a = line;
                                string b = "                        Sleep(" + selectedValue.ToString() + ")";

                                output.WriteLine(line.Replace(a, b));
                                tempVar += 1;
                            }
                            else
                            {
                                output.WriteLine(line);
                            }

                        }
                        else
                        {

                            output.WriteLine(line);
                        }


                    }
                }
                File.Delete(scriptPath);
                File.Move("script.lua", scriptPath);
            }
            catch(Exception ae)
            {
                Console.WriteLine(ae);
            }
           
        }

        private void CheckRapidFire()
        {
            try
            {
                int tempVar = 0;
                using (var input = File.OpenText(scriptPath))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        tempVar += 1;
                        if (tempVar == 12)
                        {
                            if (!line.Contains("until not"))
                            {
                                input.Close();
                                rapidFireButton.Text = "Disable Rapid Fire";
                            }
                            else
                            {
                                input.Close();
                                rapidFireButton.Text = "Enable Rapid Fire";
                            }

                        }
                    }
                }
            }
            catch (Exception ae)
            {
                Console.WriteLine(ae);
            }
        }

        private void UpdateRapidFire()
        {
            //easier just to store all these values and write them in
           
            try
            {
                int tempVar = 0;
                using (var input = File.OpenText(scriptPath))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        tempVar += 1;
                        if (tempVar == 12)
                        {
                            if (!line.Contains("until not"))
                            {
                                input.Close();
                                DeleteRapidFire();
                            }
                            else
                            {
                                input.Close();
                                WriteRapidFire();
                            }

                        }
                    }
                }
            }
            catch(Exception ae)
            {
                Console.WriteLine(ae);
            }
            //if line 12 contains "IfMouseButtonPressed"
            //rapid fire is on and disable it
            //else rapid fire is off and enable it

            //disable it:delete line 12 to 17
            //enable it: cut line 12 to the end, write in rapid fire script, write cut lines
            //line 12 to 17
            /*
               if IsMouseButtonPressed(4) then
               repeat
                   PressAndReleaseMouseButton(1)
                   Sleep(1)
               until not IsMouseButtonPressed(4)
               end
            */
        }

        private void WriteRapidFire()
        {
            string rapid1 = "                if IsMouseButtonPressed(4) then";
            string rapid2 = "                repeat";
            string rapid3 = "                    PressAndReleaseMouseButton(1)";
            string rapid4 = "                    Sleep(1)";
            string rapid5 = "                until not IsMouseButtonPressed(4)";
            string rapid6 = "                end";

            string replace1 = "            until not IsMouseButtonPressed(3)";
            string replace2 = "        end";
            string replace3 = "    end";
            string replace4 = "end";

            try
            {
                int tempVar = 0;
                using (var input = File.OpenText(scriptPath))
                using (var output = new StreamWriter("script.lua"))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        tempVar += 1;
                        if (tempVar < 12)
                        {
                            output.WriteLine(line);
                        }
                        else if (tempVar == 12)
                        {
                            output.WriteLine(rapid1);
                            output.WriteLine(rapid2);
                            output.WriteLine(rapid3);
                            output.WriteLine(rapid4);
                            output.WriteLine(rapid5);
                            output.WriteLine(rapid6);
                            output.WriteLine(replace1);
                            output.WriteLine(replace2);
                            output.WriteLine(replace3);
                            output.WriteLine(replace4);
                        }


                    }
                }
                File.Delete(scriptPath);
                File.Move("script.lua", scriptPath);

                rapidFireButton.Text = "Disable Rapid fire";
            }
            catch (Exception ae)
            {
                Console.WriteLine(ae);
            }
        }

        private void DeleteRapidFire()
        {

            string replace1 = "            until not IsMouseButtonPressed(3)";
            string replace2 = "        end";
            string replace3 = "    end";
            string replace4 = "end";

            try
            {
                int tempVar = 0;
                using (var input = File.OpenText(scriptPath))
                using (var output = new StreamWriter("script.lua"))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        tempVar += 1;
                        if (tempVar < 12)
                        {
                            output.WriteLine(line);
                        }
                        else if (tempVar == 12)
                        {
                            output.WriteLine(replace1);
                            output.WriteLine(replace2);
                            output.WriteLine(replace3);
                            output.WriteLine(replace4);
                        }


                    }
                }
                File.Delete(scriptPath);
                File.Move("script.lua", scriptPath);

                rapidFireButton.Text = "Enable Rapid fire";
            }
            catch (Exception ae)
            {
                Console.WriteLine(ae);
            }
        }
        private void UpdateHotKey(string whichKey)
        {
            int tempVar = 0;
            using (var input = File.OpenText(scriptPath))
            using (var output = new StreamWriter("script.lua"))
            {
                string line;
                while (null != (line = input.ReadLine()))
                {
                    if (line.Contains("IsKeyLockOn"))
                    {
                        if(tempVar == 0)
                        {
                            string a = line;
                            string b = @"    if IsKeyLockOn(""" + whichKey + @""")then";

                            output.WriteLine(line.Replace(a, b));
                            tempVar += 1;
                        }
                        else
                        {
                            output.WriteLine(line);
                        }
                    }
                    else
                    {
                        output.WriteLine(line);
                    }
                }
            }
            File.Delete(scriptPath);
            File.Move("script.lua", scriptPath);

        }

        private void UpdatePullDown(string amount)
        {
            //MoveMouseRelative
            int tempVar = 0;
            using (var input = File.OpenText(scriptPath))
            using (var output = new StreamWriter("script.lua"))
            {
                string line;
                while (null != (line = input.ReadLine()))
                {
                    if (line.Contains("MoveMouseRelative"))
                    {
                        if (tempVar == 0)
                        {
                            string a = line;
                            string b = @"                        MoveMouseRelative(" + amount + ")";

                            output.WriteLine(line.Replace(a, b));
                            tempVar += 1;
                        }
                        else
                        {
                            output.WriteLine(line);
                        }
                    }
                    else
                    {
                        output.WriteLine(line);
                    }
                }
            }
            File.Delete(scriptPath);
            File.Move("script.lua", scriptPath);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHotKey(listBox1.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowDialog();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            string tempString = "0," + trackBar1.Value.ToString();
            pullDownAmount.Text = tempString;
            UpdatePullDown(tempString);
            //Thread.Sleep(1000);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            UpdateRapidFire();
        }
    }

    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
