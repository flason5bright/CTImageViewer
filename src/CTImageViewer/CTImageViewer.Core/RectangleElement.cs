using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CTImageViewer.Core
{
    public class RectangleElement : SceneElement, ISceneElement
    {
        public RectangleElement( Scene parent )
        {
            myParent = parent;
        }

        public PointCollection Points { get; private set; }

        public void UpdateLocalPosition( Point startPosition, Point endPosition )
        {
            UpdatePosition( startPosition, endPosition );
        }

        public IReadOnlyList<Point> UpdateUIPosition( double scaleFactor, Vector translation )
        {
            var centerMovement = new Vector( 0.5 * myParent.Width, 0.5 * myParent.Height );
            var direction = myEndPosition - myStartPosition;
            var p1 = myStartPosition;
            var p2 = myStartPosition + new Vector( direction.X, 0 );
            var p3 = myEndPosition;
            var p4 = myStartPosition + new Vector( 0, direction.Y );
            Points = new PointCollection
            {
                    (p1 - centerMovement).Multiply( scaleFactor ) + centerMovement + translation,
                    (p2 - centerMovement).Multiply( scaleFactor ) + centerMovement + translation,
                    (p3 - centerMovement).Multiply( scaleFactor ) + centerMovement + translation,
                    (p4 - centerMovement).Multiply( scaleFactor ) + centerMovement + translation
            };
            OnPropertyChanged( nameof( Points ) );
            return Points.ToList();
        }
    }
}