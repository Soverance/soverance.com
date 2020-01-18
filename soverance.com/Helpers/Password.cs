using System;
using System.Security.Cryptography;

namespace soverance.com.Helpers
{
    public static class Password
    {
        //private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();
        private static readonly char[] PossibleCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        // This function is used to return a STABLE hash code from a string, because the built-in GetHashCode() function adds
        // the current build number as it's random seed to determine the hash code, and this build number will change depending on when the function is run
        // The function below doesn't use any randomization, and should return the same hash value for all future versions of .NET
        public static int GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        // this function generates a random password string
        // use it like this:  var password = Password.Generate(32, 12);
        public static string Generate(int length, int numberOfNonAlphanumericCharacters, string seed)
        {
            // throw error if password length is out of bounds
            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            // throw error if numberOfNonAlphanumericCharacters is out of bounds
            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            // start randomization
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);
                
                var characterBuffer = new char[length];

                int j;
                var rand = new Random(GetStableHashCode(seed));

                for (j = 0; j < length; j++)
                {
                    characterBuffer[j] = PossibleCharacters[rand.Next(0, PossibleCharacters.Length)];
                }

                return new string(characterBuffer);
            }
        }
    }
}