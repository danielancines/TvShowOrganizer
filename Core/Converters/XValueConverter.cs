﻿using System;
using System.Xml.Linq;

namespace Labs.WPF.Core.Converters
{
    public class XValueConverter
    {
        public T GetValue<T>(XElement element)
        {
            if (element == null)
                return default(T);

            if (typeof(T) == typeof(DateTime?))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(element.Value))
                        return default(T);

                    var dtResult = DateTime.Now;
                    if (DateTime.TryParse(element.Value, out dtResult))
                        return (T)Convert.ChangeType(dtResult, Nullable.GetUnderlyingType(typeof(T)));

                    return default(T);
                }
                catch
                {
                    return default(T);
                }
            }

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    var result = 0;
                    if (int.TryParse(element.Value, out result))
                        return (T)Convert.ChangeType(result, typeof(T));
                    else
                        break;
                case TypeCode.String:
                    if (element.Value == null)
                        return (T)Convert.ChangeType(string.Empty, typeof(T));
                    else
                        return (T)Convert.ChangeType(element.Value, typeof(T));
                case TypeCode.Double:
                    var dResult = default(double);
                    if (double.TryParse(element.Value, out dResult))
                        return (T)Convert.ChangeType(dResult, typeof(T));
                    else
                        break;
                case TypeCode.DateTime:
                    var dtResult = DateTime.Now;
                    DateTime.TryParse(element.Value, out dtResult);

                    try
                    {
                        var a = (T)Convert.ChangeType(dtResult, typeof(T));
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    return (T)Convert.ChangeType(dtResult, typeof(T));
                default:
                    return default(T);
            }

            return default(T);
        }
    }
}
