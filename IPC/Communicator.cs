using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Factories;
using Eryan.Responses;
using System.Threading;

namespace Eryan.IPC
{
    /// <summary>
    /// Handles the communications between Eryan and the Eve client
    /// </summary>
    public class Communicator
    {
        Pipe pipe;
        string name;
        FunctionCallFactory factory;
        ResponseFactory rfactory;
        Boolean initialized = false;
 


        
        /// <summary>
        /// Constructs a communicator out of the given pipe-name
        /// </summary>
        /// <param name="name">The pipe to build</param>
        public Communicator(string name)
        {
            pipe = new Pipe(name);
            factory = new FunctionCallFactory();
            rfactory = new ResponseFactory();
            initialized = pipe.initialize();
            this.name = name;
        }

        /// <summary>
        /// Checks if the pipe is ready, might block
        /// </summary>
        /// <returns>True if ready, false otherwise</returns>
        public bool connect()
        {
            if (!initialized)
            {
                initialized = pipe.initialize();
            }
            //Thread.Sleep(100); 
            return pipe.isReady();
        }

        /// <summary>
        /// Make the call
        /// </summary>
        /// <param name="call">Constant function represenation as given in calls struct.</param>
        /// <param name="responsetype">The expected response as defined in Responses.RESPONSETYPE</param>
        /// <returns>The response or null if failed.</returns>
        public Response sendCall(string call, string responsetype)
        {
            byte [] response;
            Response resp;
            if(!pipe.isReady())
            {
                return null;
            }

            
            response = pipe.pipeClient(factory.build(call));
            if (response == null)
            {
                Console.WriteLine("Response is Null");
            }
            resp = rfactory.build(response, responsetype);
            
            return resp;

        }

        /// <summary>
        /// Make a call with one parameter
        /// </summary>
        /// <param name="call">The function name as represented in calls struct</param>
        /// <param name="param">The parameter to send</param>
        /// <param name="responsetype">The response type expected as defined in Responses.RESPONSETYPE</param>
        /// <returns>The response object or null if failed</returns>
        public Response sendCall(string call, string param, string responsetype)
        {
            byte[] response;
            Response resp;
            if (!pipe.isReady())
            {
                return null;
            }

            response = pipe.pipeClient(factory.build(call, param));
            resp = rfactory.build(response, responsetype);

            return resp;
        }

    }
}
