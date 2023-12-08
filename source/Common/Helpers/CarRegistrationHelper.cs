using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class CarRegistrationHelper
    {
        public bool ValidateNormalRegistration(string registration)
        {
            var registrationLetters = registration.Substring(0, 2);
            var registrationNumbers = registration.Substring(2);

            bool isNumeric = int.TryParse(registrationNumbers, out _);
            bool isLetters = Regex.IsMatch(registrationLetters, @"^[a-zA-Z]+$");

            if (!isLetters || !isNumeric)
                return false;
            else
                return true;
        }

        public bool ValidateCustomRegistration(string registration)
        {
            if (string.IsNullOrWhiteSpace(registration) || registration.Length > 7)
                return false;
            else
                return true;
        }
    }
}
