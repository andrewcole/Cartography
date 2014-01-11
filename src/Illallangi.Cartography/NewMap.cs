using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace Illallangi.Cartography
{
    [Cmdlet("New", "Map")]
    public class NewMap : Cmdlet
    {
        #region Constants

        #endregion

        #region Fields

        private Bitmap currentBitmap;

        private Graphics currentGraphics;

        #endregion

        #region Properties

        #region Public Properties

        [Parameter(ValueFromPipeline = false, ValueFromPipelineByPropertyName = false)]
        public string Input
        {
            get;
            set;
        }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias(new[] { "Latitude" })]
        public double? LatitudeA { get; set; }
        
        [Alias(new[] { "DestinationLatitude" })]
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public double? LatitudeB { get; set; }

        public GeoLine Line
        {
            get
            {
                bool pointB;
                if (this.PointA == null)
                {
                    pointB = false;
                }
                else
                {
                    pointB = null != this.PointB;
                }

                bool flag = pointB;
                GeoLine geoLine = flag ? new GeoLine(this.PointA, this.PointB) : null;
                return geoLine;
            }
        }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Logo
        {
            get;
            set;
        }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("Longitude")]
        public double? LongitudeA { get; set; }
        
        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("DestinationLongitude")]
        public double? LongitudeB { get; set; }
        
        [Alias("PointName")]
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }
        
        [Parameter(ValueFromPipeline = false, ValueFromPipelineByPropertyName = false, Mandatory = true, Position = 1)]
        [ValidateNotNullOrEmpty]
        public string Output { get; set; }
        
        [Parameter(ValueFromPipeline = false, ValueFromPipelineByPropertyName = false)]
        [ValidateNotNullOrEmpty]
        public ImageFormat OutputFormat { get; set; }

        public GeoPoint PointA
        {
            get
            {
                GeoPoint geoPoint;
                bool flag;
                bool flag1;
                double valueOrDefault;
                double num;
                bool hasValue1;
                double? longitudeA = this.LongitudeA;
                if (longitudeA.HasValue)
                {
                    longitudeA = this.LatitudeA;
                    if (!longitudeA.HasValue)
                    {
                        flag1 = false;
                        flag = flag1;
                        if (flag)
                        {
                            longitudeA = this.LatitudeA;
                            if (longitudeA.HasValue)
                            {
                                valueOrDefault = (double)((double)longitudeA.GetValueOrDefault());
                            }
                            else
                            {
                                valueOrDefault = 0;
                            }
                            longitudeA = this.LongitudeA;
                            if (longitudeA.HasValue)
                            {
                                num = (double)((double)longitudeA.GetValueOrDefault());
                            }
                            else
                            {
                                num = 0;
                            }
                            geoPoint = GeoPoint.FromDegrees(valueOrDefault, num);
                        }
                        else
                        {
                            geoPoint = null;
                        }
                        return geoPoint;
                    }
                    longitudeA = this.LongitudeA;
                    bool hasValue;
                    hasValue = longitudeA.GetValueOrDefault() == 0 && longitudeA.HasValue;
                    longitudeA = this.LatitudeA;
                    hasValue1 = longitudeA.GetValueOrDefault() == 0 && longitudeA.HasValue;
                    flag1 = !(hasValue & hasValue1);
                    flag = flag1;
                    if (flag)
                    {
                        longitudeA = this.LatitudeA;
                        if (longitudeA.HasValue)
                        {
                            valueOrDefault = (double)longitudeA.GetValueOrDefault();
                        }
                        else
                        {
                            valueOrDefault = 0;
                        }

                        longitudeA = this.LongitudeA;
                        if (longitudeA.HasValue)
                        {
                            num = longitudeA.GetValueOrDefault();
                        }
                        else
                        {
                            num = 0;
                        }

                        geoPoint = GeoPoint.FromDegrees(valueOrDefault, num);
                    }
                    else
                    {
                        geoPoint = null;
                    }

                    return geoPoint;
                }
                return null;
            }
        }

        public GeoPoint PointB
        {
            get
            {
                GeoPoint geoPoint;
                bool flag;
                bool flag1;
                double valueOrDefault;
                double num;
                bool hasValue1;
                double? longitudeB = this.LongitudeB;
                if (longitudeB.HasValue)
                {
                    longitudeB = this.LatitudeB;
                    if (!longitudeB.HasValue)
                    {
                        flag1 = false;
                        flag = flag1;
                        if (flag)
                        {
                            longitudeB = this.LatitudeB;
                            if (longitudeB.HasValue)
                            {
                                valueOrDefault = (double)((double)longitudeB.GetValueOrDefault());
                            }
                            else
                            {
                                valueOrDefault = 0;
                            }
                            longitudeB = this.LongitudeB;
                            if (longitudeB.HasValue)
                            {
                                num = (double)((double)longitudeB.GetValueOrDefault());
                            }
                            else
                            {
                                num = 0;
                            }
                            geoPoint = GeoPoint.FromDegrees(valueOrDefault, num);
                        }
                        else
                        {
                            geoPoint = null;
                        }
                        return geoPoint;
                    }
                    longitudeB = this.LongitudeB;
                    bool hasValue;
                    if ((double)longitudeB.GetValueOrDefault() != 0)
                    {
                        hasValue = false;
                    }
                    else
                    {
                        hasValue = longitudeB.HasValue;
                    }
                    longitudeB = this.LatitudeB;
                    if ((double)longitudeB.GetValueOrDefault() != 0)
                    {
                        hasValue1 = false;
                    }
                    else
                    {
                        hasValue1 = longitudeB.HasValue;
                    }
                    flag1 = !(hasValue & hasValue1);
                    flag = flag1;
                    if (flag)
                    {
                        longitudeB = this.LatitudeB;
                        if (longitudeB.HasValue)
                        {
                            valueOrDefault = (double)((double)longitudeB.GetValueOrDefault());
                        }
                        else
                        {
                            valueOrDefault = 0;
                        }
                        longitudeB = this.LongitudeB;
                        if (longitudeB.HasValue)
                        {
                            num = (double)((double)longitudeB.GetValueOrDefault());
                        }
                        else
                        {
                            num = 0;
                        }
                        geoPoint = GeoPoint.FromDegrees(valueOrDefault, num);
                    }
                    else
                    {
                        geoPoint = null;
                    }
                    return geoPoint;
                }
                flag1 = false;
                flag = flag1;
                if (flag)
                {
                    longitudeB = this.LatitudeB;
                    if (longitudeB.HasValue)
                    {
                        valueOrDefault = (double)((double)longitudeB.GetValueOrDefault());
                    }
                    else
                    {
                        valueOrDefault = 0;
                    }
                    longitudeB = this.LongitudeB;
                    if (longitudeB.HasValue)
                    {
                        num = (double)((double)longitudeB.GetValueOrDefault());
                    }
                    else
                    {
                        num = 0;
                    }
                    geoPoint = GeoPoint.FromDegrees(valueOrDefault, num);
                }
                else
                {
                    geoPoint = null;
                }
                return geoPoint;
            }
        }
        
        #endregion

        #region Private Properties

        private Graphics Graphics
        {
            get
            {
                Graphics graphic = this.currentGraphics;
                Graphics graphic1 = graphic;
                if (graphic == null)
                {
                    Graphics graphic2 = Graphics.FromImage(this.Bitmap);
                    Graphics graphic3 = graphic2;
                    this.currentGraphics = graphic2;
                    graphic1 = graphic3;
                }

                Graphics graphic4 = graphic1;
                return graphic4;
            }
        }

        private Bitmap Bitmap
        {
            get
            {
                Bitmap bitmap = this.currentBitmap;
                return bitmap;
            }

            set
            {
                this.currentGraphics = null;
                this.currentBitmap = value;
            }
        }

        #endregion

        #endregion

        #region Constructor

        public NewMap()
        {
            this.Input = "DefaultInput.jpg";
            this.OutputFormat = ImageFormat.Jpeg;
        }

        #endregion

        #region Methods

        protected override void BeginProcessing()
        {
            try
            {
                this.Debug("Loading Bitmap");
                this.Bitmap = NewMap.GetBitmap(this.Input);
                this.Debug("Done");
            }
            catch (Exception exception)
            {
                Exception e = exception;
                this.Debug(e.ToString());
                throw e;
            }
        }

        protected virtual void Debug(string message)
        {
            base.WriteDebug(message);
        }

        private void DisposeOfGraphics()
        {
            bool flag = null == this.currentGraphics;
            if (!flag)
            {
                this.currentGraphics.Dispose();
                this.currentGraphics = null;
            }
        }

        protected override void EndProcessing()
        {
            this.DisposeOfGraphics();
            this.Debug(string.Format("Writing out {1} {0}", Path.GetFullPath(this.Output), this.OutputFormat.ToString()));
            this.Bitmap.Save(Path.GetFullPath(this.Output), this.OutputFormat);
        }

        private static Bitmap GetBitmap(string file)
        {
            Bitmap bitmap;
            Stream stream = NewMap.GetStream(file);
            try
            {
                bitmap = new Bitmap(stream);
            }
            finally
            {
                bool flag = stream == null;
                if (!flag)
                {
                    stream.Dispose();
                }
            }
            return bitmap;
        }

        private static Stream GetStream(string file)
        {
            Stream manifestResourceStream;
            bool length = !File.Exists(file);
            if (length)
            {
                Assembly ass = Assembly.GetAssembly(typeof(NewMap));
                string[] manifestResourceNames = ass.GetManifestResourceNames();
                int num = 0;
                while (true)
                {
                    length = num < (int)manifestResourceNames.Length;
                    if (!length)
                    {
                        break;
                    }
                    string resourceName = manifestResourceNames[num];
                    length = !resourceName.EndsWith(file);
                    if (!length)
                    {
                        manifestResourceStream = ass.GetManifestResourceStream(resourceName);
                        return manifestResourceStream;
                    }
                    num++;
                }
                throw new FileNotFoundException(string.Format("File not found: {0}", file), file);
            }
            else
            {
                manifestResourceStream = new FileStream(file, FileMode.Open);
            }
            return manifestResourceStream;
        }

        protected override void ProcessRecord()
        {
            object[] name;
            int valueOrDefault;
            int num;
            string str;
            try
            {
                bool line = null == this.Line;
                if (line)
                {
                    line = null == this.PointA;
                    if (!line)
                    {
                        int height = 64;
                        int width = 64;
                        if (this.Logo == null || string.IsNullOrEmpty(this.Logo.Trim()))
                        {
                            str = "Airplane.png";
                        }
                        else
                        {
                            str = this.Logo;
                        }
                        string logo = str;
                        Point point = this.PointA.ToPoint(this.Bitmap);
                        name = new object[6];
                        name[0] = this.Name;
                        name[1] = this.LatitudeA;
                        name[2] = this.LongitudeA;
                        name[3] = point.X;
                        name[4] = point.Y;
                        name[5] = logo;
                        this.Debug(string.Format("Drawing point {0} at {1},{2} ({3},{4}) with logo {5}", name));
                        this.Graphics.DrawImage(NewMap.GetBitmap(logo), point.X - width / 2, point.Y - height / 2, width, height);
                    }
                }
                else
                {
                    IEnumerator<KeyValuePair<Point, Point>> enumerator = this.Line.GreatCircle(100).GetLine(this.Bitmap).GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            line = enumerator.MoveNext();
                            if (!line)
                            {
                                break;
                            }
                            KeyValuePair<Point, Point> p = enumerator.Current;
                            Point pointA = p.Key;
                            Point pointB = p.Value;
                            name = new object[4];
                            name[0] = pointA.X;
                            name[1] = pointA.Y;
                            name[2] = pointB.X;
                            name[3] = pointB.Y;
                            this.Debug(string.Format("Drawing line from {0},{1} to {2},{3}", name));
                            this.Graphics.DrawLine(new Pen(Brushes.HotPink, 4f), pointA, pointB);
                        }
                    }
                    finally
                    {
                        line = enumerator == null;
                        if (!line)
                        {
                            enumerator.Dispose();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Exception e = exception;
                this.Debug(e.ToString());
                throw e;
            }
        }

        protected override void StopProcessing()
        {
            this.Debug("Aborting");
            this.Bitmap.Dispose();
        }

        #endregion
    }
}