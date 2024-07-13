using System.Linq;
namespace KogamaTools.Commands
{
    internal class ClearCommand : BaseCommand
    {
        public ClearCommand() : base("/clear", "Prints a bunch of newlines to pretend it's clearing the chat")
        {
            AddVariant(args => TextCommand.NotifyUser(string.Concat(Enumerable.Repeat("\n", 10))));
        }
    }
}
