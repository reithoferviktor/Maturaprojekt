using System.Windows;

namespace OsterHasenHilfe
{
    /// <summary>
    /// Rechnet Geo-Koordinaten (Longitude/Latitude) in Pixel-Positionen um und zurück.
    ///
    /// Funktionsweise:
    /// - Man gibt den sichtbaren Kartenausschnitt als Geo-Grenzen an (links, rechts, oben, unten)
    /// - Man gibt die Bildgröße in Pixeln an (Breite, Höhe)
    /// - Die Klasse rechnet dann beliebige Geo-Koordinaten in Pixel um (und umgekehrt)
    ///
    /// Beispiel:
    ///   var mapper = new GeoMapper(
    ///       lonLeft: 16.209, lonRight: 16.281,   // Longitude-Bereich der Karte
    ///       latBottom: 47.786, latTop: 47.846,    // Latitude-Bereich der Karte
    ///       pixelWidth: 800, pixelHeight: 600     // Bildgröße in Pixeln
    ///   );
    ///   Point pixel = mapper.GeoToPixel(16.25, 47.81);  // Geo → Pixel
    ///   (double lon, double lat) = mapper.PixelToGeo(400, 300);  // Pixel → Geo
    /// </summary>
    public class GeoMapper
    {
        // Die Geo-Grenzen des Kartenausschnitts
        private readonly double lonLeft;
        private readonly double lonRight;
        private readonly double latBottom;
        private readonly double latTop;

        // Die Bildgröße in Pixeln
        private readonly double pixelWidth;
        private readonly double pixelHeight;

        /// <summary>
        /// Erstellt einen neuen GeoMapper.
        /// </summary>
        /// <param name="lonLeft">Longitude am linken Kartenrand</param>
        /// <param name="lonRight">Longitude am rechten Kartenrand</param>
        /// <param name="latBottom">Latitude am unteren Kartenrand</param>
        /// <param name="latTop">Latitude am oberen Kartenrand</param>
        /// <param name="pixelWidth">Breite des Bildes/Canvas in Pixeln</param>
        /// <param name="pixelHeight">Höhe des Bildes/Canvas in Pixeln</param>
        public GeoMapper(double lonLeft, double lonRight, double latBottom, double latTop,
                         double pixelWidth, double pixelHeight)
        {
            this.lonLeft = lonLeft;
            this.lonRight = lonRight;
            this.latBottom = latBottom;
            this.latTop = latTop;
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;
        }

        /// <summary>
        /// Rechnet Geo-Koordinaten (Longitude, Latitude) in Pixel-Position (X, Y) um.
        ///
        /// Rechnung:
        ///   X-Achse (Longitude → Pixel):
        ///     - Wie weit ist der Punkt prozentual vom linken Rand entfernt?
        ///     - Formel: (lon - lonLinks) / (lonRechts - lonLinks) = Prozent von links
        ///     - Das mal die Bildbreite = X in Pixeln
        ///
        ///   Y-Achse (Latitude → Pixel):
        ///     - Wie weit ist der Punkt prozentual vom unteren Rand entfernt?
        ///     - ACHTUNG: Pixel-Y geht von oben nach unten, Latitude von unten nach oben!
        ///     - Darum: 1 minus den Prozentwert, damit oben = kleine Y-Werte
        ///     - Formel: (1 - (lat - latUnten) / (latOben - latUnten)) * Bildhöhe = Y in Pixeln
        /// </summary>
        public Point GeoToPixel(double lon, double lat)
        {
            // Prozentualer Anteil von links (0.0 = linker Rand, 1.0 = rechter Rand)
            double xPercent = (lon - lonLeft) / (lonRight - lonLeft);
            double x = xPercent * pixelWidth;

            // Prozentualer Anteil von unten (0.0 = unten, 1.0 = oben)
            double yPercent = (lat - latBottom) / (latTop - latBottom);
            // Invertieren weil Pixel-Y von oben nach unten geht
            double y = (1.0 - yPercent) * pixelHeight;

            return new Point(x, y);
        }

        /// <summary>
        /// Rechnet Pixel-Position (X, Y) zurück in Geo-Koordinaten (Longitude, Latitude).
        /// Das ist die Umkehrfunktion von GeoToPixel - z.B. wenn man auf die Karte klickt.
        /// </summary>
        public (double Longitude, double Latitude) PixelToGeo(double pixelX, double pixelY)
        {
            // Umkehrung der GeoToPixel-Formeln
            double lon = lonLeft + (pixelX / pixelWidth) * (lonRight - lonLeft);
            double lat = latTop - (pixelY / pixelHeight) * (latTop - latBottom);

            return (lon, lat);
        }
    }
}
