namespace IFPlanner.Components.Models;

public class InfiniteFlightAirplaneDataModel
{
    public string Name { get; set; }
    public string Icao { get; set; }
    public int MaxPax { get; set; }
    public int Mtow { get; set; }
    public int Mlw { get; set; }
    public int MaxFuel { get; set; }
    public int MaxCargo { get; set; }
    public int BagWgt { get; set; }
    public double PaxWgt { get; set; }
        
    public InfiniteFlightAirplaneDataModel(string name, string icao,int maxPax,int mtow,int mlw,int maxFuel,int maxCargo,int bagWgt,double paxwgt)
    {
        Name = name;
        Icao = icao;
        MaxPax = maxPax;
        Mtow = mtow;
        Mlw = mlw;
        MaxFuel = maxFuel;
        MaxCargo = maxCargo;
        BagWgt = bagWgt;
        PaxWgt = paxwgt;
    }
}