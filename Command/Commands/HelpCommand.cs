using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KogamaTools.Command.Commands
{
    internal class HelpCommand : BaseCommand
    {
        public HelpCommand() : base("/help", "Lists the commands available.")
        {
            AddVariant(args => throw new NotImplementedException());
        }
    }
}
