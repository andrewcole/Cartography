using System;
using System.Collections.Generic;
using System.Drawing;

namespace Illallangi.Cartography.PowerShell
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
			double d = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((this.lat1 - this.lat2) / 2), 2) + Math.Cos(this.lat1) * Math.Cos(this.lat2) * Math.Pow(Math.Sin((this.lon1 - this.lon2) / 2), 2)));
			double distanceNauticalMiles = d * 180 * 60 / 3.14159265358979;
			double num = d;
			return num;
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
		int num;
		GeoPoint geoPoint = null;
		double num1 = 1;
		if (steps > 36)
		{
			num = steps;
		}
		else
		{
			num = 36;
		}
		double num2 = num1 / (double)num;
		double num3 = 0;
		while (true)
		{
			bool flag = num3 <= 1;
			if (!flag)
			{
				break;
			}
			double num4 = Math.Sin((1 - num3) * this.GeoDistanceAsRadians) / Math.Sin(this.GeoDistanceAsRadians);
			double num5 = Math.Sin(num3 * this.GeoDistanceAsRadians) / Math.Sin(this.GeoDistanceAsRadians);
			double num6 = num4 * Math.Cos(this.lat1) * Math.Cos(this.lon1) + num5 * Math.Cos(this.lat2) * Math.Cos(this.lon2);
			double num7 = num4 * Math.Cos(this.lat1) * Math.Sin(this.lon1) + num5 * Math.Cos(this.lat2) * Math.Sin(this.lon2);
			double num8 = num4 * Math.Sin(this.lat1) + num5 * Math.Sin(this.lat2);
			double num9 = Math.Atan2(num8, Math.Sqrt(Math.Pow(num6, 2) + Math.Pow(num7, 2)));
			double num10 = Math.Atan2(num7, num6);
			GeoPoint geoPoint1 = GeoPoint.FromRadians(num9, num10);
			flag = null == geoPoint;
			if (!flag)
			{
				yield return new GeoLine(geoPoint, geoPoint1);
			}
			geoPoint = geoPoint1;
			num3 = num3 + num2;
		}
	}
}
}