namespace KogamaTools.Commands
{
#if DEBUG
    internal class ThreeArgCommand : CommandBase
    {
        public ThreeArgCommand() : base("/threeargcommand", "A command with three arguments", 3)
        {
            AddVariant(args => TextCommand.NotifyUser("Hello!")); // will never be executed because command has no variants with arguments :C
        }
    }
#endif
}