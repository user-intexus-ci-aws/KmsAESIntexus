using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using Microsoft.VisualBasic;
using UnitTestProject1.PKCS11;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "KCV_ZMK", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int KCV_ZMK(string tokenname, string passphrase, string zmklabel, StringBuilder KCV);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "KCV_ZMK_TEST", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        //private static extern string KCV_ZMK_TEST(string tokenname, string passphrase, string[] llaveTemp);
        private static extern string KCV_ZMK_TEST(string tokenname, string passphrase);
        //int STACK_CALL KCV_ZMK_TEST(char *tokenname, char *passphrase, string *llave)

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "LOAD_ZMK_DOUBLE", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int LOAD_ZMK_DOUBLE(string C1, string C2, string C3, string tokenname, string passphrase, string zmklabel, bool doublelenght);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "LOAD_ZMK_TRIPLE", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int LOAD_ZMK_TRIPLE(string C1, string C2, string C3, string tokenname, string passphrase, string zmklabel, bool doublelenght);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "Decrypt", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Decrypt(string argArchivoFuente, string argArchivoDestino, string tamArchivoFuenteArg, string tamRegistroArg,
            string llavesArg, string encriptarArg, string fuenteEbcdicArg, string modoCifradoArg);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "DecryptTest", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int DecryptTest(string argArchivoFuente, string argArchivoDestino, string tamArchivoFuenteArg, string tamRegistroArg,
            string llavesArg, string encriptarArg, string fuenteEbcdicArg, string modoCifradoArg, string tokenname, string passphrase, string zmklabel);

        [DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "GET_KEYS", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GET_KEYS(string tokenname, string passphrase, StringBuilder KCV, StringBuilder keys, out string fullName);
        //DLLDIR int STACK_CALL GET_KEYS(char *tokenname, char *passphrase, char *zmklabel, char *KCV);




        //    DLLDIR int STACK_CALL DecryptTest(char* argArchivoFuente,
        //char* argArchivoDestino,
        //char* tamArchivoFuenteArg,
        //char* tamRegistroArg,
        //char* llavesArg,
        //char* encriptarArg,
        //char* fuenteEbcdicArg,
        //char* modoCifradoArg,
        //char* tokenname,
        //char* passphrase,
        //char* zmklabel);

        //[DllImport("IntecsusKMSNShield.dll", BestFitMapping = true, PreserveSig = true, EntryPoint = "Decrypt", SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        //private static extern int DecryptRef(string argArchivoFuente, string argArchivoDestino, ref long argTam, ref long argTamRegistro,
        //    char[] argLlaves, ref int argEncriptar, ref int argFuenteEbcdic, ref int modoCifrado);

        //Decrypt(char* argArchivoFuente,
        //        char* argArchivoDestino,
        //        char* tamArchivoFuenteArg,
        //        char* tamRegistroArg,
        //        char* llavesArg,
        //        char* encriptarArg,
        //        char* fuenteEbcdicArg,
        //        char* modoCifradoArg);

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                string kcvLlave;
                string error;
                StringBuilder kcv;
                string datos = string.Empty;
                kcv = new StringBuilder("", 512);
                string tokenName = ConfigurationManager.AppSettings["TokenName"];
                string tokenPassphrase = ConfigurationManager.AppSettings["TokenPassphrase"];
                int res = KCV_ZMK(tokenName, tokenPassphrase, "KPE.INTEX", kcv);
                if (res != 0)
                {
                    error = PKCS11Error.getErrorDescription(res);
                }
                else
                {
                    kcvLlave = Strings.UCase(Strings.Mid(kcv.ToString(), 1, 6));
                }


            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            try
            {
                string[] kcvLlave = new string[3];
                StringBuilder kcv, datos;
                //string datos = string.Empty;
                kcv = new StringBuilder("", 512);
                datos = new StringBuilder("", 512);

                string tokenName = ConfigurationManager.AppSettings["TokenName"];
                string tokenPassphrase = ConfigurationManager.AppSettings["TokenPassphrase"];
                string res = KCV_ZMK_TEST(tokenName, tokenPassphrase);

            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }     

     
        [TestMethod]
        public void TestDescifrar()
        {
            try
            {
                IntecsusKMSCommon.FachadaKMS fKMS = new IntecsusKMSCommon.FachadaKMS();

                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                if (fKMS.LoadDll(PathDll, DllKMSCripto))
                {
                    string valueEncrypt = "3D5D2B2BC6A51AD4A0749455E4BF78E93D5D2B2BC6A51AD4A0749455E4BF78E93D5D2B2BC6A51AD4A0749455E4BF78E93D5D2B2BC6A51AD4A0749455E4BF78E93D5D2B2BC6A51AD4A0749455E4BF78E9";
                    string tokenName = ConfigurationManager.AppSettings["TokenName"];
                    string tokenPassphrase = ConfigurationManager.AppSettings["TokenPassphrase"];
                    string KeyName = "KPE.LLAVETRANSPORTETEST";
                    int vResultadoTrans;
                    StringBuilder hSession = new StringBuilder("", 32);
                    StringBuilder hKey = new StringBuilder("", 32);
                    if (string.IsNullOrEmpty(hSession.ToString()))
                    {
                        vResultadoTrans = fKMS.IniciarPKCS11();
                        vResultadoTrans = fKMS.IniciarRecursos(new StringBuilder(tokenName), new StringBuilder(tokenPassphrase), new StringBuilder(KeyName), hSession, hKey);
                        if (vResultadoTrans == 0)
                        {
                            string vResultado = fKMS.DescifrarDato(hSession, hKey, valueEncrypt);
                            Assert.IsTrue(string.IsNullOrEmpty(vResultado));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void TestCifrar()
        {
            try
            {
                IntecsusKMSCommon.FachadaKMS fKMS = new IntecsusKMSCommon.FachadaKMS();

                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                if (fKMS.LoadDll(PathDll, DllKMSCripto))
                {
                    string valueEncrypt = "3D5D2B2BC6A51AD4A0749455E4BF78E9";
                    string tokenName = Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                    string tokenPassphrase = Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
                    string KeyName = "KPE.LLAVETRANSPORTETEST";
                    int vResultadoTrans;
                    StringBuilder hSession = new StringBuilder("", 32);
                    StringBuilder hKey = new StringBuilder("", 32);
                    if (string.IsNullOrEmpty(hSession.ToString()))
                    {
                        vResultadoTrans = fKMS.IniciarPKCS11();
                        vResultadoTrans = fKMS.IniciarRecursos(new StringBuilder(tokenName), new StringBuilder(tokenPassphrase), new StringBuilder(KeyName), hSession, hKey);
                        if (vResultadoTrans == 0)
                        {
                            string vResultado = fKMS.CifrarDato(hSession, hKey, valueEncrypt);
                            Assert.IsTrue(string.IsNullOrEmpty(vResultado));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void TestDescifrarArchivo()
        {
            try
            {
                IntecsusKMSCommon.FachadaKMS fKMS = new IntecsusKMSCommon.FachadaKMS();
                string pathArchivoFuente = @"C:\Codigo\Archivo\Output\O_C09_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\DES_C09_IDCARDS.txt";
                string tokenName = Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                string tokenPassphrase = Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string KeyName = "KPE.LLAVETRANSPORTETEST";
                StringBuilder hSession = new StringBuilder("", 32);
                StringBuilder hKey = new StringBuilder("", 32);

                int vResultadoTrans;

                if (fKMS.LoadDll(PathDll, DllKMSCripto))
                {
                    if (string.IsNullOrEmpty(hSession.ToString()))
                    {
                        vResultadoTrans = fKMS.IniciarPKCS11();
                        vResultadoTrans = fKMS.IniciarRecursos(new StringBuilder(tokenName), new StringBuilder(tokenPassphrase), new StringBuilder(KeyName), hSession, hKey);
                        if (vResultadoTrans == 0)
                        {
                            int counter = 0;
                            StringBuilder vResultado = new StringBuilder();
                            foreach (string line in System.IO.File.ReadLines(pathArchivoFuente))
                            {
                                //string hexLine = ConvertStringToHex(line);
                                string hexString = Utilities.ByteArrayToString(Convert.FromBase64String(line));
                                string datoDescifrado = fKMS.DescifrarDato(hSession, hKey, hexString);
                                string stringBase64 = Utilities.Data_Hex_Asc(datoDescifrado) + "\n";
                                //string stringBase64 = System.Convert.ToBase64String(Utilities.HexStringToHex(datoDescifrado)) + "\n";
                                vResultado.Append(stringBase64);
                                //System.Console.WriteLine(line);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            //File.WriteAllText(pathArchivoDestino, vResultado.ToString());
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.Finalizar(hSession);
                    }

                    //Byte[] bytes = File.ReadAllBytes(pathArchivoFuente);
                    //string hex = ByteArrayToString(bytes);
                    //string archivo = hex;
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void TestCifrarArchivo()
        {
            try
            {
                IntecsusKMSCommon.FachadaKMS fKMS = new IntecsusKMSCommon.FachadaKMS();
                string pathArchivoFuente = @"C:\Codigo\Archivo\Input\I_C09_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\O_C09_IDCARDS.txt";
                string tokenName = Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                string tokenPassphrase = Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string KeyName = "KPE.LLAVETRANSPORTETEST";
                StringBuilder hSession = new StringBuilder("", 32);
                StringBuilder hKey = new StringBuilder("", 32);

                int vResultadoTrans;

                if (fKMS.LoadDll(PathDll, DllKMSCripto))
                {
                    if (string.IsNullOrEmpty(hSession.ToString()))
                    {
                        vResultadoTrans = fKMS.IniciarPKCS11();
                        vResultadoTrans = fKMS.IniciarRecursos(new StringBuilder(tokenName), new StringBuilder(tokenPassphrase), new StringBuilder(KeyName), hSession, hKey);
                        if (vResultadoTrans == 0)
                        {

                            int counter = 0;
                            StringBuilder vResultado = new StringBuilder();
                            foreach (string line in System.IO.File.ReadLines(pathArchivoFuente))
                            {
                                string hexLine = Utilities.Data_Asc_Hex(line);
                                //string hexLine = ConvertStringToHex(line);
                                string datoCifrado = fKMS.CifrarDato(hSession, hKey, hexLine);
                                vResultado.Append(Convert.ToBase64String(Utilities.HexStringToHex(datoCifrado)) + "\n");
                                string datoDesCifrado = fKMS.DescifrarDato(hSession, hKey, datoCifrado);
                                System.Console.WriteLine(line);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            //File.WriteAllText(pathArchivoDestino, vResultado.ToString());
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.Finalizar(hSession);
                        
                    }

                    //Byte[] bytes = File.ReadAllBytes(pathArchivoFuente);
                    //string hex = ByteArrayToString(bytes);
                    //string archivo = hex;
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        public string ConvertStringToHex(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString;
        }       
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


    }
}