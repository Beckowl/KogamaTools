using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace KogamaTools
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
            string[] components = commandLine.Split(new char[] { ' ' });
            string commandName = components[0];
            var command = commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

            if (command == null)
            {
                return false;
            }

            if (components.Length > 1 && (components[1] == "?" || components[1].Equals("help", StringComparison.OrdinalIgnoreCase)))
            {
                command.DisplayHelp();
                return true;
            }

            int errorCode = command.TryExecute(components.Skip(1).ToArray());

            switch (errorCode)
            {
                case 0:
                    return true;
                case 1:
                    bool plural = components.Length > 2;
                    string errorMessage = $"[{string.Join(", ", components.Skip(1))}] {(plural? "are" : "is")} not{(plural? " " : " a ")}valid argument{(plural? "s" : "")} for {commandName}.";
                    TextCommand.NotifyUser($"<color=yellow>{errorMessage}</color>");
                    return true;
                case 2:
                    TextCommand.NotifyUser($"<color=yellow>{commandName} expects at least {command.MinArgs} argument{(command.MinArgs > 1 ? "s" : "")}.</color>");
                    return true;
            }

            return false;
        }
    }
}
