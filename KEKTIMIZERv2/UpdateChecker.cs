using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class UpdateChecker
{
    private const string GitHubApiUrl = "https://api.github.com/repos/Szago/Optimizer/releases/latest";

    public bool CheckForUpdates()
    {
        using (WebClient client = new WebClient())
        {
            client.Headers.Add("User-Agent", "Optimizer");
            string json = client.DownloadString(GitHubApiUrl);
            dynamic release = JsonConvert.DeserializeObject(json);
            string latestVersion = release.tag_name;
            return latestVersion != Application.ProductVersion;
        }
    }

    public void ShowUpdateDialog()
    {
        Form customForm = new Form
        {
            Text = "NEW VERSION AVAILABLE",
            Width = 250,
            Height = 200,
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedSingle,
            MaximizeBox = false,
            MinimizeBox = false
        };

        Label label = new Label
        {
            Text = "NEW VERSION AVAILABLE!!!",
            Location = new Point(45, 50),
            AutoSize = true
        };

        LinkLabel linkLabel = new LinkLabel
        {
            Text = ">> DOWNLOAD HERE <<",
            Location = new Point(50, 80),
            AutoSize = true
        };
        linkLabel.Links.Add(0, linkLabel.Text.Length, "https://github.com/Szago/Optimizer/releases/latest");
        linkLabel.LinkClicked += (sender, e) => Process.Start(e.Link.LinkData.ToString());

        Button okButton = new Button
        {
            Text = "OK",
            Width = 60,
            Height = 30,
            Location = new Point(85, 120)
        };
        okButton.Click += (sender, e) => customForm.Close();

        customForm.Controls.Add(label);
        customForm.Controls.Add(linkLabel);
        customForm.Controls.Add(okButton);

        customForm.ShowDialog();
    }
}
