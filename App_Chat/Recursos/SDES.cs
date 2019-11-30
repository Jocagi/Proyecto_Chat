using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class SDES
    {
        #region Definitions
        private const int bufferLength = 1024;

        private string K1;
        private string K2;

        #region permutaciones
        private int[] P10 = { 7, 1, 4, 3, 8, 2, 9, 6, 5, 0 };
        private int[] P8 = { 3, 9, 8, 6, 0, 1, 2, 7 };
        private int[] P4 = { 0, 3, 2, 1 };
        private int[] EP = { 1, 3, 0, 2, 3, 2, 0, 1 };
        private int[] PI = { 0, 2, 4, 6, 1, 3, 5, 7 };
        private int[] PIn;
        #endregion

        #region SBOXES

        private static readonly string[,] S0 = new string[4, 4]
        {
            {"01", "00", "11", "10"},
            {"11", "10", "01", "00"},
            {"00", "10", "01", "11"},
            {"11", "01", "11", "10"}
        };

        private static readonly string[,] S1 = new string[4, 4]
        {
            {"00", "01", "10", "11"},
            {"10", "00", "01", "11"},
            {"11", "00", "01", "00"},
            {"10", "01", "00", "11"}
        };

        public SDES(int[] pIn)
        {
            PIn = pIn;
        }

        #endregion

        #endregion

        public SDES()
        {
            PIn = this.getInversePermutation();
        }

        private int[] getInversePermutation()
        {
            int[] PI = this.PI;
            int[] PIn = new int[8];

            for (int i = 0; i < 8; i++)
            {
                PIn[PI[i]] = i;
            }

            return PIn;
        }

        private const int _8bitDivide = 4;
        private const int _10bitDivide = 5;

        private string LeftShift(string value, int n)
        {
            string output = value + value;
            return output.Substring(n, value.Length);
        }

        private string Permutate(string value, int[] function)
        {
            string output = "";
            for (int i = 0; i < function.Length; i++)
            {
                output += value[function[i]];
            }
            return output;
        }

        private string XOR(string input, string key)
        {
            string output = "";
            for (int i = 0; i < key.Length; i++)
            {
                if (input[i] == key[i])
                {
                    output += "0";
                }
                else
                {
                    output += "1";
                }
            }
            return output;
        }

        private string SBOXES(string input)
        {
            string block1 = input.Substring(0, 4);
            string block2 = input.Substring(4, 4);

            return getSBoxValue(block1, SDES.S0) + getSBoxValue(block2, SDES.S1);
        }

        private string getSBoxValue(string input, string[,] sbox)
        {
            int row = Convert.ToInt32((Combine(input[0], input[3])), 2);
            int column = Convert.ToInt32((Combine(input[1], input[2])), 2);
            return sbox[row, column];
        }

        private string Combine(string value1, string value2)
        {
            return value1 + value2;
        }

        private string Combine(char value1, char value2)
        {
            return Convert.ToString(value1) + Convert.ToString(value2);
        }

        private string[] Divide(string value, int lenght)
        {
            string[] output = new string[2];

            output[0] = value.Substring(0, lenght);
            output[1] = value.Substring(lenght, lenght);

            return output;
        }

        private string Swap(string value)
        {
            string[] values = Divide(value, _8bitDivide);

            return values[1] + values[0];
        }

        private byte ConvertStringToByte(string value)
        {
            return Convert.ToByte(Convert.ToInt32(value, 2));
        }

        private string ConvertByteToString(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0'); // produce cadena "00111111";
        }

        private void WriteToFile(string path, byte[] value)
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(value);
            bw.Close();
        }

        private string ConvertKeyToString(int key)
        {
            return Convert.ToString(key, 2).PadLeft(10, '0');
        }

        private void GenerateKeys(int input)
        {
            string key = ConvertKeyToString(input);

            string _P10 = Permutate(key, P10); //Permutacion 10
            string[] _P10Block = Divide(_P10, _10bitDivide);

            string LS1 = LeftShift(_P10Block[0], 1) + LeftShift(_P10Block[1], 1); //Left Shift 1 a cada mital del P10
            this.K1 = Permutate(LS1, P8); //Permutacion 8 -> Primer llave

            string[] LS1Block = Divide(LS1, _10bitDivide);
            string LS2 = LeftShift(LS1Block[0], 2) + LeftShift(LS1Block[1], 2); //Left Shift 2

            this.K2 = Permutate(LS2, P8); //Permutacion 8 -> Segunda llave
        }

        private string AlgorithmSDES(string _byte, string key)
        {
            //Permutacion Inicial y division del byte en dos bloques
            string InitialPermutation = _byte;
            string[] Division = Divide(InitialPermutation, _8bitDivide);

            //Se guardan los bloques para futuras operaciones
            string Block1 = Division[0];
            string Block2 = Division[1];

            //Expandir y permitar, Xor con K1, Sboxes, y P4 para el bloque 2
            string Expansion = Permutate(Block2, EP);
            string XORkey1 = XOR(Expansion, key);
            string Sboxes = SBOXES(XORkey1);
            string _P4 = Permutate(Sboxes, this.P4);

            //XOR con el primer bloque
            string Xorblock1 = XOR(_P4, Block1);

            //Uniionde resultado con bloque 2
            string result = Combine(Xorblock1, Block2);

            return result;
        }

        public string CifrarArchivo(string path, int key)
        {
            GenerateKeys(key);

            #region Crear_Archivo
            string NuevoArchivo = Path.GetFileName(path);
            string rutaCifrado = Directories.directorioArchivos + NuevoArchivo;
            Archivo.crearArchivo(rutaCifrado);
            #endregion

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(bufferLength);
                        List<byte> CompresionBytes = new List<byte>();

                        foreach (var item in buffer)
                        {
                            //Comprimir

                            string _byte = ConvertByteToString(item);
                            string InitialPermutation = Permutate(_byte, this.PI);

                            string result1 = Swap(AlgorithmSDES(InitialPermutation, this.K1));
                            string result2 = Permutate(AlgorithmSDES(result1, K2), this.PIn);

                            CompresionBytes.Add(ConvertStringToByte(result2));
                        }

                        WriteToFile(rutaCifrado, CompresionBytes.ToArray());
                    }
                }
            }

            return rutaCifrado;
        }

        public string DescifrarArchivo(string path, int key)
        {
            GenerateKeys(key);

            #region Crear_Archivo
            string NuevoArchivo = Path.GetFileName(path);
            string rutaCifrado = Directories.directorioArchivos + NuevoArchivo;
            Archivo.crearArchivo(rutaCifrado);
            #endregion

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(bufferLength);
                        List<byte> CompresionBytes = new List<byte>();

                        foreach (var item in buffer)
                        {
                            //Descomprimir

                            string _byte = ConvertByteToString(item);
                            string InitialPermutation = Permutate(_byte, this.PI);

                            string result1 = Swap(AlgorithmSDES(InitialPermutation, this.K2));
                            string result2 = Permutate(AlgorithmSDES(result1, K1), this.PIn);

                            CompresionBytes.Add(ConvertStringToByte(result2));
                        }

                        WriteToFile(rutaCifrado, CompresionBytes.ToArray());
                    }
                }
            }
            return rutaCifrado;
        }

        public string CifrarTexto(string text, int key)
        {
            GenerateKeys(key);

            List<byte> input = new List<byte>();
            string result = "";

            //Convert input to byte
            foreach (var item in text)
            {
                input.Add((byte) item);
            }
            
            //Cifrar
            List<byte> CompresionBytes = new List<byte>();

            foreach (var item in input)
            {
                string _byte = ConvertByteToString(item);
                string InitialPermutation = Permutate(_byte, this.PI);

                string result1 = Swap(AlgorithmSDES(InitialPermutation, this.K1));
                string result2 = Permutate(AlgorithmSDES(result1, K2), this.PIn);

                CompresionBytes.Add(ConvertStringToByte(result2));
            }

            //Convertir bytes a string

            foreach (var item in CompresionBytes)
            {
                result += (char) item;
            }

            return result;
        }

        public string DescifrarTexto(string text, int key)
        {
            GenerateKeys(key);

            List<byte> input = new List<byte>();
            string result = "";

            //Convert input to byte
            foreach (var item in text)
            {
                input.Add((byte)item);
            }

            //Descifrar
            List<byte> CompresionBytes = new List<byte>();

            foreach (var item in input)
            {
                string _byte = ConvertByteToString(item);
                string InitialPermutation = Permutate(_byte, this.PI);

                string result1 = Swap(AlgorithmSDES(InitialPermutation, this.K2));
                string result2 = Permutate(AlgorithmSDES(result1, K1), this.PIn);

                CompresionBytes.Add(ConvertStringToByte(result2));
            }

            //Convertir bytes a string

            foreach (var item in CompresionBytes)
            {
                result += (char)item;
            }

            return result;
        }
    }
}
