using System.Collections;
using System.Diagnostics;
using AutoGestPro.Core.Models;
using AutoGestPro.Core.Nodes;
using AutoGestPro.Core.Structures;

namespace AutoGestPro.Core.Services;

public unsafe class ReporteService
{
    private Linked_List<Cliente> _clientesServicie;
    private Double_List<Vehiculo> _vehiculoService;
    private RingList<Repuesto> _repuestoService;
    private ListQueue<Servicio> _servicioService;
    private StackList<Factura> _facturaService;
    private MatrizDispersa<int> _matrizDispersa;

    public ReporteService()
    {
        _clientesServicie = CargaMasivaService.clientes;
        _vehiculoService = CargaMasivaService.vehiculos;
        _repuestoService = CargaMasivaService.repuestos;
        _servicioService = CargaMasivaService.servicios;
        _facturaService = FacturaService.pilaFacturas;
        _matrizDispersa = BitacoraService.MatrizDispersa;
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
                        $"C{cliente.Id} [label=\"ID: {cliente.Id}\\nnombre: {cliente.Nombre}\\nApellido: {cliente.Apellido}\\nCorreo: {cliente.Correo}\"];\n";
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
        } else if (entidad == "Bitacora")
        {
            dot += GenerarDotMatrizDispersa(_matrizDispersa);
        }

        dot += "}";
        return dot;
    }

    public StackList<Servicio> GetTopVehiculosConMasServicios(int i)
    {
        throw new NotImplementedException();
    }

    public Double_List<Vehiculo> GetTopVehiculosMasAntiguos(int i)
    {
        return _vehiculoService.GetTopVehiculosMasAntiguos(i);
    }
    
    private string GenerarDotMatrizDispersa<T>(MatrizDispersa<T> matriz) where T : unmanaged
    {
        // -- lo primero es settear los valores que nos preocupan
        String grafo = "digraph T{ \nnode[shape=box fontname=\"Arial\" fillcolor=\"white\" style=filled ]";
        grafo += "\nroot[label = \"capa: " + matriz.capa + "\", group=1]\n";
        grafo += "label = \"MATRIZ DISPERSA\" \nfontname=\"Arial Black\" \nfontsize=\"15pt\" \n\n";
        
        // --- lo siguiente es escribir los nodos encabezados, empezamos con las filas, los nodos tendran el foramto Fn
        NodoEncabezado<int>* x_fila = matriz.filas.primero;
        while (x_fila != null)
        {
            grafo += $"V{x_fila->id}[label=\"V{x_fila->id}\",fillcolor=\"plum\",group=1];\n";
            x_fila = x_fila->siguiente;
        }
        
        // --- apuntamos los nodos F entre ellos
        x_fila = matriz.filas.primero;
        while (x_fila != null)
        {
            if (x_fila->siguiente != null)
            {
                grafo += $"V{x_fila->id}->V{x_fila->siguiente->id};\n";
                grafo += $"V{x_fila->siguiente->id}->V{x_fila->id};\n";
            }
            x_fila = x_fila->siguiente;
        }
        
        // --- Luego de los nodos encabezados fila, seguimos con las columnas, los nodos tendran el foramto Cn
        NodoEncabezado<int>* y_columna = matriz.columnas.primero;
        while (y_columna != null)
        {
            int group = y_columna->id + 1;
            grafo += $"R{y_columna->id}[label=\"R{y_columna->id}\",fillcolor=\"powderblue\",group={group}];\n";
            y_columna = y_columna->siguiente;
        }
        
        // --- apuntamos los nodos C entre ellos
        int cont = 0;
        y_columna = matriz.columnas.primero;
        while (y_columna != null)
        {
            if (y_columna->siguiente != null)
            {
                grafo += $"R{y_columna->id}->R{y_columna->siguiente->id};\n";
                grafo += $"R{y_columna->siguiente->id}->R{y_columna->id};\n";
            }
            cont++;
            y_columna = y_columna->siguiente;
        }
        
        // --- luego que hemos escrito todos los nodos encabezado, apuntamos el nodo root hacua ellos
        y_columna = matriz.columnas.primero;
        x_fila = matriz.filas.primero;
        grafo += $"root->V{x_fila->id};\n root->R{y_columna->id};\n";
        grafo += "{rank=same;root;";
        cont = 0;
        y_columna = matriz.columnas.primero;
        
        while (y_columna != null)
        {
            grafo += $"R{y_columna->id};";
            cont++;
            y_columna = y_columna->siguiente;
        }
        
        grafo += "}\n";
        
        NodoEncabezado<int>* aux = matriz.filas.primero;
        NodoInterno<int>* aux2 = aux->acceso;
        cont = 0;
        
        while (aux != null)
        {
            cont += 1;
            while (aux2 != null)
            {
                grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}[label=\"{aux2->nombre}\",group=\"{aux2->coordenadaY + 1}\", fillcolor=\"yellow\"];\n";
                aux2 = aux2->derecha;
            }
            
            aux = aux->siguiente;
            if(aux != null)
            {
                aux2 = aux->acceso;
            }
        }
        
        aux = matriz.filas.primero;
        aux2 = aux->acceso;
        cont = 0;

        while (aux != null)
        {
            String rank = $"{{rank = same;V{aux->id};";
            cont = 0;

            while (aux2 != null)
            {
                if ( cont == 0)
                {
                    grafo += $"V{aux->id}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->V{aux->id};\n";
                    cont += 1;
                }

                if (aux2->derecha != null)
                {
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->N{aux2->derecha->coordenadaX}_{aux2->derecha->coordenadaY};\n";
                    grafo += $"N{aux2->derecha->coordenadaX}_{aux2->derecha->coordenadaY}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                }

                rank += $"N{aux2->coordenadaX}_{aux2->coordenadaY};";
                aux2 = aux2->derecha;
            }
            
            aux = aux->siguiente;
            if (aux != null)
            {
                aux2 = aux->acceso;
            }
            
            grafo += rank+"}\n";
        }

        aux = matriz.columnas.primero;
        aux2 = aux->acceso;
        cont = 0;

        while (aux != null)
        {
            cont = 0;
            grafo += "";
            
            while (aux2 != null)
            {
                if (cont == 0)
                {
                    grafo += $"R{aux->id}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->R{aux->id};\n";
                    cont += 1;
                }

                if (aux2->abajo != null)
                {
                    grafo += $"N{aux2->coordenadaX}_{aux2->coordenadaY}->N{aux2->abajo->coordenadaX}_{aux2->abajo->coordenadaY};\n";
                    grafo += $"N{aux2->abajo->coordenadaX}_{aux2->abajo->coordenadaY}->N{aux2->coordenadaX}_{aux2->coordenadaY};\n";
                }

                aux2 = aux2->abajo;
            }
            
            aux = aux->siguiente;
            if (aux != null)
            {
                aux2 = aux->acceso;
            }
        }
        
        return grafo;
    }
}