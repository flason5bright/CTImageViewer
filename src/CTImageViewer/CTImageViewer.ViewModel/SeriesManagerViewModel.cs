using System.Collections.Generic;
using System.Collections.ObjectModel;

using CTImageViewer.Core;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CTImageViewer.ViewModel
{
    public class SeriesManagerViewModel : ObservableObject
    {
        private readonly IDicomFileManager myDicomFileManager;

        public SeriesManagerViewModel( IDicomFileManager dicomFileManager )
        {
            myDicomFileManager = dicomFileManager;
        }

        public ObservableCollection<Series> SeriesList { get; set; }

        public IReadOnlyList<Series> LoadSeries( string selectedFolder )
        {
            SeriesList = new ObservableCollection<Series>( myDicomFileManager.LoadFrom( selectedFolder ) );
            OnPropertyChanged( nameof( SeriesList ) );
            return SeriesList;
        }
    }
}