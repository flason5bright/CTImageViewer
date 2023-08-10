using System.Windows;

namespace CTImageViewer.Core
{
    public interface ISceneManager
    {
        double ViewPortHeight { get; set; }
        double ViewPortWidth { get; set; }
        double ScaleFactor { get; set; }
        Vector Translation { get; set; }
        Vector WWWL { get; }

        Scene LoadSeries( Series series );
        void Draw( ISceneElement sceneElement, Point startPoint, Point endPoint );
        void AdjustWWWL( Vector delta );
        Scene SwitchScene( double direction );
    }
}