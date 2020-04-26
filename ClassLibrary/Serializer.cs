using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClassLibrary
{
    public static class Serializer
    {
        static public object DeserializeObject(byte[] arrBytes)
        {
            if (arrBytes.Length == 0)
                return null;
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = binForm.Deserialize(memStream);

                return obj;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        static public byte[] SerializeObject(object objToSerialize)
        {
            if (objToSerialize == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, objToSerialize);
                return ms.ToArray();
            }
        }

    }
}
