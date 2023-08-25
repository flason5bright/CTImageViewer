using System.Collections.Generic;
using System.Windows.Media.Imaging;

using FellowOakDicom;

namespace CTImageViewer.Contract
{
    public class Series
    {
        public IReadOnlyList<DicomFile> DicomFiles { get; set; }
        public byte[] Thumbnail { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BitsPerPixel { get; set; }
        public BitmapSource Source { get; set; }
        public string Id { get; set; }
    }
}