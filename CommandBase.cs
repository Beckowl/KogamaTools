using System;
using System.Collections.Generic;

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
    
    internal class CommandVariant
    {
        public List<Type> ArgumentTypes { get; }
        private readonly Action<object[]> Callback;

        public CommandVariant(List<Type> argumentTypes, Action<object[]> callback)
        {
            ArgumentTypes = argumentTypes;
            Callback = callback;
        }

        public bool TryParseArgs(string[] args, out object[] parsedArgs)
        {
            parsedArgs = new object[args.Length];

            if (args.Length != ArgumentTypes.Count)
            {
                return false;
            }

            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    parsedArgs[i] = Convert.ChangeType(args[i], ArgumentTypes[i]);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public void Execute(object[] args)
        {
            Callback(args);
        }
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

            foreach (var variant in Variants)
            {
                if (variant.TryParseArgs(args, out var parsedArgs))
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
