using System;
using System.Text.Json.Serialization;

namespace AutoGestPro.Core.Models
{
    /// <summary>
    /// Representa un registro de entrada y salida de un usuario en el sistema.
    /// </summary>
    public class UserLog
    {
        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; }

        /// <summary>
        /// Fecha y hora de entrada al sistema.
        /// </summary>
        [JsonPropertyName("entrada")]
        public DateTime Entrada { get; set; }

        /// <summary>
        /// Fecha y hora de salida del sistema.
        /// </summary>
        [JsonPropertyName("salida")]
        public DateTime? Salida { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserLog"/>.
        /// </summary>
        /// <param name="usuario">Correo electrónico del usuario.</param>
        /// <param name="entrada">Fecha y hora de entrada al sistema.</param>
        public UserLog(string usuario, DateTime entrada)
        {
            Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            Entrada = entrada;
            Salida = null;
        }

        /// <summary>
        /// Registra la salida del usuario del sistema.
        /// </summary>
        /// <param name="salida">Fecha y hora de salida.</param>
        public void RegistrarSalida(DateTime salida)
        {
            Salida = salida;
        }
    }
}