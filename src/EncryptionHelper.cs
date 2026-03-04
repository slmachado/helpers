using System.Security.Cryptography;

namespace Helpers;

/// <summary>
/// Provides simple methods for symmetric encryption and decryption using AES-256.
/// </summary>
public static class EncryptionHelper
{
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int SaltSize = 16;
    private const int Iterations = 100000;

    /// <summary>
    /// Encrypts a string using a password.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <param name="password">The password used for encryption.</param>
    /// <returns>A base64 encoded string containing the salt, IV, and ciphertext.</returns>
    public static string Encrypt(string plainText, string password)
    {
        if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        using var rfc = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] key = rfc.GetBytes(KeySize / 8);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        byte[] iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        
        // Write Salt + IV + Data
        ms.Write(salt, 0, salt.Length);
        ms.Write(iv, 0, iv.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Decrypts a string using a password.
    /// </summary>
    /// <param name="cipherText">The base64 encoded string containing salt, IV, and ciphertext.</param>
    /// <param name="password">The password used for decryption.</param>
    /// <returns>The decrypted plain text.</returns>
    public static string Decrypt(string cipherText, string password)
    {
        if (string.IsNullOrEmpty(cipherText)) throw new ArgumentNullException(nameof(cipherText));
        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

        byte[] fullCipher = Convert.FromBase64String(cipherText);
        
        byte[] salt = new byte[SaltSize];
        byte[] iv = new byte[BlockSize / 8];
        byte[] cipher = new byte[fullCipher.Length - SaltSize - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(fullCipher, SaltSize, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, SaltSize + iv.Length, cipher, 0, cipher.Length);

        using var rfc = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] key = rfc.GetBytes(KeySize / 8);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);

        return reader.ReadToEnd();
    }
}
