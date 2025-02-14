namespace AutoGestPro.UI.Components;
using Gtk;

public class MenuButton : Button
{
    public MenuButton(string texto, EventHandler evento) : base(texto)
    {
        ModifyBg(StateType.Normal, new Gdk.Color(0, 255, 0)); // Color verde
        Clicked += evento;
    }
}