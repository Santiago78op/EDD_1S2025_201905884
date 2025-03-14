using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Global;

public class Estructuras
{
    public static LinkedList Clientes = new LinkedList();
    public static DoubleList Vehiculos = new DoubleList();
    public static TreeAvl Repuestos = new TreeAvl();
    public static TreeBinary Servicios = new TreeBinary();
    public static TreeB Facturas = new TreeB(5);
}