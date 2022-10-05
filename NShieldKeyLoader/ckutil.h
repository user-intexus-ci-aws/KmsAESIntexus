#ifndef __CKUTIL_H
#define __CKUTIL_H 1

#include "cryptoki.h"

#define NULL 0

#define NUM(a) (sizeof(a) / sizeof((a)[0]))

extern CK_BBOOL true_val;
extern CK_BBOOL false_val;

extern CK_OBJECT_CLASS class_public;
extern CK_OBJECT_CLASS class_private;
extern CK_OBJECT_CLASS class_secret;
extern CK_OBJECT_CLASS class_data;
extern CK_OBJECT_CLASS class_cert;
extern CK_OBJECT_CLASS class_domain;

extern CK_MECHANISM mechanism_rsa_gen;
extern CK_MECHANISM mechanism_rsa_x931_gen;
extern CK_MECHANISM mechanism_rsa;
extern CK_MECHANISM mechanism_rsa_raw;
extern CK_MECHANISM mechanism_rsa_9796;
extern CK_MECHANISM mechanism_sha;
extern CK_MECHANISM mechanism_sha224;
extern CK_MECHANISM mechanism_sha256;
extern CK_MECHANISM mechanism_sha384;
extern CK_MECHANISM mechanism_sha512;
extern CK_MECHANISM mechanism_md2;
extern CK_MECHANISM mechanism_md5;
extern CK_MECHANISM mechanism_sha_rsa;
extern CK_MECHANISM mechanism_sha256_rsa;
extern CK_MECHANISM mechanism_sha384_rsa;
extern CK_MECHANISM mechanism_sha512_rsa;
extern CK_MECHANISM mechanism_md2_rsa;
extern CK_MECHANISM mechanism_md5_rsa;
extern CK_MECHANISM mechanism_dsa_gen;
extern CK_MECHANISM mechanism_dsa;
extern CK_MECHANISM mechanism_kcdsa_gen;
extern CK_MECHANISM mechanism_kcdsa_sha1;
extern CK_MECHANISM mechanism_kcdsa_has160;
extern CK_MECHANISM mechanism_kcdsa_ripemd160;
extern CK_MECHANISM mechanism_kcdsa_comm;
extern CK_MECHANISM mechanism_ec_gen;
extern CK_MECHANISM mechanism_ecdsa;
extern CK_MECHANISM mechanism_sha_ecdsa;
extern CK_MECHANISM mechanism_dh_gen;
extern CK_MECHANISM mechanism_sha_dsa;
extern CK_MECHANISM mechanism_generic_gen;
extern CK_MECHANISM mechanism_des_gen;
extern CK_MECHANISM mechanism_des2_gen;
extern CK_MECHANISM mechanism_des3_gen;
extern CK_MECHANISM mechanism_des_ecb;
extern CK_MECHANISM mechanism_des3_ecb;

/* CAST5 is same as CAT128, CAST128 now preferred */
extern CK_MECHANISM mechanism_cast128_gen;
extern CK_MECHANISM mechanism_cast128_ecb;
extern CK_MECHANISM mechanism_cast5_gen;
extern CK_MECHANISM mechanism_cast5_ecb;
extern CK_MECHANISM mechanism_aes_gen;
extern CK_MECHANISM mechanism_aes_ecb;
extern CK_MECHANISM mechanism_sha1_hmac_gen;
extern CK_MECHANISM mechanism_sha224_hmac_gen;
extern CK_MECHANISM mechanism_sha256_hmac_gen;
extern CK_MECHANISM mechanism_sha384_hmac_gen;
extern CK_MECHANISM mechanism_sha512_hmac_gen;
extern CK_MECHANISM mechanism_md5_hmac_gen;
extern CK_MECHANISM mechanism_sha1_hmac;
extern CK_MECHANISM mechanism_sha224_hmac;
extern CK_MECHANISM mechanism_sha256_hmac;
extern CK_MECHANISM mechanism_sha384_hmac;
extern CK_MECHANISM mechanism_sha512_hmac;
extern CK_MECHANISM mechanism_md5_hmac;
extern CK_MECHANISM mechanism_seed_gen;
extern CK_MECHANISM mechanism_seed_ecb;
extern CK_MECHANISM mechanism_seed_cbc;
extern CK_MECHANISM mechanism_seed_cbc_pad;
extern CK_MECHANISM mechanism_has160;
extern CK_MECHANISM mechanism_ripemd160;
extern CK_MECHANISM mechanism_des_mac;
extern CK_MECHANISM mechanism_des3_mac;
extern CK_MECHANISM mechanism_cast128_mac;
extern CK_MECHANISM mechanism_cast5_mac;
extern CK_MECHANISM mechanism_aes_mac;

