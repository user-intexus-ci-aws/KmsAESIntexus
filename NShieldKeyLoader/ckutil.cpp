// ckutil.cpp :
//

#include "stdafx.h"
#include "ckutil.h"
#include "cryptoki.h"

CK_BBOOL true_val = 1;
CK_BBOOL false_val = 0;

CK_OBJECT_CLASS class_public = CKO_PUBLIC_KEY;
CK_OBJECT_CLASS class_private = CKO_PRIVATE_KEY;
CK_OBJECT_CLASS class_secret = CKO_SECRET_KEY;
CK_OBJECT_CLASS class_data = CKO_DATA;
CK_OBJECT_CLASS class_cert = CKO_CERTIFICATE;
CK_OBJECT_CLASS class_domain = CKO_DOMAIN_PARAMETERS;

CK_MECHANISM mechanism_rsa_gen = { CKM_RSA_PKCS_KEY_PAIR_GEN,
NULL_PTR, 0 };
CK_MECHANISM mechanism_rsa_x931_gen = { CKM_RSA_X9_31_KEY_PAIR_GEN,
NULL_PTR, 0 };
CK_MECHANISM mechanism_rsa = { CKM_RSA_PKCS, NULL_PTR, 0 };
CK_MECHANISM mechanism_rsa_raw = { CKM_RSA_X_509, NULL_PTR, 0 };
CK_MECHANISM mechanism_rsa_9796 = { CKM_RSA_9796, NULL_PTR, 0 };

