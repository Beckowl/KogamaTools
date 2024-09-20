using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KogamaTools.Tools.Graphics;
internal static class ShadowDistModifier
{
    internal static float ShadowDistance = UnityEngine.QualitySettings.shadowDistance;
    
    internal static void ApplyShadowDistance()
    {
        UnityEngine.QualitySettings.shadowDistance = ShadowDistance;
    }
}
