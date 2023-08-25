using System.Collections.Generic;
using System.Windows;

namespace CTImageViewer.Contract
{
    public interface ISceneElement
    {
        IReadOnlyList<Point> UpdateUIPosition( double scaleFactor, Vector translation );
        void UpdateLocalPosition( Point startPosition, Point endPosition );
    }
}