using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using Microsoft.VisualBasic;
using UnitTestProject1.PKCS11;
using System.IO;

namespace UnitTestProject1
{
    public class FachadaKMS
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);
        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "INICIAR_PKCS11", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int INICIAR_PKCS11();

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "INICIAR_RECURSOS", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int INICIAR_RECURSOS(StringBuilder token_name, StringBuilder passphrase, StringBuilder key_label, StringBuilder out_hSession, StringBuilder out_hKey);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "FINALIZAR_RECURSOS", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int FINALIZAR_RECURSOS(StringBuilder in_hSession);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "TDESCBC", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TDESCBC(StringBuilder in_hSession, StringBuilder in_hKey, StringBuilder data_in, StringBuilder data_out, StringBuilder mode, StringBuilder IV);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "TDESECB", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TDESECB(StringBuilder in_hSession, StringBuilder in_hKey, StringBuilder data_in, StringBuilder data_out, StringBuilder mode);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "AESECB", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int AESECB(StringBuilder in_hSession, StringBuilder in_hKey, StringBuilder data_in, StringBuilder data_out, StringBuilder mode);


        private static object LockThis = new object();

        public bool LoadDll(string rutaDll, string NombreDll)
        {
            try
            {
                string dll2Path = rutaDll;
                SetDllDirectory(dll2Path);
                IntPtr ptr2 = LoadLibrary(NombreDll);

                if (ptr2 == IntPtr.Zero)
                {
                    Console.WriteLine(Marshal.GetLastWin32Error());
                    // Logger.Write("Error al cargar la dll:" + NombreDll + "Ruta dll : " + rutaDll + "Error generico:" + Marshal.GetLastWin32Error().ToString(), "ExceptionHandling")
                    return false;
                }
                else
                    // Logger.Write(NombreDll + " Cargada", "AppLog")

                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int IniciarPKCS11()
        {
            return INICIAR_PKCS11();
        }
        public int IniciarRecursos(StringBuilder token_name, StringBuilder passphrase, StringBuilder key_label, StringBuilder out_hSession, StringBuilder out_hKey)
        {
            return INICIAR_RECURSOS(token_name, passphrase, key_label, out_hSession, out_hKey);
        }
        public void FinalizalizarRecursos(StringBuilder hSession)
        {
            lock (LockThis)
                FINALIZAR_RECURSOS(hSession);
        }
        public string CifrarDato(StringBuilder hSession, StringBuilder hKey, string cadena)
        {
            lock (LockThis)
            {
                // Variables locales
                cadena = this.ValidarDato(cadena, "00000000000000000000000000000000");
                StringBuilder mode = new StringBuilder("E");
                StringBuilder data_in = new StringBuilder(cadena);
                StringBuilder data_out = new StringBuilder("", cadena.Length + 1);
                int rv = 0;

                string CipherType = ConfigurationManager.AppSettings["CypherType"];
                // Logger.Write("Tipo cifrado: " & CipherType, "Tracing")
                if (!string.IsNullOrEmpty(CipherType))
                {
                    switch (CipherType)
                    {
                        case "ECB":
                            {
                                rv = TDESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }

                        case "CBC":
                            {
                                string strVi = ConfigurationManager.AppSettings["VI"];
                                if (!string.IsNullOrEmpty(strVi))
                                {
                                    StringBuilder IV = new StringBuilder(strVi);
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }
                                else
                                {
                                    StringBuilder IV = new StringBuilder("0000000000000000");
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }

                                break;
                            }

                        default:
                            {
                                rv = TDESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }
                    }
                }
                else
                    rv = TDESECB(hSession, hKey, data_in, data_out, mode);

                if (rv != 0)
                    throw new Exception("HSMShield: funcion: CifrarDato, err: " + rv.ToString());
                return data_out.ToString();
            }
        }

        public string CifrarDatoAES(StringBuilder hSession, StringBuilder hKey, string cadena)
        {
            lock (LockThis)
            {
                // Variables locales
                cadena = this.ValidarDato(cadena, "00000000000000000000000000000000");
                StringBuilder mode = new StringBuilder("E");
                StringBuilder data_in = new StringBuilder(cadena);
                StringBuilder data_out = new StringBuilder("", cadena.Length + 1);
                int rv = 0;

                string CipherType = ConfigurationManager.AppSettings["CypherType"];
                // Logger.Write("Tipo cifrado: " & CipherType, "Tracing")
                if (!string.IsNullOrEmpty(CipherType))
                {
                    switch (CipherType)
                    {
                        case "ECB":
                            {
                                rv = AESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }

                        case "CBC":
                            {
                                string strVi = ConfigurationManager.AppSettings["VI"];
                                if (!string.IsNullOrEmpty(strVi))
                                {
                                    StringBuilder IV = new StringBuilder(strVi);
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }
                                else
                                {
                                    StringBuilder IV = new StringBuilder("0000000000000000");
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }

                                break;
                            }

                        default:
                            {
                                rv = TDESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }
                    }
                }
                else
                    rv = TDESECB(hSession, hKey, data_in, data_out, mode);

                if (rv != 0)
                    throw new Exception("HSMShield: funcion: CifrarDato, err: " + rv.ToString());
                return data_out.ToString();
            }
        }
        public string DescifrarDato(StringBuilder hSession, StringBuilder hKey, string cadenaCifrada)
        {
            lock (LockThis)
            {
                // Variables locales
                if (cadenaCifrada.Length / (double)2 % 8 != 0)
                    throw new Exception("HSMShield - DescifrarDato: Tamaño de la cadena no es multiplo de  8");

                StringBuilder mode = new StringBuilder("D");
                StringBuilder data_in = new StringBuilder(cadenaCifrada);
                StringBuilder data_out = new StringBuilder("", cadenaCifrada.Length + 1);
                int rv = 0;

                string CipherType = ConfigurationManager.AppSettings["CypherType"];

                // Logger.Write("Tipo cifrado: " & CipherType, "Tracing")


                if (!string.IsNullOrEmpty(CipherType))
                {
                    switch (CipherType)
                    {
                        case "ECB":
                            {
                                rv = TDESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }

                        case "CBC":
                            {
                                string strVi = ConfigurationManager.AppSettings["VI"];
                                if (!string.IsNullOrEmpty(strVi))
                                {
                                    StringBuilder IV = new StringBuilder(strVi);
                                    // Logger.Write("iv: " & IV.ToString(), "Tracing")
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }
                                else
                                {
                                    StringBuilder IV = new StringBuilder("0000000000000000");
                                    // Logger.Write("iv: " & IV.ToString(), "Tracing")
                                    rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                                }

                                break;
                            }

                        default:
                            {
                                rv = TDESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }
                    }
                }
                else
                    rv = TDESECB(hSession, hKey, data_in, data_out, mode);

                // Se llama a la función de la DLL
                // Dim IV As StringBuilder = New StringBuilder(ConfigurationManager.AppSettings("VI"))
                // rv = TDES(hSession, hKey, data_in, data_out, mode, IV)

                if (rv != 0)
                    // Logger.Write("Excepcion descifrado error: " & rv.ToString(), "AppLog")
                    throw new Exception("HSMShield: funcion: DescifrarDato, err: " + rv.ToString());
                return data_out.ToString();
            }
        }

        public string DescifrarDatoAES(StringBuilder hSession, StringBuilder hKey, string cadenaCifrada)
        {
            lock (LockThis)
            {
                // Variables locales
                if (cadenaCifrada.Length / (double)2 % 8 != 0)
                    throw new Exception("HSMShield - DescifrarDato: Tamaño de la cadena no es multiplo de  8");

                StringBuilder mode = new StringBuilder("D");
                StringBuilder data_in = new StringBuilder(cadenaCifrada);
                StringBuilder data_out = new StringBuilder("", cadenaCifrada.Length + 1);
                int rv = 0;

                string CipherType = ConfigurationManager.AppSettings["CypherType"];

                // Logger.Write("Tipo cifrado: " & CipherType, "Tracing")


                if (!string.IsNullOrEmpty(CipherType))
                {
                    switch (CipherType)
                    {
                        case "ECB":
                            {
                                rv = AESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }

                        //case "CBC":
                        //    {
                        //        string strVi = ConfigurationManager.AppSettings["VI"];
                        //        if (!string.IsNullOrEmpty(strVi))
                        //        {
                        //            StringBuilder IV = new StringBuilder(strVi);
                        //            // Logger.Write("iv: " & IV.ToString(), "Tracing")
                        //            rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                        //        }
                        //        else
                        //        {
                        //            StringBuilder IV = new StringBuilder("0000000000000000");
                        //            // Logger.Write("iv: " & IV.ToString(), "Tracing")
                        //            rv = TDESCBC(hSession, hKey, data_in, data_out, mode, IV);
                        //        }

                        //        break;
                        //    }

                        default:
                            {
                                rv = AESECB(hSession, hKey, data_in, data_out, mode);
                                break;
                            }
                    }
                }
                else
                    rv = AESECB(hSession, hKey, data_in, data_out, mode);

                // Se llama a la función de la DLL
                // Dim IV As StringBuilder = New StringBuilder(ConfigurationManager.AppSettings("VI"))
                // rv = TDES(hSession, hKey, data_in, data_out, mode, IV)

                if (rv != 0)
                    // Logger.Write("Excepcion descifrado error: " & rv.ToString(), "AppLog")
                    throw new Exception("HSMShield: funcion: DescifrarDato, err: " + rv.ToString());
                return data_out.ToString();
            }
        }
        private string ValidarDato(string cadenaCifrada, string key)
        {
            if ((cadenaCifrada.Length / (double)2) % 8 != 0)
                cadenaCifrada = this.RellenarCadena(cadenaCifrada, key);
            if (key.Length != (16 * 2))
                throw new Exception("El tamaño  del DES3 key no es de 16");

            return cadenaCifrada;
        }
        private string RellenarCadena(string cadena, string p2)
        {
            try
            {
                while ((this.ValidarTamanoDato(cadena, 8)))
                    cadena += "0";
            }
            catch (Exception ex)
            {
                throw new Exception("Tamaño de la cadena no es multiplo de  8");
            }

            return cadena;
        }
        private bool ValidarTamanoDato(string cadena, int valorValidar)
        {
            if ((cadena.Length / (double)2) % valorValidar != 0)
                return true;
            else
                return false;
        }
    }
}
