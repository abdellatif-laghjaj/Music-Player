using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Music_Player_V2
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        string[] paths, files;
        string path;


        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void selectSongsBtn_Click(object sender, EventArgs e)
        {
            WMPLib.IWMPPlaylist playlist = WindowsMediaPlayer.playlistCollection.newPlaylist("myplaylist");
            WMPLib.IWMPMedia media;

            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true;
            file.InitialDirectory = "Desktop\\";
            file.Filter = "mp3 files (*.mp3)|*.mp3 | wav files (*.wav)|*.wav | All files (*.*)|*.*";
            file.FilterIndex = 3;
            file.RestoreDirectory = true;
            if(file.ShowDialog() == DialogResult.OK)
            {
                files = file.SafeFileNames;
                paths = file.FileNames;
                path = Path.GetDirectoryName(file.FileName);

                for (int i = 0; i < files.Length; i++)
                {
                    listBoxSongs.Items.Add(files[i]);
                }

                
                foreach (string item in file.FileNames)
                {
                    media = WindowsMediaPlayer.newMedia(item);
                    playlist.appendItem(media);
                }

                WindowsMediaPlayer.currentPlaylist = playlist;
                WindowsMediaPlayer.Ctlcontrols.play();
            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSongs != null && listBoxSongs.Items != null)
            {
                WindowsMediaPlayer.URL = paths[listBoxSongs.SelectedIndex];
            }
        }

        private void listBoxSongs_DrawItem(object sender, DrawItemEventArgs e)
        {
            string selectedItem = listBoxSongs.Items[e.Index].ToString();

            // 2. Choose font 
            Font font = new Font("Roboto", 30);

            // 3. Choose colour
            SolidBrush solidBrush = new SolidBrush(Color.Red);

            // 4. Get bounds
            int left = e.Bounds.Left;
            int top = e.Bounds.Top;

            // 5. Use Draw the background within the bounds
            e.DrawBackground();

            // 6. Colorize listbox items
            e.Graphics.DrawString(selectedItem, font, solidBrush, left, top);
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
