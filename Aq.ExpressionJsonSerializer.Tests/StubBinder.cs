using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aq.ExpressionJsonSerializer.Tests
{
    public class StubBinder : ISerializationBinder
    {
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName + "XXXOOO";
            typeName = serializedType.FullName;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            assemblyName = assemblyName.Replace("XXXOOO", "");
            return Type.GetType(typeName + "," + assemblyName, true);
        }
    }
}
