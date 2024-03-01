namespace SFA.DAS.EarlyConnect.Domain.Configuration;

public class EarlyConnectWeb
{
    public LepsRegionCodes LepCodes { get; set; }
    public int? LinkValidityDays { get; set; }
}

public class LepsRegionCodes
{
    public string NorthEast { get; set; }
    public string Lancashire { get; set; }
    public string GreaterLondon { get; set; }
}