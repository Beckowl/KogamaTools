namespace KogamaTools.Command.Commands
{
#if DEBUG
    internal class ThreeArgCommand : BaseCommand
    {
        public ThreeArgCommand() : base("/threeargcommand", "A command with three arguments", 3)
        {
            AddVariant(args => TextCommand.NotifyUser("Hello!")); // will never be executed because command has no variants with arguments :C
        }
    }
#endif
}