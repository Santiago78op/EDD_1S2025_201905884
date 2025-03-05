using Gtk;

namespace AutoGestPro.UI.Components;

public class MenuButton: Button
{
    public MenuButton(string texto, EventHandler evento) : base(texto)
    {
        ModifyBg(StateType.Normal, new Gdk.Color(0, 255, 0)); // Color verde
        Clicked += evento;
    }
}