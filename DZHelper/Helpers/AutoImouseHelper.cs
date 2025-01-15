using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using System.Drawing;
//using System.Drawing.Common;
using System.IO;
using KAutoHelper;

namespace DZHelper.Helpers
{
    public static class AutoImouseHelper
    {
        private static IMouseModel CastModel(object item)
        {
            if (item == null)
                throw new InvalidCastException("Cannot cast null object to IMouseModel");
            return item as IMouseModel;
        }

        [MethodCategory(true, "IOS-", 1, false, 200)]
        public static async Task<string> LoadDevices()
        {
            return await AutoImouseApi.LoadDevicesAsync();
        }

        [MethodCategory(true, "IOS-", 2, true, 200)]
        public static async Task ATest(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory(true, "IOS-", 3, true, 200)]
        public static async Task Tap(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "Tap";
            AutoImouseApi.TapAsync(model.Id,0,0);
        }

        [MethodCategory(true, "IOS-", 4, true, 200)]
        public static async Task Swipe(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "Tap";
            AutoImouseApi.SwipeAsync(model.Id, 100, 50, 100, 300);
        }

        [MethodCategory(true, "IOS-", 5, true, 200)]
        public static async Task LongPress(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "Tap";
            AutoImouseApi.LongPressAsync(model.Id, 50, 100, 200);
        }

        

        [MethodCategory(true, "IOS-", 6, true, 200)]
        public static async Task ScreenShoot(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "ScreenShoot";
            AutoImouseApi.ScreenshotAsync(model.Id);
        }

        public static async Task FindImage(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "FindImage";
            Bitmap mainBitmap = null, subBitmap = null;
            var image = ImageScanOpenCV.FindOutPoint(mainBitmap, subBitmap);

        }

        public static async Task<Point> GetPointByImage()
        {

            return new Point(0, 0);
        }

        public static async Task<Point> GetPointByImages()
        {
            return new Point(0, 0);
        }

    }
}
