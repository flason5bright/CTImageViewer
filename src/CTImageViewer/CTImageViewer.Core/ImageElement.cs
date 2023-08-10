using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;

namespace CTImageViewer.Core
{
    public class ImageElement : SceneElement, ISceneElement
    {
        private readonly DicomImage myDicomImage;
        private readonly IPixelData myPixelData;
        private readonly double myRescaleIntercept;
        private readonly double myRescaleSlope;

        public ImageElement( Scene parent, DicomFile dicomFile )
        {
            myParent = parent;
            myDicomImage = new DicomImage( dicomFile.File.Name );
            myRescaleIntercept = double.Parse( dicomFile.Dataset.GetString( DicomTag.RescaleIntercept ) );
            myRescaleSlope = double.Parse( dicomFile.Dataset.GetString( DicomTag.RescaleSlope ) );
            var header = DicomPixelData.Create( dicomFile.Dataset );
            myPixelData = PixelDataFactory.Create( header, 0 );
            Source = CreateBitmapSource();
        }

        public Thickness LeftTop { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public BitmapSource Source { get; private set; }

        public void UpdateLocalPosition( Point startPosition, Point endPosition )
        {
            UpdatePosition( startPosition, endPosition );
        }

        public IReadOnlyList<Point> UpdateUIPosition( double scaleFactor, Vector translation )
        {
            var centerMovement = new Vector( 0.5 * myParent.Width, 0.5 * myParent.Height );
            var startPosition = (myStartPosition - centerMovement).Multiply( scaleFactor ) + centerMovement + translation;
            var endPosition = (myEndPosition - centerMovement).Multiply( scaleFactor ) + centerMovement + translation;
            LeftTop = new Thickness( startPosition.X, startPosition.Y, 0, 0 );
            var delta = endPosition - startPosition;
            Width = delta.X;
            Height = delta.Y;
            OnPropertyChanged( nameof( LeftTop ) );
            OnPropertyChanged( nameof( Width ) );
            OnPropertyChanged( nameof( Height ) );
            return new List<Point>
            {
                    startPosition,
                    endPosition
            };
        }

        public void AdjustWWWL( Vector delta )
        {
            myDicomImage.WindowWidth = delta.X;
            myDicomImage.WindowCenter = delta.Y;
            Source = CreateBitmapSource();
            OnPropertyChanged( nameof( Source ) );
        }

        public double GetHUValue( int row, int column )
        {
            return myPixelData.GetPixel( row, column ) * myRescaleSlope + myRescaleIntercept;
        }

        public Vector GetImageSize()
        {
            return new( myDicomImage.Width, myDicomImage.Height );
        }

        private BitmapSource CreateBitmapSource()
        {
            return BitmapSource.Create( myDicomImage.Width, myDicomImage.Height, 96, 96, PixelFormats.Bgra32, null, myDicomImage.RenderImage().AsBytes(), (PixelFormats.Bgra32.BitsPerPixel * 512 + 7) / 8 );
        }
    }
}