using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CTImageViewer.Core;
using CTImageViewer.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CTImageViewer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point myCurrentMousePosition;
        private bool myIsOperating;
        private ISceneElement mySceneElement;
        private SceneViewModel mySceneVM;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch ((DataContext as MainViewModel).Operation)
            {
                case CmdOperation.Draw:
                    {
                        myCurrentMousePosition = e.GetPosition(sender as Canvas);
                        myIsOperating = true;
                        mySceneElement = mySceneVM.CreateRectangleElement(myCurrentMousePosition);

                        break;
                    }
                case CmdOperation.Move:
                case CmdOperation.WWWL:
                case CmdOperation.Scale:
                    {
                        myCurrentMousePosition = e.GetPosition(sender as Canvas);
                        myIsOperating = true;
                        break;
                    }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            switch ((DataContext as MainViewModel)?.Operation)
            {
                case CmdOperation.Draw:
                    {
                        if (myIsOperating)
                        {
                            var pos = e.GetPosition(sender as Canvas);
                            mySceneVM.Draw(mySceneElement, myCurrentMousePosition, pos);
                        }

                        break;
                    }
                case CmdOperation.Move:
                    {
                        if (myIsOperating)
                        {
                            var pos = e.GetPosition(sender as Canvas);
                            var translation = pos - myCurrentMousePosition;
                            myCurrentMousePosition = pos;
                            mySceneVM.Move(translation);
                        }

                        break;
                    }
                case CmdOperation.WWWL:
                    {
                        if (myIsOperating)
                        {
                            var pos = e.GetPosition(sender as Canvas);
                            var delta = pos - myCurrentMousePosition;
                            myCurrentMousePosition = pos;
                            mySceneVM.AdjustWWWL(delta);
                        }

                        break;
                    }
                case CmdOperation.Scale:
                    {
                        if (myIsOperating)
                        {
                            var pos = e.GetPosition(sender as Canvas);
                            var delta = pos - myCurrentMousePosition;
                            const double scaleIncreament = 0.01;
                            const double scaleTriggerMovement = 10;
                            if (Math.Sqrt(delta.Y * delta.Y + delta.X * delta.X) > scaleTriggerMovement)
                            {
                                myCurrentMousePosition = pos;
                                mySceneVM.Scale(delta.Y > 0 ? -scaleIncreament : scaleIncreament);
                            }
                        }

                        break;
                    }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch ((DataContext as MainViewModel).Operation)
            {
                case CmdOperation.Move:
                case CmdOperation.WWWL:
                case CmdOperation.Draw:
                case CmdOperation.Scale:
                    {
                        myIsOperating = false;
                        break;
                    }
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var delta = 0.01;
            if (e.Delta > 0)
            {
                mySceneVM.SwitchScene(-delta);
            }
            else
            {
                mySceneVM.SwitchScene(delta);
            }
        }

        private void OnContentRendered(object? sender, EventArgs e)
        {
            var mainVM = new MainViewModel(
                                           new SceneViewModel(new SceneManager()),
                                           new SeriesManagerViewModel(new DicomFileManager()));
            mySceneVM = mainVM.SceneVM;
            mySceneVM.ViewPortWidth = DrawingArea.ActualWidth;
            mySceneVM.ViewPortHeight = DrawingArea.ActualHeight;
            DataContext = mainVM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomWindow_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Window win = (Window)((FrameworkElement)sender).TemplatedParent;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CustomWindowBtnClose_Click(object sender, RoutedEventArgs e)
        {
            Window win = (Window)((FrameworkElement)sender).TemplatedParent;
            win.Close();
        }

    }
}