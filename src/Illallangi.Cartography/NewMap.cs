using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace Illallangi.Cartography
{
    [Cmdlet(VerbsCommon.New, @"Map", DefaultParameterSetName = "Point")]
    public class NewMap : PSCmdlet
    {
        #region Constants

        private const string Point = @"Point";

        private const string Line = @"Line";

        #endregion

        #region Fields

        private Stream currentInputStream;

        private Bitmap currentInputBitmap;

        private Graphics currentInputGraphics;

        private Bitmap currentIconBitmap;

        private Stream currentIconStream;

        private ImageFormat currentOutputFormat = ImageFormat.Jpeg;

        private string currentInputPath = @"BlueMarble.jpg";

        private string currentIconPath = @"Airport.png";

        private Brush currentLineColor = Brushes.HotPink;

        private int currentLineWidth = 2;

        #endregion

        #region Properties

        #region Public Properties

        #region Common Parameters

        [Parameter]
        public string InputPath
        {
            get
            {
                return this.currentInputPath;
            }
            set
            {
                this.currentInputPath = value;
            }
        }

        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = false)]
        public ImageFormat OutputFormat
        {
            get
            {
                return this.currentOutputFormat;
            }
            set
            {
                this.currentOutputFormat = value;
            }
        }

        #endregion

        #region Point Parameters

        [ValidateRange(-90, 90)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Point)]
        public float Latitude { get; set; }

        [ValidateRange(-180, 180)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Point)]
        public float Longitude { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Point)]
        public string IconPath
        {
            get
            {
                this.currentIconBitmap = null;
                this.currentIconStream = null;
                return this.currentIconPath;
            }

            set
            {
                this.currentIconPath = value;
            }
        }

        #endregion

        #region Line Parameters

        [ValidateRange(-90, 90)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public float OriginLatitude { get; set; }

        [ValidateRange(-180, 180)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public float OriginLongitude { get; set; }

        [ValidateRange(-90, 90)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public float DestinationLatitude { get; set; }

        [ValidateRange(-180, 180)]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public float DestinationLongitude { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public Brush LineColor
        {
            get
            {
                return this.currentLineColor;
            }
            set
            {
                this.currentLineColor = value;
            }
        }

        [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, ParameterSetName = NewMap.Line)]
        public int LineWidth
        {
            get
            {
                return this.currentLineWidth;
            }
            set
            {
                this.currentLineWidth = value;
            }
        }

        #endregion

        #endregion

        #region Private Properties

        private Stream InputStream
        {
            get
            {
                return this.currentInputStream ?? (this.currentInputStream = NewMap.GetStream(this.InputPath));
            }
        }

        private Bitmap InputBitmap
        {
            get
            {
                return this.currentInputBitmap ?? (this.currentInputBitmap = NewMap.GetBitmap(this.InputStream));
            }
        }

        private Graphics InputGraphics
        {
            get
            {
                return this.currentInputGraphics ?? (this.currentInputGraphics = NewMap.GetGraphics(this.InputBitmap));
            }
        }

        private int InputHeight
        {
            get
            {
                return this.InputBitmap.Height;
            }
        }

        private int InputWidth
        {
            get
            {
                return this.InputBitmap.Width;
            }
        }

        private Stream IconStream
        {
            get
            {
                return this.currentIconStream ?? (this.currentIconStream = NewMap.GetStream(this.IconPath));
            }
        }

        private Bitmap IconBitmap
        {
            get
            {
                return this.currentIconBitmap ?? (this.currentIconBitmap = NewMap.GetBitmap(this.IconStream));
            }
        }

        private int IconHeight
        {
            get
            {
                if (NewMap.Point != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get IconHeight when ParameterSet is not Point");
                }

                return this.IconBitmap.Height;
            }
        }

        private int IconWidth
        {
            get
            {
                if (NewMap.Point != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get IconWidth when ParameterSet is not Point");
                }

                return this.IconBitmap.Width;
            }
        }

        private float X
        {
            get
            {
                if (NewMap.Point != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get X when ParameterSet is not Point");
                }

                return ((this.Longitude + 180) / 360) * this.InputBitmap.Width;
            }
        }

        private float Y
        {
            get
            {
                if (NewMap.Point != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get Y when ParameterSet is not Point");
                }

                return ((180 - (this.Latitude + 90)) / 180) * this.InputBitmap.Height;
            }
        }

        private int OriginX
        {
            get
            {
                if (NewMap.Line != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get OriginX when ParameterSet is not Line");
                }

                return (int)(((this.OriginLongitude + 180) / 360) * this.InputBitmap.Width);
            }
        }

        private int OriginY
        {
            get
            {
                if (NewMap.Line != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get OriginY when ParameterSet is not Line");
                }

                return (int)(((180 - (this.OriginLatitude + 90)) / 180) * this.InputBitmap.Height);
            }
        }

        private double OriginLatitudeRadian
        {
            get
            {
                return this.OriginLatitude * (Math.PI / 180);
            }
        }

        private double OriginLongitudeRadian
        {
            get
            {
                return this.OriginLongitude * (Math.PI / 180);
            }
        }

        private double DestinationLatitudeRadian
        {
            get
            {
                return this.DestinationLatitude * (Math.PI / 180);
            }
        }

        private double DestinationLongitudeRadian
        {
            get
            {
                return this.DestinationLongitude * (Math.PI / 180);
            }
        }

        private int DestinationX
        {
            get
            {
                if (NewMap.Line != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get DestinationX when ParameterSet is not Line");
                }

                return (int)(((this.DestinationLongitude + 180) / 360) * this.InputBitmap.Width);
            }
        }

        private int DestinationY
        {
            get
            {
                if (NewMap.Line != this.ParameterSetName)
                {
                    throw new InvalidOperationException("Attempted to get DestinationY when ParameterSet is not Line");
                }

                return (int)(((180 - (this.DestinationLatitude + 90)) / 180) * this.InputBitmap.Height);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Protected Methods

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case NewMap.Point:
                    // ReSharper disable PossibleLossOfFraction
                    this.InputGraphics.DrawImage(
                        this.IconBitmap,
                        this.X - (this.IconWidth / 2),
                        this.Y - (this.IconHeight / 2),
                        this.IconWidth,
                        this.IconHeight);
                    // ReSharper restore PossibleLossOfFraction
                    break;

                case NewMap.Line:
                    this.InputGraphics.DrawLine(
                        new Pen(Brushes.HotPink, 4),
                        new Point(this.OriginX, this.OriginY),
                        new Point(this.DestinationX, this.DestinationY));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        protected override void EndProcessing()
        {
            this.InputBitmap.Save(Path.GetFullPath(this.OutputPath), this.OutputFormat);
        }

        #endregion

        #region Private Methods

        private static Stream GetStream(string path)
        {
            if (File.Exists(Path.GetFullPath(path)))
            {
                return new FileStream(Path.GetFullPath(path), FileMode.Open);
            }

            var ass = Assembly.GetAssembly(typeof(NewMap));

            foreach (var resourceName in ass.GetManifestResourceNames().Where(resourceName => resourceName.EndsWith(path, StringComparison.InvariantCultureIgnoreCase)))
            {
                return ass.GetManifestResourceStream(resourceName);
            }

            throw new FileNotFoundException(string.Format("File not found: {0}", path), path);
        }

        private static Bitmap GetBitmap(Stream stream)
        {
            return new Bitmap(stream);
        }

        private static Graphics GetGraphics(Image bitmap)
        {
            return Graphics.FromImage(bitmap);
        }

        #endregion

        #endregion
    }
}