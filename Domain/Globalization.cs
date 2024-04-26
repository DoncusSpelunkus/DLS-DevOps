using Nager.Country;

namespace Globalization;
public class CountryConverter
{
    public static string GetCountryName(string countryCode)
    {
        try
        {
            var countryProvider = new CountryProvider();
            var countryInfo = countryProvider.GetCountry(countryCode);
            return countryInfo.CommonName;
        }
        catch (ArgumentException)
        {
            // Handle invalid country code
            return "Unknown";
        }
    }
}