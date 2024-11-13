namespace IFPlanner.Components.Models;

public class ToFlyRoutesClass
{
    public class ShortHaulScenicRoute
    {
        public string routeName { get; set; }
        public string originIcao { get; set; }
        public string destinationIcao { get; set; }
        public string aircraft { get; set; }
        public string aircraftModel { get; set; }
        public string livery { get; set; }
        public string flightTime { get; set; }
    }

    public class MediumHaulScenicRoute
    {
        public string routeName { get; set; }
        public string originIcao { get; set; }
        public string destinationIcao { get; set; }
        public string aircraft { get; set; }
        public string aircraftModel { get; set; }
        public string livery { get; set; }
        public string flightTime { get; set; }  
    }

    public class LongHaulScenicRoute 
    {
        public string routeName { get; set; }
        public string originIcao { get; set; }
        public string destinationIcao { get; set; }
        public string aircraft { get; set; }
        public string aircraftModel { get; set; }
        public string livery { get; set; }
        public string flightTime { get; set; }
    }

    public class BusyRoute
    {
        public string routeName { get; set; }
        public string originIcao { get; set; }
        public string destinationIcao { get; set; }
        public string aircraft { get; set; }
        public string aircraftModel { get; set; }
        public string livery { get; set; } 
        public string flightTime { get; set; }
    }

    public class RootObject
    {
        public List<ShortHaulScenicRoute> shortHaulScenicRoutes { get; set; }
        public List<MediumHaulScenicRoute> mediumHaulScenicRoutes { get; set; } 
        public List<LongHaulScenicRoute> longHaulScenicRoutes { get; set; }
        public List<BusyRoute> busiestRoutes { get; set; }
    }
}