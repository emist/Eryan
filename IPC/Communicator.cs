using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Factories;
using Eryan.Wrappers;

namespace Eryan.IPC
{
    class Communicator
    {
        Pipe pipe;
        string name;
        FunctionCallFactory factory;

        public struct calls
        {
            public const string ATLOGIN = FunctionCallFactory.calls.ATLOGIN;
            public const string FINDBYNAME = FunctionCallFactory.calls.FINDBYNAME;
        }


        
        /// <summary>
        /// Constructs a communicator out of the given pipe-name
        /// </summary>
        /// <param name="name">The pipe to build</param>
        public Communicator(string name)
        {
            pipe = new Pipe(name);
        }

        /// <summary>
        /// Checks if the pipe is ready, might block
        /// </summary>
        /// <returns>True if ready, false otherwise</returns>
        public bool connect()
        {
            return pipe.isReady();
        }

        /// <summary>
        /// Make the call
        /// </summary>
        /// <param name="callName">Constant function represenation as given in calls struct.</param>
        /// <returns>The response or null if failed.</returns>
        public Response sendCall(string call)
        {
            byte [] response;
            Response resp;
            if(!pipe.isReady())
            {
                return null;
            }

            response = pipe.pipeClient(factory.build(call));
            try
            {
                resp = new InterfaceResponse(response);
            }
            catch (Google.ProtocolBuffers.UninitializedMessageException e)
            {
                Console.WriteLine("Uninitalized values");
                return null;
            }

            return resp;

        }

        /// <summary>
        /// Make a call with one parameter
        /// </summary>
        /// <param name="call">The function name as represented in calls struct</param>
        /// <param name="param">The parameter to send</param>
        /// <returns>The response object or null if failed</returns>
        public Response sendCall(string call, string param)
        {
            byte[] response;
            Response resp;
            if (!pipe.isReady())
            {
                return null;
            }

            response = pipe.pipeClient(factory.build(call, param));
            try
            {
                resp = new InterfaceResponse(response);
            }
            catch (Google.ProtocolBuffers.UninitializedMessageException e)
            {
                Console.WriteLine("Uninitalized values");
                return null;
            }

            return resp;

        }

    }
}
