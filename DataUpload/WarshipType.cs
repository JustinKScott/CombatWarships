namespace DataUpload
{
	public enum WarshipType
    {
        Unknown,
        Cruiser,
        Destroyer,
        Submarine,
        Carrier,
        CoastalDefense,
        Monitor,
        Battleship,
        Battlecruiser,
        Convoy
    }

    public class WarshipTypeHelper
	{
        public static WarshipType Lookup(string abbreviation)
        {
            switch (abbreviation.ToLowerInvariant())
            {
                case "cae":
                case "cl":
                case "ca":
                case "cp":
                    return WarshipType.Cruiser;
                case "dd":
                    return WarshipType.Destroyer;
                case "ss":
                    return WarshipType.Submarine;
                case "cve":
                case "cva":
                case "cvl":
                case "cv":
                    return WarshipType.Carrier;
                case "cds":
                    return WarshipType.CoastalDefense;
                case "mn":
                    return WarshipType.Monitor;
                case "dn":
                case "pdn":
                case "bb":
                    return WarshipType.Battleship;
                case "bc":
                    return WarshipType.Battlecruiser;
                case "con":
                    return WarshipType.Convoy;
                default:
                    return WarshipType.Unknown;
            }
        }

    }
}
