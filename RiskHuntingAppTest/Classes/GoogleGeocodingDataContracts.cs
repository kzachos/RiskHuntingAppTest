using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RiskHuntingAppTest
{
    [DataContract]
    public class GoogleGeoCodeResponse
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public results[] results { get; set; }
    }

    [DataContract]
    public class results
    {
        [DataMember]
        public string formatted_address { get; set; }
        [DataMember]
        public geometry geometry { get; set; }
        [DataMember]
        public string[] types { get; set; }
        [DataMember]
        public address_component[] address_components { get; set; }

    }
    [DataContract]
    public class geometry
    {
        [DataMember]
        public string location_type { get; set; }
        [DataMember]
        public location location { get; set; }
    }
    [DataContract]
    public class location
    {
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string lng { get; set; }
    }
    [DataContract]
    public class address_component
    {
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
        [DataMember]
        public string[] types { get; set; }
    }
}
