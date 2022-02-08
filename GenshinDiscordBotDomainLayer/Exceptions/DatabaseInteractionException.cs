using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Exceptions
{
    public class DatabaseInteractionException : Exception
    {
        public DatabaseInteractionException() : base() { }
        public DatabaseInteractionException(string message) : base(message) { }
        public DatabaseInteractionException(string message, Exception innerException)
            : base(message, innerException) { }
        public DatabaseInteractionException(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext context)
            : base(serializationInfo, context) { }
    }
}
