using Algorithms;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AlgorithmVisualization.Genetic
{
    public class GeneticDrawingScreen : FrameworkElement
    {
        private GeneticViewModel _viewModel;

        public GeneticDrawingScreen()
        {
            _viewModel = (GeneticViewModel)App.Current.Resources["ViewModel"];
            _viewModel.UpdateUI += ViewModel_UpdateUI;
            _viewModel.Loaded += ViewModel_Loaded;
        }

        private void ViewModel_Loaded(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => {
                InvalidateVisual();
            });
        }

        private void ViewModel_UpdateUI(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => { 
                InvalidateVisual();
            });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen p = new Pen(Brushes.Red, 4);

            if (_viewModel.Started)
            {
                int[] copyOfBestGenes = new int[_viewModel.ShortestRouteFound.Genes.Length];
                for (int i = 0; i < copyOfBestGenes.Length; i++)
                {
                    copyOfBestGenes[i] = _viewModel.ShortestRouteFound.Genes[i];
                }

                for (int i = 0; i < copyOfBestGenes.Length; i++)
                {
                    int index = copyOfBestGenes[i];
                    int index2 = copyOfBestGenes[(i + 1) % copyOfBestGenes.Length];

                    City current = _viewModel.Cities.ElementAt(index);
                    City next = _viewModel.Cities.ElementAt(index2);

                    drawingContext.DrawEllipse(Brushes.Red, p, current.Location, 1, 1);
                    drawingContext.DrawLine(new Pen(Brushes.Green, 2), current.Location, next.Location);
                }
            }
            else
            {
                foreach (City city in _viewModel.Cities)
                {
                    drawingContext.DrawEllipse(Brushes.Red, p, city.Location, 1, 1);
                }
            }
        }
    }
}
