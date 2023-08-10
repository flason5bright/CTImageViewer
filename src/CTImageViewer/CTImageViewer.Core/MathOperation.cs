using System.Windows;

namespace CTImageViewer.Core
{
    public static class MathOperation
    {
        public static Point Multiply( this Point point, double scaleFactor )
        {
            return new( point.X * scaleFactor, point.Y * scaleFactor );
        }
    }
}