using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Responses;
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
                if (responsetype.Equals(Response.RESPONSES.BOOLEANRESPONSE))
                {
                    return new BooleanResponse(response);
                }
                if (responsetype.Equals(Response.RESPONSES.OVERVIEWRESPONSE))
                {
                    return new OverViewResponse(response);
                }
                if (responsetype.Equals(Response.RESPONSES.TARGETRESPONSE))
                {
                    return new TargetListResponse(response);
                }
                if (responsetype.Equals(Response.RESPONSES.ITEMRESPONSE))
                {
                    return new ItemResponse(response);
                }

                if (responsetype.Equals(Response.RESPONSES.STRINGRESPONSE))
                {
                    return new StringResponse(response);
                }

                if (responsetype.Equals(Response.RESPONSES.MENURESPONSE))
                {
                    return new MenuResponse(response);
                }

                if (responsetype.Equals(Response.RESPONSES.SOLARYSYSTEMRESPONSE))
                {
                    return new SystemResponse(response);
                }

                if (responsetype.Equals(Response.RESPONSES.STRINGGROUPRESPONSE))
                {
                    return new StringGroupResponse(response);
                }

                Console.WriteLine("Unidentified Response, not Built");

                return null;
            }
            catch (Google.ProtocolBuffers.UninitializedMessageException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            catch (Google.ProtocolBuffers.InvalidProtocolBufferException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}
