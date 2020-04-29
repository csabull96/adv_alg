using AlgorithmVisualization.FunctionApproximation;
using AlgorithmVisualization.Genetic;
using AlgorithmVisualization.HillClimbing;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmVisualization
{
    public partial class MainWindow : Window
    {
        private GeneticViewModel _geneticViewModel;
        private HillClimbingViewModel _hillClimbingViewModel;
        private FunctionApproximationViewModel _functionApproximationViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _geneticViewModel = (GeneticViewModel)App.Current.Resources["ViewModel"];
            _hillClimbingViewModel = (HillClimbingViewModel)App.Current.Resources["HillClimbingViewModel"];
            _functionApproximationViewModel = (FunctionApproximationViewModel)App.Current.Resources["FunctionApproximationViewModel"];
        }

        private async void Start(object sender, RoutedEventArgs e)
        {
            if (tab_ga.IsSelected)
            {
                await Task.Run(() => { _geneticViewModel.Solve(); });
            }
            else if (tab_hc.IsSelected)
            {
                await Task.Run(() => { _hillClimbingViewModel.Solve(); });
            }
            else if (tab_ps.IsSelected)
            {
                await Task.Run(() => { _functionApproximationViewModel.Solve(); });
            }
        }
    }
}