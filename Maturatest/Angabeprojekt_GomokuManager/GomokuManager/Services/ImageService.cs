using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMagick;

namespace GomokuManager.Services;

public static class ImageService
{
    // ============================================================
    // TODO 9 — Magick.NET
    // Verkleinere das Bild auf 80x80 und speichere es als
    // "profile\profile.png" (Ordner vorher anlegen!).
    //
    //   using var img = new MagickImage(sourcePath);
    //   img.Resize(80, 80);
    //   img.Write(destPath);
    // ============================================================
    public static void ResizeAndSave(string sourcePath, string destPath)
    {
        // TODO
        throw new NotImplementedException();
    }

    // ============================================================
    // TODO 10a — Bitmap Decode
    // Lade eine Bild-Datei als BitmapSource fuer WPF (Image.Source).
    //
    //   var decoder = BitmapDecoder.Create(
    //       new Uri(path, UriKind.Absolute),
    //       BitmapCreateOptions.None,
    //       BitmapCacheOption.OnLoad);
    //   return decoder.Frames[0];
    //
    // Alternativ (kurz):
    //   return new BitmapImage(new Uri(path, UriKind.Absolute));
    // ============================================================
    public static BitmapSource Load(string path)
    {
        // TODO
        throw new NotImplementedException();
    }

    // ============================================================
    // TODO 10b — Bitmap Encode
    // Speichere eine BitmapSource als PNG-Datei.
    //
    //   var enc = new PngBitmapEncoder();
    //   enc.Frames.Add(BitmapFrame.Create(source));
    //   using var fs = File.OpenWrite(path);
    //   enc.Save(fs);
    // ============================================================
    public static void Save(BitmapSource source, string path)
    {
        // TODO
        throw new NotImplementedException();
    }
}
