using System;
using System.Collections.Generic;
using KogamaTools.Command;

namespace KogamaTools
{
    internal interface ICommand
    {
        string Name { get; }
        string Description { get; }
        int MinArgs { get; }
        List<CommandVariant> Variants { get; }
        CommandResult TryExecute(string[] args);
        void DisplayHelp();
    }

    public enum CommandResult
    {
        Ok,
        InsufficientArgs,
        InvalidArgs
    }
    
    internal abstract class CommandBase : ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public List<CommandVariant> Variants { get; } = new List<CommandVariant>();

        public int MinArgs { get; }

        protected CommandBase(string name, string description, int minArgs = 0)
        {
            Name = name;
            Description = description;
            MinArgs = minArgs;
        }

        public void AddVariant(Action<object[]> callback, params List<Type> argumentTypes)
        {
            Variants.Add(new CommandVariant(argumentTypes, callback));
        }

        public void AddVariant(Action<object[]> callback)
        {
            Variants.Add(new CommandVariant(new List<Type> { }, callback));
        }

    public CommandResult TryExecute(string[] args)
        {
            if (args.Length > 0 && (args[0] == "?" || args[0].Equals("help", StringComparison.OrdinalIgnoreCase)))
            {
                DisplayHelp();
                return CommandResult.Ok;
            }

            if (args.Length < MinArgs)
            {
                return CommandResult.InsufficientArgs;
            }

            foreach (CommandVariant variant in Variants)
            {
                if (variant.TryParseArgs(args, out object[] parsedArgs))
                {
                    variant.Execute(parsedArgs);
                    return CommandResult.Ok;
                }
            }

            return CommandResult.InvalidArgs;
        }

        public void DisplayHelp()
        {
            TextCommand.NotifyUser($"{Name}: {Description}");
        }
    }

}
