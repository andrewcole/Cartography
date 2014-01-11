using System;
using System.Collections.Generic;
using System.Drawing;

namespace Illallangi.Cartography
{
    public class GeoLine
    {
        private GeoPoint A { get; set; }
    
        private GeoPoint B { get; set; }
        
        public double GeoDistanceAsRadians
        {
            get
            {
                return 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((this.Latitude1 - this.Latitude2) / 2), 2) + Math.Cos(this.Latitude1) * Math.Cos(this.Latitude2) * Math.Pow(Math.Sin((this.Longitude1 - this.Longitude2) / 2), 2)));
            }
        }

        private double Latitude1
        {
            get
            {
                return this.A.LatitudeRadians;
            }
        }

        private double Latitude2
        {
            get
            {
                return this.B.LatitudeRadians;
            }
        }

        private double Longitude1
        {
            get
            {
                return this.A.LongitudeRadians;
            }
        }

        private double Longitude2
        {
            get
            {
                return this.B.LongitudeRadians;
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

                double x = a * Math.Cos(this.Latitude1) * Math.Cos(this.Longitude1) + b * Math.Cos(this.Latitude2) * Math.Cos(this.Longitude2);
                double y = a * Math.Cos(this.Latitude1) * Math.Sin(this.Longitude1) + b * Math.Cos(this.Latitude2) * Math.Sin(this.Longitude2);
                double z = a * Math.Sin(this.Latitude1) + b * Math.Sin(this.Latitude2);

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