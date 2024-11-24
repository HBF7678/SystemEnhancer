using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Optimizer
{
    public sealed partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            Options.ApplyTheme(this);

            pictureBox1.BackColor = Options.CurrentOptions.Theme;
        }

        private void About_Load(object sender, EventArgs e)
        {
            t1.Interval = 50;
            t2.Interval = 50;

            t1.Start();
        }

        private void t1_Tick(object sender, EventArgs e)
        {
            string s0 = "";
            string s1 = "S";
            string s2 = "Sy";
            string s3 = "Sys";
            string s4 = "Syst";
            string s5 = "Syste";
            string s6 = "System";
            string s7 = "SystemE";
            string s8 = "SystemEn";
            string s9 = "SystemEnhancer";

            switch (l1.Text)
            {
                case "":
                    l1.Text = s1;
                    break;
                case "S":
                    l1.Text = s2;
                    break;
                case "Sy":
                    l1.Text = s3;
                    break;
                case "Sys":
                    l1.Text = s4;
                    break;
                case "Syst":
                    l1.Text = s5;
                    break;
                case "Syste":
                    l1.Text = s6;
                    break;
                case "System":
                    l1.Text = s7;
                    break;
                case "SystemE":
                    l1.Text = s8;
                    break;
                case "SystemEn":
                    l1.Text = s9;
                    t1.Stop();
                    t2.Start();
                    break;
                case "SystemEnhancer":
                    l1.Text = s0;
                    break;
            }
        }

        private void t2_Tick(object sender, EventArgs e)
        {
            string s0 = "";
            string s1 = "Authors:";
            string s2 = "Sri Yeswanth Tammannagari";
            string s3 = "Abhishek Vishwakarma";
            string s4 = "Divyanshu Kumar";
            string s5 = "Bobby Yadav";
            string s6 = "2024";

            switch (l2.Text)
            {
                case "":
                    l2.Text = s1;
                    break;
                case "Authors:":
                    l2.Text = s2;
                    break;
                case "Sri Yeswanth Tammannagari":
                    l2.Text = s3;
                    break;
                case "Abhishek Vishwakarma":
                    l2.Text = s4;
                    break;
                case "Divyanshu Kumar":
                    l2.Text = s5;
                    break;
                case "Bobby Yadav":
                    l2.Text = s6;
                    t2.Stop();
                    break;
                case "2024":
                    l2.Text = s0;
                    break;
            }
        }

        private void l2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Removed external link
        }
    }
}
