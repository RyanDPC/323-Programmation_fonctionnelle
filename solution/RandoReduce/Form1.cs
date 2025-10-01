using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RandoReduce
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private List<GpxPoint> points;
        private Bitmap originalMap;

        // Labels pour stats
        private Label lblDistance, lblDPlus, lblDMoins;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Carte de randonnée";
            this.Size = new Size(500, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // PictureBox
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);

            // Labels
            lblDistance = new Label { ForeColor = Color.White, BackColor = Color.Black, AutoSize = true, Top = 10, Left = 10 };
            lblDPlus = new Label { ForeColor = Color.White, BackColor = Color.Black, AutoSize = true, Top = 30, Left = 10 };
            lblDMoins = new Label { ForeColor = Color.White, BackColor = Color.Black, AutoSize = true, Top = 50, Left = 10 };
            this.Controls.Add(lblDistance);
            this.Controls.Add(lblDPlus);
            this.Controls.Add(lblDMoins);
            lblDistance.BringToFront();
            lblDPlus.BringToFront();
            lblDMoins.BringToFront();

            // Charge map
            string mapPath = @"C:\Users\pa70iyc\Documents\GitHub\323-Programmation_fonctionnelle\exos\rando\map.png";
            if (!File.Exists(mapPath))
            {
                MessageBox.Show("Fichier map.png introuvable !");
                return;
            }
            originalMap = new Bitmap(mapPath);
            pictureBox.Image = new Bitmap(originalMap);

            // Charge GPX
            string gpxPath = @"C:\Users\pa70iyc\Documents\GitHub\323-Programmation_fonctionnelle\exos\rando\gpx\gemmikandersteg.gpx";
            points = LoadGpx(gpxPath);

            // Trace et met à jour stats
            DrawPath(points);
            UpdateStats(points);
        }

        private List<GpxPoint> LoadGpx(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = "http://www.topografix.com/GPX/1/1";

            return doc.Descendants(ns + "trkpt")
                      .Select(p => new GpxPoint
                      {
                          Latitude = double.Parse(p.Attribute("lat").Value),
                          Longitude = double.Parse(p.Attribute("lon").Value),
                          Elevation = double.Parse(p.Element(ns + "ele").Value)
                      })
                      .ToList();
        }

        private void DrawPath(List<GpxPoint> pts)
        {
            if (pts == null || pts.Count < 2) return;

            Bitmap bmp = new Bitmap(originalMap.Width, originalMap.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // --- Rotation si nécessaire ---
                g.TranslateTransform(bmp.Width / 2f, bmp.Height / 2f);
                g.RotateTransform(0); // modifie si rotation voulue
                g.TranslateTransform(-bmp.Width / 2f, -bmp.Height / 2f);

                // --- Dessine la carte ---
                g.DrawImage(originalMap, new Rectangle(0, 0, bmp.Width, bmp.Height));

                Pen pen = new Pen(Color.Red, 2);

                double minLat = pts.Min(p => p.Latitude);
                double maxLat = pts.Max(p => p.Latitude);
                double minLon = pts.Min(p => p.Longitude);
                double maxLon = pts.Max(p => p.Longitude);

                int padding = 60; // padding autour du tracé
                double scaleX = (bmp.Width - 2 * padding) / (maxLon - minLon);
                double scaleY = (bmp.Height - 2 * padding) / (maxLat - minLat);
                double scale = Math.Min(scaleX, scaleY);

                double compressX = 0.8; // compressé horizontalement si besoin
                double offsetX = padding + ((bmp.Width - 2 * padding) - (maxLon - minLon) * scale * compressX) / 2;
                double offsetY = padding + ((bmp.Height - 2 * padding) - (maxLat - minLat) * scale) / 2;

                Point? prev = null;
                foreach (var p in pts)
                {
                    int x = (int)(offsetX + (p.Longitude - minLon) * scale * compressX);
                    int y = (int)(offsetY + (maxLat - p.Latitude) * scale);
                    Point current = new Point(x, y);

                    if (prev.HasValue)
                        g.DrawLine(pen, prev.Value, current);

                    prev = current;
                }
            }

            pictureBox.Image = bmp;
        }

        private void UpdateStats(List<GpxPoint> pts)
        {
            if (pts == null || pts.Count < 2) return;

            // segments consécutifs
            var segments = pts.Zip(pts.Skip(1), (a, b) => new
            {
                Distance = a.GetDistanceFrom(b),
                DPlus = Math.Max(0, b.Elevation - a.Elevation),
                DMoins = Math.Max(0, a.Elevation - b.Elevation)
            });

            // aggregate LINQ
            var totals = segments.Aggregate(
                new { Distance = 0.0, DPlus = 0.0, DMoins = 0.0 },
                (acc, s) => new
                {
                    Distance = acc.Distance + s.Distance,
                    DPlus = acc.DPlus + s.DPlus,
                    DMoins = acc.DMoins + s.DMoins
                });

            lblDistance.Text = $"Distance totale : {totals.Distance / 1000:F2} km";
            lblDPlus.Text = $"Dénivelé + : {totals.DPlus:F0} m";
            lblDMoins.Text = $"Dénivelé - : {totals.DMoins:F0} m";
        }
    }

    public class GpxPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }

        public double GetDistanceFrom(GpxPoint other)
        {
            double R = 6371000;
            double dLat = (other.Latitude - Latitude) * Math.PI / 180;
            double dLon = (other.Longitude - Longitude) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(Latitude * Math.PI / 180) * Math.Cos(other.Latitude * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
