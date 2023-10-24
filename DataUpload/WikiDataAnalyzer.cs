using DataUpload.Data;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataUpload
{
    public class WikiDataAnalyzer
    {
        private readonly Dictionary<string, string[]> _data = new Dictionary<string, string[]>();
		public string Url { get; }
        public bool HasData => _data.Count > 0;

        public WikiDataAnalyzer(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            Url = url;

            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);

            try
            {
                var infobox = htmlDoc.DocumentNode.Descendants(0)
                    .FirstOrDefault(n => n.HasClass("infobox"))?.SelectSingleNode("tbody");

                if (infobox == null)
                    return;

                foreach (var row in infobox.ChildNodes.Where(n => n.Name == "tr"))
                {
                    if (row.ChildNodes.Count != 2)
                        continue;
                    if (row.ChildNodes[0].Name != "td")
                        continue;
                    if (row.ChildNodes[1].Name != "td")
                        continue;

                    // We have our KVP!
                    string key = ConvertToText(row.ChildNodes[0].InnerText).ToLowerInvariant();
					key = NormalizeKeys(key);
                    string value = ConvertToText(row.ChildNodes[1].InnerText);
                    try
                    {
                        bool duplicateKeyExists = _data.TryAdd(key, value.Split('\n'));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in try add {key}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Parsing {url}");
            }
		}

		public bool ValidateShip(Ship ship)
        {
            if (!HasData)
                return false;

            int dataConfirmations = 0;


            var built = BuiltYear;

            if (built < 1900 || built > 1946)
                return false;

            var shipDate = ship.Launched ?? ship.Completed;
            if (shipDate.HasValue && built.HasValue)
            {
                if (!(Math.Abs(shipDate.Value - built.Value) < 10))
                    return false;
                dataConfirmations++;
            }

            var beam = Beam;
            if (ship.Beam.HasValue && beam.HasValue)
            {
                if (!(Math.Abs(ship.Beam.Value - beam.Value) < 5))
                    return false;
                dataConfirmations++;
            }

            var length = Length;
            if (ship.Length.HasValue && length.HasValue)
            {
                if (!(Math.Abs(ship.Length.Value - length.Value) < 10))
                    return false;
                dataConfirmations++;
            }

            // type
            var classType = WarshipType;
            if (classType.HasValue)
            {
               if(classType.Value != ship.ClassType) 
                    return false;
                dataConfirmations++;
            }


            if (dataConfirmations < 3)
                return false;
            
            return true;
        }

        public WarshipType? WarshipType
        {
            get
            {
                var allLines = new List<string>();
                if (_data.TryGetValue("type", out string[] lines))
                    allLines.AddRange(lines);

                if (_data.TryGetValue("class and type", out lines))
                    allLines.AddRange(lines);

                if (allLines.Count > 0)
                {
                    foreach (WarshipType warshipType in Enum.GetValues(typeof(WarshipType)))
                    {
                        string type = warshipType.ToString();
                        foreach (var line in allLines)
                        {
                            if (line.Contains(type, StringComparison.OrdinalIgnoreCase))
                                return warshipType;
                        }
                    }
                }

                return null;
            }
        }

        public string GetClass()
        {
            return null;
        }



        public int? BuiltYear
        {
            get
            {
                return GetValue("built", RegExHelper.FindYear);
            }
        }

        public double? Beam
        {
            get
            {
				return GetValue("beam", RegExHelper.FindFeet);
            }
        }

        public double? Length
        {
            get
            {
                return GetValue("length", RegExHelper.FindFeet);
            }
        }

        public double? Speed
        {
            get
            {
                return GetValue("speed", RegExHelper.FindKnots);
            }
        }


        public double? Armor
        {
            get
            {
                if (!_data.TryGetValue("armor", out string[] value))
                    return null;

                foreach (var line in value)
                {
                    if (line.Contains("belt"))
                    {
                        return RegExHelper.FindLargestInchFromRange(line);
                    }
                }
                return null;
            }
        }

        private T? GetValue<T>(string key, Func<string, T?> regexHelper)
            where T : struct, IComparable
        {
            if (!_data.TryGetValue(key, out string[] lines))
                return null;

            T? value = null;
            foreach (var line in lines)
            {
                T? temp = regexHelper(line);
                if (temp != null && temp.Value.CompareTo(value) > 0)
                    value = temp;
            }
            return value;
        }

        private static string NormalizeKeys(string key)
        {
            switch (key)
            {
                case "laid down":
                case "built":
                case "completed":
                case "in commission":
                    return "built";
                case "armour":
                    return "armor";
                default:
                    return key;
            }
        }

        private static string ConvertToText(string innerText)
        {
            innerText = innerText.Replace("&#160;", " ");
            innerText = innerText.Replace("&#8260;", "/");
            innerText = innerText.Replace("+1/2", ".5");
            innerText = innerText.Replace("+1/4", ".25");
            return innerText.ToLowerInvariant();
        }

    }
}