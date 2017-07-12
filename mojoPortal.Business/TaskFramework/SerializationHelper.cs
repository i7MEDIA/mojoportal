// Author:					
// Created:				    2007-12-30
// Last Modified:			2009-08-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using log4net;

namespace mojoPortal.Business
{
    /// <summary>
    ///
    /// </summary>
    public class SerializationHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SerializationHelper));


        public const string XmlDeclarationHeader = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

        private SerializationHelper() { }

        public static string RemoveXmlDeclaration(string inputXml)
        {
            if (inputXml == null) return null;

            return inputXml.Replace(XmlDeclarationHeader, string.Empty);
        }

        public static string RestoreXmlDeclaration(string inputXmlWithNoHeader)
        {
            if (inputXmlWithNoHeader == null) return null;

            if (inputXmlWithNoHeader.StartsWith(XmlDeclarationHeader))
            {
                return inputXmlWithNoHeader;
            }

            return XmlDeclarationHeader + inputXmlWithNoHeader;
        }
        
        /// <summary>
        /// Deserializes an object from an xml string
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public static object DeserializeFromString(Type type, string serializedObject)
        {
            
            using(XmlTextReader reader = new XmlTextReader(new StringReader(serializedObject)))
            {
                XmlSerializer serializer = new XmlSerializer(type);
               
                return serializer.Deserialize(reader);
            }
             
        }
     
        /// <summary>
        /// Serializes a serializable object to xml string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj)
        {
            // should we use an XmlWriter here?
            using(StringWriter writer = new StringWriter())
            //using(XmlTextWriter w = new XmlTextWriter(writer))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, obj, ns);
                return writer.ToString();

                //serializer.Serialize(w, obj, ns);
                // return w.ToString();
            }
            
        }

        //public static string EncodeTo64(string toEncode)
        //{

        //    byte[] toEncodeAsBytes

        //          = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncode);

        //    string returnValue

        //          = System.Convert.ToBase64String(toEncodeAsBytes);

        //    return returnValue;

        //}

        //public static string DecodeFrom64(string encodedData)
        //{

        //    byte[] encodedDataAsBytes

        //        = System.Convert.FromBase64String(encodedData);

        //    string returnValue =

        //       System.Text.ASCIIEncoding.UTF8.GetString(encodedDataAsBytes);

        //    return returnValue;

        //}

        ///// <summary>
        ///// This method can not be used in medium trust
        ///// </summary>
        ///// <param name="serializedObject"></param>
        ///// <returns></returns>
        //public static object DeserializeFromSoap(string serializedObject)
        //{
        //    MemoryStream stream = null;
        //    object obj = null;
        //    if (serializedObject.Length > 0)
        //    {
        //        try
        //        {
        //            SoapFormatter formatter = new SoapFormatter();
        //            ASCIIEncoding encoding = new ASCIIEncoding();
        //            stream = new MemoryStream(encoding.GetBytes(serializedObject));
        //            obj = formatter.Deserialize(stream);
        //        }
        //        finally
        //        {
        //            if (stream != null)
        //            {
        //                stream.Close();
        //            }
        //        }
        //    }
        //    return obj;
        //}

        ///// <summary>
        ///// This method can not be used in medium trust
        ///// </summary>
        ///// <param name="serializedObject"></param>
        ///// <returns></returns>
        //public static string SerializeToSoap(object obj)
        //{
        //    if (obj == null) return null;

        //    MemoryStream stream = null;
        //    StreamReader reader = null;
        //    string soap = null;

        //    try
        //    {
        //        stream = new MemoryStream();
        //        SoapFormatter formatter = new SoapFormatter();
        //        formatter.Serialize(stream, obj);

        //        stream.Position = 0;
        //        reader = new StreamReader(stream);
        //        soap = reader.ReadToEnd();
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //        {
        //            stream.Close();
        //        }

        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //    }

        //    return soap;
        //}

    }
}
