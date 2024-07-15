using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
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

        internal static bool TryExecuteCommand(string commandLine)
        {
            commandLine = commandLine.TrimEnd();
            string[] components = commandLine.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string commandName = components[0];
            ICommand command = commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

            if (command == null)
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
