using Il2CppInterop.Runtime;

namespace KogamaTools.Helpers
{
    // MauryDev
    internal static class Il2CppTypeHelper
    {
        internal static bool IsEqual(this Type type1, Type typ2)
        {
            return Il2CppType.From(type1) == Il2CppType.From(type1);
        }
        internal static bool IsEqual<T1, T2>()
        {
            return Il2CppType.Of<T1>() == Il2CppType.Of<T2>();
        }
        internal static bool Is<T1>(this Il2CppSystem.Type that)
        {
            return that == Il2CppType.Of<T1>();
        }
        internal static bool Is<T1>(this Il2CppSystem.Object that)
        {
            return that.GetIl2CppType() == Il2CppType.Of<T1>();
        }
        internal static Il2CppSystem.Type GetIl2Type(this Type that)
        {
            return Il2CppType.From(that);
        }
    }
}