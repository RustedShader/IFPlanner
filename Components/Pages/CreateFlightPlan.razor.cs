using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;

namespace IFPlanner.Components.Pages;

public partial class CreateFlightPlan
{
    private readonly List<Aircraft> _aircraftNames = new()
    {
        new Aircraft("Airbus A220-300", "BCS3"),
        new Aircraft("Airbus A318", "A318"),
        new Aircraft("Airbus A319", "A319"),
        new Aircraft("Airbus A320", "A320"),
        new Aircraft("Airbus A321", "A321"),
        new Aircraft("Airbus A330-200F", "A332"),
        new Aircraft("Airbus A330-300", "A333"),
        new Aircraft("Airbus A330-900", "A339"),
        new Aircraft("Airbus A340-600", "A346"),
        new Aircraft("Airbus A350", "A359"),
        new Aircraft("Airbus A380", "A388"),
        new Aircraft("Boeing 717-200", "B712"),
        new Aircraft("Boeing 737-700", "B737"),
        new Aircraft("Boeing 737-800", "B738"),
        new Aircraft("Boeing 737-900", "B739"),
        new Aircraft("Boeing 747-200", "B742"),
        new Aircraft("Boeing 747-400", "B744"),
        new Aircraft("Boeing 747-8", "B748"),
        new Aircraft("Boeing 747-AF1", "B742"),
        new Aircraft("Boeing 757-200", "B752"),
        new Aircraft("Boeing 767-300", "B763"),
        new Aircraft("Boeing 777-200ER", "B772"),
        new Aircraft("Boeing 777-200LR", "B77L"),
        new Aircraft("Boeing 777-300ER", "B77W"),
        new Aircraft("Boeing 777F", "B77F"),
        new Aircraft("Boeing 787-10", "B78X"),
        new Aircraft("Boeing 787-8", "B788"),
        new Aircraft("Boeing 787-9", "B789"),
        new Aircraft("C-17", "C17"),
        // new Aircraft("Cessna 172", "C172"),
        // new Aircraft("Cessna 208", "C208"),
        // new Aircraft("Cirrus SR22 GTS", "SR22"),
        // new Aircraft("CRJ-1000", "CRJX"),
        // new Aircraft("CRJ-200", "CRJ2"),
        // new Aircraft("CRJ-700", "CRJ7"),
        // new Aircraft("CRJ-900", "CRJ9"),
        // new Aircraft("DC-10", "DC10"),
        // new Aircraft("E175", "E175"),
        // new Aircraft("E190", "E190"),
        // new Aircraft("MD-11", "MD11"),
        // new Aircraft("MD-11F", "MD1F")
    };

    private readonly List<string> _fuelParameters = new()
    {
        "KGS",
        "LBS"
    };
    private readonly List<string> _flightPlanFormat = new()
    {
        "LIDO",
        "JBU"
    };

    private List<InfiniteFlightAirplaneDataModel> _aircraftData = new();
    private List<AirportData> _airportDataList = new();
    private List<string> _heIndentsArrival = new();
    private List<string> _heIndentsDeparture = new();
    private bool _planStepClimb = true;
    private List<RunwayData> _runwayDataList = new();
    private string _selectedHeIdentArrival;
    private string _selectedHeIdentDeparture;
    private bool _autoReserveFuel = true;
    private bool _autoPassengerCount = true;
    private bool _autoAltitude = true;
    private int _altitude;
    private string _unit = "KGS";
    private string _planFormat = "LIDO";
    

    private string SelectedAircraftIcao { get; set; }
    private string SelectedAirportIcaoDeparture { get; set; }
    private string SelectedAirportIcaoArrival { get; set; }
    private int reserveFuel { get; set; }
    private int passengers { get; set; }


    [Inject] protected IJSRuntime JsRuntime { get; set; }

    [Inject] protected NavigationManager NavigationManager { get; set; }
    
    [Inject] protected NotificationService NotificationService { get; set; }


