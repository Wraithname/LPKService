using System;
using System.Linq;
using System.Reflection;

namespace LPKService.Repository
{
    public enum ToStringBy { AttrDispalyName, AttrColumnName, PropertyName }
    public class BaseModel
    {
        public override string ToString()
        {
            return ToString(ToStringBy.PropertyName);
        }

        public string ToString(ToStringBy by)
        {
            Func<PropertyInfo, string> selector;
            switch (by)
            {
                case ToStringBy.AttrDispalyName:
                    {
                        selector = x => x.GetCustomAttribute<ColumnAttribute>().DisplayName + $" : {x.GetValue(this)}";
                        break;
                    }
                case ToStringBy.PropertyName:
                    {
                        selector = x => $"{x.Name} : {x.GetValue(this)}";
                        break;
                    }
                case ToStringBy.AttrColumnName:
                default:
                    {
                        selector = x => x.GetCustomAttribute<ColumnAttribute>().ColumnName + $" : {x.GetValue(this)}";
                        break;
                    }
            }
            return GetType()
                .GetProperties()?
                .Where(x => x.GetCustomAttribute<ColumnAttribute>() != null)
                .Select(selector)
                .DefaultIfEmpty("")
                .Aggregate((x, y) => x + "; " + y);
        }
    }
}
