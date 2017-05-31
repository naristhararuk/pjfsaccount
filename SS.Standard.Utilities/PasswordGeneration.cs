using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Security.Cryptography;

namespace SS.Standard.Utilities
{
    public class PasswordGeneration
    {
        #region Design Variable
        private readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly char[] _Numbers = "1234567890".ToCharArray();
        private readonly char[] _Symbols = "!@#$%^&*.?[](){}-_+=".ToCharArray();
        private readonly char[] _LettersThai = "กขฃคฅฆงจฉชซฌญฎาฏฐฑฒณดตถทธนบปผฝพฟภมยรลวศษสหฬอฮเแโใไะาๆ๑๒๓๔๕๖๗๘๙".ToCharArray();

        private string[] _CharacterTypes;
        #endregion Design Variable

        #region Property
        public bool IncludeUpper { get; set; }
        public bool IncludeLower { get; set; }
        public bool IncludeNumber { get; set; }
        public bool IncludeSpecial { get; set; }
        public bool IncludeThaiLetters { get; set; }
        public int MinimumLength{ get; set; }
        public int MaximumLength{ get; set; }
        #endregion Property

        #region Enum CharacterType
        enum CharacterType
        {
            Uppercase,
            Lowercase,
            Special,
            Number,
            ThaiLetters
        }
        #endregion enum CharacterType

        #region Constructor
        public PasswordGeneration()
		{
			MinimumLength       = 6;
			MaximumLength       = 20;
			IncludeSpecial      = true;
			IncludeNumber       = true;
			IncludeUpper        = true;
			IncludeLower        = true;
            IncludeThaiLetters  = true;
		}

        public PasswordGeneration(int minimumLength,int maximumLength) : this()
		{
            MinimumLength   = minimumLength;
            MaximumLength   = maximumLength;
        }

        public PasswordGeneration(bool includeNumber, bool includeSpecial) : this()
        {
            IncludeNumber = includeNumber;
            IncludeSpecial = includeSpecial;
        }

        public PasswordGeneration(bool includeUpper, bool includeLower, bool includeThaiLetters)
            : this()
        {
            IncludeUpper = includeUpper;
            IncludeLower = includeLower;
            IncludeThaiLetters = includeThaiLetters;
        }

        public PasswordGeneration(bool includeSpecial, bool includeNumber, bool includeUpper, bool includeLower, bool includeThaiLetters) : this()
        {
            IncludeNumber = includeNumber;
            IncludeSpecial = includeSpecial;
            IncludeUpper = includeUpper;
            IncludeLower = includeLower;
            IncludeThaiLetters = includeThaiLetters;
        }

        public PasswordGeneration(int minimumLength, int maximumLength, bool includeSpecial, bool includeNumber, bool includeUpper, bool includeLower, bool includeThaiLetters) : this()
        {
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
            IncludeNumber = includeNumber;
            IncludeSpecial = includeSpecial;
            IncludeUpper = includeUpper;
            IncludeLower = includeLower;
            IncludeThaiLetters = includeThaiLetters;
        }
        #endregion Constructor

        #region public string Create()
        /// <summary>
        /// Randomly creates a password.
        /// </summary>
        /// <returns>A random string of characters.</returns>
        public string Create()
        {
            _CharacterTypes = getCharacterTypes();

            StringBuilder password = new StringBuilder(MaximumLength);

            //Get a random length for the password.
            int currentPasswordLength = RandomNumber.Next(MaximumLength);

            //Only allow for passwords greater than or equal to the minimum length.
            if (currentPasswordLength < MinimumLength)
            {
                currentPasswordLength = MinimumLength;
            }

            //Generate the password
            for (int i = 0; i < currentPasswordLength; i++)
            {
                password.Append(getCharacter());
            }

            return password.ToString();
        }
        #endregion public string Create()

        #region private string[] getCharacterTypes()
        /// <summary>
        /// Determines which character types should be used to generate
        /// the current password.
        /// </summary>
        /// <returns>A string[] of character that should be used to generate the current password.</returns>
        private string[] getCharacterTypes()
        {
            ArrayList characterTypes = new ArrayList();
            foreach (string characterType in Enum.GetNames(typeof(CharacterType)))
            {
                CharacterType currentType = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
                bool addType = false;
                switch (currentType)
                {
                    case CharacterType.Lowercase:
                        addType = IncludeLower;
                        break;
                    case CharacterType.Number:
                        addType = IncludeNumber;
                        break;
                    case CharacterType.Special:
                        addType = IncludeSpecial;
                        break;
                    case CharacterType.Uppercase:
                        addType = IncludeUpper;
                        break;
                    case CharacterType.ThaiLetters:
                        addType = IncludeThaiLetters;
                        break;
                }
                if (addType)
                {
                    characterTypes.Add(characterType);
                }
            }
            return (string[])characterTypes.ToArray(typeof(string));
        }
        #endregion private string[] getCharacterTypes()

        #region private string getCharacter()
        /// <summary>
        /// Randomly determines a character type to return from the 
        /// available CharacterType enum.
        /// </summary>
        /// <returns>The string character to append to the password.</returns>
        private string getCharacter()
        {
            string characterType = _CharacterTypes[RandomNumber.Next(_CharacterTypes.Length)];
            CharacterType typeToGet = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
            switch (typeToGet)
            {
                case CharacterType.Lowercase:
                    return _Letters[RandomNumber.Next(_Letters.Length)].ToString().ToLower();
                case CharacterType.Uppercase:
                    return _Letters[RandomNumber.Next(_Letters.Length)].ToString().ToUpper();
                case CharacterType.Number:
                    return _Numbers[RandomNumber.Next(_Numbers.Length)].ToString();
                case CharacterType.Special:
                    return _Symbols[RandomNumber.Next(_Symbols.Length)].ToString();
                case CharacterType.ThaiLetters:
                    return _LettersThai[RandomNumber.Next(_LettersThai.Length)].ToString();
            }
            return null;
        }
        #endregion private string getCharacter()
    }

    #region class RandomNumber
    /// <summary>
	/// Summary description for RandomNumber.
	/// </summary>
    class RandomNumber
    {
        private static RNGCryptoServiceProvider _Random = new RNGCryptoServiceProvider();
        private static byte[] bytes = new byte[4];

        private RandomNumber() { }

        public static int Next(int max)
        {
            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException("max");
            }
            _Random.GetBytes(bytes);
            int value = BitConverter.ToInt32(bytes, 0) % max;
            if (value < 0)
            {
                value = -value;
            }
            return value;
        }
    }
    #endregion class RandomNumber
}
