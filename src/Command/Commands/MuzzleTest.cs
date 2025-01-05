using Il2CppInterop.Runtime;
using UnityEngine;

namespace KogamaTools.Command.Commands;
[CommandName("/muzzletest")]
internal class MuzzleTest : BaseCommand
{
    [CommandVariant]
    private void PrefabTest()
    {
        GameObject prefab = GameObject.Find("MuzzlePoint(Clone)");
        KogamaTools.mls.LogInfo(IL2CPP.Il2CppStringToManaged(IL2CPP.il2cpp_class_get_name(prefab.ObjectClass)));


    }
}
