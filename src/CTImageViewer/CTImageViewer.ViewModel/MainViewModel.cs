using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using CTImageViewer.Core;
using Microsoft.Toolkit.Mvvm.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using TreeView = System.Windows.Controls.TreeView;

namespace CTImageViewer.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel(SceneViewModel sceneViewModel, SeriesManagerViewModel seriesManagerViewModel)
        {
            SceneVM = sceneViewModel;
            SeriesVM = seriesManagerViewModel;
            OpenCommand = new RelayCommand(LoadSeries);
            DrawRectangleCommand = new RelayCommand(() => Operation = CmdOperation.Draw);
            MoveCommand = new RelayCommand(() => Operation = CmdOperation.Move);
            ScaleCommand = new RelayCommand(() => Operation = CmdOperation.Scale);
            AdjustWWWLCommand = new RelayCommand(() => Operation = CmdOperation.WWWL);
            Operation = CmdOperation.None;
            OpenSeriesCommand = new RelayCommand<Series>(OpenSeries);
        }

        public ICommand OpenCommand { get; }
        public ICommand DrawRectangleCommand { get; }
        public ICommand MoveCommand { get; }
        public ICommand ScaleCommand { get; }
        public ICommand AdjustWWWLCommand { get; }
        public RelayCommand<Series> OpenSeriesCommand { get; }

        public CmdOperation Operation { get; set; }

        public SceneViewModel SceneVM { get; }
        public SeriesManagerViewModel SeriesVM { get; }

        private void LoadSeries()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var seriesList = SeriesVM.LoadSeries(dialog.SelectedPath);
                SceneVM.LoadSeries(seriesList[0]);
            }
        }

        private void OpenSeries(Series series)
        {
            SceneVM.LoadSeries(series);
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