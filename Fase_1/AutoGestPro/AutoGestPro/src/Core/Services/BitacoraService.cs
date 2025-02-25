using AutoGestPro.Core.Structures;
using AutoGestPro.UI.Windows;

namespace AutoGestPro.Core.Services;

public class BitacoraService
{
    public static MatrizDispersa<int> MatrizDispersa = MatrizDispersa = new MatrizDispersa<int>(0);
    
    public void InsertarRelacion(int idVehiculo, int idRepuesto, string detalle)
    {
        
        MatrizDispersa.insert(idVehiculo, idRepuesto,  detalle);
    }
    
    public void mostrarBitacora()
    {
        MatrizDispersa.mostrar();
    }
}