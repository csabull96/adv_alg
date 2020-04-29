using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlgorithmVisualization.FunctionApproximation
{
    class FunctionDrawingScreen : FrameworkElement
    {
        private FunctionApproximationViewModel _viewModel;

        public FunctionDrawingScreen()
        {
            _viewModel = (FunctionApproximationViewModel)App.Current.Resources["FunctionApproximationViewModel"];
            _viewModel.UpdateUI += _viewModel_UpdateUI;

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // background and the gray axises
            Pen whiteSmokePen = new Pen(Brushes.WhiteSmoke, 1);
            Pen grayPen = new Pen(Brushes.LightGray, 1);
            Rect background = new Rect(0, 0, 700, 700);
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, whiteSmokePen, background);
            drawingContext.DrawLine(grayPen, new Point(0, 350), new Point(700, 350));
            drawingContext.DrawLine(grayPen, new Point(350, 0), new Point(350, 700));





            Pen redPen = new Pen(Brushes.Red, 2);
            Pen greenPen = new Pen(Brushes.Green, 2);
            Pen yellowPen = new Pen(Brushes.Yellow, 2);


            for (int i = 0; i < 700; i++)
            {
                double y = ConvertY(_viewModel.TheFunction(ConvertX(i)));
               
                double ay = ConvertY(_viewModel.ApproxFunction(ConvertX(i)));
                if (0 <= y && y <= 700)
                {
                    Point p = new Point(i, y);
                    drawingContext.DrawEllipse(Brushes.Green, null, p, 1, 1);



                    double nextY = ConvertY(_viewModel.TheFunction(ConvertX(i + 1)));
                    Point next = new Point(i + 1, nextY);
                    drawingContext.DrawLine(greenPen, p, next);
                }
                if (0 <= ay && ay <= 700)
                {
                    Point p = new Point(i, ay);
                    drawingContext.DrawEllipse(Brushes.Yellow, null, p, 1, 1);


                    double nextY = ConvertY(_viewModel.ApproxFunction(ConvertX(i + 1)));
                    Point next = new Point(i + 1, nextY);
                    drawingContext.DrawLine(yellowPen, p, next);
                }
                
            }
            foreach (Point point in _viewModel.Solutions)
            {
                drawingContext.DrawEllipse(Brushes.Red, redPen, new Point(MapX(point.X), MapY(point.Y)), 2, 2);
            }

        
        }

        private double MapX(double x)
        {
            // from framework element to "real" coordinates as user can see on the screen
            // -35 -> 0
            // 0 -> 350
            // 35 -> 700

            return x * 10 + 350;
        }

        private double MapY(double y)
        {
            // from framework element to "real" coordinates as user can see on the screen
            // -35 -> 700
            // 0 -> 350
            // 35 -> 0

            return y * -10 + 350;
        }

        private double ConvertX(double x)
        {
            // from framework element to "real" coordinates as user can see on the screen
            // 0 -> -35
            // 350 -> 0
            // 700 -> 35

            return (x - 350)/10;
        }

        private double ConvertY(double y)
        {
            //the value that comes back on the screen should be this big:
            // -35 -> 700
            // 0 -> 350
            // 35 -> 0

            return (y * -10) + 350;
        }

        private void _viewModel_UpdateUI(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => {
                InvalidateVisual();
            });
        }
    }
}
