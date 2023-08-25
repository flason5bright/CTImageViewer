using System.Windows;
using CTImageViewer.Contract;
using CTImageViewer.Core;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CTImageViewer.ViewModel
{
    public class SceneViewModel : ObservableObject
    {
        private readonly ISceneManager mySceneManager;

        public SceneViewModel( ISceneManager sceneManager )
        {
            mySceneManager = sceneManager;
        }

        public Scene SceneModel { get; private set; }

        public string InstanceNumber => SceneModel == null ? "" : $"Instance Number:{SceneModel.InstanceNumber}";
        public string WWWL => SceneModel == null ? "" : $"WW/WL:{(int) mySceneManager.WWWL.X}/{(int) mySceneManager.WWWL.Y}";

        public double ViewPortWidth
        {
            get => mySceneManager.ViewPortWidth;
            set => mySceneManager.ViewPortWidth = value;
        }

        public double ViewPortHeight
        {
            get => mySceneManager.ViewPortHeight;
            set => mySceneManager.ViewPortHeight = value;
        }

        public Scene LoadSeries( Series series )
        {
            SceneModel = mySceneManager.LoadSeries( series );
            OnPropertyChanged( nameof( SceneModel ) );
            OnPropertyChanged( nameof( WWWL ) );
            OnPropertyChanged( nameof( InstanceNumber ) );
            return SceneModel;
        }

        public void Move( Vector translation )
        {
            mySceneManager.Translation += translation;
            foreach( var sceneElement in SceneModel.SceneElements )
            {
                sceneElement.UpdateUIPosition( mySceneManager.ScaleFactor, mySceneManager.Translation );
            }
        }

        public void Draw( ISceneElement sceneElement, Point startPoint, Point endPoint )
        {
            mySceneManager.Draw( sceneElement, startPoint, endPoint );
        }

        public void AdjustWWWL( Vector delta )
        {
            mySceneManager.AdjustWWWL( delta );
            OnPropertyChanged( nameof( WWWL ) );
        }

        public void Scale( double delta )
        {
            mySceneManager.ScaleFactor += delta;
            foreach( var sceneElement in SceneModel.SceneElements )
            {
                sceneElement.UpdateUIPosition( mySceneManager.ScaleFactor, mySceneManager.Translation );
            }
        }

        public void SwitchScene( double direction )
        {
            SceneModel = mySceneManager.SwitchScene( direction );
            OnPropertyChanged( nameof( SceneModel ) );
            OnPropertyChanged( nameof( WWWL ) );
            OnPropertyChanged( nameof( InstanceNumber ) );
        }

        public RectangleElement CreateRectangleElement( Point startPosition )
        {
            return SceneModel.CreateSceneElement<RectangleElement>( startPosition, startPosition, SceneModel );
        }
    }
}