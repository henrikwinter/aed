using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Util;

namespace Dextra.Common
{


    public class CustomRequestValidation : RequestValidator
    {
        public CustomRequestValidation() { }

        protected override bool IsValidRequestString(
            HttpContext context, string value,
            RequestValidationSource requestValidationSource, string collectionKey,
            out int validationFailureIndex)
        {
            validationFailureIndex = -1;  //Set a default value for the out parameter.

            //This application does not use RawUrl directly so you can ignore the check.
            if (requestValidationSource == RequestValidationSource.RawUrl)
                return true;

            //Allow the query-string key data to have a value that is formatted like XML.
            //if ((requestValidationSource == RequestValidationSource.QueryString) &&
            if ( (requestValidationSource == RequestValidationSource.Form) &&  ( (collectionKey.StartsWith("Html")) || (collectionKey.Contains("HtmlContent"))) )
            {
                return true;
                //The querystring value "<example>1234</example>" is allowed.
                //if (value == "<example>1234</example>")
                //{
                //    validationFailureIndex = -1;
                //    return true;
                //}
                //else
                //    //Leave any further checks to ASP.NET.
                //    return base.IsValidRequestString(context, value,
                //    requestValidationSource,
                //    collectionKey, out validationFailureIndex);
            }
            //All other HTTP input checks are left to the base ASP.NET implementation.
            else
            {
               // return true;
                bool rf = base.IsValidRequestString(context, value, requestValidationSource,
                                                 collectionKey, out validationFailureIndex);

                return rf;
            }
        }
    }

    public class CustomRequestValidation44 : RequestValidator
    {
        public CustomRequestValidation44() { }
        protected override bool IsValidRequestString(
            HttpContext context,
            string value,
            RequestValidationSource requestValidationSource,
            string collectionKey,
            out int validationFailureIndex)
        {
            validationFailureIndex = -1;

            // This is just an example and should not
            // be used for production code.
            if (value.Contains("<%"))
            {
                return false;
            }
            else // Leave any further checks to ASP.NET. 
            {
                return base.IsValidRequestString(
                    context,
                    value,
                    requestValidationSource,
                    collectionKey,
                    out validationFailureIndex);
            }
        }
    }
}