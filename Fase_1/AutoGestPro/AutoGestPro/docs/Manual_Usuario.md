# 📖 Manual de Usuario

## 📌 Introducción
Este sistema permite la gestión de usuarios, vehículos, repuestos, servicios y facturación de manera eficiente. Incluye la generación de reportes en formato visual utilizando Graphviz.

## 💻 Requisitos del Sistema
- **Sistema Operativo:** Windows 10/11, Ubuntu 20.04+, MacOS
- **Dependencias:**
  - .NET 6+ instalado
  - GTK# para la interfaz gráfica
  - Graphviz para la generación de reportes

## 🚀 Instalación
1. Descarga e instala **.NET 6 SDK** desde [dotnet.microsoft.com](https://dotnet.microsoft.com/).
2. Instala **GTK#**:
   - **Linux:** `sudo apt-get install gtk-sharp2`
   - **Windows:** Descarga desde [gtk.org](https://www.gtk.org/)
3. Instala **Graphviz**:
   - **Linux:** `sudo apt-get install graphviz`
   - **Windows:** Descarga desde [graphviz.org](https://graphviz.org/)
4. Clona el repositorio y ejecuta el sistema con:
   ```sh
   git clone https://github.com/tu-repositorio
   cd tu-repositorio
   dotnet run
   ```

## 🖥️ Descripción de la Interfaz
### 1️⃣ **Menú Principal**
Contiene accesos directos a las funcionalidades principales:
- **Gestión de Usuarios**
- **Generar Servicio**
- **Cancelar Factura**
- **Generar Reportes**
- **Cerrar Sesión**

### 2️⃣ **Inicio de Sesion**
![Loggin]( ../assets/Inicio_log.png)

## 🛠️ Uso del Sistema
### **1️⃣ Gestión de Usuarios**
- **Ver Usuario:** Ingresar el ID del usuario y visualizar sus datos.
- **Editar Usuario:** Modificar Nombre, Apellido y Correo.
- **Eliminar Usuario:** Eliminar un usuario por su ID.

### **2️⃣ Generar Servicio**
- **Ingresar ID de Usuario y Vehículo**
- **Seleccionar Tipo de Servicio**
- **Asignar Repuesto (Opcional)**
- **Confirmar y Registrar**

### **3️⃣ Cancelar Factura**
- Se mostrará la última factura generada.
- Confirmar el pago y retirar de la pila.

### **4️⃣ Generar Reportes**
- Seleccionar el tipo de reporte:
  - **Usuarios y Vehículos**
  - **Repuestos y Servicios**
  - **Facturación**
- Exportar en CSV o JSON.

## 🔒 Cerrar Sesión
- Al presionar el botón de "Cerrar Sesión", se redirige a la pantalla de login.