using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dextra.Common
{
    public class UtcDateTimeConverter : DateTimeConverterBase
    {
        /// <summary> 
        /// holds ISO fromat to be used. 
        /// </summary> 
        public string DateTimeFormat { get; set; }

        /// <summary> 
        /// Sets the default IDO format 'yyyy-MM-ddTHH:mm:ss' 
        /// </summary> 
        public UtcDateTimeConverter()
        {
            this.DateTimeFormat = "yyyy-MM-dd HH:mm:ss"; //PUT HERE ANY FORMAT YOU NEED! 
        }


        /// <summary> 
        /// Invoked when caller's data has to be deserialized.  
        /// </summary> 
        /// <returns>The DateTimeOffset converted to UTC time. 
        /// In this case the offset value is allways 0.</returns> 
        public override object ReadJson(Newtonsoft.Json.JsonReader reader,
        Type objectType, object existingValue,
        Newtonsoft.Json.JsonSerializer serializer)
        {
            if (objectType == typeof(DateTime))
            {
                //SEE HERE WHY. 
                throw new NotSupportedException("DateTime not allowed");
            }
            else if (objectType == typeof(DateTimeOffset))
            {
                reader.Read();// move to datetime element 
                reader.Read();//move to datetime value 
                DateTime dt = (DateTime)reader.Value;

                reader.Read();// move to offset element 
                reader.Read();//move to offset value 
                Int64 offsetinMin = (Int64)reader.Value;

                DateTime utcTime = new DateTime(dt.AddMinutes(-offsetinMin).Ticks,
                DateTimeKind.Utc);

                reader.Read();//ensures that reader is left in valid state. 

                return new DateTimeOffset(utcTime, TimeSpan.FromMinutes(0));
            }
            else
                return null;
        }


        /// <summary> 
        /// Invoked when data has to be serialized before sending to the caller. 
        /// This method will fail if the service code tries to send DateTime  
        //  instead of DateTimeOffset. 
        /// </summary> 
        /// <param name="writer"></param> 
        /// <param name="value"></param> 
        /// <param name="serializer"></param> 
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer,
               object value,
               Newtonsoft.Json.JsonSerializer serializer)
        {
            // IF YOU WANT TO DEAL WITH DATETIME 
            // THIS SHOWS HOW TO DO THAT 
            if (value is DateTime)
            {
                DateTime utcTime = (DateTime)value;
                writer.WriteRawValue(String.Format("\"{0}\"",
               ((DateTime)utcTime.ToLocalTime()).ToString(this.DateTimeFormat)));
            }
            // EXAMPLE OF DateTimeOffset formatting 
            else if (value is DateTimeOffset)
            {
                DateTimeOffset offs = (DateTimeOffset)value;
                writer.WriteRawValue(String.Format("\"{0} UTC({1:##})\"",
                offs.ToString(this.DateTimeFormat), offs.Offset.Minutes));
            }
        }


    }
}