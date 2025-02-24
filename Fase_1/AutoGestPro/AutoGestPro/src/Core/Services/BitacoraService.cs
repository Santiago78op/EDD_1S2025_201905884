using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public class BitacoraService
{
    public static MatrizDispersa<int> matriz = new MatrizDispersa<int>(0);
    
    public void InsertarRelacion(int idVehiculo, int idRepuesto, string detalle)
    {
        
        matriz.insert(idVehiculo, idRepuesto,  detalle);
    }
    
    public void mostrarBitacora()
    {
        matriz.mostrar();
    }
}