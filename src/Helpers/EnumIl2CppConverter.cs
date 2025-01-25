using Il2CppInterop.Runtime;

namespace KogamaTools.Helpers;

internal static class EnumIl2CppConverter
{
    internal static unsafe Il2CppSystem.Object ToIl2Cpp<T>(this T value) where T : Enum
    {
        IntPtr obj = IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<T>.NativeClassPtr);
        *(long*)((byte*)obj + 8) = Convert.ToInt64(value);

        return new Il2CppSystem.Object(obj);
    }
}