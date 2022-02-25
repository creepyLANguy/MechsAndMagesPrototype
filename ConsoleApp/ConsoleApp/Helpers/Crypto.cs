using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MaM.Helpers
{
 public static class Crypto
  {
    // Generate a Key that's 64 bits / 8 bytes.
    // Distribute this key to the user who will decrypt this file.
    public static string GenerateKey()
    {
      return Encoding.Unicode.GetString(DES.Create().Key);
    }

    public static string EncryptString(string content, string key)
    {
      var contentBytes = Encoding.ASCII.GetBytes(content);
      var keyBytes = Encoding.ASCII.GetBytes(key);

      var des = new DESCryptoServiceProvider
      {
        Key = keyBytes,
        IV = keyBytes
      };

      var memStream = new MemoryStream();
      var cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
      cryptoStream.Write(contentBytes, 0, contentBytes.Length);
      cryptoStream.FlushFinalBlock();

      var encryptedContentBytes = new byte[memStream.Length];
      memStream.Position = 0;
      _ = memStream.Read(encryptedContentBytes, 0, encryptedContentBytes.Length);

      var encryptedContent = Convert.ToBase64String(encryptedContentBytes);

      return encryptedContent;
    }

    public static string DecryptString(string content, string key)
    {
      var encryptedContentBytes = Convert.FromBase64String(content);
      var keyBytes = Encoding.ASCII.GetBytes(key);

      var provider = new DESCryptoServiceProvider();
      var transform = provider.CreateDecryptor(keyBytes, keyBytes);

      var memStream = new MemoryStream();
      var cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
      cryptoStream.Write(encryptedContentBytes, 0, encryptedContentBytes.Length);
      cryptoStream.FlushFinalBlock();

      var decryptedContentBytes = new byte[memStream.Length];
      memStream.Position = 0;
      _ = memStream.Read(decryptedContentBytes, 0, decryptedContentBytes.Length);

      var decryptedContent = Encoding.UTF8.GetString(decryptedContentBytes, 0, decryptedContentBytes.Length);

      return decryptedContent;

    }

    public static void EncryptFile(string inputFilename, string outputFilename, string key)
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
