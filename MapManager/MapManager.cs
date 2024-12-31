using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace MapManager
{
    public class MapManager
    {
        private UniformGrid _tileContainer;
        private int _rows;
        private int _columns;

        public MapManager(UniformGrid tileContainer, int rows, int columns)
        {
            _tileContainer = tileContainer;
            _rows = rows;
            _columns = columns;
            InitializeMap();
        }

        private void InitializeMap()
        {
            _tileContainer.Rows = _rows;
            _tileContainer.Columns = _columns;

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    var tile = new Button
                    {
                        Content = $"{row},{col}",
                        Background = Brushes.LightGray,
                        BorderBrush = Brushes.Black,
                        Margin = new Thickness(1),
                    };

                    tile.Click += Tile_Click;
                    _tileContainer.Children.Add(tile);
                }
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button tile)
            {
                MessageBox.Show($"Clicked on Tile: {tile.Content}");
            }
        }

        public void UpdateTile(int row, int col, string newContent, Brush newColor)
        {
            int index = row * _columns + col;
            if (index >= 0 && index < _tileContainer.Children.Count)
            {
                if (_tileContainer.Children[index] is Button tile)
                {
                    tile.Content = newContent;
                    tile.Background = newColor;
                }
            }
        }
    }

}
