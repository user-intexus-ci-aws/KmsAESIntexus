using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using Microsoft.VisualBasic;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class TestsFachada
    {
        [TestMethod]
        public void TestDescifrarArchivo()
        {
            try
            {
                FachadaKMS fKMS = new FachadaKMS();
                string pathArchivoFuente = @"C:\Codigo\Archivo\Output\O_C09_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\DES_C09_IDCARDS.txt";
                string tokenName = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                string tokenPassphrase = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
                //string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMS"];
                //string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string PathDll = @"C:\Codigo\intecsus-intecsuskms\intecsus-intecsuskms\Debug";
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
                                vResultado.Append(stringBase64);
                                //System.Console.WriteLine(line);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.FinalizalizarRecursos(hSession);
                    }
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
                FachadaKMS fKMS = new FachadaKMS();


                //string[] fileEntries = Directory.GetFiles(@"C:\Codigo\Archivo\Input\");
                //foreach (string fileName in fileEntries)
                //{

                //}


                string pathArchivoFuente = @"C:\Codigo\Archivo\Input\I_C09_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\O_C09_IDCARDS.txt";
                string tokenName = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                string tokenPassphrase = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
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
                                string datoCifrado = fKMS.CifrarDato(hSession, hKey, hexLine);
                                vResultado.Append(Convert.ToBase64String(Utilities.HexStringToHex(datoCifrado)) + "\n");
                                string datoDesCifrado = fKMS.DescifrarDato(hSession, hKey, datoCifrado);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.FinalizalizarRecursos(hSession);
                    }
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void TestCifrarArchivoAES()
        {
            try
            {
                FachadaKMS fKMS = new FachadaKMS();
                string pathArchivoFuente = @"C:\Codigo\Archivo\Input\I_C11_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\O_C11_IDCARDS.txt";
                string tokenName = ConfigurationManager.AppSettings["TokenName"];
                string tokenPassphrase = ConfigurationManager.AppSettings["TokenPassphrase"];
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string KeyName = "AES.INTEXUS";
                StringBuilder hSession = new StringBuilder("", 64);
                StringBuilder hKey = new StringBuilder("", 64);

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
                                string datoCifrado = fKMS.CifrarDatoAES(hSession, hKey, hexLine);
                                vResultado.Append(Convert.ToBase64String(Utilities.HexStringToHex(datoCifrado)) + "\n");
                                string datoDesCifrado = fKMS.DescifrarDatoAES(hSession, hKey, datoCifrado);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.FinalizalizarRecursos(hSession);
                    }
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void TestDescifrarArchivoAES()
        {
            try
            {
                FachadaKMS fKMS = new FachadaKMS();
                string pathArchivoFuente = @"C:\Codigo\Archivo\Output\O_C11_IDCARDS.txt";
                string pathArchivoDestino = @"C:\Codigo\Archivo\Output\DES_C11_IDCARDS.txt";
                string tokenName = ConfigurationManager.AppSettings["TokenName"];
                string tokenPassphrase = ConfigurationManager.AppSettings["TokenPassphrase"];
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string KeyName = "AES.INTEXUS";
                StringBuilder hSession = new StringBuilder("", 64);
                StringBuilder hKey = new StringBuilder("", 64);

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
                                string datoDescifrado = fKMS.DescifrarDatoAES(hSession, hKey, hexString);
                                string stringBase64 = Utilities.Data_Hex_Asc(datoDescifrado) + "\n";
                                vResultado.Append(stringBase64);
                                //System.Console.WriteLine(line);
                                counter++;
                            }
                            byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                            File.WriteAllBytes(pathArchivoDestino, archivo);
                            Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                        }
                        fKMS.FinalizalizarRecursos(hSession);
                    }
                }
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }
        public void TestCifrarArchivos()
        {
            try
            {
                FachadaKMS fKMS = new FachadaKMS();
                string pathArchivoFuente = string.Empty;
                string pathArchivoDestino = string.Empty;
                string tokenName = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenName"]);
                string tokenPassphrase = Utilities.Base64Decode(ConfigurationManager.AppSettings["TokenPassphrase"]);
                string DllKMSCripto = ConfigurationManager.AppSettings["DllKMSCripto"];
                string PathDll = ConfigurationManager.AppSettings["PathDll"];
                string KeyName = "KPE.LLAVETRANSPORTETEST";
               
                int vResultadoTrans;

                string rutaOtigen = @"C:\Codigo\Archivo\Input\";
                string rutaDestino = @"C:\Codigo\Archivo\Output\";

                string[] fileEntries = Directory.GetFiles(rutaOtigen);
                foreach (string fileName in fileEntries)
                {
                    StringBuilder hSession = new StringBuilder("", 32);
                    StringBuilder hKey = new StringBuilder("", 32);

                    FileInfo info = new FileInfo(fileName);
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
                                pathArchivoFuente = fileName;
                                foreach (string line in System.IO.File.ReadLines(pathArchivoFuente))
                                {
                                    string hexLine = Utilities.Data_Asc_Hex(line);
                                    string datoCifrado = fKMS.CifrarDato(hSession, hKey, hexLine);
                                    vResultado.Append(Convert.ToBase64String(Utilities.HexStringToHex(datoCifrado)) + "\n");
                                    string datoDesCifrado = fKMS.DescifrarDato(hSession, hKey, datoCifrado);
                                    counter++;
                                }
                                byte[] archivo = Encoding.ASCII.GetBytes(vResultado.ToString());
                                pathArchivoDestino = rutaDestino + info.Name;
                                File.WriteAllBytes(pathArchivoDestino, archivo);
                                Assert.IsTrue(!string.IsNullOrEmpty(vResultado.ToString()));
                            }
                            fKMS.FinalizalizarRecursos(hSession);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }
    }
}
