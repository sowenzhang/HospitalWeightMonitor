using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Hospital.Web.Common.UnityEx
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly object _key = new object();

        public override object GetValue()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Items.Contains(_key))
                return HttpContext.Current.Items[_key];
            return null;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(_key))
                {
                    var obj = HttpContext.Current.Items[_key];

                    HttpContext.Current.Items.Remove(_key);
                    if (obj != null)
                    {
                        // is this really needed? 
                        if (obj is IDisposable)
                        {
                            (obj as IDisposable).Dispose();
                        }
                    }
                }
            }
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[_key] = newValue;
        }
    }
}