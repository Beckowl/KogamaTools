using System.Reflection;
using KogamaTools.Helpers;
using SoftCircuits.Parsing.Helper;

namespace KogamaTools.Command;

internal static class CommandHandler
{

    private static List<ICommand> commands = new List<ICommand>();
    static CommandHandler()
    {
        LoadCommands();
    }

    internal static void ListCommands()
    {
        foreach (BaseCommand command in commands)
        {
            command.Describe();
        }
    }

    internal static bool TryExecuteCommand(string commandLine)
    {
        List<string> components = ParseArgs(commandLine);
        string commandName = components[0];

        ICommand? command = commands.FirstOrDefault(c =>
            c.Names.Any(name => name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
        );

        if (command is null)
        {
            return false;
        }

        CommandResult errorCode = command.TryExecute(components.Skip(1).ToArray());

        return HandleCommandResult(errorCode, commandName, components);
    }

    private static List<string> ParseArgs(string commandLine)
    {
        List<string> components = new();
        ParsingHelper helper = new(commandLine);

        while (!helper.EndOfText)
        {
            helper.SkipWhiteSpace();
            if (helper.Peek() == '"' || helper.Peek() == '\'')
            {
                components.Add(helper.ParseQuotedText());
            }
            else
            {
                components.Add(helper.ParseWhile(c => !char.IsWhiteSpace(c)));
            }
        }

        return components;
    }

    private static bool HandleCommandResult(CommandResult errorCode, string commandName, List<string> components)
    {
        switch (errorCode)
        {
            case CommandResult.Ok:
                return true;
            case CommandResult.InvalidArgs:
                NotificationHelper.WarnUser($"[{string.Join(", ", components.Skip(1))}] is not a valid combination of arguments for {commandName}. Type \"{commandName} ?\" for detailed info.");
                return true;
            case CommandResult.BuildModeOnly:
                NotificationHelper.WarnUser($"{commandName} can only be used in edit mode.");
                return true;
            default:
                return false;
        }
    }
    private static void LoadCommands()
    {
        KogamaTools.mls.LogInfo("Loading chat commands...");
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttributes<CommandNameAttribute>().Any() && t.IsClass && !t.IsAbstract);

        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is ICommand command)
            {
                commands.Add(command);
                KogamaTools.mls.LogInfo($"Loaded \"{command.Names[0]}\" command!");
            }
        }
    }
}


