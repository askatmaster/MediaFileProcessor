namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enum to represent different types of filters that can be used.
/// </summary>
public enum FilterType
{
    /// <summary>
    /// Point filter.
    /// </summary>
    Point = 0,

    /// <summary>
    /// Box filter.
    /// </summary>
    Box = 1,

    /// <summary>
    /// Triangle filter.
    /// </summary>
    Triangle = 2,

    /// <summary>
    /// Cubic spline filter.
    /// </summary>
    CubicSpline = 3,

    /// <summary>
    /// Quadratic filter.
    /// </summary>
    Quadratic = 4,

    /// <summary>
    /// Gaussian filter.
    /// </summary>
    Gaussian = 5,

    /// <summary>
    /// Hermite filter.
    /// </summary>
    Hermite = 6,

    /// <summary>
    /// Cubic filter.
    /// </summary>
    Cubic = 7,

    /// <summary>
    /// Catrom filter.
    /// </summary>
    Catrom = 8,

    /// <summary>
    /// Mitchell filter.
    /// </summary>
    Mitchell = 9,

    /// <summary>
    /// Lanczos filter.
    /// </summary>
    Lanczos = 10,

    /// <summary>
    /// Blackman filter.
    /// </summary>
    Blackman = 11,

    /// <summary>
    /// Kaiser filter.
    /// </summary>
    Kaiser = 12,

    /// <summary>
    /// Welsh filter.
    /// </summary>
    Welsh = 13,

    /// <summary>
    /// Hanning filter.
    /// </summary>
    Hanning = 14,

    /// <summary>
    /// Bartlett filter.
    /// </summary>
    Bartlett = 15,

    /// <summary>
    /// Bohman filter.
    /// </summary>
    Bohman = 16
}