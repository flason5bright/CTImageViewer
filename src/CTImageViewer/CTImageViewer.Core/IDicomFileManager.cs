using System.Collections.Generic;

namespace CTImageViewer.Core
{
    public interface IDicomFileManager
    {
        IReadOnlyList<Series> LoadFrom( string folder );
    }
}