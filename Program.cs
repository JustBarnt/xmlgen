using System.Xml.Linq;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace xmlgen;

class Program
{
    static int Main(String[] args)
    {
        Option<string> messageKey = new("--message-key") { Description = "Combination MessageKey" };
        Option<List<string>> SetFields = new("--sets") { Description = "Required fields for the combination" };

        messageKey.Aliases.Add("-m");

        foreach (var a in messageKey.Aliases) {
            Console.WriteLine(a);
        }

        return 1;
    }


    private int ShowHelp() {
        Console.WriteLine("Show Usage Here");
        return 1;
    }
}
