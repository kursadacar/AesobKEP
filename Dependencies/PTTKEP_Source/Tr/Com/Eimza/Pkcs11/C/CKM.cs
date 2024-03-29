namespace Tr.Com.Eimza.Pkcs11.C
{
	internal enum CKM : uint
	{
		CKM_RSA_PKCS_KEY_PAIR_GEN = 0u,
		CKM_RSA_PKCS = 1u,
		CKM_RSA_9796 = 2u,
		CKM_RSA_X_509 = 3u,
		CKM_MD2_RSA_PKCS = 4u,
		CKM_MD5_RSA_PKCS = 5u,
		CKM_SHA1_RSA_PKCS = 6u,
		CKM_RIPEMD128_RSA_PKCS = 7u,
		CKM_RIPEMD160_RSA_PKCS = 8u,
		CKM_RSA_PKCS_OAEP = 9u,
		CKM_RSA_X9_31_KEY_PAIR_GEN = 10u,
		CKM_RSA_X9_31 = 11u,
		CKM_SHA1_RSA_X9_31 = 12u,
		CKM_RSA_PKCS_PSS = 13u,
		CKM_SHA1_RSA_PKCS_PSS = 14u,
		CKM_DSA_KEY_PAIR_GEN = 16u,
		CKM_DSA = 17u,
		CKM_DSA_SHA1 = 18u,
		CKM_DH_PKCS_KEY_PAIR_GEN = 32u,
		CKM_DH_PKCS_DERIVE = 33u,
		CKM_X9_42_DH_KEY_PAIR_GEN = 48u,
		CKM_X9_42_DH_DERIVE = 49u,
		CKM_X9_42_DH_HYBRID_DERIVE = 50u,
		CKM_X9_42_MQV_DERIVE = 51u,
		CKM_SHA256_RSA_PKCS = 64u,
		CKM_SHA384_RSA_PKCS = 65u,
		CKM_SHA512_RSA_PKCS = 66u,
		CKM_SHA256_RSA_PKCS_PSS = 67u,
		CKM_SHA384_RSA_PKCS_PSS = 68u,
		CKM_SHA512_RSA_PKCS_PSS = 69u,
		CKM_SHA224_RSA_PKCS = 70u,
		CKM_SHA224_RSA_PKCS_PSS = 71u,
		CKM_RC2_KEY_GEN = 256u,
		CKM_RC2_ECB = 257u,
		CKM_RC2_CBC = 258u,
		CKM_RC2_MAC = 259u,
		CKM_RC2_MAC_GENERAL = 260u,
		CKM_RC2_CBC_PAD = 261u,
		CKM_RC4_KEY_GEN = 272u,
		CKM_RC4 = 273u,
		CKM_DES_KEY_GEN = 288u,
		CKM_DES_ECB = 289u,
		CKM_DES_CBC = 290u,
		CKM_DES_MAC = 291u,
		CKM_DES_MAC_GENERAL = 292u,
		CKM_DES_CBC_PAD = 293u,
		CKM_DES2_KEY_GEN = 304u,
		CKM_DES3_KEY_GEN = 305u,
		CKM_DES3_ECB = 306u,
		CKM_DES3_CBC = 307u,
		CKM_DES3_MAC = 308u,
		CKM_DES3_MAC_GENERAL = 309u,
		CKM_DES3_CBC_PAD = 310u,
		CKM_CDMF_KEY_GEN = 320u,
		CKM_CDMF_ECB = 321u,
		CKM_CDMF_CBC = 322u,
		CKM_CDMF_MAC = 323u,
		CKM_CDMF_MAC_GENERAL = 324u,
		CKM_CDMF_CBC_PAD = 325u,
		CKM_DES_OFB64 = 336u,
		CKM_DES_OFB8 = 337u,
		CKM_DES_CFB64 = 338u,
		CKM_DES_CFB8 = 339u,
		CKM_MD2 = 512u,
		CKM_MD2_HMAC = 513u,
		CKM_MD2_HMAC_GENERAL = 514u,
		CKM_MD5 = 528u,
		CKM_MD5_HMAC = 529u,
		CKM_MD5_HMAC_GENERAL = 530u,
		CKM_SHA_1 = 544u,
		CKM_SHA_1_HMAC = 545u,
		CKM_SHA_1_HMAC_GENERAL = 546u,
		CKM_RIPEMD128 = 560u,
		CKM_RIPEMD128_HMAC = 561u,
		CKM_RIPEMD128_HMAC_GENERAL = 562u,
		CKM_RIPEMD160 = 576u,
		CKM_RIPEMD160_HMAC = 577u,
		CKM_RIPEMD160_HMAC_GENERAL = 578u,
		CKM_SHA256 = 592u,
		CKM_SHA256_HMAC = 593u,
		CKM_SHA256_HMAC_GENERAL = 594u,
		CKM_SHA224 = 597u,
		CKM_SHA224_HMAC = 598u,
		CKM_SHA224_HMAC_GENERAL = 599u,
		CKM_SHA384 = 608u,
		CKM_SHA384_HMAC = 609u,
		CKM_SHA384_HMAC_GENERAL = 610u,
		CKM_SHA512 = 624u,
		CKM_SHA512_HMAC = 625u,
		CKM_SHA512_HMAC_GENERAL = 626u,
		CKM_SECURID_KEY_GEN = 640u,
		CKM_SECURID = 642u,
		CKM_HOTP_KEY_GEN = 656u,
		CKM_HOTP = 657u,
		CKM_ACTI = 672u,
		CKM_ACTI_KEY_GEN = 673u,
		CKM_CAST_KEY_GEN = 768u,
		CKM_CAST_ECB = 769u,
		CKM_CAST_CBC = 770u,
		CKM_CAST_MAC = 771u,
		CKM_CAST_MAC_GENERAL = 772u,
		CKM_CAST_CBC_PAD = 773u,
		CKM_CAST3_KEY_GEN = 784u,
		CKM_CAST3_ECB = 785u,
		CKM_CAST3_CBC = 786u,
		CKM_CAST3_MAC = 787u,
		CKM_CAST3_MAC_GENERAL = 788u,
		CKM_CAST3_CBC_PAD = 789u,
		CKM_CAST5_KEY_GEN = 800u,
		CKM_CAST128_KEY_GEN = 800u,
		CKM_CAST5_ECB = 801u,
		CKM_CAST128_ECB = 801u,
		CKM_CAST5_CBC = 802u,
		CKM_CAST128_CBC = 802u,
		CKM_CAST5_MAC = 803u,
		CKM_CAST128_MAC = 803u,
		CKM_CAST5_MAC_GENERAL = 804u,
		CKM_CAST128_MAC_GENERAL = 804u,
		CKM_CAST5_CBC_PAD = 805u,
		CKM_CAST128_CBC_PAD = 805u,
		CKM_RC5_KEY_GEN = 816u,
		CKM_RC5_ECB = 817u,
		CKM_RC5_CBC = 818u,
		CKM_RC5_MAC = 819u,
		CKM_RC5_MAC_GENERAL = 820u,
		CKM_RC5_CBC_PAD = 821u,
		CKM_IDEA_KEY_GEN = 832u,
		CKM_IDEA_ECB = 833u,
		CKM_IDEA_CBC = 834u,
		CKM_IDEA_MAC = 835u,
		CKM_IDEA_MAC_GENERAL = 836u,
		CKM_IDEA_CBC_PAD = 837u,
		CKM_GENERIC_SECRET_KEY_GEN = 848u,
		CKM_CONCATENATE_BASE_AND_KEY = 864u,
		CKM_CONCATENATE_BASE_AND_DATA = 866u,
		CKM_CONCATENATE_DATA_AND_BASE = 867u,
		CKM_XOR_BASE_AND_DATA = 868u,
		CKM_EXTRACT_KEY_FROM_KEY = 869u,
		CKM_SSL3_PRE_MASTER_KEY_GEN = 880u,
		CKM_SSL3_MASTER_KEY_DERIVE = 881u,
		CKM_SSL3_KEY_AND_MAC_DERIVE = 882u,
		CKM_SSL3_MASTER_KEY_DERIVE_DH = 883u,
		CKM_TLS_PRE_MASTER_KEY_GEN = 884u,
		CKM_TLS_MASTER_KEY_DERIVE = 885u,
		CKM_TLS_KEY_AND_MAC_DERIVE = 886u,
		CKM_TLS_MASTER_KEY_DERIVE_DH = 887u,
		CKM_TLS_PRF = 888u,
		CKM_SSL3_MD5_MAC = 896u,
		CKM_SSL3_SHA1_MAC = 897u,
		CKM_MD5_KEY_DERIVATION = 912u,
		CKM_MD2_KEY_DERIVATION = 913u,
		CKM_SHA1_KEY_DERIVATION = 914u,
		CKM_SHA256_KEY_DERIVATION = 915u,
		CKM_SHA384_KEY_DERIVATION = 916u,
		CKM_SHA512_KEY_DERIVATION = 917u,
		CKM_SHA224_KEY_DERIVATION = 918u,
		CKM_PBE_MD2_DES_CBC = 928u,
		CKM_PBE_MD5_DES_CBC = 929u,
		CKM_PBE_MD5_CAST_CBC = 930u,
		CKM_PBE_MD5_CAST3_CBC = 931u,
		CKM_PBE_MD5_CAST5_CBC = 932u,
		CKM_PBE_MD5_CAST128_CBC = 932u,
		CKM_PBE_SHA1_CAST5_CBC = 933u,
		CKM_PBE_SHA1_CAST128_CBC = 933u,
		CKM_PBE_SHA1_RC4_128 = 934u,
		CKM_PBE_SHA1_RC4_40 = 935u,
		CKM_PBE_SHA1_DES3_EDE_CBC = 936u,
		CKM_PBE_SHA1_DES2_EDE_CBC = 937u,
		CKM_PBE_SHA1_RC2_128_CBC = 938u,
		CKM_PBE_SHA1_RC2_40_CBC = 939u,
		CKM_PKCS5_PBKD2 = 944u,
		CKM_PBA_SHA1_WITH_SHA1_HMAC = 960u,
		CKM_WTLS_PRE_MASTER_KEY_GEN = 976u,
		CKM_WTLS_MASTER_KEY_DERIVE = 977u,
		CKM_WTLS_MASTER_KEY_DERIVE_DH_ECC = 978u,
		CKM_WTLS_PRF = 979u,
		CKM_WTLS_SERVER_KEY_AND_MAC_DERIVE = 980u,
		CKM_WTLS_CLIENT_KEY_AND_MAC_DERIVE = 981u,
		CKM_KEY_WRAP_LYNKS = 1024u,
		CKM_KEY_WRAP_SET_OAEP = 1025u,
		CKM_CMS_SIG = 1280u,
		CKM_KIP_DERIVE = 1296u,
		CKM_KIP_WRAP = 1297u,
		CKM_KIP_MAC = 1298u,
		CKM_CAMELLIA_KEY_GEN = 1360u,
		CKM_CAMELLIA_ECB = 1361u,
		CKM_CAMELLIA_CBC = 1362u,
		CKM_CAMELLIA_MAC = 1363u,
		CKM_CAMELLIA_MAC_GENERAL = 1364u,
		CKM_CAMELLIA_CBC_PAD = 1365u,
		CKM_CAMELLIA_ECB_ENCRYPT_DATA = 1366u,
		CKM_CAMELLIA_CBC_ENCRYPT_DATA = 1367u,
		CKM_CAMELLIA_CTR = 1368u,
		CKM_ARIA_KEY_GEN = 1376u,
		CKM_ARIA_ECB = 1377u,
		CKM_ARIA_CBC = 1378u,
		CKM_ARIA_MAC = 1379u,
		CKM_ARIA_MAC_GENERAL = 1380u,
		CKM_ARIA_CBC_PAD = 1381u,
		CKM_ARIA_ECB_ENCRYPT_DATA = 1382u,
		CKM_ARIA_CBC_ENCRYPT_DATA = 1383u,
		CKM_SKIPJACK_KEY_GEN = 4096u,
		CKM_SKIPJACK_ECB64 = 4097u,
		CKM_SKIPJACK_CBC64 = 4098u,
		CKM_SKIPJACK_OFB64 = 4099u,
		CKM_SKIPJACK_CFB64 = 4100u,
		CKM_SKIPJACK_CFB32 = 4101u,
		CKM_SKIPJACK_CFB16 = 4102u,
		CKM_SKIPJACK_CFB8 = 4103u,
		CKM_SKIPJACK_WRAP = 4104u,
		CKM_SKIPJACK_PRIVATE_WRAP = 4105u,
		CKM_SKIPJACK_RELAYX = 4106u,
		CKM_KEA_KEY_PAIR_GEN = 4112u,
		CKM_KEA_KEY_DERIVE = 4113u,
		CKM_FORTEZZA_TIMESTAMP = 4128u,
		CKM_BATON_KEY_GEN = 4144u,
		CKM_BATON_ECB128 = 4145u,
		CKM_BATON_ECB96 = 4146u,
		CKM_BATON_CBC128 = 4147u,
		CKM_BATON_COUNTER = 4148u,
		CKM_BATON_SHUFFLE = 4149u,
		CKM_BATON_WRAP = 4150u,
		CKM_ECDSA_KEY_PAIR_GEN = 4160u,
		CKM_EC_KEY_PAIR_GEN = 4160u,
		CKM_ECDSA = 4161u,
		CKM_ECDSA_SHA1 = 4162u,
		CKM_ECDH1_DERIVE = 4176u,
		CKM_ECDH1_COFACTOR_DERIVE = 4177u,
		CKM_ECMQV_DERIVE = 4178u,
		CKM_JUNIPER_KEY_GEN = 4192u,
		CKM_JUNIPER_ECB128 = 4193u,
		CKM_JUNIPER_CBC128 = 4194u,
		CKM_JUNIPER_COUNTER = 4195u,
		CKM_JUNIPER_SHUFFLE = 4196u,
		CKM_JUNIPER_WRAP = 4197u,
		CKM_FASTHASH = 4208u,
		CKM_AES_KEY_GEN = 4224u,
		CKM_AES_ECB = 4225u,
		CKM_AES_CBC = 4226u,
		CKM_AES_MAC = 4227u,
		CKM_AES_MAC_GENERAL = 4228u,
		CKM_AES_CBC_PAD = 4229u,
		CKM_AES_CTR = 4230u,
		CKM_BLOWFISH_KEY_GEN = 4240u,
		CKM_BLOWFISH_CBC = 4241u,
		CKM_TWOFISH_KEY_GEN = 4242u,
		CKM_TWOFISH_CBC = 4243u,
		CKM_DES_ECB_ENCRYPT_DATA = 4352u,
		CKM_DES_CBC_ENCRYPT_DATA = 4353u,
		CKM_DES3_ECB_ENCRYPT_DATA = 4354u,
		CKM_DES3_CBC_ENCRYPT_DATA = 4355u,
		CKM_AES_ECB_ENCRYPT_DATA = 4356u,
		CKM_AES_CBC_ENCRYPT_DATA = 4357u,
		CKM_DSA_PARAMETER_GEN = 8192u,
		CKM_DH_PKCS_PARAMETER_GEN = 8193u,
		CKM_X9_42_DH_PARAMETER_GEN = 8194u,
		CKM_VENDOR_DEFINED = 2147483648u
	}
}
