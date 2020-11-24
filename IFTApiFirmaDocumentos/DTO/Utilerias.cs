using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace IFTApiFirmaDocumentos.DTO
{
    public class Utilerias
    {
        public static int BufferSize = 4096 * 8;
        public static byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[BufferSize];
            MemoryStream ms = new MemoryStream();

            int read = 0;

            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
        public static XmlDocument Serializar(object o)
        {
            XmlSerializer s = new XmlSerializer(o.GetType());
            XmlDocument xml = null;
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, new UTF8Encoding());
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = ' ';
            writer.Indentation = 5;

            try
            {
                s.Serialize(writer, o);
                xml = new XmlDocument();
                string xmlString = ASCIIEncoding.UTF8.GetString(ms.ToArray());
                xml.LoadXml(xmlString);
            }
            finally
            {
                writer.Close();
                ms.Close();
            }
            return xml;
        }
        public static object Deserializar(XmlDocument xml, Type type)
        {
            XmlSerializer s = new XmlSerializer(type);
            string xmlString = xml.OuterXml.ToString();
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(ms);
            object o = null;
            try
            {
                o = s.Deserialize(reader);
            }
            finally
            {
                reader.Close();
            }
            return o;
        }
        public static object Deserializar<T>(XmlDocument xml)
        {
            return (T)Deserializar(xml, typeof(T));
        }
        public static string ConvertHex(string hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }
                return ascii;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}