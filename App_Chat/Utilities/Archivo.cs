using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities
{
    public class Archivo
    {
        public static void crearArchivo(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream fs = File.Create(path);
            fs.Close();
        }

        public static void eliminarArchivo(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void escribirEnArchivo(string path, string text)
        {
            if (File.Exists(path))
            {
                File.AppendAllText(path, text);
            }
            else
            {
                throw new Exception("El Archivo no existe");
            }
        }

        public static void InsertData(string path, int position, byte[] data)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                stream.Position = position;
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
