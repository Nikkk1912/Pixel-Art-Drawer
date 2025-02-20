using System.Collections;
using System.Windows.Media;

namespace PixelArt.Model;

public class PixelSheet
{
    private Pixel[,] _pixels;

    public PixelSheet(int rows, int cols)
    {
        _pixels = new Pixel[rows, cols];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                _pixels[i, j] = new Pixel(Brushes.Transparent, 0);
            }
        }

        Console.WriteLine("Pixel sheet array size: " + _pixels.GetLength(0) * _pixels.GetLength(1));
    }

    public Pixel GetPixel(int rowNum, int colNum)
    {
        return _pixels[rowNum, colNum];
    }

    public void SetPixelColor(int rowNum, int colNum, Brush color)
    {
        _pixels[rowNum, colNum].Color = color;
        Console.WriteLine($"Pixel at ({rowNum}, {colNum}) has color: {color}");
    }
}