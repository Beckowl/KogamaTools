﻿namespace KogamaTools.Commands
{
#if DEBUG
    internal class TestCommand : CommandBase
    {
        public TestCommand() : base("/testmsg", "Prints a message to the console.")
        {
            AddVariant(args => TextCommand.NotifyUser("Test Command is working!! :)"));
        }
    }
#endif
}