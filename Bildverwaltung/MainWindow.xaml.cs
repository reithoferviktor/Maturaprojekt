using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bildverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Bild> bllist = new ObservableCollection<Bild>();
        public static readonly RoutedUICommand MoveFile =
        new RoutedUICommand("Datei verschieben", "MoveFile", typeof(MainWindow), 
            new InputGestureCollection() {
            new KeyGesture(Key.M, ModifierKeys.Control)});
        public static readonly RoutedUICommand RotateFile =
        new RoutedUICommand("Datei Rotieren", "RotateFile", typeof(MainWindow),        new InputGestureCollection()
        {
            new KeyGesture(Key.R, ModifierKeys.Control)});
        public MainWindow()
        {
            InitializeComponent();
            String[] dirs = Directory.GetDirectories(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images"));
            foreach (string dir in dirs)
            {
                cb.Items.Add(System.IO.Path.GetFileName(dir));
            }
            lb.ItemsSource = bllist;
            lb.SelectionMode = SelectionMode.Extended;
        }

        private void NewAlbumCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewAlbumCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String x = "";
            NewAlbumDial nd = new NewAlbumDial();
            nd.Show();
            nd.oK.Click += (sender, e) =>
            {
                x = nd.tb.Text.ToString();
                if (Directory.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images/" + x)))
                {
                    MessageBox.Show("Album existiert bereits");
                }
                else
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images/" + x));
                    cb.Items.Add(x);
                    nd.Close();
                }
            };


        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bllist.Clear();
            var paths = System.IO.Directory.GetFiles(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images/" + cb.SelectedItem.ToString()));
            foreach (String path in paths)
            {
                bool add = true;
                Bild b = new Bild();
                b.Imgpth = path;
                b.img = new BitmapImage();
                b.img.BeginInit();
                b.img.CacheOption = BitmapCacheOption.OnLoad;
                b.img.UriSource = new Uri(path);
                b.img.EndInit();
                b.Flnm = System.IO.Path.GetFileNameWithoutExtension(path);
                b.FFn = System.IO.Path.GetFileName(path);

                foreach (var item in bllist)
                {
                    if (item.Imgpth == b.Imgpth)
                    {
                        add = false;
                    }

                }
                if (add)
                {
                    bllist.Add(b);
                }
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "(*.zip)|*.zip";
            opf.Multiselect = true;
            opf.ShowDialog();
            String[] zps = opf.FileNames;
            foreach (String z in zps)
            {
                ZipFile.ExtractToDirectory(z, System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images/" + cb.SelectedItem.ToString()), true);
            }
            var paths = System.IO.Directory.GetFiles(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images/" + cb.SelectedItem.ToString()));
            foreach (String path in paths)
            {
                bool add = true;
                Bild b = new Bild();
                b.Imgpth = path;
                b.img = new BitmapImage();
                b.img.BeginInit();
                b.img.CacheOption = BitmapCacheOption.OnLoad;
                b.img.UriSource = new Uri(path);
                b.img.EndInit();
                b.Flnm = System.IO.Path.GetFileNameWithoutExtension(path);
                b.FFn = System.IO.Path.GetFileName(path);
                foreach (var item in bllist)
                {
                    if (item.Imgpth == b.Imgpth)
                    {
                        add = false;
                    }

                }
                if (add)
                {
                    bllist.Add(b);
                }
            }

        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lb.SelectedItems.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<Bild> bl = lb.SelectedItems.Cast<Bild>().ToList();
            foreach (var item in bl)
            {
                bllist.Remove(item);
                File.Delete(item.ToString());
            }


        }
        private void Move_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lb.SelectedItems.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void Move_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String dest = "";
            movedial mv = new movedial();
            mv.mvbtn.Click += (sender, e) =>
            {
                if (dest != "")
                {
                    List<Bild> bl = lb.SelectedItems.Cast<Bild>().ToList();
                    foreach (var item in bl)
                    {
                        File.Move(item.ToString(), System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images/" + dest + "/" + item.FFn));
                        bllist.Remove(item);

                    }
                }
                mv.Close();
            };
            foreach (var item in cb.Items)
            {
                mv.cb1.Items.Add(item);
            }
            mv.cb1.SelectionChanged += (sender, e) =>
            {
                dest = mv.cb1.SelectedItem.ToString();
            };
            mv.Show();


        }

        private void Rotate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lb.SelectedItems.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void Rotate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<Bild> temp = lb.SelectedItems.Cast<Bild>().ToList();
            foreach (var im in temp)
            {
                foreach (Bild tem in bllist)
                {
                    if (tem == im)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(tem.Imgpth);

                        bitmap.EndInit();
                        TransformedBitmap rotated = new TransformedBitmap(bitmap, new RotateTransform(90));
                        //tem.bildimage = bitmap;

                        BitmapSource bitmapSource = rotated;
                        string pathpath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", cb.SelectedItem.ToString(), System.IO.Path.GetFileName(tem.Imgpth));



                        string ex = System.IO.Path.GetExtension(tem.Imgpth);
                        if (ex == ".png")
                        {
                            using (
                            FileStream stream = new FileStream(pathpath, FileMode.Create))
                            {
                                PngBitmapEncoder encoder = new PngBitmapEncoder();

                                encoder.Interlace = PngInterlaceOption.On;
                                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                                encoder.Save(stream);
                            }
                        }
                        else if (ex == ".jpeg")
                        {
                            using (
                            FileStream stream = new FileStream(pathpath, FileMode.Create))
                            {
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.QualityLevel = 30;
                                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                                encoder.Save(stream);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Format " + ex + " wird nicht unterstÃ¼tzt");
                        }


                    }
                }
            }
            string[] bilder = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", cb.SelectedItem.ToString()));
            bllist.Clear();
            foreach (string s in bilder)
            {
                BitmapImage bitmap2 = new BitmapImage();
                bitmap2.BeginInit();
                bitmap2.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap2.UriSource = new Uri(s);
                bitmap2.EndInit();
                //bitmap.Freeze();

                Bild template = new Bild();
                template.img = bitmap2;
                template.FFn = System.IO.Path.GetFileNameWithoutExtension(s);
                template.Imgpth = s;
                bllist.Add(template);
            }
        }


        public class Bild : INotifyPropertyChanged
        {
            private BitmapImage _img;
            private string _imgpth;
            private string _flnm;
            private string _ffn;

            public BitmapImage img
            {
                get => _img;
                set
                {
                    if (_img != value)
                    {
                        _img = value;
                        OnPropertyChanged();
                    }
                }
            }

            public string Imgpth
            {
                get => _imgpth;
                set
                {
                    if (_imgpth != value)
                    {
                        _imgpth = value;
                        OnPropertyChanged();
                    }
                }
            }

            public string Flnm
            {
                get => _flnm;
                set
                {
                    if (_flnm != value)
                    {
                        _flnm = value;
                        OnPropertyChanged();
                    }
                }
            }

            public string FFn
            {
                get => _ffn;
                set
                {
                    if (_ffn != value)
                    {
                        _ffn = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public override string ToString()
            {
                return Imgpth;
            }
        }
    }
}
    
