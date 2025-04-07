namespace AutoGestPro.UI.Extensions;

using Gtk;


/// <summary>
/// Extensiones para widgets de Gtk.
/// </summary>
public static class WidgetExtensions
{
    /// <summary>
    /// Agrega una clase CSS al contexto de estilo del widget.
    /// </summary>
    /// <param name="widget">El widget al que se le agregará la clase CSS.</param>
    /// <param name="cssClass">La clase CSS a agregar.</param>
    public static void AddCssClass(this Widget widget, string cssClass)
    {
        if (widget == null)
        {
            throw new ArgumentNullException(nameof(widget));
        }

        if (string.IsNullOrWhiteSpace(cssClass))
        {
            throw new ArgumentException("CSS class cannot be null or whitespace.", nameof(cssClass));
        }

        widget.StyleContext?.AddClass(cssClass);
    }
}
