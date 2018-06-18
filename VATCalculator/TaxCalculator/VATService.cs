using System;
using VATCalculator.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Cryptolens.VATCalculator
{
    public class VATService
    {
        public static int CalculateVAT(string ISOCountryName, string VATId)
        {
            JsonConvert.DeserializeObject<Dictionary<string,string>>(Resources.ISO2Country);
            return 0;
        }

        /// <summary>
        /// Verifies a VAT number using the EU Comission service. Note, this call is not error free
        /// and can throw errors, so it's important to use try catch. You can see current health of
        /// the service at http://ec.europa.eu/taxation_customs/vies/monitoring.html.
        /// </summary>
        /// <param name="VATId">The VAT id, eg SE559116174901.</param>
        /// <returns></returns>
        public static VATResponse IsValidVAT(string VATId)
        {
            // all requests sent to http://ec.europa.eu/taxation_customs/vies/checkVatService.wsdl
            // tech docs at http://ec.europa.eu/taxation_customs/vies/help.html

            var param = new VATCheck.checkVatRequest(VATId.Substring(0,2), VATId.Substring(2));

            var service = new VATCheck.checkVatPortTypeClient();

            var result = Task.Run(async () => await service.checkVatAsync(param)).Result;

            return new VATResponse
            {
                IsValid = result.valid,
                CompanyName = result.name != "---" ? result.name : null,
                Address = result.address != "---" ? result.address : null
            };
        }
    }

    public class VATResponse
    {
        /// <summary>
        /// True if the VAT id is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The company name (can be null).
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// The address of the company (can be null).
        /// </summary>
        public string Address { get; set; }
    }
}
