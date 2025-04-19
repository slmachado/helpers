using System.Net;
using System.Net.Security;
// ReSharper disable InconsistentNaming

namespace Helpers;

public static class SslHelper
{
    private static readonly RemoteCertificateValidationCallback? _defaultValidationCallback = ServicePointManager.ServerCertificateValidationCallback;

    /// <summary>
    /// Ignores all SSL certificate warnings. Use only for development purposes.
    /// </summary>
    public static void IgnoreCertificateWarning()
    {
        ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;
    }

    /// <summary>
    /// Restores the default SSL certificate validation behavior.
    /// </summary>
    public static void RestoreCertificateValidation()
    {
        ServicePointManager.ServerCertificateValidationCallback = _defaultValidationCallback;
    }

    /// <summary>
    /// Sets a custom SSL certificate validation callback.
    /// </summary>
    /// <param name="validationCallback">The custom validation callback to use.</param>
    public static void SetCustomCertificateValidation(RemoteCertificateValidationCallback validationCallback)
    {
        ServicePointManager.ServerCertificateValidationCallback = validationCallback;
    }

    /// <summary>
    /// Temporarily ignores SSL certificate warnings for a specific action.
    /// </summary>
    /// <param name="action">The action to execute while ignoring SSL certificate warnings.</param>
    public static void IgnoreCertificateWarningTemporarily(Action action)
    {
        var originalCallback = ServicePointManager.ServerCertificateValidationCallback;
        try
        {
            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
            action();
        }
        finally
        {
            ServicePointManager.ServerCertificateValidationCallback = originalCallback;
        }
    }
}
