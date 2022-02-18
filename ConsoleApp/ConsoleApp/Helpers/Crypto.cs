using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace MaM.Helpers
{
  internal static class Crypto
  {
    // Call this function to remove the key from memory after use for security
    [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
    private static extern bool ZeroMemory(IntPtr destination, int length);

    // Generate a Key that's 64 bits / 8 bytes.
    // Distribute this key to the user who will decrypt this file.
    public static string GenerateKey()
    {
      return Encoding.Unicode.GetString(DES.Create().Key);
    }

    public static void EncryptFile(string inputFilename, string outputFilename,string key)
    {
      

      var fsInput = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);

      var fsEncrypted = new FileStream(outputFilename, FileMode.Create, FileAccess.Write);
      
      var des = new DESCryptoServiceProvider
      {
        Key = Encoding.ASCII.GetBytes(key),
        IV = Encoding.ASCII.GetBytes(key)
      };

      var desEncrypt = des.CreateEncryptor();
      var cryptoStream = new CryptoStream(fsEncrypted, desEncrypt, CryptoStreamMode.Write);

      var byteArrayInput = new byte[fsInput.Length];
      _ = fsInput.Read(byteArrayInput, 0, byteArrayInput.Length);
      
      cryptoStream.Write(byteArrayInput, 0, byteArrayInput.Length);
      
      cryptoStream.Close();
      fsInput.Close();
      fsEncrypted.Close();
    }

    public static void DecryptFile(string inputFilename, string outputFilename, string key)
    {
      // A 64 bit key and IV is required for this provider.
      var des = new DESCryptoServiceProvider
      {
        Key = Encoding.ASCII.GetBytes(key), 
        IV = Encoding.ASCII.GetBytes(key)
      };

      var fsRead = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);

      var desDecrypt = des.CreateDecryptor();

      var cryptoStream = new CryptoStream(fsRead, desDecrypt, CryptoStreamMode.Read);

      var fsDecrypted = new StreamWriter(outputFilename);
      fsDecrypted.Write(new StreamReader(cryptoStream).ReadToEnd());

      fsDecrypted.Flush();
      fsDecrypted.Close();
    }
  }
}
