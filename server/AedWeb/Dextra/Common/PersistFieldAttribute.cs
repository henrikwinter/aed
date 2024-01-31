using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Dextra.Common
{
    public enum Location
    {
        Nowhere = 0x00,
        Context = 0x01,
        ViewState = 0x02,
        Session = 0x04,
        Application = 0x08,
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PersistFieldAttribute : Attribute
    {
        Location loc;
        string key;

        public PersistFieldAttribute()
            //: this(Location.Nowhere, null)
            : this(Location.Session, null)
        {
        }

        public PersistFieldAttribute(Location location)
            : this(location, null)
        {
        }

        public PersistFieldAttribute(Location location, string key)
        {
            Location = location;
            Key = key;
        }

        public string GetKeyFor(MemberInfo mi)
        {
            return (Key != null ? Key + "_" + mi.Name : mi.Name);
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public Location Location
        {
            get { return loc; }
            set { loc = value; }
        }

        public static PersistFieldAttribute GetAttribute(MemberInfo mi)
        {
            return (PersistFieldAttribute)Attribute.GetCustomAttribute(mi,
                                typeof(PersistFieldAttribute));
        }

        public static PersistFieldAttribute GetAttribute(MemberInfo mi,
                                Location forLocation)
        {
            PersistFieldAttribute attr = GetAttribute(mi);
            return (attr != null && attr.Location == forLocation ? attr : null);
        }
    }






}