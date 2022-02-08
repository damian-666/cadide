﻿using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using TriangleNet.Geometry;
using TriangleNet.Meshing;

using LineSegment = System.Windows.Media.LineSegment;
using Point = System.Windows.Point;
using Vector3D = global::SharpDX.Vector3;
using Point3D = global::SharpDX.Vector3;

namespace IDE.Controls
{
    public static class TextExtruder
    {
        public static HelixToolkit.Wpf.SharpDX.Geometry3D ExtrudeText(string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight,Vector3D textDirection, Point3D p0, Point3D p1)
        {

            var fontSizeMM = 0.0254 * 1000 * fontSize / 96.0d;

            var tf = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);

            var formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                tf,
                fontSizeMM,
                Brushes.Black,
                pixelsPerDip: 1.0d);


            var origin = new Point();

            var textGeometry = formattedText.BuildGeometry(origin);

            var outlineList = new List<List<Point[]>>();
            AppendOutlines(textGeometry, outlineList);

            // Build the polygon to mesh (using Triangle.NET to triangulate)
            var polygon = new TriangleNet.Geometry.Polygon();
            int marker = 0;

            var builder = new MeshBuilder();

            foreach (var outlines in outlineList)
            {
                var outerOutline = outlines.OrderBy(x => x.AreaOfSegment()).Last();

                for (int i = 0; i < outlines.Count; i++)
                {
                    var outline = outlines[i];
                    var isHole = i != outlines.Count - 1 && IsPointInPolygon(outerOutline, outline[0]);
                    polygon.AddContour(outline.Select(p => new Vertex(p.X, p.Y)), marker++, isHole);
                    builder.AddExtrudedSegments(outline.ToSegments().Select(x => new SharpDX.Vector2((float)x.X, (float)x.Y)).ToList(),
                        textDirection, p0, p1);
                }
            }

            var mesher = new GenericMesher();
            var options = new ConstraintOptions();
            var mesh = mesher.Triangulate(polygon, options);

            var u = textDirection;
            u.Normalize();
            var z = p1 - p0;
            var h = z.Length();
            z.Normalize();
            var v = Vector3D.Cross(z, u);

            // Convert the triangles
            foreach (var t in mesh.Triangles)
            {
                var v0 = t.GetVertex(2);
                var v1 = t.GetVertex(1);
                var v2 = t.GetVertex(0);

                // Add the top triangle.
                // Project the X/Y vertices onto a plane defined by textdirection, p0 and p1.                
                builder.AddTriangle(v0.Project(p0, u, v, z, h), v1.Project(p0, u, v, z, h), v2.Project(p0, u, v, z, h));

                // Add the bottom triangle.
                builder.AddTriangle(v2.Project(p0, u, v, z, 0), v1.Project(p0, u, v, z, 0), v0.Project(p0, u, v, z, 0));
            }
            if (builder.CreateNormals)
            {
                builder.Normals = null;
                builder.ComputeNormalsAndTangents(MeshFaces.Default, builder.HasTangents);
            }

            return builder.ToMesh();
        }

        private static void AppendOutlines(Geometry geometry, List<List<Point[]>> outlines)
        {
            var group = geometry as GeometryGroup;
            if (group != null)
            {
                foreach (var g in group.Children)
                {
                    AppendOutlines(g, outlines);
                }

                return;
            }

            var pathGeometry = geometry as PathGeometry;
            if (pathGeometry != null)
            {
                var figures = pathGeometry.Figures.Select(figure => figure.ToPolyLine()).ToList();
                outlines.Add(figures);
                return;
            }

            throw new NotImplementedException();
        }

        static Point[] ToPolyLine(this PathFigure figure)
        {
            var outline = new List<Point> { figure.StartPoint };
            var previousPoint = figure.StartPoint;
            foreach (var segment in figure.Segments)
            {
                var polyline = segment as PolyLineSegment;
                if (polyline != null)
                {
                    outline.AddRange(polyline.Points);
                    previousPoint = polyline.Points.Last();
                    continue;
                }

                var polybezier = segment as PolyBezierSegment;
                if (polybezier != null)
                {
                    for (int i = -1; i + 3 < polybezier.Points.Count; i += 3)
                    {
                        var p1 = i == -1 ? previousPoint : polybezier.Points[i];
                        outline.AddRange(FlattenBezier(p1, polybezier.Points[i + 1], polybezier.Points[i + 2], polybezier.Points[i + 3], 10));
                    }

                    previousPoint = polybezier.Points.Last();
                    continue;
                }

                var lineSegment = segment as LineSegment;
                if (lineSegment != null)
                {
                    outline.Add(lineSegment.Point);
                    previousPoint = lineSegment.Point;
                    continue;
                }

                var bezierSegment = segment as BezierSegment;
                if (bezierSegment != null)
                {
                    outline.AddRange(FlattenBezier(previousPoint, bezierSegment.Point1, bezierSegment.Point2, bezierSegment.Point3, 10));
                    previousPoint = bezierSegment.Point3;
                    continue;
                }

                throw new NotImplementedException();
            }

            return outline.ToArray();
        }

        static double AreaOfSegment(this Point[] segment)
        {
            return Math.Abs(segment.Take(segment.Length - 1)
                .Select((p, i) => (segment[i + 1].X - p.X) * (segment[i + 1].Y + p.Y))
                .Sum() / 2);
        }

        static Point3D Project(this Vertex v, Point3D p0, Vector3D x, Vector3D y, Vector3D z, double h)
        {
            return p0 + x * (float)v.X - y * (float)v.Y + z * (float)h;
        }

        static bool IsPointInPolygon(IList<Point> polygon, Point testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y) || (polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y))
                {
                    if (polygon[i].X + ((testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X)) < testPoint.X)
                    {
                        result = !result;
                    }
                }

                j = i;
            }

            return result;
        }

        static IEnumerable<Point> ToSegments(this IEnumerable<Point> input)
        {
            bool first = true;
            var previous = default(Point);
            foreach (var point in input)
            {
                if (!first)
                {
                    yield return previous;
                    yield return point;
                }
                else
                {
                    first = false;
                }

                previous = point;
            }
        }


        private static IEnumerable<Point> FlattenBezier(Point p1, Point p2, Point p3, Point p4, int n)
        {
            // http://tsunami.cis.usouthal.edu/~hain/general/Publications/Bezier/bezier%20cccg04%20paper.pdf
            // http://en.wikipedia.org/wiki/De_Casteljau's_algorithm
            for (int i = 1; i <= n; i++)
            {
                var t = (double)i / n;
                var u = 1 - t;
                yield return new Point(
                    (u * u * u * p1.X) + (3 * t * u * u * p2.X) + (3 * t * t * u * p3.X) + (t * t * t * p4.X),
                    (u * u * u * p1.Y) + (3 * t * u * u * p2.Y) + (3 * t * t * u * p3.Y) + (t * t * t * p4.Y));
            }
        }

    }

}
