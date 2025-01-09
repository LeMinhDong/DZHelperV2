using DZHelper.Commands;
using DZHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDll.Temp
{
    public static class LdplayerController
    {
        /// <summary>
        /// Hàm này gọi 1 method tĩnh (có MethodInfo), parse kiểu trả về (Task<T> hoặc Task).
        /// Sau khi thực thi, nó trả về object (nếu là Task<T>) hoặc null (nếu Task).
        /// </summary>
        public static async Task<object> InvokeLdCommandAsync(CommandInfo commandInfo, params object[] otherParams)
        {
            try
            {
                // Lấy MethodInfo
                var method = commandInfo.MethodInfo;

                // Gọi method bằng Reflection
                // Tạo mảng tham số
                var parameters = new List<object>();

                if (otherParams != null && otherParams.Length > 0)
                    parameters.AddRange(otherParams);

                parameters = parameters.Take(method.GetParameters().Count()).ToList();
                // Thực thi method -> trả về object = Task hoặc Task<T>
                object invokeResult = method.Invoke(null, parameters.ToArray());
                return invokeResult;
            }
            catch (Exception exx)
            {
                return null;
            }
        }

        /// <summary>
        /// Ví dụ hàm xử lý kết quả sau khi Invoke xong, 
        /// tùy vào kiểu "thật" mà cập nhật UI (Status, v.v.)
        /// </summary>
        public static async Task<object> HandleResultAsync(CommandInfo cmdInfo, object param, params object[] otherParams)
        {
            // Thực thi reflection
            object result = await InvokeLdCommandAsync(cmdInfo, param, otherParams);
            return result;
            if (result is Task<List<LdModel>> xdevices)
            {
                List<LdModel> devices = await xdevices;
            }
            
            // Xác định kiểu "thật" của result
            switch (result)
            {
                //case string s:
                //    // Update LdDevice.Status = s, SettingUI.Result = s, ...
                //    break;
                //case bool b:
                //    // Update status = b.ToString(), ...
                //    break;
                //case List<LdModel> listLdModes:
                //    // Chuyển sang List<LdDevice> 
                //    // ...
                //    break;
                //case null:
                //    // Task void, không trả gì
                //    break;
                default:
                    // Có thể là 1 kiểu T khác, ...
                    break;
            }
        }
    }
}
