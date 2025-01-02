using System.Windows;
using System.Windows.Media.Imaging;
using Engine.ViewModels;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly GameSession _gameSession;
        public MainWindow()
        {
            InitializeComponent();
            _gameSession = new GameSession();
            DataContext = _gameSession;

            var mapBuilder = new MapBuilder(_gameSession);
            MapBuilderPlaceholder.Content = mapBuilder;
        }

        private void OnClick_MoveUp(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveUp();
        }
        private void OnClick_MoveDown(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveDown();
        }
        private void OnClick_MoveLeft(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveLeft();
        }
        private void OnClick_MoveRight(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveRight();
        }

        private void MapBuilder_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}