using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TesteFacil.Dominio.Extensions;

public static class EnumExtensions
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName();
    }
}