using System.Reflection;
using System.Text.RegularExpressions;
using KogamaTools.Helpers;

namespace KogamaTools.Command
{
    internal static class CommandHandler
    {
        private static List<ICommand> commands = new List<ICommand>();

        internal static void LoadCommands()
        {
            KogamaTools.mls.LogInfo("Loading chat commands...");
            Type interfaceType = typeof(ICommand);
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is ICommand command)
                {
                    commands.Add(command);
                    KogamaTools.mls.LogInfo(string.Format($"Loaded \"{command.Name}\" Command!"));
                }
            }
        }

        static List<string> ParseArgs(string args)
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

        internal static bool TryExecuteCommand(string commandLine)
        {
            List<string> components = ParseArgs(commandLine);
            string commandName = components[0];
            ICommand command = commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

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
                    NotificationHelper.WarnUser($"[{string.Join(", ", components.Skip(1))}] is not a valid combination of arguments for {command.Name}. Type \"{command.Name} ?\" for detailed info.");
                    return true;
            }

            return false;
        }
    }
}
