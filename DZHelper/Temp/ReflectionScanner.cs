using DZHelper.Commands;
using DZHelper.HelperCsharf;
using DZHelper.Helpers.AttributeHelper;
using System.Reflection;

namespace DZHelper.Temp
{
    public static class ReflectionScanner
    {
        public static List<CommandInfo> ScanLdCommands(Type type)
        {
            var result = new List<CommandInfo>();

            //// Lấy tất cả class static (hoặc bất kỳ) trong assembly
            //var types = assembly.GetTypes()
            //                    .Where(t => t.IsClass && t.IsAbstract && t.IsSealed).ToList();
            // (IsClass && IsAbstract && IsSealed) => static class.

            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
            {
                // Xem method có [MethodCategoryAttribute] không
                var attr = method.GetCustomAttribute<MethodCategoryAttribute>();
                if (attr != null)
                {
                    var cmdInfo = new CommandInfo
                    {
                        Name = method.Name.ConvertToSpacedStringManual(),
                        Group = attr.Group,
                        Index = attr.Index,
                        ForeachDevices = attr.ForeachDevices,
                        MethodInfo = method
                    };

                    result.Add(cmdInfo);
                }
            }

            // Sắp xếp theo Order
            result = result.OrderBy(c => c.Index).ToList();
            return result;
        }

        public static MethodInfo GetMethodByName(Type type, string methodName, bool isStatic = true)
        {
            // BindingFlags tùy thuộc bạn muốn lấy method tĩnh hay instance
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic;
            flags |= isStatic ? BindingFlags.Static : BindingFlags.Instance;

            // Tìm method khớp với tên
            MethodInfo method = type.GetMethod(methodName, flags);
            return method;
        }
    }
}
