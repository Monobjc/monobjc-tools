namespace Monobjc.Tools.Utilities
{
    internal enum SecItemClass : uint
    {
        kSecInternetPasswordItemClass = 0x696E6574, // 'inet'
        kSecGenericPasswordItemClass = 0x67656E70, // 'genp'
        kSecAppleSharePasswordItemClass = 0x61736870, // 'ashp'
        kSecCertificateItemClass = CSSM_DL_DB_RECORD.CSSM_DL_DB_RECORD_X509_CERTIFICATE,
        kSecPublicKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_PUBLIC_KEY,
        kSecPrivateKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_PRIVATE_KEY,
        kSecSymmetricKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_SYMMETRIC_KEY
    }
}