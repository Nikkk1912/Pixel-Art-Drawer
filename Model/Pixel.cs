using System.Windows.Media;

namespace PixelArt.Model;

public class Pixel(Brush color, int layer)
{
    private Brush _color = color;

    public Brush Color { get; set; } = color;

    public int Layer { get; set; } = layer;
}