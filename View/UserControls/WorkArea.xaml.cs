using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PixelArt.Model;

namespace PixelArt.View.UserControls;

public partial class WorkArea : UserControl
{
    private int _columns = 5;
    private int _rows = 5;
    
    private double _pixelSize = 40;

    private PixelSheet _pixelSheet;
    private ColorInventory _colorInventory;

    private Canvas _pixelCanvas;
    private Grid _containerGrid;

    private bool _isPainting;
    private Brush _selectedColor;

    public WorkArea()
    {
        InitializeComponent();

        _pixelSheet = new PixelSheet(_rows, _columns);
        _colorInventory = new ColorInventory();

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
        // double width = WorkAreaGrid.ActualWidth;
        // double height = WorkAreaGrid.ActualHeight;
        // Console.WriteLine("Width - " + width);
        // Console.WriteLine("Height - " + height);

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
                
                // double positionX = ((WorkAreaGrid.ActualWidth / 2) - (_pixelSize * (_columns / 2))) + (x * _pixelSize);
                // double positionY = ((WorkAreaGrid.ActualHeight / 2) - (_pixelSize * (_rows / 2))) + (y * _pixelSize);
                
                double positionX = ((WorkAreaGrid.ActualWidth - (_pixelSize * _columns)) / 2) + (x * _pixelSize);
                double positionY = ((WorkAreaGrid.ActualHeight - (_pixelSize * _rows)) / 2) + (y * _pixelSize);


                Canvas.SetLeft(rect, positionX);
                Canvas.SetTop(rect, positionY);
                
                rect.MouseDown += (s, e) =>
                {
                    UpdateSelectedColor(e);
                    _isPainting = true;
                    PaintPixel(s, e); 
                };
                
                rect.MouseMove += (s, e) =>
                {
                    
                    if (_isPainting)
                    {
                        PaintPixel(s, e);
                    }
                };
                
                rect.MouseUp += (s, e) =>
                {
                    _isPainting = false;
                };

                _pixelCanvas.Children.Add(rect);
            }
        }
    }
    
    private void PaintPixel(object sender, MouseEventArgs e)
    {
        Rectangle rect = sender as Rectangle;
        if (rect == null) return;
        
        Point mousePosition = e.GetPosition(_pixelCanvas);
        
        // int clickedColumn = (int)((mousePosition.X - ((WorkAreaGrid.ActualWidth / 2) - (_pixelSize * (_columns / 2)))) / _pixelSize);
        // int clickedRow = (int)((mousePosition.Y - ((WorkAreaGrid.ActualHeight / 2) - (_pixelSize * (_rows / 2)))) / _pixelSize);
        
        int clickedColumn = (int)((mousePosition.X - ((WorkAreaGrid.ActualWidth - (_pixelSize * _columns)) / 2)) / _pixelSize);
        int clickedRow = (int)((mousePosition.Y - ((WorkAreaGrid.ActualHeight - (_pixelSize * _rows)) / 2)) / _pixelSize);


        if (rect.Fill != _selectedColor && clickedRow < _rows && clickedColumn < _columns)
        {
            rect.Fill = _selectedColor;
            _pixelSheet.SetPixelColor(clickedRow, clickedColumn, rect.Fill);
        }
    }
    
    private void UpdateSelectedColor(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            _selectedColor = _colorInventory.Color1; 
        }
        else if (e.ChangedButton == MouseButton.Right)
        {
            _selectedColor = _colorInventory.Color2; 
        }
    }

    private void WorkArea_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        double maxPixelSizeWidth = _containerGrid.ActualWidth / _columns;
        double maxPixelSizeHeight = _containerGrid.ActualHeight / _columns;

        double maxPixelSize = Math.Min(maxPixelSizeHeight, maxPixelSizeWidth);
        double minPixelSize = _columns * 4;

        if (_pixelSize <= maxPixelSizeWidth && _pixelSize <= maxPixelSizeHeight && _pixelSize >= minPixelSize)
        {
            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
            _pixelSize *= zoomFactor;

            if (_pixelSize > maxPixelSize)
            {
                _pixelSize = maxPixelSize;
            }

            
            if (_pixelSize < minPixelSize)
            {
                _pixelSize = minPixelSize;
            }
            Console.WriteLine("Pixel size - " + _pixelSize);
        }

        GeneratePixelCanvas();
    }
    
    
}