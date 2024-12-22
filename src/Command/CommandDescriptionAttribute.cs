namespace KogamaTools.Command;

[AttributeUsage(AttributeTargets.Class)]
public class CommandDescriptionAttribute : Attribute
{
    public string Description { get; }

    public CommandDescriptionAttribute(string description)
    {
        Description = description;
    }
}
