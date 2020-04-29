using Algorithms;
using Algorithms.HillClimbing;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlgorithmVisualization.HillClimbing
{
    public class HillClimbingDrawingScreen : FrameworkElement
    {
        private HillClimbingViewModel _viewModel;

        private Polygon _boundaryPolygon;

        public HillClimbingDrawingScreen()
        {
            _viewModel = (HillClimbingViewModel)App.Current.Resources["HillClimbingViewModel"];
            _viewModel.UpdateUI += ViewModel_UpdateUI;
            _viewModel.Loaded += ViewModel_Loaded;

            _boundaryPolygon = new Polygon(_viewModel.BoundaryPolygon);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen redPen = new Pen(Brushes.Red, 2);
            Pen greenPen = new Pen(Brushes.Green, 2);
            Pen whiteSmokePen = new Pen(Brushes.WhiteSmoke, 2);

            Rect background = new Rect(0, 0, 1000, 700);
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, whiteSmokePen, background);

            foreach (Point point in _viewModel.PointsToEnclose)
            {
                drawingContext.DrawEllipse(Brushes.Red, redPen, point, 1, 1);
            }

            foreach (Point point in _boundaryPolygon.Vertices)
            {
                drawingContext.DrawEllipse(Brushes.Green, greenPen, point, 2, 2);
            }

            foreach (Side side in _boundaryPolygon.Sides)
            {
                drawingContext.DrawLine(greenPen, side.Start, side.End);
            }
        }

        private void ViewModel_Loaded(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => {
                InvalidateVisual();
            });
        }

        private void ViewModel_UpdateUI(object sender, EventArgs e)
        {
            _boundaryPolygon = (e as HillClimbingEventArgs).BoundaryPolygon;
            App.Current.Dispatcher.Invoke(() => {
                InvalidateVisual();
            });
        }
    }
}
