using System;
using System.Collections.Generic;
using System.Drawing;

namespace Illallangi.Cartography
{
    public class GeoLine
    {
        public GeoPoint A
        {
            get;
            set;
        }

        public GeoPoint B
        {
            get;
            set;
        }

        public double GeoDistanceAsRadians
        {
            get
            {
                return 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((this.lat1 - this.lat2) / 2), 2) + Math.Cos(this.lat1) * Math.Cos(this.lat2) * Math.Pow(Math.Sin((this.lon1 - this.lon2) / 2), 2)));
            }
        }

        private double lat1
        {
            get
            {
                double latitudeRadians = this.A.LatitudeRadians;
                return latitudeRadians;
            }
        }

        private double lat2
        {
            get
            {
                double latitudeRadians = this.B.LatitudeRadians;
                return latitudeRadians;
            }
        }

        private double lon1
        {
            get
            {
                double longitudeRadians = this.A.LongitudeRadians;
                return longitudeRadians;
            }
        }

        private double lon2
        {
            get
            {
                double longitudeRadians = this.B.LongitudeRadians;
                return longitudeRadians;
            }
        }

        public GeoLine(GeoPoint a, GeoPoint b)
        {
            this.A = a;
            this.B = b;
        }

        public IEnumerable<KeyValuePair<Point, Point>> GetLine(Bitmap bitmap)
        {
            bool x;
            if (this.A.Longitude >= -90 || this.B.Longitude <= 90)
            {
                if (this.A.Longitude <= 90)
                {
                    x = true;
                }
                else
                {
                    x = this.B.Longitude >= -90;
                }
            }
            else
            {
                x = false;
            }

            if (x)
            {
                yield return new KeyValuePair<Point, Point>(this.A.ToPoint(bitmap), this.B.ToPoint(bitmap));
            }
            else
            {
                Point point = this.A.ToPoint(bitmap);
                Point point1 = this.B.ToPoint(bitmap);
                x = point.X <= point1.X;
                if (!x)
                {
                    Point point2 = point;
                    point = point1;
                    point1 = point2;
                }
                double num = (double)point.X / ((double)point.X + (double)bitmap.Width - (double)point1.X);
                double y = (double)point.Y - (double)(point.Y - point1.Y) * num;
                yield return new KeyValuePair<Point, Point>(point, new Point(0, (int)y));
                yield return new KeyValuePair<Point, Point>(new Point(bitmap.Width, (int)y), point1);
            }
        }

        public Rectangle GetRectangle(Bitmap bitmap, int padding)
        {
            Point a1 = this.A.ToPoint(bitmap);
            Point b1 = this.B.ToPoint(bitmap);
            int x = Math.Min(a1.X, b1.X);
            int y = Math.Min(a1.Y, b1.Y);
            int width = Math.Max(a1.X, b1.X) - x;
            int height = Math.Max(a1.Y, b1.Y) - y;
            Rectangle rectangle = new Rectangle(x - padding, y - padding, width + padding + padding, height + padding + padding);
            return rectangle;
        }

        public IEnumerable<GeoLine> GreatCircle(int steps)
        {
            GeoPoint geoPoint = null;
            
            var increment = 1 / (double)(steps > 36 ? steps : 36);
            double f = 0;

            while (true)
            {
                if (f <= 1)
                {
                    break;
                }

                double a = Math.Sin((1 - f) * this.GeoDistanceAsRadians) / Math.Sin(this.GeoDistanceAsRadians);
                double b = Math.Sin(f * this.GeoDistanceAsRadians) / Math.Sin(this.GeoDistanceAsRadians);

                double x = a * Math.Cos(this.lat1) * Math.Cos(this.lon1) + b * Math.Cos(this.lat2) * Math.Cos(this.lon2);
                double y = a * Math.Cos(this.lat1) * Math.Sin(this.lon1) + b * Math.Cos(this.lat2) * Math.Sin(this.lon2);
                double z = a * Math.Sin(this.lat1) + b * Math.Sin(this.lat2);

                double lat = Math.Atan2(z, Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
                double lon = Math.Atan2(y, x);

                GeoPoint geoPoint1 = GeoPoint.FromRadians(lat, lon);
                if (null != geoPoint)
                {
                    yield return new GeoLine(geoPoint, geoPoint1);
                }
                geoPoint = geoPoint1;
                f = f + increment;
            }
        }
    }
}