extern CK_MECHANISM mechanism_wrap_crt_rsa;

/* New PSS mechs added s11.20 */
extern CK_MECHANISM mechanism_rsa_pss1;
extern CK_MECHANISM mechanism_rsa_pss224;
extern CK_MECHANISM mechanism_rsa_pss256;
extern CK_MECHANISM mechanism_rsa_pss384;
extern CK_MECHANISM mechanism_rsa_pss512;
extern CK_MECHANISM mechanism_sha_rsa_pss;
extern CK_MECHANISM mechanism_sha224_rsa_pss;
extern CK_MECHANISM mechanism_sha256_rsa_pss;
extern CK_MECHANISM mechanism_sha384_rsa_pss;
extern CK_MECHANISM mechanism_sha512_rsa_pss;

extern CK_MECHANISM mechanism_rsa_oaep1;
extern CK_MECHANISM mechanism_rsa_oaep224;
extern CK_MECHANISM mechanism_rsa_oaep256;
extern CK_MECHANISM mechanism_rsa_oaep384;
extern CK_MECHANISM mechanism_rsa_oaep512;
extern CK_MECHANISM mechanism_rsa_oaep1_data;
extern CK_MECHANISM mechanism_rsa_oaep224_data;
extern CK_MECHANISM mechanism_rsa_oaep256_data;
extern CK_MECHANISM mechanism_rsa_oaep384_data;
extern CK_MECHANISM mechanism_rsa_oaep512_data;

extern CK_KEY_TYPE key_type_rsa;
extern CK_KEY_TYPE key_type_dsa;
extern CK_KEY_TYPE key_type_dh;
extern CK_KEY_TYPE key_type_kcdsa;
extern CK_KEY_TYPE key_type_ec;
extern CK_KEY_TYPE key_type_kea;
extern CK_KEY_TYPE key_type_generic_secret;
extern CK_KEY_TYPE key_type_rc2;
extern CK_KEY_TYPE key_type_des;
extern CK_KEY_TYPE key_type_des2;
extern CK_KEY_TYPE key_type_des3;
extern CK_KEY_TYPE key_type_aes;
extern CK_KEY_TYPE key_type_seed;
extern CK_KEY_TYPE key_type_cast;
extern CK_KEY_TYPE key_type_cast3;
extern CK_KEY_TYPE key_type_cast5;
extern CK_KEY_TYPE key_type_cast128;
extern CK_KEY_TYPE key_type_rc5;
extern CK_KEY_TYPE key_type_idea;
extern CK_KEY_TYPE key_type_skipjack;
extern CK_KEY_TYPE key_type_baton;
extern CK_KEY_TYPE key_type_juniper;
extern CK_KEY_TYPE key_type_cdmf;
extern CK_KEY_TYPE key_type_sha1hmac;
extern CK_KEY_TYPE key_type_md5hmac;
extern CK_KEY_TYPE key_type_sha224hmac;
extern CK_KEY_TYPE key_type_sha256hmac;
extern CK_KEY_TYPE key_type_sha384hmac;
extern CK_KEY_TYPE key_type_sha512hmac;

/* Use a single arbitrary initialization vector for testing.
For real use, use C_Random to fill IV, and don't reuse. */
extern CK_BYTE initialization_vector[8];
extern CK_BYTE initialization_vector_16[16];
extern CK_MECHANISM mechanism_des_cbc;
extern CK_MECHANISM mechanism_des_cbc_pad;
extern CK_MECHANISM mechanism_des3_cbc;
extern CK_MECHANISM mechanism_des3_cbc_pad;
extern CK_MECHANISM mechanism_cast5_cbc;
extern CK_MECHANISM mechanism_cast5_cbc_pad;
extern CK_MECHANISM mechanism_cast128_cbc;
extern CK_MECHANISM mechanism_cast128_cbc_pad;
extern CK_MECHANISM mechanism_aes_cbc;
extern CK_MECHANISM mechanism_aes_cbc_pad;

/* custom/vendor mechs for  s11.20 */
extern CK_MECHANISM mechanism_public_from_private;

extern CK_CERTIFICATE_TYPE ckc_x_509;

#endif /* __CKUTIL_H */
