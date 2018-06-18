using System;
using VATCalculator.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cryptolens.VATCalculator
{
    public class VATCalculator
    {
        public static int CalculateVAT(string ISOCountryName, string VATId)
        {
            JsonConvert.DeserializeObject<Dictionary<string,string>>(Resources.ISO2Country);

            return 0;
        }
    }
}
