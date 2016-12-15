using System;

namespace Blobify.Shared.Helpers
{
    public static class Safe
    {
        public static bool Run(Action action)
        {
            try
            {
                action();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Run<T>(T instance, Action<T> action)
        {
            try
            {
                action(instance);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
