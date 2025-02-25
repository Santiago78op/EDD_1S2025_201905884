using AutoGestPro.Core.Structures;
using AutoGestPro.UI.Windows;

namespace AutoGestPro.Core.Services;

public class BitacoraService
{
    public static MatrizDispersa<int> matrizDispersa = new MatrizDispersa<int>(0);
    
    public void InsertarRelacion(int idVehiculo, int idRepuesto, string detalle)
    {
        
        matrizDispersa.insert(idVehiculo, idRepuesto,  detalle);
        mostrarBitacora();
    }
    
    public void mostrarBitacora()
    {
        matrizDispersa.mostrar();
    }
}