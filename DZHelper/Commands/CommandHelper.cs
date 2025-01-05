
using DZHelper.HelperCsharf;
using DZHelper.Helpers;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using System.Reflection;

namespace DZHelper.Commands
{
    public static  class CommandHelper
    {
        
        public static List<CommandInfo> GetCommandsInfo_LdPlayer()
        {
            // Lấy danh sách các phương thức static từ LdplayerHelper
            List<CommandInfo> commands = new List<CommandInfo>();

            // Lấy methods trả về là Dictionary<string, List<MethodInfo>> theo group
            var categorizedMethods = GetCategorizedMethods();
            foreach (var group in categorizedMethods)
            {
                foreach (var method in group.Value)
                {
                    var parameters = method.GetParameters();
                    var returnType = method.ReturnType;
                    string name = method.Name.ConvertToSpacedStringManual();
                    if (name == "Load Runnings"|| name == "Load All")
                    {

                    }
                    Delegate action = GenerateAction(method, returnType, parameters);
                    commands.Add(new CommandInfo
                    {
                        Name = method.Name.ConvertToSpacedStringManual(),
                        Group = method.GetCustomAttribute<MethodCategoryAttribute>().Group,
                        MethodInfo = method,
                        Action = parameters.Length == 0
                            ? GenerateActionWithoutParameters(method)
                            : null,
                        ActionWithParameters = parameters.Length > 0
                            ? GenerateActionWithParameters(method)
                            : null
                    });
                }
            }

            return commands;
        }

        private static Func<Task<object>> GenerateActionWithoutParameters(MethodInfo method)
        {
            return async () =>
            {
                var task = (Task)method.Invoke(null, null);

                if (method.ReturnType == typeof(Task))
                {
                    await task;
                    return null;
                }

                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            };
        }

        private static Func<object[], Task<object>> GenerateActionWithParameters(MethodInfo method)
        {
            return async args =>
            {
                var task = (Task)method.Invoke(null, args);

                if (method.ReturnType == typeof(Task))
                {
                    await task;
                    return null;
                }

                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            };
        }


        // Hàm hỗ trợ để tạo Action tùy thuộc vào tham số của method
        private static Func<object[], Task<object>> GenerateAction(MethodInfo method, Type returnType, ParameterInfo[] parameters)
        {
            return async (args) =>
            {
                var task = (Task)method.Invoke(null, args);

                if (method.ReturnType == typeof(Task))
                {
                    await task;
                    return null;
                }
                else if (method.ReturnType.IsGenericType)
                {
                    var resultProperty = task.GetType().GetProperty("Result");
                    return resultProperty?.GetValue(task);
                }

                return null;
            };

        }

        private static Delegate CreateGenericTaskWithArgsDelegate(MethodInfo method, Type genericType)
        {
            var funcType = typeof(Func<object[], Task<object>>);
            return new Func<object[], Task<object>>(async args =>
            {
                var task = (Task)method.Invoke(null, args);
                await task;
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            });
        }

        public static Dictionary<string, List<MethodInfo>> GetCategorizedMethods()
        {
            Type type = typeof(LdplayerHelper);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            return methods
            .Where(m => m.GetCustomAttribute<MethodCategoryAttribute>() != null) // Chỉ lấy phương thức có attribute
            .OrderBy(m => m.GetCustomAttribute<MethodCategoryAttribute>().Index) //sắp xếp theo index
            .GroupBy(m => m.GetCustomAttribute<MethodCategoryAttribute>().Group) // Nhóm theo Category
            .ToDictionary(g => g.Key, g => g.ToList());
        }
       
    }
}
