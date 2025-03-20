# 🛠️ Manual Técnico

## 📌 Introducción
Este documento proporciona detalles técnicos sobre la arquitectura, tecnologías y funcionamiento del sistema de gestión.

## 💻 Requisitos del Sistema
- **Sistema Operativo:** Windows 10/11, Ubuntu 20.04+, MacOS
- **Dependencias:**
    - .NET 6+ instalado
    - GTK# para la interfaz gráfica
    - Graphviz para la generación de reportes
    - Newtonsoft.Json para manejo de JSON

## 🚀 Instalación y Configuración
1. Instalar **.NET 6 SDK** desde [dotnet.microsoft.com](https://dotnet.microsoft.com/)
2. Instalar **GTK#**:
    - **Linux:** `sudo apt-get install gtk-sharp2`
    - **Windows:** Descargar desde [gtk.org](https://www.gtk.org/)
3. Instalar **Graphviz**:
    - **Linux:** `sudo apt-get install graphviz`
    - **Windows:** Descargar desde [graphviz.org](https://graphviz.org/)
4. Clonar el repositorio:
   ```sh
   git clone https://github.com/Santiago78op/EDD_1S2025_201905884.git
   cd tu-repositorio
   dotnet build
   dotnet run
   ```

## 🔧 Arquitectura del Sistema
- **Lenguaje:** C#
- **Framework:** .NET 6
- **Interfaz Gráfica:** GTK#
- **Base de Datos:** SQLite / JSON Storage
- **Generación de Reportes:** Graphviz

## 📂 Estructura del Proyecto
```
📦 Proyecto
 ┣ 📂 Core
 ┃ ┣ 📂 Models (Modelos de datos)
 ┃ ┣ 📂 Services (Lógica de negocio)
 ┣ 📂 UI
 ┃ ┣ 📂 Windows (Ventanas GTK#)
 ┃ ┣ 📂 Components (Componentes reutilizables)
 ┣ 📂 Utils (Utilidades generales)
 ┣ 📜 Program.cs (Punto de entrada)
```

## 🏗️ Componentes Principales
### **1️⃣ Gestión de Usuarios**
- **Usuarios almacenados en JSON**
- **Operaciones CRUD**
- **Manejo de validaciones y errores**

### **2️⃣ Servicios y Facturación**
- **Relación entre usuarios y servicios**
- **Generación automática de facturas en pila (Stack)**
- **Exportación de datos en JSON / CSV**

### **3️⃣ Generación de Reportes con Graphviz**
- **Reportes de Usuarios, Vehículos y Servicios**
- **Generación de `.dot` y conversión a `.png`**
- **Uso de `ProcessStartInfo` para ejecutar `dot`**

## 🛡️ Seguridad
- **Validaciones de entrada de datos**
- **Manejo de excepciones**
- **Restricciones de acceso en ciertos módulos**
## 2. Arquitectura del Sistema
El sistema está basado en una arquitectura modular con las siguientes estructuras de datos:
- **Árbol AVL** para la gestión de repuestos.
- **Árbol Binario** para la gestión de servicios.
- **Árbol B de orden 5** para la gestión de facturas.
- **Matriz Dispersa** para la bitácora de repuestos y servicios.

## 3. Módulos Implementados

### 3.1 Gestión de Entidades
- **Usuarios**: Visualización, búsqueda, edición y eliminación de usuarios.
- **Vehículos**: Gestión de vehículos asociados a los usuarios.

### 3.2 Control de Logueo
- Registro de ingresos y salidas de usuarios en formato JSON.

### 3.3 Generación de Reportes
- Creación de gráficos con **Graphviz** para visualizar:
    - Árbol AVL de repuestos.
    - Árbol Binario de servicios.
    - Árbol B de facturas.
    - Relación entre usuarios, vehículos y servicios.

### 3.4 Administración de Servicios
- Creación de servicios asignados a un usuario y su vehículo.
- Generación automática de facturas al crear un servicio.
- Asociación de repuestos con cada servicio generado.

### 3.5 Visualización de Facturas
- Los usuarios pueden ver sus facturas pendientes en una tabla.
- Cancelación de facturas eliminándolas del sistema una vez pagadas.

## 4. Estructuras de Datos Implementadas

### 4.1 Árbol AVL (Repuestos)
- Permite una rápida inserción, eliminación y búsqueda de repuestos.
- Se mantiene balanceado para optimizar el rendimiento.

### 4.2 Árbol Binario (Servicios)
- Organización de servicios para su fácil búsqueda y filtrado (PRE-ORDEN, IN-ORDEN, POST-ORDEN).

### 4.3 Árbol B de Orden 5 (Facturas)
- Estructura balanceada para manejar grandes volúmenes de facturación con búsquedas eficientes.

### 4.4 Matriz Dispersa (Bitácora de Servicios)
- Relaciona repuestos utilizados con los vehículos en los que fueron instalados.

## 5. Generación de Reportes con Graphviz
Ejemplo de generación de un reporte de servicios:
```dot

digraph Servicios {
    node [shape=ellipse, style=filled, fillcolor=lightgray];
    "Servicio 1" -> "Servicio 2";
    "Servicio 1" -> "Servicio 3";
    "Servicio 2" -> "Servicio 4";
    "Servicio 3" -> "Servicio 5";
}