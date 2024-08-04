using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KogamaTools.Helpers;
using KogamaTools.Patches;

namespace KogamaTools.Command.Commands
{
    internal class FastRespawnCommand : BaseCommand
    {
        public FastRespawnCommand() : base("/fastrespawn", "")
        {
            AddVariant(args => Toggle());
        }

        private void Toggle()
        {
            FastRespawn.Enabled = !FastRespawn.Enabled;
            NotificationHelper.NotifySuccess($"Fast respawn {(FastRespawn.Enabled ? "enabled" : "disabled")}.");
        }
    }
}
