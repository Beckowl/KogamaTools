using KogamaTools.Helpers;

namespace KogamaTools.Command;

internal interface ICommand
{
    string Name { get; }
    string Description { get; }
    List<CommandVariant> Variants { get; }
    CommandResult TryExecute(string[] args);
    void DisplayHelp();
}

public enum CommandResult
{
    Ok,
    InvalidArgs
}

internal abstract class BaseCommand : ICommand
{
    public string Name { get; }
    public string Description { get; }
    public List<CommandVariant> Variants { get; } = new List<CommandVariant>();

    protected BaseCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected CommandVariant AddVariant(Action<object[]> callback, params Type[] argumentTypes)
    {
        var variant = new CommandVariant(new List<Type>(argumentTypes), callback);
        Variants.Add(variant);
        return variant;
    }

    protected CommandVariant AddVariant(Action<object[]> callback)
    {
        var variant = new CommandVariant(new List<Type> { }, callback);
        Variants.Add(variant);
        return variant;
    }

    public CommandResult TryExecute(string[] args)
    {
        if (args.Length > 0 && (args[0] == "?" || args[0].Equals("help", StringComparison.OrdinalIgnoreCase)))
        {
            DisplayHelp();
            return CommandResult.Ok;
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
        NotificationHelper.NotifyUser($"{Name}: {Description}");
        NotificationHelper.NotifyUser("Usage:");

        foreach (CommandVariant variant in Variants)
        {
            if (string.IsNullOrEmpty(variant.Usage))
            {
                NotificationHelper.NotifyUser(Name);
                continue;
            }


            NotificationHelper.NotifyUser(variant.Usage);
        }
    }
}
