using System.Collections;
using System.Diagnostics;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public unsafe class ReporteService
{
    private Linked_List<Cliente> _clientesServicie;
    private Double_List<Vehiculo> _vehiculoService;
    private RingList<Repuesto> _repuestoService;
    private ListQueue<Servicio> _servicioService;
    private StackList<Factura> _facturaService;

    public ReporteService()
    {
        _clientesServicie = CargaMasivaService.clientes;
        _vehiculoService = CargaMasivaService.vehiculos;
        _repuestoService = CargaMasivaService.repuestos;
        _servicioService = CargaMasivaService.servicios;
        _facturaService = FacturaService.pilaFacturas;
    }

    public bool GenerarReporteGraphviz(string entidad, string rutaSalida)
    {
        string contenidoDot = GenerarDot(entidad);
        if (string.IsNullOrEmpty(contenidoDot)) return false;

        string rutaDot = $"reporte_{entidad}.dot";
        File.WriteAllText(rutaDot, contenidoDot);

        // Ejecutar Graphviz para generar la imagen
        ProcessStartInfo startInfo = new ProcessStartInfo("dot")
        {
            Arguments = $"-Tpng {rutaDot} -o {rutaSalida}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();

        return File.Exists(rutaSalida);
    }

    private string GenerarDot(string entidad)
    {
        string dot = "";

        if (entidad == "Usuarios")
        {
            dot += "digraph G {\nnode [shape=box];\nrankdir=LR;\n";
            if (_clientesServicie.Length == 0) return "";
    
            // Metodo ineficiente por que recorre la lista y otiene el nodo en GetNode
            for (int i = 0; i < _clientesServicie.Length; i++)
            {   
                Cliente cliente = _clientesServicie.GetNode(i)->_data;
                if (cliente != null)
                {
                    dot +=
                        $"C{cliente.Id} [label=\"ID: {cliente.Id}\\nNombre: {cliente.Nombre}\\nApellido: {cliente.Apellido}\\nCorreo: {cliente.Correo}\"];\n";
                    if (i > 0)
                    {
                        dot += $"C{_clientesServicie.GetNode(i - 1)->_data.Id} -> C{cliente.Id};\n";
                    }
                }
            }
        }
        else if (entidad == "Vehículos")
        {
            dot += "digraph G {\nnode [shape=box];\nrankdir=LR;\n";
            
            if (_vehiculoService.Length == 0) return "";
            
            for (int i = 0; i < _vehiculoService.Length; i++)
            {
                Vehiculo vehiculo = _vehiculoService.GetNode(i)->_data;
                if (vehiculo != null)
                {
                    dot +=
                        $"V{vehiculo.Id} [label=\"ID: {vehiculo.Id}\\nIdUsuario: {vehiculo.Id_Usuario}\\nMarca: {vehiculo.Marca}\\nModelo: {vehiculo.Modelo}\\nPlaca: {vehiculo.Placa}\"];\n";
                }
                if (i > 0)
                {
                    Vehiculo vehiculoPrev = _vehiculoService.GetNode(i - 1)->_data;
                    if (vehiculoPrev != null)
                    {
                        dot += $"V{vehiculoPrev.Id} -> V{vehiculo.Id};\n";
                        dot += $"V{vehiculo.Id} -> V{vehiculoPrev.Id};\n";
                    }
                }
            }
        }
        else if (entidad == "Repuestos")
        {
            dot += "digraph G {\nnode [shape=box];\nrankdir=LR;\n";
            
            if (_repuestoService.Length == 0) return "";
            
            for (int i = 0; i < _repuestoService.Length; i++)
            {
                Repuesto repuesto = _repuestoService.GetNode(i)->_data;
                if (repuesto != null)
                {
                    dot +=
                        $"R{repuesto.Id} [label=\"ID: {repuesto.Id}\\nRepuesto: {repuesto.Repuesto1}\\nDetalles: {repuesto.Detalle}\\nCosto: {repuesto.Costo}\"];\n";
                    if (i > 0)
                    {
                        Repuesto repuestoPrev = _repuestoService.GetNode(i - 1)->_data;
                        if (repuestoPrev != null)
                        {
                            dot += $"R{repuestoPrev.Id} -> R{repuesto.Id};\n";
                        }
                    }
                }
            }
            
            // Conectar el último nodo con el primero para emular una lista circular
            if (_repuestoService.Length > 1)
            {
                Repuesto firstRepuesto = _repuestoService.Head->_data;
                Repuesto lastRepuesto = _repuestoService.Tail->_data;
                if (firstRepuesto != null && lastRepuesto != null)
                {
                    dot += $"R{lastRepuesto.Id} -> R{firstRepuesto.Id};\n";
                }
            }
        }
        else if (entidad == "Servicios")
        {
            dot += "digraph G {\nnode [shape=box];\nrankdir=LR;\n";
            
            if (_servicioService.Length == 0) return "";            

            for (int i = 0; i < _servicioService.Length; i++)
            {
                Servicio servicio = _servicioService.GetNode(i)->_data;
                if (servicio != null)
                {
                    dot +=
                        $"S{servicio.Id} [label=\"ID: {servicio.Id}\\nID_Vehículo: {servicio.Id_Vehiculo}\\nID_Repuesto: {servicio.Id_Repuesto}\\nDetalles: {servicio.Detalle}\\nCosto: {servicio.Costo}\"];\n";
                    
                    if(i > 0)
                    {
                        Servicio servicioPrev = _servicioService.GetNode(i - 1)->_data;
                        if (servicioPrev != null)
                        {
                            dot += $"S{servicioPrev.Id} -> S{servicio.Id};\n";
                        }
                    }
                }
            }
        }
        else if (entidad == "Facturación")
        {
            dot += "digraph G {\nnode [shape=box];\nrankdir=TB;\n";
            
            if (_facturaService.Height == 0) return "";

            for (int i = 0; i < _facturaService.Height; i++)
            {
                Factura factura = _facturaService.GetNode(i)->_data;
                if (factura != null)
                {
                    dot +=
                        $"F{factura.Id} [label=\"ID: {factura.Id}\\nID_Orden: {factura.IdOrden}\\nTotal: {factura.Total}\"];\n";
                }

                if (i > 0)
                {
                    Factura facturaPrev = _facturaService.GetNode(i - 1)->_data;
                    if (facturaPrev != null)
                    {
                        dot += $"F{facturaPrev.Id} -> F{factura.Id};\n";
                    }
                }
            }
        }

        dot += "}";
        return dot;
    }

    public IEnumerable GetTopVehiculosConMasServicios(int i)
    {
        throw new NotImplementedException();
    }

    public IEnumerable GetTopVehiculosMasAntiguos(int i)
    {
        throw new NotImplementedException();
    }
}