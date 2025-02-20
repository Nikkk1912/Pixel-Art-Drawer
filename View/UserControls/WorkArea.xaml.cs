using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PixelArt.Model;

namespace PixelArt.View.UserControls;

public partial class WorkArea : UserControl
{
    private int _columns = 10;
    private int _rows = 10;
    private double _pixelSize = 20; 
    private Canvas _pixelCanvas;
    private Grid _containerGrid;
    private PixelSheet _pixelSheet;

    public WorkArea()
    {
        InitializeComponent();

        _pixelSheet = new PixelSheet(_rows, _columns);
        
        CreateUi();
        UpdatePixelSize();
    }

    private void CreateUi()
    {
        if (WorkAreaGrid != null && PixelCanvas != null)
        {
            _containerGrid = WorkAreaGrid;
            
            _pixelCanvas = PixelCanvas;

            
            Content = _containerGrid;
            
            SizeChanged += WorkArea_SizeChanged; 
            MouseWheel += WorkArea_MouseWheel;
        }
    }

    private void WorkArea_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdatePixelSize();
        GeneratePixelCanvas();
    }

    private void UpdatePixelSize()
    {
        
        double width = WorkAreaGrid.ActualWidth;
        double height = WorkAreaGrid.ActualHeight;
        
        Console.WriteLine("Width - " + width);
        Console.WriteLine("Height - " + height);
        
        if (_containerGrid is { ActualWidth: > 0, ActualHeight: > 0 })
        {
            _pixelSize = Math.Min(_containerGrid.ActualWidth / _columns, _containerGrid.ActualHeight / _rows);
            Console.WriteLine("Pixel size - " + _pixelSize);
        }
    }

    private void GeneratePixelCanvas()
    {
        _pixelCanvas.Children.Clear();

        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                Rectangle rect = new Rectangle
                {
                    Width = _pixelSize,
                    Height = _pixelSize,
                    Stroke = Brushes.Black,
                    Fill = _pixelSheet.GetPixel(y, x).Color
                };

                double positionX = ((WorkAreaGrid.ActualWidth / 2) - (_pixelSize * (_columns / 2))) + (x * _pixelSize);
                double positionY = ((WorkAreaGrid.ActualHeight / 2) - (_pixelSize * (_rows / 2))) + (y * _pixelSize);

                Canvas.SetLeft(rect,  positionX);
                Canvas.SetTop(rect, positionY);
                
                rect.MouseDown += (s, e) =>
                {
                    Point mousePosition = e.GetPosition(_pixelCanvas);
                    
                    int clickedColumn = (int)((mousePosition.X - ((WorkAreaGrid.ActualWidth / 2) - (_pixelSize * (_columns / 2)))) / _pixelSize);
                    int clickedRow = (int)((mousePosition.Y - ((WorkAreaGrid.ActualHeight / 2) - (_pixelSize * (_rows / 2)))) / _pixelSize);

                    Console.WriteLine("Clicked at X: " + mousePosition.X + ", Y: " + mousePosition.Y);
                    Console.WriteLine("Row: " + clickedRow + ", Column: " + clickedColumn);
                    
                    rect.Fill = (rect.Fill == Brushes.Transparent) ? Brushes.Black : Brushes.Transparent;
                    _pixelSheet.SetPixelColor(clickedRow, clickedColumn, rect.Fill);
                };

                _pixelCanvas.Children.Add(rect);
            }
        }
    }

    private void WorkArea_MouseWheel(object sender, MouseWheelEventArgs e)
    {
       
        double maxPixelSize = _containerGrid.ActualWidth / _columns;
        
        if (_pixelSize <= maxPixelSize)
        {
            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
            _pixelSize *= zoomFactor;
            
            if (_pixelSize > maxPixelSize)
            {
                _pixelSize = maxPixelSize;
            }
            
            double minPixelSize = 10; 
            if (_pixelSize < minPixelSize)
            {
                _pixelSize = minPixelSize;
            }
        }

        GeneratePixelCanvas(); 
    }
}