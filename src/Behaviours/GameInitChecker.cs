using System.Reflection;
using UnityEngine;

namespace KogamaTools.Behaviours;

internal class GameInitChecker : MonoBehaviour
{
    internal static OnGameInitializedDelegate OnGameInitialized = delegate { };
    internal delegate void OnGameInitializedDelegate();

    internal static bool IsInitialized = false;

    private void Update()
    {
        if (MVGameControllerBase.IsInitialized && !IsInitialized)
        {
            try
            {
                OnGameInitialized.Invoke();
                InvokeInitMethods();
            }
            catch (Exception ex)
            {
                KogamaTools.mls.LogError($"Error during initialization: {ex.ToString()}");
            }
            finally
            {
                IsInitialized = true;
                Destroy(this);
            }
        }
    }

    private void InvokeInitMethods()
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        var methods = executingAssembly.GetTypes()
            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            .Where(method => method.GetCustomAttribute<InvokeOnInitAttribute>() != null)
            .Select(method => new
            {
                Method = method,
                method.GetCustomAttribute<InvokeOnInitAttribute>()!.Priority
            })
            .OrderBy(m => m.Priority)
            .ToList();

        foreach (var method in methods)
        {
            try
            {
                if (method.Method.IsStatic)
                {
#if DEBUG
                    KogamaTools.mls.LogInfo($"InvokeOnInit: Invoking {method.Method.DeclaringType}.{method.Method.Name}.");
#endif
                    method.Method.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                KogamaTools.mls.LogError($"Error invoking method {method.Method.Name}: {ex.Message}");
            }
        }
    }
}


[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class InvokeOnInitAttribute : Attribute
{
    public int Priority { get; }

    public InvokeOnInitAttribute(int priority = int.MaxValue)
    {
        Priority = priority;
    }
}