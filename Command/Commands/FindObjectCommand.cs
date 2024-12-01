using KogamaTools.Helpers;
using UnityEngine;

namespace KogamaTools.Command.Commands;

#if DEBUG
[CommandName("/findobj")]
[CommandDescription("Finds a gameobject by its name and notifies whether it was found or not.")] // how would you describe it?

internal class FindObjectCommand : BaseCommand
{
    [CommandVariant]
    private void FindObject(string name)
    {
        UnityEngine.Object obj = GameObject.Find(name);

        NotificationHelper.NotifyUser((obj == null).ToString());
    }
}
#endif