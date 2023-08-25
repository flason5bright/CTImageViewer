using System.Collections.Generic;

namespace CTImageViewer.Contract
{
    public interface IDicomFileManager
    {
        IReadOnlyList<Series> LoadFrom( string folder );
    }
}