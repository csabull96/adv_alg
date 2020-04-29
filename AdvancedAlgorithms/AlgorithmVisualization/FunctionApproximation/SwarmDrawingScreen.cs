using Algorithms.FunctionApproximation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlgorithmVisualization.FunctionApproximation
{
    class SwarmDrawingScreen : FrameworkElement
    {
        private FunctionApproximationViewModel _viewModel;

        public SwarmDrawingScreen()
        {
            _viewModel = (FunctionApproximationViewModel)App.Current.Resources["FunctionApproximationViewModel"];
            _viewModel.UpdateUI += _viewModel_UpdateUI;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
           
            Pen redPen = new Pen(Brushes.Red, 2);
            Pen greenPen = new Pen(Brushes.Green, 2);

            foreach (Individual individual in _viewModel.ViewModelSwarm.Population)
            {
                drawingContext.DrawEllipse(Brushes.Red, redPen, 
                    new Point(ConvertX(individual.Position.X), ConvertY(individual.Position.Y)),
                    2, 2);
            }

            drawingContext.DrawEllipse(Brushes.Red, greenPen, 
                new Point(ConvertX(_viewModel.ViewModelSwarm.GlobalOpt.X), ConvertY(_viewModel.ViewModelSwarm.GlobalOpt.X)),
                2, 2);
        }

        private double ConvertX(double x)
        {
            // from framework element to "real" coordinates as user can see on the screen
            // -350 -> 0
            // 0 -> 350
            // 350 -> 700

            return x + 350;
        }

        private double ConvertY(double y)
        {
            // -350 -> 700
            // 0 -> 350
            // 350 -> 0 ami bejön 350 két (valós koordináta) az a képernyőn 0

            return -y + 350;
        }

        private void _viewModel_UpdateUI(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => {
                InvalidateVisual();
            });
        }
    }
}
