using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RiskHuntingAppTest
{
    public sealed class GoogleReverseGeocoder
    {
        public static string SecretKey { get; set; }
        public static string ClientId { get; set; }
        private const string googleWebAddress = "http://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&sensor=false";
        private const string googleWebAddressByAddress = "http://maps.googleapis.com/maps/api/geocode/json?address={0}&region={1}&sensor=false";
        private GoogleReverseGeocoder()
        {
        }

        public static GoogleGeoCodeResponse GeocodeGetWholeResponse(string address, string region)
        {
            return GeocodeGetWholeResponse(address, region, SecretKey);
        }

        public static GoogleGeoCodeResponse GeocodeGetWholeResponse(string address, string region, string secretKey)
        {
            string temp = new System.Net.WebClient().DownloadString(Sign(string.Format(googleWebAddressByAddress, address, region), secretKey));
            byte[] response = Encoding.Unicode.GetBytes(temp);
            using (MemoryStream ms = new MemoryStream(response))
            {
                var deserialiser = new DataContractJsonSerializer(typeof(GoogleGeoCodeResponse));

                GoogleGeoCodeResponse result = (GoogleGeoCodeResponse)deserialiser.ReadObject(ms);

                return result;
            }
        }

        public static GoogleGeoCodeResponse ReverseGeocodeGetWholeResponse(double latitude, double longitude)
        {
            return ReverseGeocodeGetWholeResponse(latitude, longitude, SecretKey);
        }

        public static GoogleGeoCodeResponse ReverseGeocodeGetWholeResponse(double latitude, double longitude, string secretKey)
        {
            string temp = new System.Net.WebClient().DownloadString(Sign(string.Format(googleWebAddress, latitude, longitude), secretKey));
            byte[] response = Encoding.Unicode.GetBytes(temp);
            using (MemoryStream ms = new MemoryStream(response))
            {
                var deserialiser = new DataContractJsonSerializer(typeof(GoogleGeoCodeResponse));

                GoogleGeoCodeResponse result = (GoogleGeoCodeResponse)deserialiser.ReadObject(ms);

                return result;
            }
        }

        public static string ReverseGeocode(double latitude, double longitude, string secretKey, out bool success)
        {
            try
            {
                string endResult = ProcessResult(ReverseGeocodeGetWholeResponse(latitude, longitude, secretKey), out success);
                return endResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public static string ReverseGeocode(double latitude, double longitude, out bool success)
        {
            return ReverseGeocode(latitude, longitude, SecretKey, out success);
        }

        private static string Sign(string url, string secretKey)
        {
            if (!string.IsNullOrEmpty(ClientId))
                url += string.Format("&client={0}", ClientId);
            if (!string.IsNullOrEmpty(secretKey))
            {
                string usablePrivateKey = secretKey.Replace("-", "+").Replace("_", "/");
                byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);
                Uri uri = new Uri(url);
                byte[] encodedPathAndQueryBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(uri.LocalPath + uri.Query);
                // compute the hash       
                HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
                byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);
                // convert the bytes to string and make url-safe by replacing '+' and '/' characters       
                string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
                // Add the signature to the existing URI.       
                return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
            }
            return url;
        }

        private static string ProcessResult(GoogleGeoCodeResponse result, out bool success)
        {
            if (result.status.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                if (result.results.Length > 0)
                {
                    success = true;
                    var streetRecord = from r in result.results
                                       where r.types.Contains("street_address")
                                       select r.formatted_address;

                    if (streetRecord.Count() > 0)
                        return streetRecord.First();

                    var routeRecord = from r in result.results
                                      where r.types.Contains("route")
                                      select r.formatted_address;

                    if (routeRecord.Count() > 0)
                        return routeRecord.First();

                    var intersectionRecord = from r in result.results
                                             where r.types.Contains("intersection")
                                             select r.formatted_address;

                    if (intersectionRecord.Count() > 0)
                        return intersectionRecord.First();

                    var postal_codeRecord = from r in result.results
                                            where r.types.Contains("postal_code")
                                            select r.formatted_address;

                    if (postal_codeRecord.Count() > 0)
                        return postal_codeRecord.First();

                    return result.results[0].formatted_address;
                }
            }
            success = false;
            return result.status;
        }
    }
}
