using System.Xml.Linq;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace xmlgen;

class Program
{
    //ComandLine usage something like: xmlgen --messagekeys EG,ERG,ELG --structure {Foo,Bar(Baz)[Foobar,Barbaz]}
    // {} - Indicates a Set (Required Fields)
    // () - Indicates a Choice (Conditional Fields)
    // [] - Indicates an Any (Optional fields)
    // {}()[] - followed by a :2 or :1-4
    //     Ex - {Foo,Bar(Baz,FooBar):2} or {Foo,Bar(Baz,FooBar,Barbaz):2-3}
    //     should expand to 
    //     <Set>
    //       <Field reference="Foo"/>
    //       <Field reference="Bar"/>
    //       <Choice maxOccurences="2">
    //          <Field reference="FooBar"/>
    //          <Field reference="Barbaz"/>
    //       </Choice>
    //     </Set>
    //     OR
    //     <Set>
    //       <Field reference="Foo"/>
    //       <Field reference="Bar"/>
    //       <Choice minOccurences="2" maxOccurences="3">
    //          <Field reference="FooBar"/>
    //          <Field reference="Barbaz"/>
    //       </Choice>
    //     </Set>

    private static int Main(string[] args)
    {
        RootCommand rootCommand = BuildRootCommand();
        return rootCommand.Parse(args).Invoke();
    }

    private static RootCommand BuildRootCommand()
    {
        // Setting up our rootCommand like this will allow use to exapnd and add subcommands if needed
        // in the future
        RootCommand rootCommand = new("XmlGen - Generate XML Content");
        rootCommand.Subcommands.Add(BuildMetadata());
        return rootCommand;
    }

    private static Command BuildMetadata()
    {
        Option<List<string>> messageKeysOption = new("--mke", "-m")
        {
            Description = "Message Key(s) releated to the combinations being built",
            Required = true,
            CustomParser = result => result.Tokens
                .SelectMany(t => t.Value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList()
        };

        // This will end up not being a "string" type it will likely be some type of
        // List<Dictionary> or something that will handle the nested structure we need to take account for
        // Option<string> structure = new("--structure", "-s")
        // { 
        //     Description = "Structure of XML to generate {Sets}(Choice)[Any]",
        //     Required = true
        // };

        var command = new Command("metadata", "Process `--mke` and `--structure` to output a generated XML structure")
        {
            messageKeysOption
        };

        command.SetAction(parserResult =>
                {
                    // Do stuff here with message key and structure
                    // at this point structure should be a List<Dictionary> or some type that can
                    // represent the structure passed in the command line args
                });

        return command;
    }
}
