using System;
using System.Drawing;

namespace Illallangi.Cartography.PowerShell
{
    public class GeoPoint
    {
        #region Properties

        /// <summary>
        /// Gets or sets the latitude of the point, in degrees.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the point, in radians.
        /// </summary>
        public double LatitudeRadians
        {
            get
            {
                double radians = GeoPoint.degreesToRadians(this.Latitude);
                return radians;
            }
            set
            {
                this.Latitude = GeoPoint.radiansToDegrees(value);
            }
        }

        /// <summary>
        /// Gets or sets the longitude of the point, in degrees.
        /// </summary>
        public double Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the longitude of the point, in radians.
        /// </summary>
        public double LongitudeRadians
        {
            get
            {
                double radians = GeoPoint.degreesToRadians(this.Longitude);
                return radians;
            }
            set
            {
                this.Longitude = GeoPoint.radiansToDegrees(value);
            }
        }

        public static double MaxLatitude
        {
            get
            {
                double num = 90;
                return num;
            }
        }

        public static double MaxLongitude
        {
            get
            {
                double num = 180;
                return num;
            }
        }

        public static double MidLatitude
        {
            get
            {
                double num = 0;
                return num;
            }
        }

        public static double MidLongitude
        {
            get
            {
                double num = 0;
                return num;
            }
        }

        public static double MinLatitude
        {
            get
            {
                double num = -90;
                return num;
            }
        }

        public static double MinLongitude
        {
            get
            {
                double num = -180;
                return num;
            }
        }

        private GeoPoint()
        {
        }

        private static double degreesToRadians(double degrees)
        {
            double num = degrees * 0.0174532925199433;
            return num;
        }

        public static GeoPoint FromDegrees(double latitude, double longitude)
        {
            GeoPoint geoPoint = new GeoPoint();
            geoPoint.Latitude = latitude;
            geoPoint.Longitude = longitude;
            GeoPoint geoPoint1 = geoPoint;
            return geoPoint1;
        }

        public static GeoPoint FromRadians(double latitude, double longitude)
        {
            GeoPoint geoPoint = new GeoPoint();
            geoPoint.Latitude = GeoPoint.radiansToDegrees(latitude);
            geoPoint.Longitude = GeoPoint.radiansToDegrees(longitude);
            GeoPoint geoPoint1 = geoPoint;
            return geoPoint1;
        }

        private static double radiansToDegrees(double radians)
        {
            double num = radians * 57.2957795130823;
            return num;
        }

        public Point ToPoint(int width, int height)
        {
            int x = (int)((this.Longitude + 180) / 360 * (double)width);
            int y = (int)((90 - this.Latitude) / 180 * (double)height);
            Point point = new Point(x, y);
            return point;
        }

        public Point ToPoint(Bitmap bitmap)
        {
            Point point = this.ToPoint(bitmap.Width, bitmap.Height);
            return point;
        }
    }
}