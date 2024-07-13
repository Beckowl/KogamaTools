namespace KogamaTools.Command.Commands
{
#if DEBUG
    internal class TestCommand : BaseCommand
    {
        public TestCommand() : base("/testmsg", "Prints a message to the console.")
        {
            AddVariant(args => TextCommand.NotifyUser("Test Command is working!! :)"));
        }
    }
#endif
}