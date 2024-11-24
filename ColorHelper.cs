using System;
using System.Drawing;
using System.Text;

namespace SystemEnhancer
{
    /// <summary>
    /// Provides color conversion and manipulation utilities
    /// </summary>
    public static class ColorUtilities
    {
        /// <summary>
        /// Converts RGB color to HSV (Hue, Saturation, Value) components
        /// </summary>
        public static (double Hue, double Saturation, double Value) RgbToHsv(Color color)
        {
            try
            {
                double r = color.R / 255.0;
                double g = color.G / 255.0;
                double b = color.B / 255.0;

                double min = Math.Min(r, Math.Min(g, b));
                double max = Math.Max(r, Math.Max(g, b));
                double delta = max - min;

                double hue = 0;
                double saturation = max == 0 ? 0 : delta / max;
                double value = max;

                if (delta == 0)
                {
                    return (0, saturation, value);
                }

                hue = max switch
                {
                    var m when m == r => 60 * ((g - b) / delta),
                    var m when m == g => 60 * (2 + (b - r) / delta),
                    _ => 60 * (4 + (r - g) / delta)
                };

                if (hue < 0)
                {
                    hue += 360;
                }

                return (hue, saturation, value);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ColorUtilities.RgbToHsv", 
                    $"Error converting RGB to HSV: {ex.Message}", ex.StackTrace);
                return (0, 0, 0);
            }
        }

        /// <summary>
        /// Creates a Color from HSV (Hue, Saturation, Value) components
        /// </summary>
        public static Color HsvToRgb(double hue, double saturation, double value)
        {
            try
            {
                if (saturation == 0)
                {
                    int gray = (int)(value * 255);
                    return Color.FromArgb(gray, gray, gray);
                }

                hue = hue switch
                {
                    360 => 0,
                    < 0 => 0,
                    > 360 => 360,
                    _ => hue
                };

                hue /= 60;
                int i = (int)Math.Floor(hue);
                double f = hue - i;

                value *= 255;
                int v = (int)value;
                int p = (int)(value * (1 - saturation));
                int q = (int)(value * (1 - f * saturation));
                int t = (int)(value * (1 - (1 - f) * saturation));

                return i switch
                {
                    0 => Color.FromArgb(v, t, p),
                    1 => Color.FromArgb(q, v, p),
                    2 => Color.FromArgb(p, v, t),
                    3 => Color.FromArgb(p, q, v),
                    4 => Color.FromArgb(t, p, v),
                    _ => Color.FromArgb(v, p, q)
                };
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ColorUtilities.HsvToRgb", 
                    $"Error converting HSV to RGB: {ex.Message}", ex.StackTrace);
                return Color.Empty;
            }
        }

        /// <summary>
        /// Adjusts the brightness of a color
        /// </summary>
        public static Color AdjustBrightness(Color color, double factor)
        {
            try
            {
                if (factor < 0 || factor > 1)
                {
                    throw new ArgumentException("Brightness factor must be between 0 and 1", nameof(factor));
                }

                var hsl = new HslColor(color);
                hsl.L *= factor;
                return hsl;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ColorUtilities.AdjustBrightness", 
                    $"Error adjusting brightness: {ex.Message}", ex.StackTrace);
                return color;
            }
        }
    }

    /// <summary>
    /// Represents a color in HSL (Hue, Saturation, Luminance) color space
    /// </summary>
    [Serializable]
    public readonly struct HslColor : IEquatable<HslColor>
    {
        private readonly double _hue;
        private readonly double _saturation;
        private readonly double _luminance;
        private readonly int _alpha;
        private readonly bool _isEmpty;

        public static readonly HslColor Empty = new HslColor { IsEmpty = true };

        public HslColor(double hue, double saturation, double luminance)
            : this(255, hue, saturation, luminance)
        {
        }

        public HslColor(int alpha, double hue, double saturation, double luminance)
        {
            _alpha = Math.Clamp(alpha, 0, 255);
            _hue = Math.Clamp(hue, 0, 359);
            _saturation = Math.Clamp(saturation, 0, 1);
            _luminance = Math.Clamp(luminance, 0, 1);
            _isEmpty = false;
        }

        public HslColor(Color color)
        {
            _alpha = color.A;
            _hue = color.GetHue();
            _saturation = color.GetSaturation();
            _luminance = color.GetBrightness();
            _isEmpty = false;
        }

        public double H => _hue;
        public double S => _saturation;
        public double L => _luminance;
        public int A => _alpha;
        public bool IsEmpty => _isEmpty;

        public Color ToRgbColor() => ToRgbColor(_alpha);

        public Color ToRgbColor(int alpha)
        {
            try
            {
                alpha = Math.Clamp(alpha, 0, 255);

                if (_isEmpty)
                {
                    return Color.Empty;
                }

                double q = _luminance < 0.5 
                    ? _luminance * (1 + _saturation)
                    : _luminance + _saturation - (_luminance * _saturation);
                
                double p = 2 * _luminance - q;
                double hk = _hue / 360.0;

                var tc = new[] { hk + 1.0/3.0, hk, hk - 1.0/3.0 };
                var rgb = new double[3];

                for (int i = 0; i < 3; i++)
                {
                    tc[i] = tc[i] switch
                    {
                        < 0 => tc[i] + 1,
                        > 1 => tc[i] - 1,
                        _ => tc[i]
                    };

                    rgb[i] = tc[i] switch
                    {
                        var t when t < 1.0/6.0 => p + ((q - p) * 6 * t),
                        var t when t < 1.0/2.0 => q,
                        var t when t < 2.0/3.0 => p + ((q - p) * 6 * (2.0/3.0 - t)),
                        _ => p
                    };

                    rgb[i] = Math.Clamp(rgb[i] * 255, 0, 255);
                }

                return Color.FromArgb(alpha, (int)rgb[0], (int)rgb[1], (int)rgb[2]);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HslColor.ToRgbColor", 
                    $"Error converting HSL to RGB: {ex.Message}", ex.StackTrace);
                return Color.Empty;
            }
        }

        public override string ToString()
        {
            return _isEmpty 
                ? "HslColor [Empty]" 
                : $"HslColor [H={_hue:F2}, S={_saturation:F2}, L={_luminance:F2}, A={_alpha}]";
        }

        public override bool Equals(object obj) => 
            obj is HslColor color && Equals(color);

        public bool Equals(HslColor other) =>
            _alpha == other._alpha &&
            Math.Abs(_hue - other._hue) < 0.001 &&
            Math.Abs(_saturation - other._saturation) < 0.001 &&
            Math.Abs(_luminance - other._luminance) < 0.001;

        public override int GetHashCode() =>
            HashCode.Combine(_alpha, _hue, _saturation, _luminance);

        public static bool operator ==(HslColor left, HslColor right) => 
            left.Equals(right);

        public static bool operator !=(HslColor left, HslColor right) => 
            !left.Equals(right);

        public static implicit operator HslColor(Color color) => 
            new HslColor(color);

        public static implicit operator Color(HslColor color) => 
            color.ToRgbColor();
    }
}
