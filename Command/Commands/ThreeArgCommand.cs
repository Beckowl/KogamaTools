using KogamaTools.Helpers;

namespace KogamaTools.Command.Commands
{
#if DEBUG
    internal class ThreeArgCommand : BaseCommand
    {
        public ThreeArgCommand() : base("/threeargcommand", "A command with three arguments")
        {
            AddVariant(args => NotificationHelper.NotifyUser("Hello!")); // will never be executed because command has no variants with arguments :C
        }
    }
#endif
}