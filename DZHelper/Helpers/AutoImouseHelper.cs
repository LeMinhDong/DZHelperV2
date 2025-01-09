using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using System.Drawing;

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

        [MethodCategory("IOS-", 1, true)]
        public static async Task ATest(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory("IOS-", 1, true)]
        public static async Task Click(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory("IOS-", 1, true)]
        public static async Task Tap(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory("IOS-", 1, true)]
        public static async Task Swipe(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory("IOS-", 1, true)]
        public static async Task LongPress(object item)
        {
            var model = CastModel(item);
        }

        [MethodCategory("IOS-", 1, true)]
        public static async Task<Point> GetPointByImage(object item)
        {
            var model = CastModel(item);
            return new Point(0, 0);
        }
        [MethodCategory("IOS-", 1, true)]
        public static async Task<Point> GetPointByImages(object item)
        {
            var model = CastModel(item);
            return new Point(0, 0);
        }
        [MethodCategory("IOS-", 1, true)]
        public static async Task ScreenShoot(object item)
        {
            var model = CastModel(item);
        }

    }
}
