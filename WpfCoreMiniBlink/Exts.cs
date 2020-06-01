using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;

namespace Miniblink
{
    internal static class Exts
    {
        private static Dictionary<long, object> _keepref = new Dictionary<long, object>();

        public static long ToLong(this DateTime time)
        {
            var now = time.ToUniversalTime();
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (now.Ticks - start.Ticks) / 10000;
        }

        public static DateTime ToDate(this long time)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            time = start.Ticks + time * 10000;
            return new DateTime(time, DateTimeKind.Utc).ToLocalTime();
        }

        public static bool SW(this string str, string value)
        {
            if (str == value) return true;
            if (str == null || value == null) return false;
            return str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        public static string WKEToUTF8String(this IntPtr ptr)
        {
            return MBApi.wkeGetString(ptr).ToUTF8String();
        }

        public static string ToUTF8String(this IntPtr ptr)
        {
            var data = new List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, off++);
                if (ch == 0)
                {
                    break;
                }
                data.Add(ch);
            }
            return Encoding.UTF8.GetString(data.ToArray());
        }

        public static string ToStringW(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringUni(ptr);
        }

        public static void UIInvoke(this Border control, Action callback)
        {
            if (control.Dispatcher.Thread != System.Windows.Application.Current.Dispatcher.Thread)
            {
                control.Dispatcher.Invoke(callback);
            }
            else
            {
                callback();
            }
        }

        public static object ToValue(this long value, IntPtr es)
        {
            if (value == 0) return null;

            jsType type = MBApi.jsTypeOf(value);
            switch (type)
            {
                case jsType.NULL:
                case jsType.UNDEFINED:
                    return null;
                case jsType.NUMBER:
                    return MBApi.jsToDouble(es, value);
                case jsType.BOOLEAN:
                    return MBApi.jsToBoolean(es, value);
                case jsType.STRING:
                    return MBApi.jsToTempStringW(es, value).ToStringW();
                case jsType.FUNCTION:
                    return new JsFunc(new JsFuncWapper(value, es).Call);
                case jsType.ARRAY:
                    var len = MBApi.jsGetLength(es, value);
                    var array = new object[len];
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = MBApi.jsGetAt(es, value, i).ToValue(es);
                    }

                    return array;
                case jsType.OBJECT:
                    var ptr = MBApi.jsGetKeys(es, value);
                    var jskeys = (jsKeys) Marshal.PtrToStructure(ptr, typeof(jsKeys));
                    var keys = Utils.PtrToStringArray(jskeys.keys, jskeys.length);
                    var exp = new ExpandoObject();
                    var map = (IDictionary<string, object>) exp;
                    foreach (var k in keys)
                    {
                        map.Add(k, MBApi.jsGet(es, value, k).ToValue(es));
                    }

                    return exp;
                default:
                    throw new NotSupportedException();
            }
        }

        public static long ToJsValue(this object obj, IntPtr es)
        {
            if (obj == null)
                return MBApi.jsUndefined();
            if (obj is int)
                return MBApi.jsInt((int) obj);
            if (obj is bool)
                return MBApi.jsBoolean((bool) obj);
            if (obj is double || obj is decimal)
                return MBApi.jsDouble((double) obj);
            if (obj is float)
                return MBApi.jsFloat((float) obj);
            if (obj is DateTime)
                return MBApi.jsDouble(((DateTime) obj).ToLong());
            if (obj is string)
                return MBApi.jsString(es, obj.ToString());
            if (obj is IEnumerable)
            {
                var list = new List<object>();
                foreach (var item in (IEnumerable) obj)
                    list.Add(item);
                var array = MBApi.jsEmptyArray(es);
                MBApi.jsSetLength(es, array, list.Count);
                for (var i = 0; i < list.Count; i++)
                {
                    MBApi.jsSetAt(es, array, i, list[i].ToJsValue(es));
                }
                return array;
            }
            if (obj is TempNetFunc)
            {
                var func = (TempNetFunc) obj;
                var funcptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(jsData)));
                var jsfunc = new jsCallAsFunctionCallback(
                    (fes, fobj, fargs, fcount) =>
                    {
                        var fps = new List<object>();
                        for (var i = 0; i < fcount; i++)
                        {
                            fps.Add(MBApi.jsArg(fes, i).ToValue(fes));
                        }
                        return func(fps.ToArray()).ToJsValue(fes);
                    });
                _keepref.Add(funcptr.ToInt64(), jsfunc);
                var funcdata = new jsData
                {
                    typeName = "function",
                    callAsFunction = jsfunc,
                    finalize = FunctionFinalize
                };
                Marshal.StructureToPtr(funcdata, funcptr, false);
                return MBApi.jsFunction(es, funcptr);
            }
            if (obj is Delegate)
                return MBApi.jsUndefined();

            var jsobj = MBApi.jsEmptyObject(es);
            var ps = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var p in ps)
            {
                var v = p.GetValue(obj, null);
                if (v == null) continue;
                MBApi.jsSet(es, jsobj, p.Name, v.ToJsValue(es));
            }
            return jsobj;
        }

        private static void FunctionFinalize(IntPtr funcptr)
        {
            Marshal.FreeHGlobal(funcptr);
            var key = funcptr.ToInt64();
            if (_keepref.ContainsKey(key))
                _keepref.Remove(key);
        }

        public static T GetCustomAttribute<T>(this MethodInfo method)
        {
            var items = method.GetCustomAttributes(typeof(T), true);
            return items.Length > 0 ? (T) items.First() : default(T);
        }

        public static T GetCustomAttribute<T>(this Type t)
        {
            var items = t.GetCustomAttributes(typeof(T), true);
            return items.Length > 0 ? (T)items.First() : default(T);
        }
    }
}



