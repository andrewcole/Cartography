using System.Management.Automation;
using System.Drawing;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Illallangi.Cartography.PowerShell
{
[Cmdlet("New", "Map")]
public class NewMap : Cmdlet
{
	private Bitmap currentBitmap;

	private Graphics currentGraphics;

	private double? currentLatitudeA;

	private double? currentLatitudeB;

	private double? currentLongitudeA;

	private double? currentLongitudeB;

	private const int DefaultHeight = 64;

	private const string DefaultLogo = "Airplane.png";

	private const int DefaultWidth = 64;

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

	private GeoLine BoundingBox
	{
		get
		{
			double valueOrDefault;
			double num;
			double valueOrDefault1;
			double num1;
			double? maxLat = this.MaxLat;
			if (maxLat.HasValue)
			{
				valueOrDefault = (double)((double)maxLat.GetValueOrDefault());
			}
			else
			{
				valueOrDefault = 0;
			}
			double padding = valueOrDefault + (double)this.Padding;
			maxLat = this.MaxLon;
			if (maxLat.HasValue)
			{
				num = (double)((double)maxLat.GetValueOrDefault());
			}
			else
			{
				num = 0;
			}
			GeoPoint geoPoint = GeoPoint.FromDegrees(padding, num + (double)this.Padding);
			maxLat = this.MinLat;
			if (maxLat.HasValue)
			{
				valueOrDefault1 = (double)((double)maxLat.GetValueOrDefault());
			}
			else
			{
				valueOrDefault1 = 0;
			}
			double padding1 = valueOrDefault1 - (double)this.Padding;
			maxLat = this.MinLon;
			if (maxLat.HasValue)
			{
				num1 = (double)((double)maxLat.GetValueOrDefault());
			}
			else
			{
				num1 = 0;
			}
			GeoLine geoLine = new GeoLine(geoPoint, GeoPoint.FromDegrees(padding1, num1 - (double)this.Padding));
			return geoLine;
		}
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false)]
	public double? CenterLongitude
	{
		get;
		set;
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false)]
	public bool Clip
	{
		get;
		set;
	}

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

	[Parameter(ValueFromPipelineByPropertyName=true)]
	public int? Height
	{
		get;
		set;
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false)]
	public string Input
	{
		get;
		set;
	}

	[Parameter(ValueFromPipelineByPropertyName=true)]
	[Alias(new string[] { "Latitude" })]
	public double? LatitudeA
	{
		get
		{
			double? nullable = this.currentLatitudeA;
			return nullable;
		}
		set
		{
			double? nullable;
			bool hasValue;
			bool flag;
			bool hasValue1;
			bool flag1;
			bool hasValue2;
			bool flag2;
			this.currentLatitudeA = value;
			double? minLat = value;
			if (0 != (double)minLat.GetValueOrDefault())
			{
				hasValue = true;
			}
			else
			{
				hasValue = !minLat.HasValue;
			}
			if (!hasValue)
			{
				flag = true;
			}
			else
			{
				minLat = this.MinLat;
				if (!minLat.HasValue)
				{
					flag = false;
				}
				else
				{
					minLat = this.MinLat;
					nullable = value;
					if ((double)minLat.GetValueOrDefault() <= (double)nullable.GetValueOrDefault())
					{
						flag2 = false;
					}
					else
					{
						flag2 = minLat.HasValue & nullable.HasValue;
					}
					flag = !flag2;
				}
			}
			bool flag3 = flag;
			if (!flag3)
			{
				this.MinLat = value;
			}
			minLat = value;
			if (0 != (double)minLat.GetValueOrDefault())
			{
				hasValue1 = true;
			}
			else
			{
				hasValue1 = !minLat.HasValue;
			}
			if (!hasValue1)
			{
				flag1 = true;
			}
			else
			{
				minLat = this.MaxLat;
				if (!minLat.HasValue)
				{
					flag1 = false;
				}
				else
				{
					minLat = this.MaxLat;
					nullable = value;
					if ((double)minLat.GetValueOrDefault() >= (double)nullable.GetValueOrDefault())
					{
						hasValue2 = false;
					}
					else
					{
						hasValue2 = minLat.HasValue & nullable.HasValue;
					}
					flag1 = !hasValue2;
				}
			}
			flag3 = flag1;
			if (!flag3)
			{
				this.MaxLat = value;
			}
		}
	}

	[Alias(new string[] { "DestinationLatitude" })]
	[Parameter(ValueFromPipelineByPropertyName=true)]
	public double? LatitudeB
	{
		get
		{
			double? nullable = this.currentLatitudeB;
			return nullable;
		}
		set
		{
			double? nullable;
			bool hasValue;
			bool flag;
			bool hasValue1;
			bool flag1;
			bool hasValue2;
			bool flag2;
			this.currentLatitudeB = value;
			double? minLat = value;
			if (0 != (double)minLat.GetValueOrDefault())
			{
				hasValue = true;
			}
			else
			{
				hasValue = !minLat.HasValue;
			}
			if (!hasValue)
			{
				flag = true;
			}
			else
			{
				minLat = this.MinLat;
				if (!minLat.HasValue)
				{
					flag = false;
				}
				else
				{
					minLat = this.MinLat;
					nullable = value;
					if ((double)minLat.GetValueOrDefault() <= (double)nullable.GetValueOrDefault())
					{
						flag2 = false;
					}
					else
					{
						flag2 = minLat.HasValue & nullable.HasValue;
					}
					flag = !flag2;
				}
			}
			bool flag3 = flag;
			if (!flag3)
			{
				this.MinLat = value;
			}
			minLat = value;
			if (0 != (double)minLat.GetValueOrDefault())
			{
				hasValue1 = true;
			}
			else
			{
				hasValue1 = !minLat.HasValue;
			}
			if (!hasValue1)
			{
				flag1 = true;
			}
			else
			{
				minLat = this.MaxLat;
				if (!minLat.HasValue)
				{
					flag1 = false;
				}
				else
				{
					minLat = this.MaxLat;
					nullable = value;
					if ((double)minLat.GetValueOrDefault() >= (double)nullable.GetValueOrDefault())
					{
						hasValue2 = false;
					}
					else
					{
						hasValue2 = minLat.HasValue & nullable.HasValue;
					}
					flag1 = !hasValue2;
				}
			}
			flag3 = flag1;
			if (!flag3)
			{
				this.MaxLat = value;
			}
		}
	}

	public GeoLine Line
	{
		get
		{
			GeoLine geoLine;
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
			if (flag)
			{
				geoLine = new GeoLine(this.PointA, this.PointB);
			}
			else
			{
				geoLine = null;
			}
			return geoLine;
		}
	}

	[Parameter(ValueFromPipelineByPropertyName=true)]
	public string Logo
	{
		get;
		set;
	}

	[Parameter(ValueFromPipelineByPropertyName=true)]
	[Alias(new string[] { "Longitude" })]
	public double? LongitudeA
	{
		get
		{
			double? nullable = this.currentLongitudeA;
			return nullable;
		}
		set
		{
			double? nullable;
			bool hasValue;
			bool flag;
			bool hasValue1;
			bool flag1;
			bool hasValue2;
			bool flag2;
			this.currentLongitudeA = value;
			double? minLon = value;
			if (0 != (double)minLon.GetValueOrDefault())
			{
				hasValue = true;
			}
			else
			{
				hasValue = !minLon.HasValue;
			}
			if (!hasValue)
			{
				flag = true;
			}
			else
			{
				minLon = this.MinLon;
				if (!minLon.HasValue)
				{
					flag = false;
				}
				else
				{
					minLon = this.MinLon;
					nullable = value;
					if ((double)minLon.GetValueOrDefault() <= (double)nullable.GetValueOrDefault())
					{
						flag2 = false;
					}
					else
					{
						flag2 = minLon.HasValue & nullable.HasValue;
					}
					flag = !flag2;
				}
			}
			bool flag3 = flag;
			if (!flag3)
			{
				this.MinLon = value;
			}
			minLon = value;
			if (0 != (double)minLon.GetValueOrDefault())
			{
				hasValue1 = true;
			}
			else
			{
				hasValue1 = !minLon.HasValue;
			}
			if (!hasValue1)
			{
				flag1 = true;
			}
			else
			{
				minLon = this.MaxLon;
				if (!minLon.HasValue)
				{
					flag1 = false;
				}
				else
				{
					minLon = this.MaxLon;
					nullable = value;
					if ((double)minLon.GetValueOrDefault() >= (double)nullable.GetValueOrDefault())
					{
						hasValue2 = false;
					}
					else
					{
						hasValue2 = minLon.HasValue & nullable.HasValue;
					}
					flag1 = !hasValue2;
				}
			}
			flag3 = flag1;
			if (!flag3)
			{
				this.MaxLon = value;
			}
		}
	}

	[Parameter(ValueFromPipelineByPropertyName=true)]
	[Alias(new string[] { "DestinationLongitude" })]
	public double? LongitudeB
	{
		get
		{
			double? nullable = this.currentLongitudeB;
			return nullable;
		}
		set
		{
			double? nullable;
			bool hasValue;
			bool flag;
			bool hasValue1;
			bool flag1;
			bool hasValue2;
			bool flag2;
			this.currentLongitudeB = value;
			double? minLon = value;
			if (0 != (double)minLon.GetValueOrDefault())
			{
				hasValue = true;
			}
			else
			{
				hasValue = !minLon.HasValue;
			}
			if (!hasValue)
			{
				flag = true;
			}
			else
			{
				minLon = this.MinLon;
				if (!minLon.HasValue)
				{
					flag = false;
				}
				else
				{
					minLon = this.MinLon;
					nullable = value;
					if ((double)minLon.GetValueOrDefault() <= (double)nullable.GetValueOrDefault())
					{
						flag2 = false;
					}
					else
					{
						flag2 = minLon.HasValue & nullable.HasValue;
					}
					flag = !flag2;
				}
			}
			bool flag3 = flag;
			if (!flag3)
			{
				this.MinLon = value;
			}
			minLon = value;
			if (0 != (double)minLon.GetValueOrDefault())
			{
				hasValue1 = true;
			}
			else
			{
				hasValue1 = !minLon.HasValue;
			}
			if (!hasValue1)
			{
				flag1 = true;
			}
			else
			{
				minLon = this.MaxLon;
				if (!minLon.HasValue)
				{
					flag1 = false;
				}
				else
				{
					minLon = this.MaxLon;
					nullable = value;
					if ((double)minLon.GetValueOrDefault() >= (double)nullable.GetValueOrDefault())
					{
						hasValue2 = false;
					}
					else
					{
						hasValue2 = minLon.HasValue & nullable.HasValue;
					}
					flag1 = !hasValue2;
				}
			}
			flag3 = flag1;
			if (!flag3)
			{
				this.MaxLon = value;
			}
		}
	}

	private double? MaxLat
	{
		get;
		set;
	}

	private double? MaxLon
	{
		get;
		set;
	}

	private double? MinLat
	{
		get;
		set;
	}

	private double? MinLon
	{
		get;
		set;
	}

	[Alias(new string[] { "PointName" })]
	[Parameter(ValueFromPipelineByPropertyName=true)]
	public string Name
	{
		get;
		set;
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false, Mandatory=true, Position=1)]
	[ValidateNotNullOrEmpty]
	public string Output
	{
		get;
		set;
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false)]
	[ValidateNotNullOrEmpty]
	public ImageFormat OutputFormat
	{
		get;
		set;
	}

	[Parameter(ValueFromPipeline=false, ValueFromPipelineByPropertyName=false)]
	public int Padding
	{
		get;
		set;
	}

	public GeoPoint PointA
	{
		get
		{
			GeoPoint geoPoint;
			bool flag;
			bool flag1;
			double valueOrDefault;
			double num;
			bool hasValue;
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
				if ((double)longitudeA.GetValueOrDefault() != 0)
				{
					hasValue = false;
				}
				else
				{
					hasValue = longitudeA.HasValue;
				}
				longitudeA = this.LatitudeA;
				if ((double)longitudeA.GetValueOrDefault() != 0)
				{
					hasValue1 = false;
				}
				else
				{
					hasValue1 = longitudeA.HasValue;
				}
				flag1 = !(hasValue & hasValue1);
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
			bool hasValue;
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

	[Parameter(ValueFromPipelineByPropertyName=true)]
	public int? Width
	{
		get;
		set;
	}

	public NewMap()
	{
		this.Input = "DefaultInput.jpg";
		this.OutputFormat = ImageFormat.Jpeg;
		this.Padding = 5;
		this.Clip = false;
	}

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
		double valueOrDefault;
		this.DisposeOfGraphics();
		this.Debug(string.Format("Writing out {1} {0}", Path.GetFullPath(this.Output), this.OutputFormat.ToString()));
		bool clip = !this.Clip;
		if (clip)
		{
			double? centerLongitude = this.CenterLongitude;
			clip = !centerLongitude.HasValue;
			if (clip)
			{
				(new Bitmap(this.Bitmap)).Save(Path.GetFullPath(this.Output), this.OutputFormat);
			}
			else
			{
				Bitmap bitmap = new Bitmap(this.Bitmap.Width, this.Bitmap.Height);
				try
				{
					Graphics graphics = Graphics.FromImage(bitmap);
					try
					{
						this.Debug(string.Format("Centering at {0}", this.CenterLongitude));
						double midLatitude = GeoPoint.MidLatitude;
						centerLongitude = this.CenterLongitude;
						if (centerLongitude.HasValue)
						{
							valueOrDefault = (double)((double)centerLongitude.GetValueOrDefault());
						}
						else
						{
							valueOrDefault = 0;
						}
						Point middle = GeoPoint.FromDegrees(midLatitude, valueOrDefault).ToPoint(this.Bitmap);
						graphics.DrawImage(this.Bitmap, -middle.X + this.Bitmap.Width / 2, 0, this.Bitmap.Width, this.Bitmap.Height);
						graphics.DrawImage(this.Bitmap, -middle.X + this.Bitmap.Width / 2 + this.Bitmap.Width, 0, this.Bitmap.Width, this.Bitmap.Height);
					}
					finally
					{
						clip = graphics == null;
						if (!clip)
						{
							graphics.Dispose();
						}
					}
					bitmap.Save(Path.GetFullPath(this.Output), this.OutputFormat);
				}
				finally
				{
					clip = bitmap == null;
					if (!clip)
					{
						bitmap.Dispose();
					}
				}
			}
		}
		else
		{
			this.Bitmap.Clone(this.BoundingBox.GetRectangle(this.Bitmap, 0), PixelFormat.Undefined).Save(Path.GetFullPath(this.Output), this.OutputFormat);
		}
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
					int? nullable = this.Height;
					if (nullable.HasValue)
					{
						valueOrDefault = nullable.GetValueOrDefault();
					}
					else
					{
						valueOrDefault = 64;
					}
					int height = valueOrDefault;
					nullable = this.Width;
					if (nullable.HasValue)
					{
						num = nullable.GetValueOrDefault();
					}
					else
					{
						num = 64;
					}
					int width = num;
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
}
}