    private string RunwayInfoArrival(string icao , string runway)
    {
        if (!string.IsNullOrEmpty(icao) && _runwayDataList != null)
        {
            var filteredAirportData = _runwayDataList.Where(a => a.AirportIdent == icao).ToList();
            if (filteredAirportData.Any())
            {
                var x = filteredAirportData.Where(f => f.HeIdent == runway).Select(f => $"Length: {f.length_ft} ft, Width: {f.width_ft} ft").ToList();
                var y = filteredAirportData.Where(f => f.LeIdent == runway).Select(f => $"Length: {f.length_ft} ft, Width: {f.width_ft} ft").ToList();
                return x.FirstOrDefault() ?? y.FirstOrDefault();
            }
            
        }
        return "NA";
    }
    private string RunwayInfoDeparture(string icao , string runway)
    {
        if (!string.IsNullOrEmpty(icao) && _runwayDataList != null)
        {
            var filteredAirportData = _runwayDataList.Where(a => a.AirportIdent == icao).ToList();
            if (filteredAirportData.Any())
            {
                var x = filteredAirportData.Where(f => f.HeIdent == runway).Select(f => $"Length: {f.length_ft} ft, Width: {f.width_ft} ft").ToList();
                var y = filteredAirportData.Where(f => f.LeIdent == runway).Select(f => $"Length: {f.length_ft} ft, Width: {f.width_ft} ft").ToList();
                return x.FirstOrDefault() ?? y.FirstOrDefault();
            }
            
        }
        return "NA";
    }
    private void SearchAirport(ChangeEventArgs input, bool isDeparture)
    {
        var inputValue = input.Value?.ToString()?.ToUpper();

        if (!string.IsNullOrEmpty(inputValue) && _runwayDataList != null)
        {
            var filteredAirportData = _runwayDataList.Where(a => a.AirportIdent == inputValue);
            var airportDatas = filteredAirportData.ToArray();
            if (airportDatas.Any())
            {
                List<string> heIndents;
                if (isDeparture)
                {
                    _heIndentsDeparture = airportDatas.Select(a => a.HeIdent).ToList();
                    heIndents = _heIndentsDeparture;
                }
                else
                {
                    _heIndentsArrival = airportDatas.Select(a => a.HeIdent).ToList();
                    heIndents = _heIndentsArrival;
                }

                var leIndents = airportDatas.Select(a => a.LeIdent).ToList();
                heIndents.AddRange(leIndents);

                if (heIndents.Any())
                {
                    if (isDeparture)
                        _selectedHeIdentDeparture = heIndents.First();
                    else
                        _selectedHeIdentArrival = heIndents.First();
                }
            }
            else
            {
                if (isDeparture)
                {
                    _heIndentsDeparture = new List<string>();
                    _selectedHeIdentDeparture = null;
                }
                else
                {
                    _heIndentsArrival = new List<string>();
                    _selectedHeIdentArrival = null;
                }
            }
        }
    }
    
    private void SearchAirport( string input, bool isDeparture)
    {
        var inputValue = input.ToUpper();

        if (!string.IsNullOrEmpty(inputValue) && _runwayDataList != null)
        {
            var filteredAirportData = _runwayDataList.Where(a => a.AirportIdent == inputValue);
            var airportDatas = filteredAirportData.ToArray();
            if (airportDatas.Any())
            {
                List<string> heIndents;
                if (isDeparture)
                {
                    _heIndentsDeparture = airportDatas.Select(a => a.HeIdent).ToList();
                    heIndents = _heIndentsDeparture;
                }
                else
                {
                    _heIndentsArrival = airportDatas.Select(a => a.HeIdent).ToList();
                    heIndents = _heIndentsArrival;
                }

                var leIndents = airportDatas.Select(a => a.LeIdent).ToList();
                heIndents.AddRange(leIndents);

                if (heIndents.Any())
                {
                    if (isDeparture)
                        _selectedHeIdentDeparture = heIndents.First();
                    else
                        _selectedHeIdentArrival = heIndents.First();
                }
            }
            else
            {
                if (isDeparture)
                {
                    _heIndentsDeparture = new List<string>();
                    _selectedHeIdentDeparture = null;
                }
                else
                {
                    _heIndentsArrival = new List<string>();
                    _selectedHeIdentArrival = null;
                }
            }
        }
    }

    private void HandleInputAsync(ChangeEventArgs args, bool isDeparture)
    {
        var value = args.Value?.ToString()?.ToUpper();
        if (!string.IsNullOrEmpty(value) && value.Length == 4)
        {
            if (isDeparture)
            {
                SelectedAirportIcaoDeparture = value;
                SearchAirport(args, true);
            }
            else
            {
                SelectedAirportIcaoArrival = value;
                SearchAirport(args, false);
            }
        }
    }
    

    private void HandleInputChangeAsync(ChangeEventArgs args, bool isDeparture)
    {
        HandleInputAsync(args, isDeparture);
    }

    private void OnSubmit()
    {
        NotificationService.Notify(NotificationSeverity.Success, "Success", "Form submitted successfully!");
    }

    private void OnIvalidSubmit()
    {
        NotificationService.Notify(NotificationSeverity.Error, "Error", "Form has errors!");
    }

