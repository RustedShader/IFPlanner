using System.Xml.Linq;
using IFPlanner.Components.Classes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace IFPlanner.Components.Pages;

public partial class FlightPlan
{
    private readonly ApiClient _apiClient = new();

    private readonly List<ChecklistCategory> _categories = new()
    {
        new ChecklistCategory
        {
            CategoryName = "Pre Flight",
            Items = new List<ChecklistItem>
            {
                new() { Name = "PARKING BREAK: SET", IsChecked = false },
                new() { Name = "JET BRIDGE: CONNECTED", IsChecked = false },
                new() { Name = "GROUND SERVICES: CONNECTED", IsChecked = false },
                new() { Name = "ATIS: CHECK", IsChecked = false },
                new() { Name = "WEIGHT & BALANCE: CHECK", IsChecked = false },
                new() { Name = "FUEL: CHECK", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Startup",
            Items = new List<ChecklistItem>
            {
                new() { Name = "BATTERY: ON", IsChecked = false },
                new() { Name = "APU: ON", IsChecked = false },
                new() { Name = "SEAT BELT: ON", IsChecked = false },
                new() { Name = "NO SMOKING: ON", IsChecked = false },
                new() { Name = "NAV LIGHT: ON", IsChecked = false },
                new() { Name = "BEACON LIGHT: ON", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Pushback",
            Items = new List<ChecklistItem>
            {
                new() { Name = "JET BRIDGE: STOWED", IsChecked = false },
                new() { Name = "PUSHBACK CLEARANCE: CLEARED", IsChecked = false },
                new() { Name = "TUG: ATTACHED", IsChecked = false },
                new() { Name = "PARKING BREAK: RELEASE", IsChecked = false },
                new() { Name = "ENGINES: START", IsChecked = false },
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Before Taxi",
            Items = new List<ChecklistItem>
            {
                new() { Name = "TUG: DETACHED", IsChecked = false },
                new() { Name = "PARKING BREAK: SET", IsChecked = false },
                new() { Name = "ENGINES: STABLE", IsChecked = false },
                new() { Name = "FLAPS: TAKEOFF", IsChecked = false },
                new() { Name = "APU: OFF", IsChecked = false },
                new() { Name = "TAXI CLEARANCE: CLEARED", IsChecked = false },
                new() { Name = "PARKING BREAK: RELEASE", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Before Takeoff",
            Items = new List<ChecklistItem>
            {
                new() { Name = "FLIGHT CONTROL: CHECKED", IsChecked = false },
                new() { Name = "CALIBRATE: CHECKED", IsChecked = false },
                new() { Name = "STROBES LIGHT: ON", IsChecked = false },
                new() { Name = "A/P PREP: DONE", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Takeoff",
            Items = new List<ChecklistItem>
            {
                new() { Name = "TAKEOFF CLEARANCE: CLEARED", IsChecked = false },
                new() { Name = "LANDING LIGHTS: ON", IsChecked = false },
                new() { Name = "THRUST: TO/GA", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "After Takeoff",
            Items = new List<ChecklistItem>
            {
                new() { Name = "LANDING LIGHTS: OFF", IsChecked = false },
                new() { Name = "GEAR: UP", IsChecked = false },
                new() { Name = "FLAPS: RETRACTED", IsChecked = false },
                new() { Name = "AUTOBRAKES: OFF", IsChecked = false },
                new() { Name = "A/P: ENGAGED", IsChecked = false },
                new() { Name = "SEATBELTS: OFF", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "Approach",
            Items = new List<ChecklistItem>
            {
                new() { Name = "APP SPEED: CHECK", IsChecked = false },
                new() { Name = "FLAPS: DOWN", IsChecked = false },
                new() { Name = "AUTOBRAKES: SET", IsChecked = false },
                new() { Name = "ILS: SET", IsChecked = false },
                new() { Name = "SEATBELTS: ON", IsChecked = false },
                new() { Name = "APPR: SET", IsChecked = false },
                new() { Name = "SPOILERS: ARMED", IsChecked = false },
                new() { Name = "GEAR: DOWN", IsChecked = false },
                new() { Name = "LANDING LIGHTS: ON", IsChecked = false },
                new() { Name = "LANDING CLEARANCE: CLEARED", IsChecked = false },
                new() { Name = "LANDING SPEED: CHECK", IsChecked = false }
            }
        },
        new ChecklistCategory
        {
            CategoryName = "After Landing",
            Items = new List<ChecklistItem>
            {
                new() { Name = "REVERSERS: IDLE", IsChecked = false },
                new() { Name = "AUTOBRAKES: OFF", IsChecked = false },
                new() { Name = "SPOILERS: RETRACTED", IsChecked = false },
                new() { Name = "FLAPS: UP", IsChecked = false },
                new() { Name = "STROBES LIGHT: OFF", IsChecked = false },
                new() { Name = "LANDING LIGHT: OFF", IsChecked = false },
                new() { Name = "APU: ON", IsChecked = false },
                new() { Name = "TAXI CLEARANCE: CLEARED", IsChecked = false }
            }
        }
    };

    private List<AircraftData> _aircraftDataList = new();


    private List<DestinationParameters> _destinationParameters = new();
    private List<FixData> _fixesParameters = new();
    private string _flightPlan = string.Empty;
    private string _flightPlanFixes = string.Empty;
    private string _flightPlanShort = string.Empty;
    private List<FuelData> _fuelDataList = new();
    private List<GeneralParameters> _generalParameters = new();

    private string _hemisphere = string.Empty;
    private List<ImageData> _imageDataList = new();

    private bool _isInitialized;
    private string _meridian = string.Empty;
    private List<OriginParameters> _originParameters = new();
    private List<ParameterData> _parameterDataList = new();
    private List<FilesData> _pdfsData = new();
    private List<TimeData> _timeData = new();
    private List<WeightParameters> _weightParameters = new();

    [Inject] protected IJSRuntime JsRuntime { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    

    


    // Returns Flight Plan Url from the ofp Id redirected from generate flight plan page
    private async Task<string> GetFlightPlanUrl()
    {
        var uri = new Uri(NavigationManager.Uri);
        var ofpId = uri.Query
            .TrimStart('?')
            .Split('&')
            .Select(part => part.Split('='))
            .FirstOrDefault(pair => pair.Length == 2 && pair[0] == "ofp_id")?[1];
        return "https://www.simbrief.com/ofp/flightplans/xml/" + ofpId + ".xml";
    }

    // Gives Vertical speed as Output by taking various Parameters 
    private static int VerticalSpeed(double distance, double groundSpeed, double currentAltitude, double nextAltitude)
    {
        var diffInAltitude = nextAltitude - currentAltitude;
        var timeSec = distance / groundSpeed * 3600.0; // Convert from hours to seconds
        return (int)Math.Round(diffInAltitude / timeSec * 60.0); // Convert from meters/second to feet/minute
    }

    // Copy to clipboard javascript function caller
    private async Task CopyToClipboard(string text)
    {
        // await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        await JsRuntime.InvokeVoidAsync("Clipboard.copy", text);
    }

    // Parse Xml Data to Lists and string by taking xmlDoc as parameter
    private async Task ParseXmlData(XDocument xmlDoc)
    {
        if (xmlDoc?.Root != null)
        {
            _generalParameters = xmlDoc.Root.Descendants("general")
                .Select(element => new GeneralParameters
                {
                    IsEtops = (int)element.Element("is_etops"),
                    IsDetailedProfile = (int)element.Element("is_detailed_profile"),
                    CruiseProfile = (string)element.Element("cruise_profile"),
                    ClimbProfile = (string)element.Element("climb_profile"),
                    DescentProfile = (string)element.Element("descent_profile"),
                    AlternateProfile = (string)element.Element("alternate_profile"),
                    ReserveProfile = (string)element.Element("reserve_profile"),
                    Costindex = (int)element.Element("costindex"),
                    InitialAltitude = (int)element.Element("initial_altitude"),
                    StepclimbString = (string)element.Element("stepclimb_string"),
                    AvgTempDev = (int)element.Element("avg_temp_dev"),
                    AvgTropopause = (int)element.Element("avg_tropopause"),
                    AvgWindComp = (int)element.Element("avg_wind_comp"),
                    AvgWindDir = (int)element.Element("avg_wind_dir"),
                    AvgWindSpd = (int)element.Element("avg_wind_spd"),
                    GcDistance = (int)element.Element("gc_distance"),
                    RouteDistance = (int)element.Element("route_distance"),
                    AirDistance = (int)element.Element("air_distance"),
                    TotalBurn = (int)element.Element("total_burn"),
                    CruiseMach = (double)element.Element("cruise_mach"),
                    Passengers = (int)element.Element("passengers"),
                    Route = (string)element.Element("route")
                }).ToList();
            _fixesParameters = xmlDoc.Root.Descendants("navlog").Descendants("fix")
                .Select(element => new FixData
                {
                    Ident = element.Element("ident")?.Value,
                    Name = element.Element("name")?.Value,
                    Type = element.Element("type")?.Value,
                    Stage = element.Element("stage")?.Value,
                    IsSidStar = element.Element("is_sid_star")?.Value == "1",
                    Distance = int.Parse(element.Element("distance")?.Value ?? "0"),
                    AltitudeFeet = int.Parse(element.Element("altitude_feet")?.Value ?? "0"),
                    IndicatedAirspeed = int.Parse(element.Element("ind_airspeed")?.Value ?? "0"),
                    TrueAirspeed = int.Parse(element.Element("true_airspeed")?.Value ?? "0"),
                    Mach = double.Parse(element.Element("mach")?.Value ?? "0"),
                    GroundSpeed = int.Parse(element.Element("groundspeed")?.Value ?? "0"),
                    PosLat = double.Parse(element.Element("pos_lat")?.Value ?? "0"),
                    PosLong = double.Parse(element.Element("pos_long")?.Value ?? "0")
                })
                .ToList();
            _aircraftDataList = xmlDoc.Root.Descendants("aircraft")
                .Select(element => new AircraftData
                {
                    IcaoCode = (string)element.Element("icao_code"),
                    Name = (string)element.Element("name"),
                    FuelFactor = (int)element.Element("fuelfactor"),
                    MaxPassengers = (int)element.Element("max_passengers"),
                    InternalId = (string)element.Element("internal_id"),
                    IsCustom = (int)element.Element("is_custom")
                }).ToList();
            _fuelDataList = xmlDoc.Root.DescendantsAndSelf("fuel")
                .Where(f => f.Parent != null && f.Parent.Name != "bucket")
                .Select(element => new FuelData
                {
                    Taxi = (int)element.Element("taxi"),
                    EnrouteBurn = (int)element.Element("enroute_burn"),
                    Contingency = (int)element.Element("contingency"),
                    AlternateBurn = (int)element.Element("alternate_burn"),
                    Reserve = (int)element.Element("reserve"),
                    Etops = (int)element.Element("etops"),
                    Extra = (int)element.Element("extra"),
                    MinTakeoff = (int)element.Element("min_takeoff"),
                    PlanTakeoff = (int)element.Element("plan_takeoff"),
                    PlanRamp = (int)element.Element("plan_ramp"),
                    PlanLanding = (int)element.Element("plan_landing"),
                    AvgFuelFlow = (int)element.Element("avg_fuel_flow"),
                    MaxTanks = (int)element.Element("max_tanks")
                }).ToList();
            _originParameters = xmlDoc.Root.Descendants("origin")
                .Select(element => new OriginParameters
                {
                    IcaoCode = (string)element.Element("icao_code"),
                    Elevation = (int)element.Element("elevation"),
                    PosLat = (double)element.Element("pos_lat"),
                    PosLong = (double)element.Element("pos_long"),
                    Name = (string)element.Element("name"),
                    PlanRwy = (string)element.Element("plan_rwy")
                }).ToList();
            _destinationParameters = xmlDoc.Root.Descendants("destination")
                .Select(element => new DestinationParameters
                {
                    IcaoCode = (string)element.Element("icao_code"),
                    Elevation = (int)element.Element("elevation"),
                    PosLat = (double)element.Element("pos_lat"),
                    PosLong = (double)element.Element("pos_long"),
                    Name = (string)element.Element("name"),
                    PlanRwy = (string)element.Element("plan_rwy")
                }).ToList();
            _weightParameters = xmlDoc.Root.Descendants("weights")
                .Select(element => new WeightParameters
                {
                    Oew = (int)element.Element("oew"),
                    PaxCount = (int)element.Element("pax_count"),
                    BagCount = (int)element.Element("bag_count"),
                    PaxCountActual = (int)element.Element("pax_count_actual"),
                    BagCountActual = (int)element.Element("bag_count_actual"),
                    PaxWeight = (double)element.Element("pax_weight"),
                    BagWeight = (double)element.Element("bag_weight"),
                    FreightAdded = (int)element.Element("freight_added"),
                    Cargo = (int)element.Element("cargo"),
                    Payload = (int)element.Element("payload"),
                    EstZfw = (int)element.Element("est_zfw"),
                    MaxZfw = (int)element.Element("max_zfw"),
                    EstTow = (int)element.Element("est_tow"),
                    MaxTow = (int)element.Element("max_tow"),
                    MaxTowStruct = (int)element.Element("max_tow_struct"),
                    EstLdw = (int)element.Element("est_ldw"),
                    MaxLdw = (int)element.Element("max_ldw"),
                    EstRamp = (int)element.Element("est_ramp")
                }).ToList();
            _parameterDataList = xmlDoc.Root.Descendants("params")
                .Select(element => new ParameterData
                {
                    OfpLayout = (string)element.Element("ofp_layout"),
                    Airac = (int)element.Element("airac"),
                    Units = (string)element.Element("units")
                }).ToList();
            _imageDataList = xmlDoc.Root.Descendants("images")
                .Select(element => new ImageData
                {
                    Directory = (string)element.Element("directory"),
                    Maps = element.Elements("map")
                        .Select(mapElement => new MapData
                        {
                            Name = (string)mapElement.Element("name"),
                            Link = (string)mapElement.Element("link")
                        }).ToList()
                }).ToList();
            _timeData = xmlDoc.Root.Descendants("times")
                .Select(element => new TimeData
                {
                    EstTimeEnroute = (int)element.Element("est_time_enroute"),
                    SchedTimeEnroute = (int)element.Element("sched_time_enroute"),
                    SchedOut = (long)element.Element("sched_out"),
                    SchedOff = (long)element.Element("sched_off"),
                    SchedOn = (long)element.Element("sched_on"),
                    SchedIn = (long)element.Element("sched_in"),
                    SchedBlock = (int)element.Element("sched_block"),
                    EstOut = (long)element.Element("est_out"),
                    EstOff = (long)element.Element("est_off"),
                    EstOn = (long)element.Element("est_on"),
                    EstIn = (long)element.Element("est_in"),
                    EstBlock = (int)element.Element("est_block"),
                    TaxiOut = (int)element.Element("taxi_out"),
                    TaxiIn = (int)element.Element("taxi_in"),
                    ReserveTime = (int)element.Element("reserve_time"),
                    Endurance = (int)element.Element("endurance"),
                    ContFuelTime = (int)element.Element("contfuel_time"),
                    EtopsFuelTime = (int)element.Element("etopsfuel_time"),
                    ExtraFuelTime = (int)element.Element("extrafuel_time")
                }).ToList();
            _pdfsData = xmlDoc.Root.Descendants("files")
                .Select(element => new FilesData
                {
                    Directory = (string)element.Element("directory"),
                    Pdfs = element.Elements("pdf")
                        .Select(pdfElement => new PdfData
                        {
                            PdfName = (string)pdfElement.Element("name"),
                            PdfLink = (string)pdfElement.Element("link")
                        }).ToList()
                }).ToList();
        }
    }


    private void ConvertLatitudeLongitude()
    {
        var latFixes = _fixesParameters?.Where(x => x.Type == "wpt")?.Select(x => x.PosLat).ToList();
        var longFixes = _fixesParameters?.Where(x => x.Type == "wpt")?.Select(x => x.PosLong).ToList();

        if (latFixes != null && longFixes != null)
            for (var i = 0; i <= latFixes.Count() - 1; i++)
            {
                var latitude = latFixes[i];
                var latDegrees = (int)Math.Abs(latitude);
                var latMinutesDecimal = (Math.Abs(latitude) - latDegrees) * 60;
                var latMin = (int)Math.Abs(latMinutesDecimal);

                var formattedLatDegrees = latDegrees.ToString().PadLeft(2, '0');
                var formattedLatMinutes = latMin.ToString().PadLeft(2, '0');

                // Console.WriteLine(latitude);
                // Console.WriteLine("Lat Degree -> " + formattedLatDegrees);
                // Console.WriteLine("Lat Min -> " + formattedLatMinutes);

                if (latitude >= 0)
                    _hemisphere = "N";
                else
                    _hemisphere = "S";

                var longitude = longFixes[i];
                var longDegree = (int)Math.Abs(longitude);
                var lonMinutesDecimal = (Math.Abs(longitude) - longDegree) * 60;
                var lonMinutes = (int)Math.Abs(lonMinutesDecimal);

                var formattedLonDegrees = longDegree.ToString().PadLeft(2, '0');
                var formattedLonMinutes = lonMinutes.ToString().PadLeft(2, '0');

                // Console.WriteLine(longitude);
                // Console.WriteLine("Long Degree -> " + formattedLonDegrees);
                // Console.WriteLine("Long Min -> " + formattedLonMinutes);

                if (longitude >= 0)
                    _meridian = "E";
                else
                    _meridian = "W";

                _flightPlan +=
                    $"{formattedLatDegrees}{formattedLatMinutes}{_hemisphere}/{formattedLonDegrees}{formattedLonMinutes}{_meridian} ";
            }
        // Console.WriteLine("Flight Plan "+ _flightPlan);
    }

    // OnInitialized
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var flightPlanUrl = await GetFlightPlanUrl();
            var response = await _apiClient.GetApiResponseAsString(flightPlanUrl);
            if (response != null)
            {
                var xmlDoc = XDocument.Parse(response);
                await ParseXmlData(xmlDoc);
                ConvertLatitudeLongitude();
                _flightPlanFixes = _originParameters.FirstOrDefault()?.IcaoCode + " " +
                                   string.Join(" ",
                                       _fixesParameters?.Where(x => x.Type == "wpt")?.Select(x => x.Ident) ??
                                       Enumerable.Empty<string>()) + " " +
                                   _destinationParameters.FirstOrDefault()?.IcaoCode;
                _flightPlanShort = _generalParameters.Select(x => x.Route).FirstOrDefault();
                _isInitialized = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Handle the exception as needed, e.g., log it, notify the user, etc.
        }
    }


    public class FixData
    {
        public string Ident { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Frequency { get; set; }
        public double PosLat { get; set; }
        public double PosLong { get; set; }
        public string Stage { get; set; }
        public string ViaAirway { get; set; }
        public bool IsSidStar { get; set; }
        public int Distance { get; set; }
        public int TrackTrue { get; set; }
        public int TrackMag { get; set; }
        public int HeadingTrue { get; set; }
        public int HeadingMag { get; set; }
        public int AltitudeFeet { get; set; }
        public int IndicatedAirspeed { get; set; }
        public int TrueAirspeed { get; set; }
        public double Mach { get; set; }
        public double MachThousandths { get; set; }
        public int WindComponent { get; set; }
        public int GroundSpeed { get; set; }
        public int TimeLeg { get; set; }
        public int TimeTotal { get; set; }
        public int FuelFlow { get; set; }
        public int FuelLeg { get; set; }
        public int FuelTotalUsed { get; set; }
        public int FuelMinOnboard { get; set; }
        public int FuelPlanOnboard { get; set; }
        public int Oat { get; set; }
        public int OatIsaDev { get; set; }
        public int WindDir { get; set; }
        public int WindSpd { get; set; }
        public int Shear { get; set; }
        public int TropopauseFeet { get; set; }
        public int GroundHeight { get; set; }
        public int Mora { get; set; }
        public string Fir { get; set; }
        public string FirUnits { get; set; }
        public string FirValidLevels { get; set; }
        public List<WindDataLevel> WindData { get; set; }
        public string FirCrossing { get; set; }
    }

    public class WindDataLevel
    {
        public int Altitude { get; set; }
        public int WindDir { get; set; }
        public int WindSpd { get; set; }
        public int Oat { get; set; }
    }

    public class GeneralParameters
    {
        public int Release { get; set; }
        public string IcaoAirline { get; set; }
        public string FlightNumber { get; set; }
        public int IsEtops { get; set; }
        public string DxRmk { get; set; }
        public string SysRmk { get; set; }
        public int IsDetailedProfile { get; set; }
        public string CruiseProfile { get; set; }
        public string ClimbProfile { get; set; }
        public string DescentProfile { get; set; }
        public string AlternateProfile { get; set; }
        public string ReserveProfile { get; set; }
        public int Costindex { get; set; }
        public string ContRule { get; set; }
        public int InitialAltitude { get; set; }
        public string StepclimbString { get; set; }
        public int AvgTempDev { get; set; }
        public int AvgTropopause { get; set; }
        public int AvgWindComp { get; set; }
        public int AvgWindDir { get; set; }
        public int AvgWindSpd { get; set; }
        public int GcDistance { get; set; }
        public int RouteDistance { get; set; }
        public int AirDistance { get; set; }
        public int TotalBurn { get; set; }
        public int CruiseTas { get; set; }
        public double CruiseMach { get; set; }
        public int Passengers { get; set; }
        public string Route { get; set; }
        public string RouteIfps { get; set; }
        public string RouteNavigraph { get; set; }
    }

    public class OriginParameters
    {
        public string IcaoCode { get; set; }
        public int Elevation { get; set; }
        public double PosLat { get; set; }
        public double PosLong { get; set; }
        public string Name { get; set; }

        public string PlanRwy { get; set; }
    }

    public class DestinationParameters
    {
        public string IcaoCode { get; set; }

        public int Elevation { get; set; }
        public double PosLat { get; set; }
        public double PosLong { get; set; }
        public string Name { get; set; }

        public string PlanRwy { get; set; }
    }

    public class WeightParameters
    {
        public int Oew { get; set; }
        public int PaxCount { get; set; }
        public int BagCount { get; set; }
        public int PaxCountActual { get; set; }
        public int BagCountActual { get; set; }
        public double PaxWeight { get; set; }
        public double BagWeight { get; set; }
        public int FreightAdded { get; set; }
        public int Cargo { get; set; }
        public int Payload { get; set; }
        public int EstZfw { get; set; }
        public int MaxZfw { get; set; }
        public int EstTow { get; set; }
        public int MaxTow { get; set; }
        public int MaxTowStruct { get; set; }
        public string TowLimitCode { get; set; }
        public int EstLdw { get; set; }
        public int MaxLdw { get; set; }
        public int EstRamp { get; set; }
    }

    public class ParameterData
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public long TimeGenerated { get; set; }
        public string StaticId { get; set; }
        public string XmlFile { get; set; }
        public string OfpLayout { get; set; }
        public int Airac { get; set; }
        public string Units { get; set; }
    }

    public class AircraftData
    {
        public string IcaoCode { get; set; }
        public string IataCode { get; set; }
        public string BaseType { get; set; }
        public string Name { get; set; }
        public string Registration { get; set; }
        public string Fin { get; set; }
        public string Selcal { get; set; }
        public string Equipment { get; set; }
        public int FuelFactor { get; set; }
        public int MaxPassengers { get; set; }
        public string InternalId { get; set; }
        public int IsCustom { get; set; }
    }

    public class FuelData
    {
        public int Taxi { get; set; }
        public int EnrouteBurn { get; set; }
        public int Contingency { get; set; }
        public int AlternateBurn { get; set; }
        public int Reserve { get; set; }
        public int Etops { get; set; }
        public int Extra { get; set; }
        public int MinTakeoff { get; set; }
        public int PlanTakeoff { get; set; }
        public int PlanRamp { get; set; }
        public int PlanLanding { get; set; }
        public int AvgFuelFlow { get; set; }
        public int MaxTanks { get; set; }
    }

    public class ImageData
    {
        public string Directory { get; set; }
        public List<MapData> Maps { get; set; }
    }

    public class FilesData
    {
        public string Directory { get; set; }

        public List<PdfData> Pdfs { get; set; }
    }

    public class PdfData
    {
        public string PdfName { get; set; }
        public string PdfLink { get; set; }
    }

    public class MapData
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class TimeData
    {
        public int EstTimeEnroute { get; set; }
        public int SchedTimeEnroute { get; set; }
        public long SchedOut { get; set; }
        public long SchedOff { get; set; }
        public long SchedOn { get; set; }
        public long SchedIn { get; set; }
        public int SchedBlock { get; set; }
        public long EstOut { get; set; }
        public long EstOff { get; set; }
        public long EstOn { get; set; }
        public long EstIn { get; set; }
        public int EstBlock { get; set; }
        public int TaxiOut { get; set; }
        public int TaxiIn { get; set; }
        public int ReserveTime { get; set; }
        public int Endurance { get; set; }
        public int ContFuelTime { get; set; }
        public int EtopsFuelTime { get; set; }
        public int ExtraFuelTime { get; set; }
    }

    public class ChecklistItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ChecklistCategory
    {
        public string CategoryName { get; set; }
        public List<ChecklistItem> Items { get; set; }
    }
}