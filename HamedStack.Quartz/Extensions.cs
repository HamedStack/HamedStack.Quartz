
// ReSharper disable All 
namespace Quartz
{
    internal static class Extensions
    {
        internal static Action<object>? Convert<T>(this Action<T> myActionT)
        {
            return myActionT == null ? null : new Action<object>(o => myActionT((T)o));
        }
    }
}
