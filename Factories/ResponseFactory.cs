using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Wrappers;
using eveobjects;

namespace Eryan.Factories
{
    /// <summary>
    /// Builds reponse objects to interpret the responses comming from the EVE client
    /// </summary>
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
            catch (Google.ProtocolBuffers.InvalidProtocolBufferException e)
            {
                Console.WriteLine("Empty response");
                return null;
            }
        }

    }
}
