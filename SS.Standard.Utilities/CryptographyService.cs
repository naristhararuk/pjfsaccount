using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Utilities
{
    public class CryptographyService
    {
        #region Design Valiable
		private Cryptography crypt = null;

		public string KeyString{ set; get; }
		public string SaltString{ set; get; }
        #endregion Design Valiable

		#region CryptographyKey
		/// <summary>
		/// CryptographyKey
		/// </summary>
		public CryptographyService()
		{
            KeyString   = "KeyConfigElement";
            SaltString  = "SaltConfigElement";

			crypt	    = new Cryptography(Cryptography.ServiceProviderEnum.TripleDES);
            crypt.Key   = KeyString;
            crypt.Salt  = SaltString;
		}

		/// <summary>
		/// CryptographyKey
		/// </summary>
		/// <param name="CryptType"></param>
		/// <param name="strKey"></param>
		/// <param name="strSalt"></param>
        public CryptographyService(Cryptography.ServiceProviderEnum CryptType, string strKey, string strSalt)
		{
            KeyString   = strKey;
            SaltString  = strSalt;

			crypt	    = new Cryptography(CryptType);
            crypt.Key   = KeyString;
            crypt.Salt  = SaltString;
		}
		#endregion CryptographyKey

		#region Encryption
		/// <summary>
		/// Encryption
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public string Encryption(string plainText)
		{
			return crypt.Encrypt(plainText);
		}
		#endregion Encryption

		#region Decryption
		/// <summary>
		/// Decryption
		/// </summary>
		/// <param name="cipherText"></param>
		/// <returns></returns>
		public string Decryption(string cipherText)
		{
			return crypt.Decrypt(cipherText);
		}
		#endregion Decryption
    }
}
