using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparrow.Chart
{
    #region Enums
    /// <summary>
    /// Set Theme for SparrowChart
    /// </summary>
    public enum Theme
    {
        Arctic,
        Autmn,
        Cold,
        Flower,
        Forest,
        Grayscale,
        Ground,
        Lialac,
        Natural,
        Pastel,
        Rainbow,
        Spring,
        Summer,
        Warm,
        Metro,                
        Custom
    }

    /// <summary>
    /// Set the axis tick position
    /// </summary>
    public enum TickPosition
    {
        Inside,
        Cross,
        Outside
    }

    /// <summary>
    /// Set the XAxis Position for Sparrow Chart
    /// </summary>
    public enum XAxisPosition
    {
        Bottom,
        Top
    }

    /// <summary>
    /// Set the YAxis Position for Sparrow Chart
    /// </summary>
    public enum YAxisPosition
    {
        Left,
        Right
    }

    /// <summary>
    /// Set the Axis Position for Sparrow Chart
    /// </summary>
    internal enum AxisPosition
    {
        Left,
        Top,
        Right,
        Bottom
    }

    /// <summary>
    /// Set the Legend position for Sparrow Chart
    /// </summary>
    public enum LegendPosition
    {
        Inside,
        Outside
    }

    /// <summary>
    /// Set ValueType of XAxis
    /// </summary>
    public enum XType
    {
        Double, // For use Double type Axis
        DateTime, // For use DateTime type Axis
        Category // For use Category type axis

    }

    internal enum ActualType
    {
        Double, // For use Double type Axis
        DateTime, // For use DateTime type Axis
        Category // For use Category type axis
    }

    /// <summary>
    /// Set ValueType of YAxis
    /// </summary>
    public enum YType
    {
        Double, // For use Double type Axis
        DateTime, // For use DateTime type Axis
    }

    /// <summary>
    /// Set SmoothingMode of GDI and BitmapGraphics
    /// </summary>
    public enum SmoothingMode
    {
        AntiAlias,
        Default,
        HighQuality,
        HighSpeed,
        Invalid,
        None
    }

    /// <summary>
    /// Set CompositingQuality of GDI and BitmapGraphics
    /// </summary>
    public enum CompositingQuality
    {
        AssumeLinear,
        Default,
        GammaCorrected,
        HighQuality,
        HighSpeed,
        Invalid
    }

    /// <summary>
    /// Set RenderingMode of SparrowChart(GDIRendering-Good Performance,DefaultRendering-Medium Performance,Default-Good Quality of Rendering)
    /// </summary>
    public enum RenderingMode
    {
        Default,
#if DIRECTX2D
        DirectX2D,
#endif
#if WPF
        GDIRendering,  
        WritableBitmap
#endif
    }

    /// <summary>
    /// Set CompositingMode of GDI and BitmapGraphics
    /// </summary>
    public enum CompositingMode
    {
        SourceCopy,
        SourceOver
    }
   


#if !WPF

    // Summary:
    //     Specifies values that control the behavior of a control positioned inside
    //     another control.
    public enum Dock
    {
        // Summary:
        //     Specifies that the control should be positioned on the left of the control.
        Left = 0,
        //
        // Summary:
        //     Specifies that the control should be positioned on top of the control.
        Top = 1,
        //
        // Summary:
        //     Specifies that the control should be positioned on the right of the control.
        Right = 2,
        //
        // Summary:
        //     Specifies that the control should be positioned at the bottom of the control.
        Bottom = 3,
    }
#endif

#endregion
}