CK_MECHANISM mechanism_sha = { CKM_SHA_1, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha224 = { CKM_SHA224, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha256 = { CKM_SHA256, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha384 = { CKM_SHA384, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha512 = { CKM_SHA512, NULL_PTR, 0 };
CK_MECHANISM mechanism_md2 = { CKM_MD2, NULL_PTR, 0 };
CK_MECHANISM mechanism_md5 = { CKM_MD5, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha_rsa = { CKM_SHA1_RSA_PKCS,
NULL_PTR, 0 };
CK_MECHANISM mechanism_sha256_rsa = { CKM_SHA256_RSA_PKCS,
NULL_PTR, 0 };
CK_MECHANISM mechanism_sha384_rsa = { CKM_SHA384_RSA_PKCS,
NULL_PTR, 0 };
CK_MECHANISM mechanism_sha512_rsa = { CKM_SHA512_RSA_PKCS,
NULL_PTR, 0 };
CK_MECHANISM mechanism_md2_rsa = { CKM_MD2_RSA_PKCS, NULL_PTR, 0 };
CK_MECHANISM mechanism_md5_rsa = { CKM_MD5_RSA_PKCS, NULL_PTR, 0 };
CK_MECHANISM mechanism_dsa_gen = { CKM_DSA_KEY_PAIR_GEN,
NULL_PTR, 0 };
CK_MECHANISM mechanism_dsa = { CKM_DSA, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha_dsa = { CKM_DSA_SHA1, NULL_PTR, 0 };
CK_MECHANISM mechanism_generic_gen = { CKM_GENERIC_SECRET_KEY_GEN,
NULL_PTR, 0 };
CK_MECHANISM mechanism_kcdsa_gen = { CKM_KCDSA_KEY_PAIR_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_kcdsa_sha1 = { CKM_KCDSA_SHA1, NULL_PTR, 0 };
CK_MECHANISM mechanism_kcdsa_has160 = { CKM_KCDSA_HAS160, NULL_PTR, 0 };
CK_MECHANISM mechanism_kcdsa_ripemd160 = { CKM_KCDSA_RIPEMD160, NULL_PTR, 0 };
CK_MECHANISM mechanism_kcdsa_comm = { CKM_KCDSA_PARAMETER_GEN, NULL_PTR, 0 };

CK_MECHANISM mechanism_ec_gen = { CKM_EC_KEY_PAIR_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_ecdsa = { CKM_ECDSA, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha_ecdsa = { CKM_ECDSA_SHA1, NULL_PTR, 0 };

CK_MECHANISM mechanism_des_gen = { CKM_DES_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_des2_gen = { CKM_DES2_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_des3_gen = { CKM_DES3_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_des_ecb = { CKM_DES_ECB, NULL_PTR, 0 };
CK_MECHANISM mechanism_des3_ecb = { CKM_DES3_ECB, NULL_PTR, 0 };

CK_MECHANISM mechanism_des_mac = { CKM_DES_MAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_des3_mac = { CKM_DES3_MAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_cast5_mac = { CKM_CAST5_MAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_cast128_mac = { CKM_CAST5_MAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_aes_mac = { CKM_AES_MAC, NULL_PTR, 0 };

CK_MECHANISM mechanism_cast5_gen = { CKM_CAST5_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_cast5_ecb = { CKM_CAST5_ECB, NULL_PTR, 0 };
CK_MECHANISM mechanism_cast128_gen = { CKM_CAST128_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_cast128_ecb = { CKM_CAST128_ECB, NULL_PTR, 0 };
CK_MECHANISM mechanism_aes_gen = { CKM_AES_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_aes_ecb = { CKM_AES_ECB, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha1_hmac_gen = { CKM_SHA_1_HMAC_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha224_hmac_gen = { CKM_NC_SHA224_HMAC_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha256_hmac_gen = { CKM_NC_SHA256_HMAC_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha384_hmac_gen = { CKM_NC_SHA384_HMAC_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha512_hmac_gen = { CKM_NC_SHA512_HMAC_KEY_GEN, NULL_PTR, 0 };

CK_MECHANISM mechanism_md5_hmac_gen = { CKM_MD5_HMAC_KEY_GEN, NULL_PTR, 0 };

CK_MECHANISM mechanism_sha1_hmac = { CKM_SHA_1_HMAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha224_hmac = { CKM_SHA224_HMAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha256_hmac = { CKM_SHA256_HMAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha384_hmac = { CKM_SHA384_HMAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_sha512_hmac = { CKM_SHA512_HMAC, NULL_PTR, 0 };
CK_MECHANISM mechanism_md5_hmac = { CKM_MD5_HMAC, NULL_PTR, 0 };

CK_MECHANISM mechanism_dh_gen = { CKM_DH_PKCS_KEY_PAIR_GEN,
NULL_PTR, 0 };
CK_MECHANISM mechanism_wrap_crt_rsa = { CKM_WRAP_RSA_CRT_COMPONENTS, NULL_PTR, 0 };
CK_MECHANISM mechanism_seed_gen = { CKM_SEED_KEY_GEN, NULL_PTR, 0 };
CK_MECHANISM mechanism_seed_ecb = { CKM_SEED_ECB, NULL_PTR, 0 };
CK_MECHANISM mechanism_has160 = { CKM_HAS160, NULL_PTR, 0 };
CK_MECHANISM mechanism_ripemd160 = { CKM_RIPEMD160, NULL_PTR, 0 };


CK_BYTE initialization_vector[8] = { 0, 1, 2, 3, 4, 5, 6, 7 };
CK_BYTE initialization_vector_16[16] = { 0, 1, 2, 3, 4, 5, 6, 7,
0, 1, 2, 3, 4, 5, 6, 7 };

CK_MECHANISM mechanism_des_cbc =
{ CKM_DES_CBC, &initialization_vector, sizeof(initialization_vector) };
CK_MECHANISM mechanism_des_cbc_pad =
{ CKM_DES_CBC_PAD, &initialization_vector, sizeof(initialization_vector) };

CK_MECHANISM mechanism_des3_cbc =
{ CKM_DES3_CBC, &initialization_vector, sizeof(initialization_vector) };
CK_MECHANISM mechanism_des3_cbc_pad =
{ CKM_DES3_CBC_PAD, &initialization_vector, sizeof(initialization_vector) };

CK_MECHANISM mechanism_cast5_cbc =
{ CKM_CAST5_CBC, &initialization_vector, sizeof(initialization_vector) };
CK_MECHANISM mechanism_cast5_cbc_pad =
{ CKM_CAST5_CBC_PAD, &initialization_vector, sizeof(initialization_vector) };

CK_MECHANISM mechanism_cast128_cbc =
{ CKM_CAST128_CBC, &initialization_vector, sizeof(initialization_vector) };
CK_MECHANISM mechanism_cast128_cbc_pad =
{ CKM_CAST128_CBC_PAD, &initialization_vector, sizeof(initialization_vector) };

CK_MECHANISM mechanism_seed_cbc =
{ CKM_SEED_CBC, &initialization_vector_16, sizeof(initialization_vector_16) };
CK_MECHANISM mechanism_seed_cbc_pad =
{ CKM_SEED_CBC_PAD, &initialization_vector_16, sizeof(initialization_vector_16) };

CK_MECHANISM mechanism_aes_cbc =
{ CKM_AES_CBC, &initialization_vector_16, sizeof(initialization_vector_16) };
CK_MECHANISM mechanism_aes_cbc_pad =
{ CKM_AES_CBC_PAD, &initialization_vector_16, sizeof(initialization_vector_16) };

CK_RSA_PKCS_PSS_PARAMS pss_sha1_params = { CKM_SHA_1, CKG_MGF1_SHA1, 20 };
CK_RSA_PKCS_PSS_PARAMS pss_sha224_params = { CKM_SHA224, CKG_MGF1_SHA224, 28 };
CK_RSA_PKCS_PSS_PARAMS pss_sha256_params = { CKM_SHA256, CKG_MGF1_SHA256, 32 };
CK_RSA_PKCS_PSS_PARAMS pss_sha384_params = { CKM_SHA384, CKG_MGF1_SHA384, 48 };
CK_RSA_PKCS_PSS_PARAMS pss_sha512_params = { CKM_SHA512, CKG_MGF1_SHA512, 64 };


CK_MECHANISM mechanism_rsa_pss1 = { CKM_RSA_PKCS_PSS,
&pss_sha1_params,
sizeof(pss_sha1_params) };
CK_MECHANISM mechanism_rsa_pss224 = { CKM_RSA_PKCS_PSS,
&pss_sha224_params,
sizeof(pss_sha224_params) };
CK_MECHANISM mechanism_rsa_pss256 = { CKM_RSA_PKCS_PSS,
&pss_sha256_params,
sizeof(pss_sha256_params) };
CK_MECHANISM mechanism_rsa_pss384 = { CKM_RSA_PKCS_PSS,
&pss_sha384_params,
sizeof(pss_sha384_params) };
CK_MECHANISM mechanism_rsa_pss512 = { CKM_RSA_PKCS_PSS,
&pss_sha512_params,
sizeof(pss_sha512_params) };

CK_MECHANISM mechanism_sha_rsa_pss = { CKM_SHA1_RSA_PKCS_PSS,
&pss_sha1_params,
sizeof(pss_sha1_params) };
CK_MECHANISM mechanism_sha224_rsa_pss = { CKM_SHA224_RSA_PKCS_PSS,
&pss_sha224_params,
sizeof(pss_sha224_params) };
CK_MECHANISM mechanism_sha256_rsa_pss = { CKM_SHA256_RSA_PKCS_PSS,
&pss_sha256_params,
sizeof(pss_sha256_params) };
CK_MECHANISM mechanism_sha384_rsa_pss = { CKM_SHA384_RSA_PKCS_PSS,
&pss_sha384_params,
sizeof(pss_sha384_params) };
CK_MECHANISM mechanism_sha512_rsa_pss = { CKM_SHA512_RSA_PKCS_PSS,
&pss_sha512_params,
sizeof(pss_sha512_params) };
/* Custom/vendor mechs s11.20 */
CK_MECHANISM mechanism_public_from_private = { CKM_PUBLIC_FROM_PRIVATE,
NULL_PTR, 0 };

/* NEW OAEP with SHA greater than 1 */
CK_BYTE pOAEPSourceData[] = { 0, 1, 2, 3, 4, 5, 6, 7 };

CK_RSA_PKCS_OAEP_PARAMS oaep_sha1_params = { CKM_SHA_1, CKG_MGF1_SHA1,
0, NULL, 0 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha224_params = { CKM_SHA224, CKG_MGF1_SHA224,
0, NULL, 0 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha256_params = { CKM_SHA256, CKG_MGF1_SHA256,
0, NULL, 0 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha384_params = { CKM_SHA384, CKG_MGF1_SHA384,
0, NULL, 0 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha512_params = { CKM_SHA512, CKG_MGF1_SHA512,
0, NULL, 0 };

CK_RSA_PKCS_OAEP_PARAMS oaep_sha1_params_data = { CKM_SHA_1, CKG_MGF1_SHA1,
CKZ_DATA_SPECIFIED,
pOAEPSourceData,
8 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha224_params_data = { CKM_SHA224, CKG_MGF1_SHA224,
CKZ_DATA_SPECIFIED,
pOAEPSourceData,
8 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha256_params_data = { CKM_SHA256, CKG_MGF1_SHA256,
CKZ_DATA_SPECIFIED,
pOAEPSourceData,
8 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha384_params_data = { CKM_SHA384, CKG_MGF1_SHA384,
CKZ_DATA_SPECIFIED,
pOAEPSourceData,
8 };
CK_RSA_PKCS_OAEP_PARAMS oaep_sha512_params_data = { CKM_SHA512, CKG_MGF1_SHA512,
CKZ_DATA_SPECIFIED,
pOAEPSourceData,
8 };

CK_MECHANISM mechanism_rsa_oaep1 = { CKM_RSA_PKCS_OAEP,
&oaep_sha1_params,
sizeof(oaep_sha1_params) };
CK_MECHANISM mechanism_rsa_oaep224 = { CKM_RSA_PKCS_OAEP,
&oaep_sha224_params,
sizeof(oaep_sha224_params) };
CK_MECHANISM mechanism_rsa_oaep256 = { CKM_RSA_PKCS_OAEP,
&oaep_sha256_params,
sizeof(oaep_sha256_params) };
CK_MECHANISM mechanism_rsa_oaep384 = { CKM_RSA_PKCS_OAEP,
&oaep_sha384_params,
sizeof(oaep_sha384_params) };
CK_MECHANISM mechanism_rsa_oaep512 = { CKM_RSA_PKCS_OAEP,
&oaep_sha512_params,
sizeof(oaep_sha512_params) };

CK_MECHANISM mechanism_rsa_oaep1_data = { CKM_RSA_PKCS_OAEP,
&oaep_sha1_params_data,
sizeof(oaep_sha1_params_data) };
CK_MECHANISM mechanism_rsa_oaep224_data = { CKM_RSA_PKCS_OAEP,
&oaep_sha224_params_data,
sizeof(oaep_sha224_params_data) };
CK_MECHANISM mechanism_rsa_oaep256_data = { CKM_RSA_PKCS_OAEP,
&oaep_sha256_params_data,
sizeof(oaep_sha256_params_data) };
CK_MECHANISM mechanism_rsa_oaep384_data = { CKM_RSA_PKCS_OAEP,
&oaep_sha384_params_data,
sizeof(oaep_sha384_params_data) };
CK_MECHANISM mechanism_rsa_oaep512_data = { CKM_RSA_PKCS_OAEP,
&oaep_sha512_params_data,
sizeof(oaep_sha512_params_data) };


CK_KEY_TYPE key_type_rsa = CKK_RSA;
CK_KEY_TYPE key_type_dsa = CKK_DSA;
CK_KEY_TYPE key_type_dh = CKK_DH;
CK_KEY_TYPE key_type_kcdsa = CKK_KCDSA;
CK_KEY_TYPE key_type_ec = CKK_EC;
CK_KEY_TYPE key_type_kea = CKK_KEA;
CK_KEY_TYPE key_type_generic_secret = CKK_GENERIC_SECRET;
CK_KEY_TYPE key_type_rc2 = CKK_RC2;
CK_KEY_TYPE key_type_des = CKK_DES;
CK_KEY_TYPE key_type_des2 = CKK_DES2;
CK_KEY_TYPE key_type_des3 = CKK_DES3;
CK_KEY_TYPE key_type_aes = CKK_AES;
CK_KEY_TYPE key_type_seed = CKK_SEED;
CK_KEY_TYPE key_type_cast = CKK_CAST;
CK_KEY_TYPE key_type_cast3 = CKK_CAST3;
CK_KEY_TYPE key_type_cast5 = CKK_CAST5;
CK_KEY_TYPE key_type_cast128 = CKK_CAST128;
CK_KEY_TYPE key_type_rc5 = CKK_RC5;
CK_KEY_TYPE key_type_idea = CKK_IDEA;
CK_KEY_TYPE key_type_skipjack = CKK_SKIPJACK;
CK_KEY_TYPE key_type_baton = CKK_BATON;
CK_KEY_TYPE key_type_juniper = CKK_JUNIPER;
CK_KEY_TYPE key_type_cdmf = CKK_CDMF;
CK_KEY_TYPE key_type_sha1hmac = CKK_SHA_1_HMAC;
CK_KEY_TYPE key_type_md5hmac = CKK_MD5_HMAC;
CK_KEY_TYPE key_type_sha224hmac = CKK_SHA224_HMAC;
CK_KEY_TYPE key_type_sha256hmac = CKK_SHA256_HMAC;
CK_KEY_TYPE key_type_sha384hmac = CKK_SHA384_HMAC;
CK_KEY_TYPE key_type_sha512hmac = CKK_SHA512_HMAC;

CK_CERTIFICATE_TYPE ckc_x_509 = CKC_X_509;
