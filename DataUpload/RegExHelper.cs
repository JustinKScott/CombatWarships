using System.Text.RegularExpressions;

namespace DataUpload
{
	public static class RegExHelper
	{
        private static Regex _knotsRegex = new Regex("(\\d?\\d[;.]?\\d?\\d?) (knots|kn|kt)");
		private static Regex _inchesRegex = new Regex("(\\d?\\d[;.]?\\d?\\d?) (in|inch|inches)");
        private static Regex _feetRegex = new Regex("(\\d?\\d?\\d?\\d[;.]?\\d?\\d?) (ft|feet)");
        private static Regex _yearRegex = new Regex("(19|20)\\d\\d");

        private static string _secondNumberSearch = "(\\d?\\d[;.]?\\d?\\d?)([–]|[-]| to ){0}";

        public static int? FindYear(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            text = text.ToLowerInvariant();

            var match = _yearRegex.Match(text);
            if (!match.Success)
                return null;

            var valueRaw = match.Groups[0].Value;
            if(!int.TryParse(valueRaw, out int value))
                return null;

            return value;
        }
        public static double? FindKnots(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            text = text.ToLowerInvariant();

            var match = _knotsRegex.Match(text);
            if (!match.Success)
                return null;

            var valueRaw = match.Groups[1].Value;
            if (!double.TryParse(valueRaw, out double value))
                return null;

            return value;
        }

        public static double? FindFeet(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            text = text.ToLowerInvariant();

            var match = _feetRegex.Match(text);
            if (!match.Success)
                return null;

            var valueRaw = match.Groups[1].Value;
            if (!double.TryParse(valueRaw, out double value))
                return null;

            return value;
        }

        public static double? FindInches(string text)
        {
            if (string.IsNullOrEmpty(text)) 
                return null;

            text = text.ToLowerInvariant();

            var match = _inchesRegex.Match(text);
            if (!match.Success)
                return null;

            var valueRaw = match.Groups[1].Value;
            if (!double.TryParse(valueRaw, out double value))
                return null;

            return value;
        }

        public static double? FindLargestInchFromRange(string text)
        {
            var match = _inchesRegex.Match(text);
            if (!match.Success)
                return null;

            var value1Raw = match.Groups[1].Value;
            if (!double.TryParse(value1Raw, out double value))
            {
                return null;
            }

            // See if there is a range of numbers
            Regex secondNumberRegex = new Regex(string.Format(_secondNumberSearch, match.Groups[0]));
            var match2 = secondNumberRegex.Match(text);

            if (match2.Success)
            {
                var value2Raw = match2.Groups[1].Value;

                if (double.TryParse(value2Raw, out double value2))
                {
                    value = Math.Max(value, value2);
                }
            }

            //Console.WriteLine($"Captured: {value} FROM {text}");
            return value;
        }
    }
}
