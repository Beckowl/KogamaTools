using System.Reflection;
using Il2CppSystem.Reflection;
using KogamaTools.Helpers;

namespace KogamaTools.Command;

internal interface ICommand
{
    List<string> Names { get; }
    string Description { get; }
    bool BuildModeOnly { get; }
    List<CommandVariant> Variants { get; }
    CommandResult TryExecute(string[] args);
    void DisplayHelp();
}

public enum CommandResult
{
    Ok,
    InvalidArgs,
    BuildModeOnly
}

internal abstract class BaseCommand : ICommand
{
    public List<string> Names { get; private set; } = new List<string>();
    public string Description { get; private set; } = String.Empty;
    public bool BuildModeOnly { get; private set; }
    public List<CommandVariant> Variants { get; } = new List<CommandVariant>();

    protected BaseCommand()
    {
        LoadMetadata();
    }

    public CommandResult TryExecute(string[] args)
    {
        if (args.Length > 0 && (args[0] == "?" || args[0].Equals("help", StringComparison.OrdinalIgnoreCase)))
        {
            DisplayHelp();
            return CommandResult.Ok;
        }

        if (MVGameControllerBase.GameMode == MV.Common.MVGameMode.Play && BuildModeOnly)
            return CommandResult.BuildModeOnly;

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

    public void Describe()
    {
        NotificationHelper.NotifyUser($"{string.Join(", ", Names)}: {Description}\n");
    }

    public void DisplayHelp()
    {
        Describe();
        NotificationHelper.NotifyUser("Usage:");

        foreach (CommandVariant variant in Variants)
        {
            NotificationHelper.NotifyUser($"{Names[0]} {variant.Usage}");
        }
    }

    private void LoadNames(Type type)
    {
        IEnumerable<CommandNameAttribute> nameAttributes = type.GetCustomAttributes<CommandNameAttribute>();
        foreach (var nameAttr in nameAttributes)
        {
            Names.Add(nameAttr.Name);
        }
    }

    private void LoadDescription(Type type)
    {
        CommandDescriptionAttribute? descriptionAttribute = type.GetCustomAttribute<CommandDescriptionAttribute>();
        if (descriptionAttribute != null)
        {
            Description = descriptionAttribute.Description;
        }
    }

    private void LoadVariants(Type type)
    {
        IEnumerable<System.Reflection.MethodInfo> methods = type.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
           .Where(m => m.GetCustomAttributes<CommandVariantAttribute>().Any());

        foreach (var method in methods)
        {
            System.Reflection.ParameterInfo[] parameters = method.GetParameters();
            List<Type> argumentTypes = parameters.Select(p => p.ParameterType).ToList();
            List<string?> ArgumentNames = method.GetParameters().Select(p => p.Name).ToList();

            Action<object[]> callback = (args) => method.Invoke(this, args);

            CommandVariant variant = new CommandVariant(argumentTypes, callback);
            variant.SetUsage(string.Join(" ", ArgumentNames.Select(name => $"<{name}>")));

            Variants.Add(variant);
        }
    }

    private void LoadMetadata()
    {
        Type type = GetType();

        LoadNames(type);
        LoadDescription(type);
        LoadVariants(type);

        BuildModeOnly = type.GetCustomAttributes(typeof(BuildModeOnlyAttribute)).Any();
    }
}
