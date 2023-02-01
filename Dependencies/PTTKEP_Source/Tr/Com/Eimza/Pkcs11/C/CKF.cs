namespace Tr.Com.Eimza.Pkcs11.C
{
	internal class CKF
	{
		public const uint CKF_TOKEN_PRESENT = 1u;

		public const uint CKF_REMOVABLE_DEVICE = 2u;

		public const uint CKF_HW_SLOT = 4u;

		public const uint CKF_RNG = 1u;

		public const uint CKF_WRITE_PROTECTED = 2u;

		public const uint CKF_LOGIN_REQUIRED = 4u;

		public const uint CKF_USER_PIN_INITIALIZED = 8u;

		public const uint CKF_RESTORE_KEY_NOT_NEEDED = 32u;

		public const uint CKF_CLOCK_ON_TOKEN = 64u;

		public const uint CKF_PROTECTED_AUTHENTICATION_PATH = 256u;

		public const uint CKF_DUAL_CRYPTO_OPERATIONS = 512u;

		public const uint CKF_TOKEN_INITIALIZED = 1024u;

		public const uint CKF_SECONDARY_AUTHENTICATION = 2048u;

		public const uint CKF_USER_PIN_COUNT_LOW = 65536u;

		public const uint CKF_USER_PIN_FINAL_TRY = 131072u;

		public const uint CKF_USER_PIN_LOCKED = 262144u;

		public const uint CKF_USER_PIN_TO_BE_CHANGED = 524288u;

		public const uint CKF_SO_PIN_COUNT_LOW = 1048576u;

		public const uint CKF_SO_PIN_FINAL_TRY = 2097152u;

		public const uint CKF_SO_PIN_LOCKED = 4194304u;

		public const uint CKF_SO_PIN_TO_BE_CHANGED = 8388608u;

		public const uint CKF_RW_SESSION = 2u;

		public const uint CKF_SERIAL_SESSION = 4u;

		public const uint CKF_ARRAY_ATTRIBUTE = 1073741824u;

		public const uint CKF_HW = 1u;

		public const uint CKF_ENCRYPT = 256u;

		public const uint CKF_DECRYPT = 512u;

		public const uint CKF_DIGEST = 1024u;

		public const uint CKF_SIGN = 2048u;

		public const uint CKF_SIGN_RECOVER = 4096u;

		public const uint CKF_VERIFY = 8192u;

		public const uint CKF_VERIFY_RECOVER = 16384u;

		public const uint CKF_GENERATE = 32768u;

		public const uint CKF_GENERATE_KEY_PAIR = 65536u;

		public const uint CKF_WRAP = 131072u;

		public const uint CKF_UNWRAP = 262144u;

		public const uint CKF_DERIVE = 524288u;

		public const uint CKF_EC_F_P = 1048576u;

		public const uint CKF_EC_F_2M = 2097152u;

		public const uint CKF_EC_ECPARAMETERS = 4194304u;

		public const uint CKF_EC_NAMEDCURVE = 8388608u;

		public const uint CKF_EC_UNCOMPRESS = 16777216u;

		public const uint CKF_EC_COMPRESS = 33554432u;

		public const uint CKF_EXTENSION = 2147483648u;

		public const uint CKF_LIBRARY_CANT_CREATE_OS_THREADS = 1u;

		public const uint CKF_OS_LOCKING_OK = 2u;

		public const uint CKF_DONT_BLOCK = 1u;

		public const uint CKF_NEXT_OTP = 1u;

		public const uint CKF_EXCLUDE_TIME = 2u;

		public const uint CKF_EXCLUDE_COUNTER = 4u;

		public const uint CKF_EXCLUDE_CHALLENGE = 8u;

		public const uint CKF_EXCLUDE_PIN = 16u;

		public const uint CKF_USER_FRIENDLY_OTP = 32u;
	}
}
