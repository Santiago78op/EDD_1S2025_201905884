using System.Text;
using AutoGestPro.Core.Global;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Graphs;

public static class ReporteService
{
    private static LinkedList _clientService;
    private static DoubleList _vehicleService;
    private static TreeAvl _repuestoService;
    
    static ReporteService()
    {
        _clientService = Estructuras.Clientes;
        _vehicleService = Estructuras.Vehiculos;
        _repuestoService = Estructuras.Repuestos;
    }
    
    // ✅ Reporte de Usuarios
    public static string GenerarDotUsuarios()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Usuarios {");
        dot.AppendLine("node [shape=box, style=filled, fillcolor=lightblue];");
        dot.AppendLine("rankdir=LR;");

        NodeLinked? nodeCliente = _clientService.Head;

        while (nodeCliente != null)
        {
            Cliente c = (Cliente)nodeCliente.Data;
            dot.AppendLine(
                $"C{c.Id} [label=\"ID: {c.Id}\\nnombre: {c.Nombres}\\nApellido: {c.Apellidos}\\nEdad: {c.Edad} \\nCorreo: {c.Correo}\"];\n");
            if (nodeCliente.Next != null)
            {
                Cliente cNext = (Cliente)nodeCliente.Next.Data;
                dot.AppendLine($"\"C{c.Id}\" -> \"C{cNext.Id}\";");
            }
            
            nodeCliente = nodeCliente.Next;
        }
        
        dot.AppendLine("}");
        return dot.ToString();
    }
    
    // ✅ Reporte de Vehículos
    public static string GenerarDotVehiculos()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Vehiculos {");
        dot.AppendLine("node [shape=box, style=filled, fillcolor=lightblue];");
        dot.AppendLine("rankdir=LR;");
        
        NodeDouble? nodeVehicle = _vehicleService.Head;
        
        while (nodeVehicle != null)
        {
            Vehiculo v = (Vehiculo)nodeVehicle.Data;
            dot.AppendLine( $"V{v.Id} [label=\"ID: {v.Id}\\nIdUsuario: {v.Id_Usuario}\\nMarca: {v.Marca}\\nModelo: {v.Modelo}\\nPlaca: {v.Placa}\"];\n");
            if (nodeVehicle.Next != null)
            {
                Vehiculo vNext = (Vehiculo)nodeVehicle.Next.Data;
                dot.AppendLine($"\"V{v.Id}\" -> \"V{vNext.Id}\";");
            }
            
            // Apunta al nodo anterior
            if (nodeVehicle.Previous != null)
            {
                Vehiculo vPrev = (Vehiculo)nodeVehicle.Previous.Data;
                dot.AppendLine($"\"V{v.Id}\" -> \"V{vPrev.Id}\";");
            }
            
            nodeVehicle = nodeVehicle.Next;
        }
        
        
        dot.AppendLine("}");
        return dot.ToString();
    }
    
    // ✅ Reporte de Repuestos
    public static string GenerarDotRepuestos()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Repuestos {");
        dot.AppendLine("node [shape=box, style=filled, fillcolor=lightblue];");
        dot.AppendLine("rankdir=LR;");
        
        NodeTreeAvl? nodeRepuesto = _repuestoService.Root;
        
        while (nodeRepuesto != null)
        {
            Repuesto r = (Repuesto)nodeRepuesto.Value;
            dot.AppendLine( $"R{r.Id} [label=\"ID: {r.Id}\\nRepuesto: {r.Repuesto1}\\nDetalles: {r.Detalles}\\nCosto: {r.Costo}\"];\n");
            if (nodeRepuesto.Left != null)
            {
                Repuesto rLeft = (Repuesto)nodeRepuesto.Left.Value;
                dot.AppendLine($"\"R{r.Id}\" -> \"R{rLeft.Id}\";");
            }
            
            if (nodeRepuesto.Right != null)
            {
                Repuesto rRight = (Repuesto)nodeRepuesto.Right.Value;
                dot.AppendLine($"\"R{r.Id}\" -> \"R{rRight.Id}\";");
            }
            
            nodeRepuesto = nodeRepuesto.Left;
        }
        
        dot.AppendLine("}");
        return dot.ToString();
    }
}