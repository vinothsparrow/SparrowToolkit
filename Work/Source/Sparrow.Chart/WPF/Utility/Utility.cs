﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
#if !WINRT
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
#endif

namespace Sparrow.Chart
{       
    /// <summary>
    /// BezierSpline
    /// </summary>
    public static class BezierSpline
    {
        /// <summary>
        /// Get open-ended Bezier Spline Control Points.
        /// </summary>
        /// <param name="knots">Input Knot Bezier spline points.</param>
        /// <param name="firstControlPoints">Output First Control points array of knots.Length - 1 length.</param>
        /// <param name="secondControlPoints">Output Second Control points array of knots.Length - 1 length.</param>
        /// <exception cref="ArgumentNullException"><paramref name="knots"/> parameter must be not null.</exception>
        /// <exception cref="ArgumentException"><paramref name="knots"/> array must containg at least two points.</exception>
        public static void GetCurveControlPoints(Point[] knots, out Point[] firstControlPoints, out Point[] secondControlPoints)
        {
            if (knots == null)
                throw new ArgumentNullException("knots");
            int n = knots.Length - 1;
            if (n < 1)
                throw new ArgumentException("At least two knot points required", "knots");
            if (n == 1)
            { // Special case: Bezier curve should be a straight line.
                firstControlPoints = new Point[1];
                // 3P1 = 2P0 + P3
                firstControlPoints[0].X = (2 * knots[0].X + knots[1].X) / 3;
                firstControlPoints[0].Y = (2 * knots[0].Y + knots[1].Y) / 3;

                secondControlPoints = new Point[1];
                // P2 = 2P1 – P0
                secondControlPoints[0].X = 2 * firstControlPoints[0].X - knots[0].X;
                secondControlPoints[0].Y = 2 * firstControlPoints[0].Y - knots[0].Y;
                return;
            }

            // Calculate first Bezier control points
            // Right hand side vector
            double[] rhs = new double[n];

            // Set right hand side X values
            for (int i = 1; i < n - 1; ++i)
                rhs[i] = 4 * knots[i].X + 2 * knots[i + 1].X;
            rhs[0] = knots[0].X + 2 * knots[1].X;
            rhs[n - 1] = (8 * knots[n - 1].X + knots[n].X) / 2.0;
            // Get first control points X-values
            double[] x = GetFirstControlPoints(rhs);

            // Set right hand side Y values
            for (int i = 1; i < n - 1; ++i)
                rhs[i] = 4 * knots[i].Y + 2 * knots[i + 1].Y;
            rhs[0] = knots[0].Y + 2 * knots[1].Y;
            rhs[n - 1] = (8 * knots[n - 1].Y + knots[n].Y) / 2.0;
            // Get first control points Y-values
            double[] y = GetFirstControlPoints(rhs);

            // Fill output arrays.
            firstControlPoints = new Point[n];
            secondControlPoints = new Point[n];
            for (int i = 0; i < n; ++i)
            {
                // First control point
                firstControlPoints[i] = new Point(x[i], y[i]);
                // Second control point
                if (i < n - 1)
                    secondControlPoints[i] = new Point(2 * knots[i + 1].X - x[i + 1], 2 * knots[i + 1].Y - y[i + 1]);
                else
                    secondControlPoints[i] = new Point((knots[n].X + x[n - 1]) / 2, (knots[n].Y + y[n - 1]) / 2);
            }
        }

        /// <summary>
        /// Solves a tridiagonal system for one of coordinates (x or y) of first Bezier control points.
        /// </summary>
        /// <param name="rhs">Right hand side vector.</param>
        /// <returns>Solution vector.</returns>
        private static double[] GetFirstControlPoints(double[] rhs)
        {
            int n = rhs.Length;
            double[] x = new double[n]; // Solution vector.
            double[] tmp = new double[n]; // Temp workspace.

            double b = 2.0;
            x[0] = rhs[0] / b;
            for (int i = 1; i < n; i++) // Decomposition and forward substitution.
            {
                tmp[i] = 1 / b;
                b = (i < n - 1 ? 4.0 : 3.5) - tmp[i];
                x[i] = (rhs[i] - x[i - 1]) / b;
            }
            for (int i = 1; i < n; i++)
                x[n - i - 1] -= tmp[n - i] * x[n - i]; // Backsubstitution.

            return x;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Themes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Brush> MetroBrushes()
        {
            List<Brush> metroBrushes = new List<Brush>();
            for (int i = 0; i < 10; i++)
            {
                metroBrushes.Add(new SolidColorBrush(Colors.Black));
            }
            metroBrushes[0] = new SolidColorBrush(Color.FromArgb(0xFF,0x1B, 0xA1, 0xE2));
            metroBrushes[4] = new SolidColorBrush(Color.FromArgb(0xFF,160, 80, 0));
            metroBrushes[2] = new SolidColorBrush(Color.FromArgb(0xFF,0xFF, 0x00, 0x00));
            metroBrushes[1] = new SolidColorBrush(Color.FromArgb(0xFF,0xA2, 0xC1, 0x39));
            metroBrushes[3] = new SolidColorBrush(Color.FromArgb(0xFF,0xD8, 0x00, 0x73)); 
            metroBrushes[5] = new SolidColorBrush(Color.FromArgb(0xFF,230, 113, 84));
            metroBrushes[6] = new SolidColorBrush(Color.FromArgb(0xFF,240, 150, 9));
            metroBrushes[7] = new SolidColorBrush(Color.FromArgb(0xFF,27, 161, 226));
            metroBrushes[8] = new SolidColorBrush(Color.FromArgb(0xFF,0x33, 0x99, 0x33));
            metroBrushes[9] = new SolidColorBrush(Color.FromArgb(0xFF,0, 171, 169));
            return metroBrushes;
        }
        
    } 
}