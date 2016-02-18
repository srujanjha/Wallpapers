using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wallpapers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }        
        public string[] ImageInitialize(string mrkt)
        {
            string url = "", name = "";
            try
            {
                HttpWebRequest request = WebRequest.Create("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=" + mrkt) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string res = reader.ReadToEnd();
                        int i = res.IndexOf("\"url\":\"") + 7;
                        int r = res.IndexOf("\"", i);
                        int n = res.LastIndexOf("/", r);
                        url = "http://www.bing.com" + res.Substring(i, r - i);
                        name = res.Substring(n + 1, r - n - 1);
                        name= name.Substring(0, name.IndexOf("_"));
                    }
                }
            }
            catch (Exception e1)
            { }
            return new string[] {url,name};
        }
        public void downloadImage(string url,string name)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileAsync(new Uri(url), @Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +"//Bing Wallpapers//" + name + ".jpg");
                }
            }
            catch (Exception e1)
            { }
        }
        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    Grid grid = (Grid)sender as Grid;
                    Image img = (Image)grid.Children[0];
                    downloadImage(img.Source.ToString(), img.Tag.ToString());
                    Images.Children.Remove(grid);
                    if (Images.Children.Count == 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = "No new images available today!";
                        txt.Margin = new Thickness(20, 108, 0, 0);
                        txt.Height = 25;
                        txt.Background = System.Windows.Media.Brushes.White;
                        txt.Foreground = System.Windows.Media.Brushes.Black;
                        grid = new Grid();
                        grid.Width = 384;
                        grid.Margin = new Thickness(10);
                        grid.Children.Add(txt);
                        grid.MouseDown += NoImage_MouseDown;
                        ToolTip ToolTip1 = new ToolTip();
                        ToolTip1.Content = "Double-Click to check again.";
                        grid.ToolTip = ToolTip1;
                        Images.Children.Add(grid);
                    }
                }
            }
            catch (Exception e1)
            { }
        }
        private void NoImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Initialize();
        }
        private void Initialize()
        {
            if(!CheckForInternetConnection())
            {
                TextBlock txt = new TextBlock();
                txt.Text = "Connect to Internet...";
                txt.Margin = new Thickness(30, 100, 0, 0);
                txt.Height = 30;
                txt.Background = Brushes.White;
                txt.Foreground = Brushes.Black;
                Grid grid = new Grid();
                grid.Width = 384;
                grid.Margin = new Thickness(10);
                grid.Children.Add(txt);
                grid.MouseDown += NoImage_MouseDown;
                ToolTip ToolTip1 = new ToolTip();
                ToolTip1.Content = "Double-Click to check again.";
                grid.ToolTip = ToolTip1;
                Images.Children.Clear();
                Images.Children.Add(grid);
                return;
            }
            try
            {
                string[] markets = new string[] { "EN-IN", "EN-US", "ZH-CN", "JA-JP", "EN-AU", "EN-UK", "DE-DE", "EN-NZ" };
                if (!Directory.Exists(@Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "//Bing Wallpapers//"))
                    Directory.CreateDirectory(@Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "//Bing Wallpapers//");
                string[] files = Directory.GetFiles(@Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "//Bing Wallpapers//");
                Images.Children.Clear();
                List<string> listFiles = new List<string>();
                foreach (string mrkt in markets)
                {
                    string[] ar = ImageInitialize(mrkt);
                    bool flag = false;
                    foreach (string file in files) { if (Path.GetFileNameWithoutExtension(file).Equals(ar[1])) { flag = true; break; } }
                    if (listFiles.Contains(ar[1])) continue;
                    listFiles.Add(ar[1]);
                    if (flag) continue;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(ar[0], UriKind.Absolute);
                    bitmap.EndInit();
                    Image img = new Image();
                    img.Height = 216;
                    img.Width = 384;
                    img.Tag = ar[1];
                    img.Source = bitmap;
                    TextBlock txt = new TextBlock();
                    txt.Text = "  " + addSpaces(ar[1]);
                    txt.Margin = new Thickness(0, 180, 0, 0);
                    txt.Height = 30;
                    txt.Background = Brushes.Black;
                    txt.Foreground = Brushes.White;
                    txt.Opacity = 0.5;
                    Grid grid = new Grid();
                    grid.Width = 384;
                    grid.Margin = new Thickness(10);
                    grid.Children.Add(img);
                    grid.Children.Add(txt);
                    grid.Background = Brushes.Transparent;
                    ToolTip ToolTip1 = new ToolTip();
                    ToolTip1.Content = "Double-Click on the image to save it.";
                    grid.ToolTip = ToolTip1;
                    grid.MouseDown += Img_MouseDown;
                    grid.MouseEnter += Image_MouseEnter;
                    grid.MouseLeave += Image_MouseLeave;
                    Images.Children.Add(grid);
                }
                if (Images.Children.Count == 0)
                {
                    TextBlock txt = new TextBlock();
                    txt.Text = "No new images available today!";
                    txt.Margin = new Thickness(30, 100, 0, 0);
                    txt.Height = 30;
                    txt.Background = Brushes.White;
                    txt.Foreground = Brushes.Black;
                    Grid grid = new Grid();
                    grid.Background = Brushes.Transparent;
                    grid.Width = 384;
                    grid.Margin = new Thickness(10);
                    grid.Children.Add(txt);
                    grid.MouseDown += NoImage_MouseDown;
                    ToolTip ToolTip1 = new ToolTip();
                    ToolTip1.Content= "Double-Click to check again.";
                    grid.ToolTip = ToolTip1;
                    Images.Children.Add(grid);
                }
            }
            catch (Exception e1)
            { }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private string addSpaces(string name)
        {
            string N = "";
            foreach(char c in name)
            {
                if (c >= 65 && c <= 90) N += " " + c;
                else N += c;
            }
            return N;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Initialize();
        }
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender as Grid;
            Image img = new Image();
            img.Height = 120;
            img.Opacity = 0.5;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("download.png", UriKind.Relative);
            bitmap.EndInit();
            img.Source = bitmap;
            if(grid.Children.Count==3)grid.Children.RemoveAt(2);
            grid.Children.Add(img);
        }
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender as Grid;
            if (grid.Children.Count == 3) grid.Children.RemoveAt(2);
        }
    }
}
