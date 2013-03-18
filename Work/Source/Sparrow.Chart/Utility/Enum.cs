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
        Metro,                
        Custom
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
    /// Set RenderingMode of SparrowChart(GDIRendering-Good Performance,DefaultWPFRendering-Medium Performance,DefaultWPFRendering-Good Quality of Rendering)
    /// </summary>
    public enum RenderingMode
    {
        DefaultWPFRendering,
#if DIRECTX2D
        DirectX2D,
#endif
        GDIRendering,        
        WritableBitmap
    }

    /// <summary>
    /// Set CompositingMode of GDI and BitmapGraphics
    /// </summary>
    public enum CompositingMode
    {
        SourceCopy,
        SourceOver
    }
#endregion
}
