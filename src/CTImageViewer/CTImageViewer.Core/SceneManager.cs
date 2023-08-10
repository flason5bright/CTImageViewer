using System.Collections.Generic;
using System.Windows;

using FellowOakDicom;

namespace CTImageViewer.Core
{
    public class SceneManager : ISceneManager
    {
        private Scene myCurrentScene;
        private int myDicomFileIndex;
        private IReadOnlyList<DicomFile> myDicomFiles;
        private Dictionary<string, Scene> myScenes;

        public SceneManager()
        {
            ScaleFactor = 1.0;
            myDicomFileIndex = 0;
            Translation = new Vector( 0.0, 0.0 );
            myScenes = new Dictionary<string, Scene>();
        }

        public Vector WWWL { get; private set; }

        public Vector Translation { get; set; }

        public double ScaleFactor { get; set; }

        public double ViewPortHeight { get; set; }

        public double ViewPortWidth { get; set; }

        public Scene LoadSeries( Series series )
        {
            ScaleFactor = 1.0;
            myDicomFileIndex = 0;
            Translation = new Vector( 0.0, 0.0 );
            myScenes = new Dictionary<string, Scene>();
            myDicomFiles = series.DicomFiles;
            WWWL = new Vector( double.Parse( myDicomFiles[ myDicomFileIndex ].Dataset.GetString( DicomTag.WindowWidth ).Split( "\\" )[ 0 ].Trim() ),
                               double.Parse( myDicomFiles[ myDicomFileIndex ].Dataset.GetString( DicomTag.WindowCenter ).Split( "\\" )[ 0 ].Trim() ) );
            myCurrentScene = GetScene( myDicomFileIndex );
            return myCurrentScene;
        }

        public void Draw( ISceneElement sceneElement, Point startPoint, Point endPoint )
        {
            sceneElement.UpdateLocalPosition( ConvertToLocal( startPoint ), ConvertToLocal( endPoint ) );
            sceneElement.UpdateUIPosition( ScaleFactor, Translation );
        }

        public void AdjustWWWL( Vector delta )
        {
            WWWL += delta;
            var imageElement = myCurrentScene.GetImageElement();
            imageElement.AdjustWWWL( WWWL );
        }

        public Scene SwitchScene( double direction )
        {
            if( direction >= 0 )
            {
                myDicomFileIndex++;
            }
            else
            {
                myDicomFileIndex--;
            }

            if( myDicomFileIndex >= myDicomFiles.Count )
            {
                myDicomFileIndex = myDicomFiles.Count - 1;
            }

            if( myDicomFileIndex < 0 )
            {
                myDicomFileIndex = 0;
            }

            myCurrentScene = GetScene( myDicomFileIndex );
            var imageElement = myCurrentScene.GetImageElement();
            imageElement.AdjustWWWL( WWWL );
            foreach( var sceneElement in myCurrentScene.SceneElements )
            {
                sceneElement.UpdateUIPosition( ScaleFactor, Translation );
            }

            return myCurrentScene;
        }

        private Point ConvertToLocal( Point point )
        {
            var centerMovement = new Vector( 0.5 * ViewPortWidth, 0.5 * ViewPortHeight );
            return (point - Translation + centerMovement * (ScaleFactor - 1)).Multiply( 1 / ScaleFactor );
        }

        private Scene GetScene( int dicomFileIndex )
        {
            var dicomFile = myDicomFiles[ dicomFileIndex ];
            var sopInstanceUID = dicomFile.Dataset.GetString( DicomTag.SOPInstanceUID );
            if( !myScenes.ContainsKey( sopInstanceUID ) )
            {
                myScenes[ sopInstanceUID ] = new Scene( dicomFile, ViewPortWidth, ViewPortHeight );
                myScenes[ sopInstanceUID ].GetImageElement().UpdateUIPosition( ScaleFactor, Translation );
            }

            return myScenes[ sopInstanceUID ];
        }
    }
}