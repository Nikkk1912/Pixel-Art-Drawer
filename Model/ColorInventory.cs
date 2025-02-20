using System.Windows.Media;

namespace PixelArt.Model;

public class ColorInventory
{
    private Brush _color1;
    private Brush _color2;

    public ColorInventory()
    {
        _color1 = Brushes.Black;
        _color2 = Brushes.White;
    }

    public Brush Color1
    {
        get => _color1;
        set => _color1 = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Brush Color2
    {
        get => _color2;
        set => _color2 = value ?? throw new ArgumentNullException(nameof(value));
    }
}