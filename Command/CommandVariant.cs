using System;
using System.Collections.Generic;

namespace KogamaTools.Command
{
    internal class CommandVariant
    {
        public List<Type> ArgumentTypes { get; }
        private readonly Action<object[]> Callback;
        public string Usage { get; set; } = "";

        public CommandVariant(List<Type> argumentTypes, Action<object[]> callback)
        {
            ArgumentTypes = argumentTypes;
            Callback = callback;
        }

        public void SetUsage(string usage)
        {
            Usage = usage;
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
}
