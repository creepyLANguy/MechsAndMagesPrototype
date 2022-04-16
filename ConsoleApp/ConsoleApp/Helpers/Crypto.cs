using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MaM.Helpers
{
 public static class Crypto
  {
    // Generate a 64-bit/8-byte key.
    // Symmetrically used, so safely distribute to authorized parties that may decrypt the file.
    public static string GenerateKey()
    {
      return Encoding.Unicode.GetString(DES.Create().Key);
    }

    public static string EncryptString(string input, string key)
    {
      var inputBytes = Encoding.ASCII.GetBytes(input);
      var transform = GetDesEncryptor(key);
      return PerformDesTransform(inputBytes, transform);
    }

    public static string DecryptString(string input, string key)
    {
      var inputBytes = Convert.FromBase64String(input);
      var transform = GetDesDecryptor(key);
      return PerformDesTransform(inputBytes, transform);
    }

    public static void EncryptFile(string inputFilename, string outputFilename, string key)
    {
      var fsInput = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);

      var fsOutput = new FileStream(outputFilename, FileMode.Create, FileAccess.Write);

      var des = GetDesProvider(key);
      var transform = des.CreateEncryptor();
      var cryptoStream = new CryptoStream(fsOutput, transform, CryptoStreamMode.Write);

      var byteArrayInput = new byte[fsInput.Length];
      _ = fsInput.Read(byteArrayInput, 0, byteArrayInput.Length);
      
      cryptoStream.Write(byteArrayInput, 0, byteArrayInput.Length);

      cryptoStream.Flush();
      cryptoStream.Close();
      fsOutput.Close();
      fsInput.Close();
    }

    public static void DecryptFile(string inputFilename, string outputFilename, string key)
    {
      var fsInput = new FileStream(inputFilename, FileMode.Open, FileAccess.Read);
     
      var fsOutput = new StreamWriter(outputFilename);
      
      var des = GetDesProvider(key);
      var transform = des.CreateDecryptor();
      var cryptoStream = new CryptoStream(fsInput, transform, CryptoStreamMode.Read);

      fsOutput.Write(new StreamReader(cryptoStream).ReadToEnd());

      cryptoStream.Close();
      fsOutput.Flush();
      fsOutput.Close();
      fsInput.Close();
    }

    private static DESCryptoServiceProvider GetDesProvider(string key)
    {
      // A 64-bit key is required for this provider.
      // Use the same key as the IV.
      return new DESCryptoServiceProvider
      {
        Key = Encoding.ASCII.GetBytes(key),
        IV = Encoding.ASCII.GetBytes(key)
      };
    }

    private static ICryptoTransform GetDesEncryptor(string key)
    {
      var des = GetDesProvider(key);
      return des.CreateEncryptor(des.Key, des.IV);
    }    

    private static ICryptoTransform GetDesDecryptor(string key)
    {
      var des = GetDesProvider(key);
      return des.CreateDecryptor(des.Key, des.IV);
    }

    private static string PerformDesTransform(byte[] inputBytes, ICryptoTransform transform)
    {
      var memStream = new MemoryStream();

      var cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
      cryptoStream.Write(inputBytes, 0, inputBytes.Length);
      cryptoStream.FlushFinalBlock();

      var outputBytes = new byte[memStream.Length];
      memStream.Position = 0;
      _ = memStream.Read(outputBytes, 0, outputBytes.Length);

      var output = Convert.ToBase64String(outputBytes);
      return output;
    }
  }
}
