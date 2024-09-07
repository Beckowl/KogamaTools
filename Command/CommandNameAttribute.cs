namespace KogamaTools.Command;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandNameAttribute : Attribute
{
    public string Name { get; }

    public CommandNameAttribute(string name)
    {
        Name = name;
    }
}
