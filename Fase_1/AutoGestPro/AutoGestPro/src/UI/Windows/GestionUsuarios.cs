using AutoGestPro.Core.Models;
using AutoGestPro.Core.Services;
using AutoGestPro.Core.Structures;
using Gtk;

namespace AutoGestPro.UI.Windows;

public  unsafe class GestionUsuarios : Window
{
    private TreeView treeViewUsuarios;
    private ListStore listStore;
    private Entry entryId, entryNombre, entryApellido, entryCorreo, entryContrasenia;
    
    public GestionUsuarios() : base("Gestión de Usuarios")
    {
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Hide(); // Oculta en vez de cerrar
        
        VBox vbox = new VBox(false, 5);
        
        // Crear tabla de usuarios
        treeViewUsuarios = new TreeView();
        CrearColumnasUsuarios();
        RefrescarUsuarios();
        
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(treeViewUsuarios);
        vbox.PackStart(scrolledWindow, true, true, 5);
        
        // Sección de edición
        HBox hboxEditar = new HBox(false, 5);
        entryId = new Entry { PlaceholderText = "ID" };
        entryNombre = new Entry { PlaceholderText = "Nombres" };
        entryApellido = new Entry { PlaceholderText = "Apellidos" };
        entryCorreo = new Entry { PlaceholderText = "Correo" };
        entryContrasenia = new Entry { PlaceholderText = "Contraseña" };
        hboxEditar.PackStart(entryId, false, false, 5);
        hboxEditar.PackStart(entryNombre, false, false, 5);
        hboxEditar.PackStart(entryApellido, false, false, 5);
        hboxEditar.PackStart(entryCorreo, false, false, 5);
        hboxEditar.PackStart(entryContrasenia, false, false, 5);
        vbox.PackStart(hboxEditar, false, false, 5);
        
        // Botones
        HBox hboxBotones = new HBox(false, 5);
        Button btnVer = new Button("Ver Usuario");
        Button btnEditar = new Button("Editar Usuario");
        Button btnEliminar = new Button("Eliminar Usuario");
        
        btnVer.Clicked += OnVerUsuarioClicked;
        btnEditar.Clicked += OnEditarUsuarioClicked;
        btnEliminar.Clicked += OnEliminarUsuarioClicked;
        
        hboxBotones.PackStart(btnVer, true, true, 5);
        hboxBotones.PackStart(btnEditar, true, true, 5);
        hboxBotones.PackStart(btnEliminar, true, true, 5);
        vbox.PackStart(hboxBotones, false, false, 5);
        
        Add(vbox);
        ShowAll();
        
        // Evento para auto rellenar los campos
        treeViewUsuarios.RowActivated += OnTreeViewUsuariosRowActivated;
    }
    
    // Evento para auto rellenar los campos con los datos del usuario seleccionado
    private void OnTreeViewUsuariosRowActivated(object sender, RowActivatedArgs args)
    {
        TreeIter iter;
        if (treeViewUsuarios.Model.GetIter(out iter, args.Path))
        {
            entryId.Text = treeViewUsuarios.Model.GetValue(iter, 0).ToString();
            entryNombre.Text = treeViewUsuarios.Model.GetValue(iter, 1).ToString();
            entryApellido.Text = treeViewUsuarios.Model.GetValue(iter, 2).ToString();
            entryCorreo.Text = treeViewUsuarios.Model.GetValue(iter, 3).ToString();
            entryContrasenia.Text = treeViewUsuarios.Model.GetValue(iter, 4).ToString();
        }
    }
    
    // ✅ Crea las columnas para la tabla de usuarios
    private void CrearColumnasUsuarios()
    {
        listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string));
        treeViewUsuarios.Model = listStore;
        
        treeViewUsuarios.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeViewUsuarios.AppendColumn("Nombres", new CellRendererText(), "text", 1);
        treeViewUsuarios.AppendColumn("Apellidos", new CellRendererText(), "text", 2);
        treeViewUsuarios.AppendColumn("Correo", new CellRendererText(), "text", 3);
        treeViewUsuarios.AppendColumn("Contraseña", new CellRendererText(), "text", 4);
    }
    
    // ✅ Refresca la lista de usuarios
    private void RefrescarUsuarios()
    {
        listStore.Clear();
        for (int i = 0; i < CargaMasivaService.clientes.Length; i++)
        {
            if (CargaMasivaService.clientes.GetNode(i)->_data is Cliente cliente)
            {
                listStore.AppendValues(cliente.Id, cliente.Nombre, cliente.Apellido, cliente.Correo, cliente.Contrasenia);
            }
            else
            {
                throw new Exception("Error al mostrar datos en el TreeView");
            }
        }
    }
    
    // ✅ Ver Usuario
    private void OnVerUsuarioClicked(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(entryId.Text, out id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }

        Cliente cliente = CargaMasivaService.clientes.SearchNode(id)->_data;
        if (cliente != null)
        {
            entryNombre.Text = cliente.Nombre;
            entryApellido.Text = cliente.Apellido;
            entryCorreo.Text = cliente.Correo;

            string mensaje = $"Usuario: {cliente.Nombre} {cliente.Apellido}\nCorreo: {cliente.Correo}\nVehículos:\n";
            
            for (int i = 0; i < CargaMasivaService.vehiculos.Length; i++)
            {
                if (CargaMasivaService.vehiculos.GetNode(i)->_data is Vehiculo vehiculo)
                {
                    if ( vehiculo.Id_Usuario == id)
                    {
                        mensaje += $"- {vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Placa})\n";
                    }
                }
                else
                {
                    mensaje += $"El usuario no tiene vehículos.";
                    throw new Exception("El usuario no tiene vehículos.");
                }
            }

            MostrarMensaje("Información del Usuario", mensaje);
        }
        else
        {
            MostrarMensaje("Error", "Usuario no encontrado.");
        }
    }

    // ✅ Editar Usuario
    private void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(entryId.Text, out id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }
            
        // Auto rellenar los campos con los datos del usuario seleccionado
        
        
        
        Cliente cliente = new Cliente(id, entryNombre.Text, entryApellido.Text, entryCorreo.Text, entryContrasenia.Text);
        
        if (CargaMasivaService.clientes.ModifyNode(id ,cliente))
        {
            MostrarMensaje("Éxito", "Usuario actualizado correctamente.");
            RefrescarUsuarios();
        }
        else
        {
            MostrarMensaje("Error", "Usuario no encontrado.");
        }
    }

    // ✅ Eliminar Usuario
    private void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(entryId.Text, out id))
        {
            MostrarMensaje("Error", "Ingrese un ID válido.");
            return;
        }

        if (CargaMasivaService.clientes.DeleteNode(id))
        {
            MostrarMensaje("Éxito", "Usuario eliminado correctamente.");
            RefrescarUsuarios();
        }
        else
        {
            MostrarMensaje("Error", "Usuario no encontrado.");
        }
    }

    // ✅ Muestra un mensaje
    private void MostrarMensaje(string titulo, string mensaje)
    {
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
        dialog.Title = titulo;
        dialog.Run();
        dialog.Destroy();
    }
}