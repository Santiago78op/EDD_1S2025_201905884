using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AutoGestPro.Core.Models;

namespace AutoGestPro.Core.Services
{
    /// <summary>
    /// Servicio para gestionar los logs de entrada y salida de usuarios en el sistema.
    /// </summary>
    public class LogService
    {
        private static readonly List<UserLog> _activeLogs = new List<UserLog>();
        private static readonly List<UserLog> _completedLogs = new List<UserLog>();
        private static readonly object _logsLock = new object();

        /// <summary>
        /// Obtiene la ruta absoluta a la carpeta de logs.
        /// </summary>
        public static string GetLogsPath()
        {
            string projectRootPath = GetProjectRootPath();
            string logsPath = Path.Combine(projectRootPath, "Logs");

            // Crear la carpeta "Logs" si no existe
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }

            return logsPath;
        }

        /// <summary>
        /// Obtiene la ruta absoluta a la raíz del proyecto.
        /// </summary>
        private static string GetProjectRootPath()
        {
            // Obtiene la ubicación del ejecutable actual
            string exePath = Assembly.GetExecutingAssembly().Location;

            // Sube tres niveles para llegar a la raíz del proyecto
            string projectRoot = new DirectoryInfo(Path.GetDirectoryName(exePath))
                .Parent?.Parent?.Parent?.FullName;

            if (projectRoot == null)
                throw new DirectoryNotFoundException("No se pudo encontrar la raíz del proyecto");

            return projectRoot;
        }

        /// <summary>
        /// Registra la entrada de un usuario al sistema.
        /// </summary>
        /// <param name="usuario">Correo electrónico del usuario.</param>
        /// <returns>El log creado.</returns>
        public UserLog RegistrarEntrada(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
                throw new ArgumentException("El usuario no puede estar vacío", nameof(usuario));

            var log = new UserLog(usuario, DateTime.Now);

            lock (_logsLock)
            {
                _activeLogs.Add(log);
            }

            return log;
        }

        /// <summary>
        /// Registra la salida de un usuario del sistema.
        /// </summary>
        /// <param name="usuario">Correo electrónico del usuario.</param>
        /// <returns>El log actualizado o null si no se encontró un log activo para el usuario.</returns>
        public UserLog RegistrarSalida(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
                throw new ArgumentException("El usuario no puede estar vacío", nameof(usuario));

            UserLog log = null;

            lock (_logsLock)
            {
                // Buscar log activo del usuario
                var activeLogIndex = _activeLogs.FindIndex(l => l.Usuario == usuario);
                if (activeLogIndex >= 0)
                {
                    log = _activeLogs[activeLogIndex];
                    log.RegistrarSalida(DateTime.Now);

                    // Mover a logs completados
                    _activeLogs.RemoveAt(activeLogIndex);
                    _completedLogs.Add(log);
                }
            }

            return log;
        }

        /// <summary>
        /// Obtiene todos los logs registrados (activos y completados).
        /// </summary>
        /// <returns>Lista combinada de todos los logs.</returns>
        public List<UserLog> ObtenerTodosLogs()
        {
            var result = new List<UserLog>();

            lock (_logsLock)
            {
                result.AddRange(_activeLogs);
                result.AddRange(_completedLogs);
            }

            // Ordenar por fecha de entrada descendente (más reciente primero)
            result.Sort((a, b) => b.Entrada.CompareTo(a.Entrada));

            return result;
        }

        /// <summary>
        /// Exporta todos los logs a un archivo JSON.
        /// </summary>
        /// <returns>Ruta del archivo generado.</returns>
        public async Task<string> ExportarLogsAsync()
        {
            var logs = ObtenerTodosLogs();
            
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(GetLogsPath(), $"user_logs_{timestamp}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonData = JsonSerializer.Serialize(logs, options);
            await File.WriteAllTextAsync(filePath, jsonData);

            return filePath;
        }

        /// <summary>
        /// Exporta los logs de un periodo específico a un archivo JSON.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del periodo.</param>
        /// <param name="fechaFin">Fecha de fin del periodo.</param>
        /// <returns>Ruta del archivo generado.</returns>
        public async Task<string> ExportarLogsPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var allLogs = ObtenerTodosLogs();
            var logsFiltrados = allLogs.FindAll(log => 
                log.Entrada >= fechaInicio && 
                (log.Salida == null || log.Salida <= fechaFin));
            
            string rangoFechas = $"{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}";
            string filePath = Path.Combine(GetLogsPath(), $"user_logs_{rangoFechas}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonData = JsonSerializer.Serialize(logsFiltrados, options);
            await File.WriteAllTextAsync(filePath, jsonData);

            return filePath;
        }
    }
}