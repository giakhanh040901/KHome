using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolsUtils
{
    class RSACSPSample
    {
        public static void Test()
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
                byte[] encryptedData;
                byte[] decryptedData;


                RSACryptoServiceProvider RSA1 = new RSACryptoServiceProvider();
                var publickey = RSA1.ExportRSAPublicKey(); //Header là BEGIN RSA PUBLIC KEY
                //RSA1.ExportSubjectPublicKeyInfo(); //Header là BEGIN PUBLIC KEY
                var privateKey = RSA1.ExportRSAPrivateKey();

                //dạng base 64
                string publicKeyBase64 = Convert.ToBase64String(publickey);
                string privateKeyBase64 = Convert.ToBase64String(privateKey);

                //write pemfile
                string publicPem = new string(PemEncoding.Write("PUBLIC KEY", publickey));
                string privatePem = new string(PemEncoding.Write("PRIVATE KEY", privateKey));

                RSACryptoServiceProvider rsaService = new RSACryptoServiceProvider();
                File.WriteAllText("public.pem", publicPem);

                Regex publicRSAKeyRegex = new Regex(@"-----(BEGIN|END) PUBLIC KEY-----[\W]*");
                var publicPem2 = publicRSAKeyRegex.Replace(publicPem, "").Replace("\n", "");

                //byte[162]
                byte[] rsaPublicKeyBytes = Convert.FromBase64String(publicPem2);

                rsaService.ImportRSAPublicKey(rsaPublicKeyBytes, out _);
                //rsaService.ImportRSAPublicKey(publickey, out _);
                rsaService.ImportRSAPrivateKey(privateKey, out _);

                //xử lý mã hoá giải mã
                encryptedData = RSAEncrypt(dataToEncrypt, rsaService.ExportParameters(false), false);
                decryptedData = RSADecrypt(encryptedData, rsaService.ExportParameters(true), false);
                Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));

                //xử lý chữ ký
                byte[] sign = rsaService.SignData(dataToEncrypt, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                string signBase64 = Convert.ToBase64String(sign);

                bool isSign = rsaService.VerifyData(dataToEncrypt, Convert.FromBase64String(signBase64), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                Console.WriteLine("Encryption failed.");
            }
        }

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
    }
}