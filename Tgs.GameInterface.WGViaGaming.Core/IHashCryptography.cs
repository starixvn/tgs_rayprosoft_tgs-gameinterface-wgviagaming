namespace Tgs.GameInterface.WGViaGaming.Core;

public interface IHashCryptography
{
    ValueTask<string> GetAesEncryptAsync(
        string data,
        string secretKey,
        string secretIV,
        CancellationToken cancellationToken = default);
}
