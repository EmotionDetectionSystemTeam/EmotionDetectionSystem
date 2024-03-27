using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{



    public class PasswordGenerator
    {
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()_+={[}]|;:,<.>/?-";

        public string GenerateRandomPassword()
        {
            Random random = new Random();

            // Ensure at least one character from each character set
            string password = GetRandomCharacter(UppercaseLetters, random)
                            + GetRandomCharacter(LowercaseLetters, random)
                            + GetRandomCharacter(Digits, random)
                            + GetRandomCharacter(SpecialCharacters, random);

            // Fill the rest of the password with random characters
            for (int i = password.Length; i < 8; i++)
            {
                string characterSet = GetRandomCharacterSet(random);
                password += GetRandomCharacter(characterSet, random);
            }

            // Shuffle the password characters
            password = new string(password.ToCharArray().OrderBy(x => random.Next()).ToArray());

            return password;
        }

        private string GetRandomCharacter(string characterSet, Random random)
        {
            int index = random.Next(characterSet.Length);
            return characterSet[index].ToString();
        }

        private string GetRandomCharacterSet(Random random)
        {
            string[] characterSets = { UppercaseLetters, LowercaseLetters, Digits, SpecialCharacters };
            int index = random.Next(characterSets.Length);
            return characterSets[index];
        }
    }


}
