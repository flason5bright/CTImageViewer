using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using FellowOakDicom;

namespace CTImageViewer.Contract
{
    public class Scene
    {
        public Scene( DicomFile dicomFile, double viewPortWidth, double viewPortHeight )
        {
            SceneElements = new ObservableCollection<ISceneElement>();
            Width = viewPortWidth;
            Height = viewPortHeight;
            var aspectRatio = viewPortHeight / viewPortWidth;
            double width;
            double height;
            var x = 0.0;
            var y = 0.0;
            if( aspectRatio > 1 )
            {
                width = viewPortWidth;
                height = viewPortWidth;
                y = (viewPortHeight - height) * 0.5;
            }
            else
            {
                height = viewPortHeight;
                width = viewPortHeight;
                x = (viewPortWidth - width) * 0.5;
            }

            var startPosition = new Point( x, y );
            InstanceNumber = dicomFile.Dataset.GetString( DicomTag.InstanceNumber );
            CreateSceneElement<ImageElement>( startPosition, startPosition + new Vector( width, height ), this, dicomFile );
        }

        public double Width { get; }
        public double Height { get; }

        public string InstanceNumber { get; }

        public ObservableCollection<ISceneElement> SceneElements { get; }

        public ImageElement GetImageElement()
        {
            return SceneElements.OfType<ImageElement>().First();
        }

        public T CreateSceneElement<T>( Point startPosition, Point endPosition, params object[] parameters ) where T : ISceneElement
        {
            var sceneElement = (T) Activator.CreateInstance( typeof( T ), parameters );
            sceneElement.UpdateLocalPosition( startPosition, endPosition );
            SceneElements.Add( sceneElement );
            return sceneElement;
        }
    }
}