using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using FellowOakDicom;
using FellowOakDicom.Imaging;

namespace CTImageViewer.Core
{
    public class DicomFileManager : IDicomFileManager
    {
        public IReadOnlyList<Series> LoadFrom( string folder )
        {
            var files = Directory.GetFiles( folder );
            Dictionary<string, List<DicomFile>> dicomFilesBySeriesInstanceUID = new();
            foreach( var filePath in files )
            {
                var isDicomFile = DicomFile.HasValidHeader( filePath );
                if( isDicomFile )
                {
                    var file = DicomFile.Open( filePath );
                    var seriesInstanceUID = file.Dataset.GetString( DicomTag.SeriesInstanceUID );
                    if( dicomFilesBySeriesInstanceUID.ContainsKey( seriesInstanceUID ) is false )
                    {
                        dicomFilesBySeriesInstanceUID[ seriesInstanceUID ] = new List<DicomFile>();
                    }

                    dicomFilesBySeriesInstanceUID[ seriesInstanceUID ].Add( file );
                }
            }

            var seriesList = new List<Series>();

            foreach( var dicomFiles in dicomFilesBySeriesInstanceUID )
            {
                var orderedDicomFiles = dicomFiles.Value.OrderBy( dicomFile => int.Parse( dicomFile.Dataset.GetString( DicomTag.InstanceNumber ) ) ).ToList();
                var middleDicomFileIndex = orderedDicomFiles.Count / 2;
                var dicomImage = new DicomImage( orderedDicomFiles[ middleDicomFileIndex ].File.Name );
                var imageBytes = dicomImage.RenderImage().AsBytes();
                seriesList.Add( new Series
                {
                        DicomFiles = orderedDicomFiles,
                        Width = dicomImage.Width,
                        Height = dicomImage.Height,
                        Thumbnail = imageBytes,
                        BitsPerPixel = 32,
                        Source = BitmapSource.Create( dicomImage.Width, dicomImage.Height, 96, 96, PixelFormats.Bgra32, null, imageBytes, (PixelFormats.Bgra32.BitsPerPixel * 512 + 7) / 8 )
                } );
            }

            return seriesList;
        }

        public byte[] ReadInGroup()
        {
            var files = Directory.GetFiles( @"D:\SimulatorImageData\Fluoro" );
            Dictionary<string, List<DicomFile>> dicomFilesBySeriesInstanceUID = new();
            foreach( var filePath in files )
            {
                var isDicomFile = DicomFile.HasValidHeader( filePath );
                if( isDicomFile )
                {
                    var file = DicomFile.Open( filePath );
                    var seriesInstanceUID = file.Dataset.GetString( DicomTag.SeriesInstanceUID );
                    if( dicomFilesBySeriesInstanceUID.ContainsKey( seriesInstanceUID ) is false )
                    {
                        dicomFilesBySeriesInstanceUID[ seriesInstanceUID ] = new List<DicomFile>();
                    }

                    dicomFilesBySeriesInstanceUID[ seriesInstanceUID ].Add( file );
                }
            }

            var middleDicomPath = dicomFilesBySeriesInstanceUID.First().Value.First().File.Name;
            var dicomImage = new DicomImage( middleDicomPath );
            var inMemoryImage = dicomImage.RenderImage().AsBytes();
            dicomImage.WindowCenter = 200;
            dicomImage.WindowWidth = 1200;
            var inMemoryImageChangeWW = dicomImage.RenderImage().AsBytes();
            var countOfPixels = dicomImage.Width * dicomImage.Height;
            var minPixel = int.MaxValue;
            var maxPixel = int.MinValue;
            for( var pixelsIndex = 0; pixelsIndex < countOfPixels; pixelsIndex++ )
            {
                var pixel = inMemoryImage[ pixelsIndex ];
                if( pixel < minPixel )
                {
                    minPixel = pixel;
                }

                if( pixel > maxPixel )
                {
                    maxPixel = pixel;
                }
            }

            return inMemoryImageChangeWW;
        }
    }
}