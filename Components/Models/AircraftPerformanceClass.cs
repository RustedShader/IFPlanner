namespace IFPlanner.Components.Models;

public class AircraftPerformance
{
        public string name { get; set; }
        public string icao { get; set; }
        public int maxpax { get; set; }
        public int oew { get; set; }
        public int mzfw { get; set; }
        public int mtow { get; set; }
        public int mlw { get; set; }
        public int maxfuel { get; set; }
        public int maxcargo { get; set; }
        public string hexcode { get; set; }
        public int bagwgt { get; set; }
        public int paxwgt { get; set; }
        public int fuelburn { get; set; }
        public string engines { get; set; }
        public int maxAltitude { get; set; }
        public double wingspan { get; set; }
        public double length { get; set; }
        public string manufacturer { get; set; }
        public string type { get; set; }
        public string wakeTurbulenceCategory { get; set; }
        public int approachSpeed { get; set; }
        public int engineCount { get; set; }
        public double wheelbase { get; set; }
        public int maxRange { get; set; }
        public string aircraftType { get; set; }
        public int maxSpeed { get; set; }
        public RunwayRequirements runwayRequirements { get; set; }
        public int serviceCeiling { get; set; }
        public int climbRate { get; set; }
        public int descentRate { get; set; }
        public double thrustToWeightRatio { get; set; }
        public double fuelEfficiency { get; set; }
        public int operatingCost { get; set; }
        public EnvironmentalImpact environmentalImpact { get; set; }
        public List<SeatingConfiguration> seatingConfigurations { get; set; }
        public string productionStatus { get; set; }
}

public class RunwayRequirements
{
        public int takeoff { get; set; }
        public int landing { get; set; }
}

public class EnvironmentalImpact
{
        public double co2Emissions { get; set; }
        public int noiseLevel { get; set; }
}

public class SeatingConfiguration
{
        public string @class { get; set; }
        public int seats { get; set; }
}