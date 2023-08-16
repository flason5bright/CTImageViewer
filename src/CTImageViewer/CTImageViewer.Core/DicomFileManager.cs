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
            foreach ( var filePath in files )
            {
                var isDicomFile = DicomFile.HasValidHeader( filePath );
                if( isDicomFile )
                {
                    var file = DicomFile.Open( filePath );
                    if (!file.Dataset.Contains(DicomTag.PixelData))
                    {
                        continue;
                    }
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
                        Source = BitmapSource.Create( dicomImage.Width, dicomImage.Height, 96, 96, PixelFormats.Bgra32, null, imageBytes, (PixelFormats.Bgra32.BitsPerPixel * dicomImage.Width + 7) / 8 ),
                        Id = dicomFiles.Key
                } );
            }

            return seriesList;
        }

    }
}