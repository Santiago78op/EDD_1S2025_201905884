using Gtk;

namespace AutoGestPro.UI.Components;

public class MenuButton: Button
{
    public MenuButton(string texto, EventHandler evento, String bgColor, String fgColor) : base(texto)
    {
        Name = "btnName";
        var provider = new CssProvider();
        provider.LoadFromData($@"
        #btnName {{
            background-image: none;
            background-color: #{bgColor};
            color: {fgColor};
            border-radius: 10px;
            padding: 5px;
        }}
        ");
        StyleContext.AddProvider(provider, 800);
        Clicked += evento;
    }
}