using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using Newtonsoft.Json;

namespace Optimizer
{
    public partial class OPTIMIZER : Form
    {
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {         
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4); //cień?
        }

        private readonly UpdateChecker _updateChecker;
        private string _selectedPath = @"C:\";
        private BackgroundWorker _backgroundWorker;
        public OPTIMIZER()
        {
            InitializeComponent();         
            this.FormBorderStyle = FormBorderStyle.FixedSingle;


            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
           // _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            button1.Click += Button_Click;
            button2.Click += Button2_Click;
            selectedPathTextBox.Text = _selectedPath;

            UpdateButtonStatus();


            _updateChecker = new UpdateChecker();

            if (_updateChecker.CheckForUpdates())
            {
                _updateChecker.ShowUpdateDialog();
            }
        }
        private void UpdateProgressBarMax(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Maximum += value;
                });
            }
            else
            {
                progressBar1.Maximum += value;
            }
        }
        public void UpdateProgressBar(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value += value;
                });
            }
            else
            {
                progressBar1.Value += value;
            }
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Maximum = 1;
                });
            }
            else
            {
                progressBar1.Maximum = 1;
            }

            UpdateProgressBar(1);
            
            if (checkBox1.Checked)
                UpdateProgressBarMax(2);
            if (checkBox2.Checked)
                UpdateProgressBarMax(2);
            if (checkBox3.Checked)
                UpdateProgressBarMax(2);
            if (checkBox4.Checked)
                UpdateProgressBarMax(2);
            if (checkBox5.Checked)
                UpdateProgressBarMax(2);
            if (checkBox6.Checked)
                UpdateProgressBarMax(2);
            if (checkBox7.Checked)
                UpdateProgressBarMax(2);
            if (checkBox8.Checked)
                UpdateProgressBarMax(2);
            if (checkBox9.Checked)
                UpdateProgressBarMax(2);
            if (checkBox10.Checked)
                UpdateProgressBarMax(2);
            if (checkBox11.Checked)
                UpdateProgressBarMax(2);

            if (checkBox1.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.CreateBackup(_selectedPath);
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox2.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.OptimizeRegistry();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox3.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.OptimizeBCDEdit();
                    UpdateProgressBar(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wystąpił błąd: " + ex.Message);
                }
            }

            if (checkBox4.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.SetPowerPlan();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox5.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.DisableWindowsDefender();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox6.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.DisableGenuineNotification();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox7.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.OptimizeMouse();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox8.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.DisableCortana();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox9.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.DisableMaintenance();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox10.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.CreateGodModeFolder();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }

            if (checkBox11.Checked)
            {
                try
                {
                    UpdateProgressBar(1);
                    Optimizations.DisableWindowsUpdate();
                    UpdateProgressBar(1);
                }
                catch
                {
                    MessageBox.Show("ERROR: .");
                }
            }
            System.Threading.Thread.Sleep(1000);

            MessageBox.Show("Complete!");
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = 0;
                });
            }
            else
            {
                progressBar1.Value = 0;
            }
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    
                    _selectedPath = dialog.SelectedPath;
                    
                    selectedPathTextBox.Text = _selectedPath;

                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            _backgroundWorker.RunWorkerAsync();
        }
        private void UpdateButtonStatus()
        {
            bool anyChecked = checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked || checkBox6.Checked || checkBox7.Checked || checkBox8.Checked || checkBox9.Checked || checkBox10.Checked || checkBox11.Checked;
            button1.Enabled = anyChecked;
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // aktualizajce UI
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox7.Checked = true;
            checkBox9.Checked = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;

        }
        private void Optimizer_Load(object sender, EventArgs e)
        {

        }

        
    }
}