    private async Task SimbriefSubmit()
    {
        var baseUri = NavigationManager.BaseUri;
        await JsRuntime.InvokeAsync<string>("simbriefsubmit", $"{baseUri}flight-plan");
    }

    private string kilogram_to_pound_thousands(double kgWeight)
    {
        var weightInPound = kgWeight * 2.20462 / 1000;
        return $"{weightInPound:F3}";
    }

    private async Task UrlCheck()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);
        if (query.ContainsKey("departureAirport")) SelectedAirportIcaoDeparture = query["departureAirport"];
        var eventArgs = new ChangeEventArgs
        {
            Value = SelectedAirportIcaoDeparture
        };

        // Do something with the event args
        SearchAirport(eventArgs, true);
        if (query.ContainsKey("arrivalAirport")) SelectedAirportIcaoArrival = query["arrivalAirport"];
        var eventArgsArrival = new ChangeEventArgs
        {
            Value = SelectedAirportIcaoArrival
        };

        // Do something with the event args
        SearchAirport(eventArgsArrival, false);

        if (query.ContainsKey("aircraftModel")) SelectedAircraftIcao = query["aircraftModel"];
    }

    private bool _isInitialized;
    
    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            var ifData = await LoadJsonAsync<InfiniteFlightAirplaneDataModel>("wwwroot/IFData.json") ?? new List<InfiniteFlightAirplaneDataModel>();
            _aircraftData = ifData;

            var runwayData = await LoadCsvAsync<RunwayData>("wwwroot/runways.csv");
            _runwayDataList = runwayData;

            var airportData = await LoadCsvAsync<AirportData>("wwwroot/airports.csv");
            _airportDataList = airportData.ToList();

            await UrlCheck();

            _isInitialized = true;
        }
    }
    
    private async Task<List<T>> LoadJsonAsync<T>(string filePath)
    {
        var fileContent = await GetFileContentAsync(filePath);
        return JsonConvert.DeserializeObject<List<T>>(fileContent);
    }

    private async Task<List<T>> LoadCsvAsync<T>(string filePath)
    {
        // Read the file content asynchronously
        var fileContent = await GetFileContentAsync(filePath);

        // Use a StringReader for in-memory string operations
        using (var reader = new StringReader(fileContent))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            // Use a buffered approach if supported by the CSV library
            var records = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }
            return records;
        }
    }

    private async Task<string> GetFileContentAsync(string filePath)
    {
        if (!FileCache.ContainsKey(filePath))
        {
            var fileContent = await File.ReadAllTextAsync(filePath);
            FileCache[filePath] = fileContent;
        }

        return FileCache[filePath];
    }

    private Dictionary<string, string> FileCache { get; } = new Dictionary<string, string>();
    

    public class Aircraft
    {
        public Aircraft(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class InfiniteFlightAirplaneDataModel
    {
        public InfiniteFlightAirplaneDataModel()
        {
            // Parameterless constructor
        }

        public InfiniteFlightAirplaneDataModel(string name, string icao, int maxpax, double oew, double mzfw, int mtow,
            int mlw, int maxfuel, int maxcargo, string hexcode, int bagwgt, double paxwgt, int fuelburn, string engines)
        {
            Name = name;
            Icao = icao;
            MaxPax = maxpax;
            Oew = oew;
            Mzfw = mzfw;
            Mtow = mtow;
            Mlw = mlw;
            MaxFuel = maxfuel;
            MaxCargo = maxcargo;
            Hexcode = hexcode;
            BagWgt = bagwgt;
            PaxWgt = paxwgt;
            Fuelburn = fuelburn;
            Engines = engines;
        }

        public string Name { get; set; }
        public string Icao { get; set; }
        public int MaxPax { get; set; }
        public double Oew { get; set; }
        public double Mzfw { get; set; }
        public int Mtow { get; set; }
        public int Mlw { get; set; }
        public int MaxFuel { get; set; }
        public int MaxCargo { get; set; }
        public string Hexcode { get; set; }
        public int BagWgt { get; set; }
        public double PaxWgt { get; set; }
        public int Fuelburn { get; set; }
        public string Engines { get; set; }
    }

    public class RunwayData
    {
        
        [Name("airport_ident")] public string AirportIdent { get; set; }

        [Name("he_ident")] public string HeIdent { get; set; }

        [Name("le_ident")] public string LeIdent { get; set; }
        
        [Name("length_ft")] public string length_ft { get; set; }

        [Name("width_ft")] public string width_ft { get; set; }
    }

    public class AirportData
    {
        [Name("ident")] public string AirportIdent { get; set; }

        [Name("name")] public string AirportName { get; set; }
        
    }
}
