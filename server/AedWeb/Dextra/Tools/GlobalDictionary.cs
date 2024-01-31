using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dextra.Toolsspace
{

    public class GDictionaryElement
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string Param { get; set; }
        public string Value { get; set; }
        public string Dvalue { get; set; }

        public GDictionaryElement()
        {

        }

        public GDictionaryElement(string group, string name, string param, string value,string dval)
        {
            Group = group;
            Name = name;
            Param = param;
            Value = value;
            Dvalue = dval;
        }

    }

    public class GlobalDictionary
    {
        public List<GDictionaryElement> AppDictionary { get; set; }

        public GlobalDictionary()
        {
            AppDictionary = new List<GDictionaryElement>();
            AppDictionary.Add(new GDictionaryElement("Group", "Name", "param","1", "OkValue"));
        }

        public string GetValue(string group, string name, string param, string value)
        {
            string retv = "";
            List<GDictionaryElement> v = AppDictionary.FindAll(r => r.Group == group && r.Name == name && r.Param == param && r.Value == value);
            if (v.Count == 1)
            {
                retv = v[0].Dvalue;
            } else if(v.Count==0)
            {
                retv = value;
            } else
            {
                retv = v[0].Dvalue;
            }
            return retv;
        }


    }
}