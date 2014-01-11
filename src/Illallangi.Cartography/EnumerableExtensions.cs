using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Illallangi.Cartography
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> ToPairs<T>(this IEnumerable<T> e) where T : struct
        {
            T? lastO = null;
            foreach (var o in e)
            {
                if (lastO.HasValue)
                {
                    yield return new Tuple<T, T>(lastO.Value, o);
                }

                lastO = o;
            }
        }

        public static IEnumerable<Tuple<Point, Point>> FixWraparound(this IEnumerable<Tuple<Point, Point>> lines, int width)
        {
            foreach (var line in lines)
            {
                var right = line.RightMost();
                var left = line.LeftMost();

                var wrapWidth = (left.X + width) - right.X;
                var lineWidth = right.X - left.X;

                if (lineWidth <= wrapWidth)
                {
                    yield return line;
                }
                else
                {
                    var y = left.Y + ((left.X / wrapWidth) * (left.Y - right.Y));

                    yield return new Tuple<Point, Point>(left, new Point(0, y));
                    yield return new Tuple<Point, Point>(new Point(width, y), right);
                }
            }
        }

        public static Point LeftMost(this Tuple<Point, Point> points)
        {
            return new[] { points.Item1, points.Item2 }.LeftMost();
        }

        public static Point RightMost(this Tuple<Point, Point> points)
        {
            return new[] { points.Item1, points.Item2 }.RightMost();
        }

        public static Point LeftMost(this IEnumerable<Point> points)
        {
            return points.OrderBy(p => p.X).First();
        }

        public static Point RightMost(this IEnumerable<Point> points)
        {
            return points.OrderBy(p => p.X).Last();
        }
    }
}