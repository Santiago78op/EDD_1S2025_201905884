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
    private static TreeBinary _serviceService;
    private static TreeB _facturaService;
    
    static ReporteService()
    {
        _clientService = Estructuras.Clientes;
        _vehicleService = Estructuras.Vehiculos;
        _repuestoService = Estructuras.Repuestos;
        _serviceService = Estructuras.Servicios;
        _facturaService = Estructuras.Facturas;
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
        dot.AppendLine("node [shape=record, style=filled, fillcolor=lightblue];");
        
        GenerarDotAvl(_repuestoService.Root, dot);
        
        dot.AppendLine("}");
        return dot.ToString();
    }

    private static void GenerarDotAvl(NodeTreeAvl nodo, StringBuilder dot)
    {
        if (nodo != null)
        {
            Repuesto r = (Repuesto)nodo.Value;
            dot.AppendLine($"R{r.Id} [label=\"{{<izq> | ID: {r.Id} | Nombre: {r.Repuesto1} | Detalles: {r.Detalles} |Costo: {r.Costo} | <der>}}\"]");
            if (nodo.Left != null)
            {
                Repuesto rLeft = (Repuesto)nodo.Left.Value;
                dot.AppendLine($"R{r.Id} -> R{rLeft.Id}");
                GenerarDotAvl(nodo.Left, dot);
            }
            if (nodo.Right != null)
            {
                Repuesto rRight = (Repuesto)nodo.Right.Value;
                dot.AppendLine($"R{r.Id} -> R{rRight.Id}");
                GenerarDotAvl(nodo.Right, dot);
            }
        }
    }
    
    // ✅ Reporte de Servicios
    public static string GenerarDotServicios()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph Servicios {");
        dot.AppendLine("node [shape=ellipse, style=filled, fillcolor=lightblue];");
        
        GenararDotBinaryTree(_serviceService.Root, dot);
        
        dot.AppendLine("}");
        return dot.ToString();
    }
    
    // Generar el archivo .dot para Graphviz
    private static void GenararDotBinaryTree(NodeTreeBinary nodo, StringBuilder dot)
    {
        if (nodo != null)
        {
            Servicio s = (Servicio)nodo.Value;
            dot.AppendLine($"S{s.Id} [label=\"ID: {s.Id}\\nRepuesto: {s.IdRepuesto} | Vehículo: {s.IdVehiculo}\\n {s.Detalles}\\nCosto: Q{s.Costo} \"]");
            if (nodo.Left != null)
            {
                Servicio sLeft = (Servicio)nodo.Left.Value;
                dot.AppendLine($"S{s.Id} -> S{sLeft.Id}");
                GenararDotBinaryTree(nodo.Left, dot);
            }
            if (nodo.Right != null)
            {
                Servicio sRight = (Servicio)nodo.Right.Value;
                dot.AppendLine($"S{s.Id} -> S{sRight.Id}");
                GenararDotBinaryTree(nodo.Right, dot);
            }
        }
    }
    
    // ✅ Reporte de Facturaspublic static string GenerarDotFacturas()
    public static string GenerarDotFacturas()
    {
        StringBuilder dot = new StringBuilder();
        dot.AppendLine("digraph BTree {");
        dot.AppendLine("node [shape=record, style=filled, fillcolor=lightblue];");
        GenerateGraphviz(_facturaService.Root, dot);
        dot.AppendLine("}");
        // llamar a la clase Print de TreeB
        _facturaService.Print();
        return dot.ToString();
    }
    
    private static void GenerateGraphviz(NodeTreeB node, StringBuilder dot)
    {
        if (node == null)
            return;
    
        string nodeLabel = $"\"<f0> |";
        for (int i = 0; i < node.Count; i++)
        {
            Factura factura = (Factura)node.Values[i];
            nodeLabel += $" {{ ID: {factura.Id} | ID_Orden: {factura.IdServicio} | Total: {factura.Total} }} | <f{i + 1}> |";
        }
        nodeLabel += "\"";
        dot.AppendLine($"N{node.GetHashCode()} [label={nodeLabel}];");
    
        for (int i = 0; i <= node.Count; i++)
        {
            if (node.Children[i] != null)
            {
                dot.AppendLine($"N{node.GetHashCode()} -> N{node.Children[i].GetHashCode()};");
                GenerateGraphviz(node.Children[i], dot);
            }
        }
    }
    
}