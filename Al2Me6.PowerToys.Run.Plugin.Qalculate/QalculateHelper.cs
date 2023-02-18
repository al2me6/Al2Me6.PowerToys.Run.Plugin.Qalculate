using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Al2Me6.PowerToys.Run.Plugin.Qalculate
{
    public static partial class QalculateHelper
    {
        [GeneratedRegex(@"^(?:[0-9\.,\+\-\*/\^\(\)\[\]\{\}Ee&|~\s])+$")]
        private static partial Regex BasicExpressionChars();

        private static readonly string[] ConstantNames = { "e", "pi", "tau", "phi", "c", "avogadro", "boltzmann" };

        [GeneratedRegex(
            @"(
              abs|min|max|floor|ceil|sgn|round
              |pow|sqrt|cbrt
              |exp|ln|log|log2|log10
              |(a|arc)?(sin|cos|tan|csc|sec|cot)h?
              |integrate|diff
              |sum|product
              |mean|median|mode|stdevp?
              |dot|cross
              |bin|hex|oct
            )\(",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
        private static partial Regex FunctionNames();

        [GeneratedRegex(
            @"^(\-?\d+[\d\.\-\*\^Ee]*\s*)
            (
              (
                [nμumckMGTPEZY]??  # SI prefixes
                (
                  [smgAK]|sec|mol|cd  # SI base units
                  |[LlNJWTVCF]|liter|litre|Hz|hertz|Pa|pascal|bar|ohm  # SI derived units
                )
              )
              |min|h|hr|hour|d|day|week|month|yr|year  # Time
              |in|ft|foot|feet|yd|yard|mi|mile|ly|pc|parsec  # Length
              |ha|acre  # Area
              |floz|cup|gal|gallon|barrel  # Volume
              |oz|lb|pound|k\s*lb|ton|tonne  # Mass
              |oC|oF|[Cc]elsius|[Ff]ahrenheit  # Temperature
              |atm|mmHg|[Tt]orr  # Pressure
              |([kMGTPEZY]i?)?B  # Information
            )",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
        private static partial Regex UnitConversion();

        [GeneratedRegex(@"^(?:0[BbOoXx])?[0-9A-Fa-f]+(?:\s+to\s+(?:bin|binary|oct|octal|hex|hexadecimal))?$")]
        private static partial Regex BaseConversion();

        private static readonly string? _qalculatePath = null;

        static QalculateHelper()
        {
            var sb = new StringBuilder("qalc.exe");
            if (PathFindOnPath(sb, null))
            {
                _qalculatePath = sb.ToString();
            }
        }

        public static bool QalculateFound => _qalculatePath != null;

        public static bool ShouldEvaluateGlobally(string query)
        {
            return (BasicExpressionChars().IsMatch(query) && query.Any(char.IsAsciiDigit))
                || ConstantNames.Contains(query)
                || FunctionNames().IsMatch(query)
                || UnitConversion().IsMatch(query)
                || BaseConversion().IsMatch(query);
        }

        public static string? Evaluate(string query)
        {
            if (_qalculatePath == null)
            {
                throw new InvalidOperationException("Qalculate is not found on this system.");
            }

            query = query.Replace("\"", @"\""");
            using var qalc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _qalculatePath,
                    Arguments = $"-defaults -nocurrencies -terse \"{query}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                },
            };
            qalc.Start();
            return qalc.StandardOutput.ReadLine();
        }

        // https://stackoverflow.com/a/52435685
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        private static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[]? ppszOtherDirs);
    }
}
