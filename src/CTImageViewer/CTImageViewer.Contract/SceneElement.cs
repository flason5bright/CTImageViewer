using System.Windows;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CTImageViewer.Contract
{
    public class SceneElement : ObservableObject
    {
        protected Point myEndPosition;
        protected Scene myParent;
        protected Point myStartPosition;

        protected void UpdatePosition( Point startPosition, Point endPosition )
        {
            myStartPosition = startPosition;
            myEndPosition = endPosition;
        }
    }
}