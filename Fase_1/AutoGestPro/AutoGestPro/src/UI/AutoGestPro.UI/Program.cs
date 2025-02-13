using System;
using AutoGestPro.UI.Windows;
using Gtk;

class MainClass
{
    public static void Main(string[] args)
    {
        Application.Init();
        LoginWindow loginWindow = new LoginWindow();
        Application.Run();
    }
}