using System.Reflection;
using System.Text.RegularExpressions;
using KogamaTools.Helpers;

namespace KogamaTools.Command;

internal static class CommandHandler
{
    static CommandHandler()
    {
        LoadCommands();
    }

    private static List<ICommand> commands = new List<ICommand>();

    internal static bool TryExecuteCommand(string commandLine)
    {
        commandLine = commandLine.Trim(); // i thought the command handler already had this

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

        switch (errorCode)
        {
            case CommandResult.Ok:
                return true;
            case CommandResult.InvalidArgs:
                NotificationHelper.WarnUser($"[{string.Join(", ", components.Skip(1))}] is not a valid combination of arguments for {commandName}. Type \"{command.Names.First()} ?\" for detailed info.");
                return true;
            case CommandResult.BuildModeOnly:
                NotificationHelper.WarnUser($"{commandName} can only be used in edit mode.");
                return true;
        }

        return false;
    }

    internal static void ListCommands()
    {
        foreach (BaseCommand command in commands)
        {
            command.Describe();
        }
    }

    private static List<string> ParseArgs(string args)
    {
        List<string> result = new List<string>();
        string pattern = @"([""'])(.*?)\1|(\S+)";

        foreach (Match match in Regex.Matches(args, pattern))
        {
            if (match.Groups[2].Success)
            {
                result.Add(match.Groups[2].Value);
            }
            else if (match.Groups[3].Success)
            {
                result.Add(match.Groups[3].Value);
            }
        }
        return result;
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


