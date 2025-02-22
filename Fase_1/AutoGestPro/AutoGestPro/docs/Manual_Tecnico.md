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
   git clone https://github.com/tu-repositorio
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
