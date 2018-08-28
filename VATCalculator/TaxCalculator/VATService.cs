using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Cryptolens.VATCalculator
{
    /// <summary>
    /// Methods to help calculating VAT within the EU.
    /// </summary>
    public class VATService
    {
        /// <summary>
        /// Calculates the tax that should be applied to the customer.
        /// </summary>
        /// <param name="ISOCountryName">An ISO country code (eg. SE).</param>
        /// <param name="VATId">The VAT id, eg SE559116174901.</param>
        /// <returns>Returns an integer, eg 25.5% means 2550 will be returned.</returns>
        public static VAT CalculateVAT(string ISOCountryName, string VATId)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string,int>>(Resources.ISO2Country);

            if(!dict.ContainsKey(ISOCountryName))
            {
                // export (outside of EU)
                return new VAT { Tax = 0, Type= VATType.NonEU};
            }

            if(!string.IsNullOrEmpty(VATId))
            {
                // reverse tax liability (omvänd skattskyldighet)
                // we assume the VATId has been validated
                return new VAT {Tax = 0, Type = VATType.ReverseTaxLiability };
            }

            return new VAT { Tax = dict[ISOCountryName] * 100, Type = VATType.PrivateEUResident };
        }

        /// <summary>
        /// Verifies a VAT number using the EU Comission service. Note, this call is not error free
        /// and can throw errors, so it's important to use try catch. You can see current health of
        /// the service at http://ec.europa.eu/taxation_customs/vies/monitoring.html.
        /// </summary>
        /// <param name="VATId">The VAT id, eg SE559116174901.</param>
        public static VATResponse IsValidVAT(string VATId)
        {
            // all requests sent to http://ec.europa.eu/taxation_customs/vies/checkVatService.wsdl
            // tech docs at http://ec.europa.eu/taxation_customs/vies/help.html

            if(VATId == null || VATId.Length < 3)
            {
                Debug.WriteLine("VATId cannot be null or less 3 chars in length.");
                return new VATResponse { IsValid = false };
            }

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

    /// <summary>
    /// Used to store response of <see cref="VATService.IsValidVAT(string)"/>.
    /// </summary>
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

    /// <summary>
    /// Describes the amount of VAT and reason for it.
    /// </summary>
    public class VAT
    {
        /// <summary>
        /// The tax the customer should pay.
        /// </summary>
        public int Tax { get; set; }

        /// <summary>
        /// What type of tax is this.
        /// </summary>
        public VATType Type { get; set; }
    }

    public enum VATType
    {
        /// <summary>
        /// Export
        /// </summary>
        NonEU,

        /// <summary>
        /// Company inside EU
        /// </summary>
        ReverseTaxLiability,

        /// <summary>
        /// Private EU resident
        /// </summary>
        PrivateEUResident
    }
}
