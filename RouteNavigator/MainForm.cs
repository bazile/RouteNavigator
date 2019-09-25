using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using RouteNavigator.Properties;

// TODO: treeview

namespace RouteNavigator
{
    public partial class MainForm : Form
    {
        private const string TITLE = "Route navigator";

        private IRouteDisplay _routeDisplay;
        private string _currentPath;

        public MainForm()
        {
            InitializeComponent();
            _routeDisplay = listViewScreen;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = TITLE;

            var settings = Settings.Default;
            //settings.Upgrade();

            if (settings.RecentFiles?.Count > 0)
            {
                _currentPath = settings.RecentFiles[0];
                DisplayRoutesFrom(_currentPath);
            }
        }

        private void LoadDLLsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "Dll files (*.dll)|*.dll";
                ofd.FilterIndex = 1;
                ofd.Multiselect = false;
                if (_currentPath != null)
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(_currentPath);
                }
                if (ofd.ShowDialog() != DialogResult.OK) return;
                DisplayRoutesFrom(ofd.FileName);
            }
        }

        private void DisplayRoutesFrom(string path)
        {
            List<Route> routes;
            try
            {
                routes = RouteHelper.GetAllRoutes(path);
            }
            catch (BadImageFormatException)
            {
                MessageBox.Show("Selected file does not appear to be a managed assembly");
                return;
            }

            switch (routes.Count)
            {
                case 0:
                    toolStripStatusLabel.Text = "No routes were found";
                    break;
                case 1:
                    toolStripStatusLabel.Text = "1 route was found";
                    break;
                default:
                    toolStripStatusLabel.Text = $"{routes.Count} routes were found";
                    break;
            }
            _routeDisplay.Routes = routes;
            _routeDisplay.DisplayRoutes();

            _currentPath = path;

            SaveRecentPath(path);

            Text = TITLE + ": " + path;
        }

        private void SaveRecentPath(string path)
        {
            var settings = Settings.Default;
            if (settings.RecentFiles == null)
            {
                settings.RecentFiles = new StringCollection { path };
            }
            else
            {
                var recentFiles = settings.RecentFiles;
                for (int i = 0; i < recentFiles.Count; i++)
                {
                    if (path.Equals(recentFiles[i], StringComparison.OrdinalIgnoreCase))
                    {
                        recentFiles.RemoveAt(i);
                        i--;
                    }
                }
                recentFiles.Insert(0, path);
            }
            settings.Save();

            // Update Recent menu
            foreach (ToolStripItem item in recentToolStripMenuItem.DropDownItems)
            {
                item.Click -= OnRecentItem_Click;
            }
            recentToolStripMenuItem.DropDownItems.Clear();

            foreach (string recentPath in Settings.Default.RecentFiles)
            {
                var item = recentToolStripMenuItem.DropDownItems.Add(Path.GetFileName(recentPath));
                item.Tag = recentPath;
                item.Click += OnRecentItem_Click;
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentPath != null)
            {
                DisplayRoutesFrom(_currentPath);
            }
        }

        private void OnFileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentToolStripMenuItem.Enabled = Settings.Default.RecentFiles?.Count > 0;
        }

        private void OnRecentItem_Click(object sender, EventArgs e)
        {
            DisplayRoutesFrom((string)((ToolStripItem) sender).Tag);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
