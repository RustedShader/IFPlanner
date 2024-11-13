namespace IFPlanner.Components.Models;

public class InfiniteFlightApi
{
    public class GetSessions
    {
        public class LiveAPIResponse
        {
            public int errorCode { get; set; }
            public List<SessionInfo> result { get; set; }
        }

        public class SessionInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public int maxUsers { get; set; }
            public int userCount { get; set; }
            public int type { get; set; }
            public int worldType { get; set; }
            public int minimumGradeLevel { get; set; }
        }
    }

    public class WorldStatus
    {
        public class LiveAPIResponse
        {
            public int errorCode { get; set; }
            public List<AirportStatus> result { get; set; }
        }

        public class AirportStatus
        {
            public string airportIcao { get; set; }
            public string airportName { get; set; }
            public int inboundFlightsCount { get; set; }
            public List<string> inboundFlights { get; set; }
            public int outboundFlightsCount { get; set; }
            public List<string> outboundFlights { get; set; }
            public List<ActiveATCFacility> atcFacilities { get; set; }
            public string serverType { get; set; }
        }

        public class ActiveATCFacility
        {
            public string frequencyId { get; set; }
            public string userId { get; set; }
            public string username { get; set; }
            public string virtualOrganization { get; set; }
            public string airportName { get; set; }
            public int type { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string startTime { get; set; }
        }
    }

    public class Flights
    {
        public class FlightEntry
        {
            public string FlightId { get; set; }
            public string UserId { get; set; }
            public string AircraftId { get; set; }
            public string LiveryId { get; set; }
            public string Username { get; set; }
            public string VirtualOrganization { get; set; }
            public string Callsign { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Altitude { get; set; }
            public double Speed { get; set; }
            public double VerticalSpeed { get; set; }
            public double Track { get; set; }
            public float Heading { get; set; }
            public string LastReport { get; set; }
            public string serverType { get; set; }
        }

        public class LiveAPIResponse
        {
            public int ErrorCode { get; set; }
            public List<FlightEntry> Result { get; set; }
        }
    }

    public  class Atis
    {
        public int errorCode { get; set; }
        public string result { get; set; }
    }

    public  class UserStatsApi
    {
        public class ViolationCountByLevel
        {
            public int level1 { get; set; }
            public int level2 { get; set; }
            public int level3 { get; set; }
        }

        public class UserStats
        {
            public int onlineFlights { get; set; }
            public int violations { get; set; }
            public double xp { get; set; }
            public int landingCount { get; set; }
            public double flightTime { get; set; }
            public int atcOperations { get; set; }
            public int? atcRank { get; set; }
            public int grade { get; set; }
            public string hash { get; set; }
            public ViolationCountByLevel violationCountByLevel { get; set; }
            public List<int> roles { get; set; }
            public string userId { get; set; }
            public string virtualOrganization { get; set; } 
            public string discourseUsername { get; set; }
            public List<string> groups { get; set; }
            public int errorCode { get; set; }
        }

        public class LiveAPIResponse
        {
            public int errorCode { get; set; }
            public List<UserStats> result { get; set; }
        } 
    }

    public class Notams
    {
        public class SampleResponse
        {
            public int errorCode { get; set; }
            public List<NotamResult> result { get; set; }
        }

        public class NotamResult
        {
            public string id { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public int type { get; set; }
            public string sessionId { get; set; }
            public float radius { get; set; }
            public string message { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string icao { get; set; }
            public int floor { get; set; }
            public int ceiling { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
        }
    }

    public class Airports()
    {
        public class LiveAPIResponse
        {
            public int ErrorCode { get; set; }
            public AirportInfo Result { get; set; }
        }

        public class AirportInfo
        {
            public string Icao { get; set; }
            public string Iata { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public Country Country { get; set; }
            public int Class { get; set; }
            public int FrequenciesCount { get; set; }
            public int Elevation { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Timezone { get; set; }
            public bool Has3dBuildings { get; set; }
            public bool HasJetbridges { get; set; }
            public bool HasSafedockUnits { get; set; }
            public bool HasTaxiwayRouting { get; set; }
        }

        public class Country
        {
            public string Name { get; set; }
            public string IsoCode { get; set; }
        }
    }

    public class UserFlightData
    {
        public class Parameters
        {
            public string userId { get; set; }
            public int? page { get; set; } // Nullable int
        }

        public class Violation
        {
            public Issuer issuedBy { get; set; }
            public int level { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public DateTime created { get; set; }
        }

        public class Issuer
        {
            public string id { get; set; }
            public string username { get; set; }
            public string callsign { get; set; }
            public DiscourseUser discourseUser { get; set; }
        }

        public class DiscourseUser
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string virtualOrganization { get; set; }
            public string avatarTemplate { get; set; }
        }

        public class UserFlight
        {
            public string id { get; set; }
            public DateTime created { get; set; }
            public string userId { get; set; }
            public string aircraftId { get; set; }
            public string liveryId { get; set; }
            public string callsign { get; set; }
            public string server { get; set; }
            public float dayTime { get; set; }
            public float nightTime { get; set; }
            public float totalTime { get; set; }
            public int landingCount { get; set; }
            public string originAirport { get; set; }
            public string destinationAirport { get; set; }
            public int xp { get; set; }
            public int worldType { get; set; }
            public List<Violation> violations { get; set; }
        }

        public class PaginatedList
        {
            public int pageIndex { get; set; }
            public int totalPages { get; set; }
            public int totalCount { get; set; }
            public bool hasPreviousPage { get; set; }
            public bool hasNextPage { get; set; }
            public List<UserFlight> data { get; set; }
        }

        public class LiveAPIResponse
        {
            public int errorCode { get; set; }
            public PaginatedList result { get; set; }
        }
    }
}