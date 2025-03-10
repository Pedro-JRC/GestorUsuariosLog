using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GestorUsuariosLog.Models;

namespace GestorUsuariosLog.Helpers
{
    public static class LogHelper
    {
        // Ruta relativa del archivo de log (se creará en la carpeta de ejecución)
        private static readonly string logFilePath = "Usarioslog.txt";

        /// <summary>
        /// Agrega un nuevo registro de usuario al log. 
        /// Se lee el archivo existente (formateado como un arreglo JSON), se añade el registro y se reescribe el archivo.
        /// Si el archivo no existe o está vacío, se crea un nuevo arreglo.
        /// </summary>
        public static void AppendUserLog(Usuario usuario)
        {
            try
            {
                List<object> logRecords;

                if (File.Exists(logFilePath))
                {
                    string content = File.ReadAllText(logFilePath);
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        logRecords = new List<object>();
                    }
                    else
                    {
                        // Intentar deserializar el contenido en una lista de objetos
                        logRecords = JsonSerializer.Deserialize<List<object>>(content) ?? new List<object>();
                    }
                }
                else
                {
                    logRecords = new List<object>();
                }

                // Añadimos el nuevo registro
                logRecords.Add(usuario);

                // Serializamos todo el arreglo con indentación
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(logRecords, options);
                File.WriteAllText(logFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al escribir el log: " + ex.Message);
            }
        }

        /// <summary>
        /// Lee y devuelve el contenido del log, que es un arreglo JSON formateado.
        /// Si el archivo no existe o está vacío, se devuelve un arreglo vacío ("[]").
        /// </summary>
        public static string ReadLog()
        {
            try
            {
                if (!File.Exists(logFilePath))
                {
                    return "[]";
                }
                string content = File.ReadAllText(logFilePath);
                if (string.IsNullOrWhiteSpace(content))
                {
                    return "[]";
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer el log: " + ex.Message);
            }
        }
    }
}
