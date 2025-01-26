using System.Globalization;
using UnityEngine;

namespace KogamaTools.Config.BindConverters;
public class UnityVector3BindConverter : IBindConverter
{
    private static string GroupSeparator => NumberFormatInfo.CurrentInfo.NumberGroupSeparator;
    public string Serialize(object value)
    {
        if (value is not Vector3 vector3)
        {
            throw new ConvertUnsupportedTypeException(this, value.GetType());
        }

        return vector3.ToString();
    }

    public object Deserialize(string text)
    {
        text = text.Trim('(', ')').Trim();

        string[] components = text.Split(',');

        float x = float.Parse(components[0].Trim(), CultureInfo.InvariantCulture);
        float y = float.Parse(components[1].Trim(), CultureInfo.InvariantCulture);
        float z = float.Parse(components[2].Trim(), CultureInfo.InvariantCulture);

        return new Vector3(x, y, z);
    }
}
