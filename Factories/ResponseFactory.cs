using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Wrappers;
using eveobjects;

namespace Eryan.Factories
{
    class ResponseFactory
    {
        public Response build(byte[] response, string responsetype)
        {
            try
            {
                if (responsetype.Equals(Response.RESPONSES.INTERFACERESPONSE))
                {
                    return new InterfaceResponse(response);
                }

                return null;
            }
            catch (Google.ProtocolBuffers.UninitializedMessageException e)
            {
                Console.WriteLine("Uninitalized values");
                return null;
            }
        }

    }
}
