using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.Input;

namespace CTImageViewer.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel( SceneViewModel sceneViewModel, SeriesManagerViewModel seriesManagerViewModel )
        {
            SceneVM = sceneViewModel;
            SeriesVM = seriesManagerViewModel;
            OpenCommand = new RelayCommand( LoadSeries );
            DrawRectangleCommand = new RelayCommand( () => Operation = CmdOperation.Draw );
            MoveCommand = new RelayCommand( () => Operation = CmdOperation.Move );
            ScaleCommand = new RelayCommand( () => Operation = CmdOperation.Scale );
            AdjustWWWLCommand = new RelayCommand( () => Operation = CmdOperation.WWWL );
            Operation = CmdOperation.None;
        }

        public ICommand OpenCommand { get; }
        public ICommand DrawRectangleCommand { get; }
        public ICommand MoveCommand { get; }
        public ICommand ScaleCommand { get; }
        public ICommand AdjustWWWLCommand { get; }

        public CmdOperation Operation { get; set; }

        public SceneViewModel SceneVM { get; }
        public SeriesManagerViewModel SeriesVM { get; }

        private void LoadSeries()
        {
            SceneVM.LoadSeries( SeriesVM.LoadSeries( @"D:\SimulatorImageData\Head" )[ 0 ] );
        }
    }

    public enum CmdOperation
    {
        None,
        Move,
        Draw,
        Scale,
        WWWL
    }
}