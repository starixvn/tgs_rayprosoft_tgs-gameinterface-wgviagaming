using System.Security.Cryptography;
using System.Text;

namespace Tgs.GameInterface.WGViaGaming.Core;

public class HashCryptography : IHashCryptography
{
	public async ValueTask<string> GetAesEncryptAsync(
		string data,
		string secretKey,
		string secretIV,
		CancellationToken cancellationToken = default)
	{
		byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
		byte[] ivBytes = Encoding.UTF8.GetBytes(secretIV);
		byte[] dataBytes = Encoding.UTF8.GetBytes(data);
		using var aes = Aes.Create();
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.KeySize = 128;
		aes.BlockSize = 128;
		aes.Key = keyBytes;
		aes.IV = ivBytes;

		byte[] encrypted;
		using var cryptoTransform = aes.CreateEncryptor();
		using var memoryStream = new MemoryStream();
		using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
		await cryptoStream
				.WriteAsync(
					buffer: dataBytes,
					cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		await cryptoStream
				.FlushFinalBlockAsync(
					cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		encrypted = memoryStream.ToArray();

		var base64Str = Convert.ToBase64String(encrypted, 0, encrypted.Length);

		return base64Str;
	}
}
