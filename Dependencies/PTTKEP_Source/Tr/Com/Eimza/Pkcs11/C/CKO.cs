namespace Tr.Com.Eimza.Pkcs11.C
{
	internal enum CKO : uint
	{
		CKO_DATA = 0u,
		CKO_CERTIFICATE = 1u,
		CKO_PUBLIC_KEY = 2u,
		CKO_PRIVATE_KEY = 3u,
		CKO_SECRET_KEY = 4u,
		CKO_HW_FEATURE = 5u,
		CKO_DOMAIN_PARAMETERS = 6u,
		CKO_MECHANISM = 7u,
		CKO_OTP_KEY = 8u,
		CKO_VENDOR_DEFINED = 2147483648u
	}
}
