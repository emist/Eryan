using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using Eryan.Input;
using Eryan.Factories;
using Eryan.Responses;
using Eryan.IPC;
using Eryan.Wrappers;

namespace Eryan.InputHandler
{
    /// <summary>
    /// Camera handler
    /// </summary>
    public class Camera
    {
        Mouse m;
        PreciseMouse pm;
        Random ran = new Random();
        Communicator com;

        /// <summary>
        /// Build a camera handler with the given mouse, pm and communicator references
        /// </summary>
        /// <param name="m">The mouse reference attached to this handler</param>
        /// <param name="pm">The precise mouse reference attached to this handler</param>
        /// <param name="com">The communicator reference attached to this handler</param>
        public Camera(Mouse m, PreciseMouse pm, Communicator com)
        {
            this.m = m;
            this.pm = pm;
            this.com = com;
        }

        private bool isEmpty(List<Rectangle> recs, Point pt)
        {
            foreach (Rectangle rec in recs)
                if (rec.Contains(pt))
                    return false;
            return true;
        }

        /// <summary>
        /// Find an open area in the inflight interface
        /// </summary>
        /// <returns>An empty point in space</returns>
        public Point getOpenSpace()
        {
            List<Rectangle> recs = new List<Rectangle>();
            OverViewResponse items = (OverViewResponse)com.sendCall(FunctionCallFactory.CALLS.GETINTERFACEWINDOWS, Response.RESPONSES.OVERVIEWRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return new Point(-1, -1);
            }

            InterfaceResponse inflight = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETINFLIGHTINTERFACE, Response.RESPONSES.INTERFACERESPONSE);
            if (inflight == null)
            {
                Console.WriteLine("inflight is null");
                return new Point(-1, -1);
            }

            Point pt = new Point(ran.Next(inflight.X, inflight.X + inflight.Width), ran.Next(inflight.Y, inflight.Y + inflight.Height));

            foreach (OverViewEntry it in (List<OverViewEntry>)items.Data)
            {
                recs.Add(new Rectangle(it.X, it.Y, it.Width, it.Height));
            }

            while (!isEmpty(recs, pt))
                pt = new Point(ran.Next(inflight.X, inflight.X + inflight.Width), ran.Next(inflight.Y, inflight.Y + inflight.Height));

            return pt;
        }

        /// <summary>
        /// Rotate the camera to the left
        /// </summary>
        /// <returns>True</returns>
        public bool rotateLeft()
        {
            int speed = 0;
            m.move(getOpenSpace());
            Thread.Sleep(300);
            m.holdLeftButton();
            Thread.Sleep(300);
            speed = m.Speed;
            m.Speed = ran.Next(2, 4);
            m.move(new Point(m.x - ran.Next(60,100), ran.Next(m.y - 10, m.y)), 0, 0);
            Thread.Sleep(ran.Next(1500, 2000));
            m.Speed = speed;
            pm.synchronize(m);
            m.releaseLeftButton();
            return true;
        }


        /// <summary>
        /// Rotate the camera to the Right
        /// </summary>
        /// <returns>True</returns>
        public bool rotateRight()
        {
            int speed = 0;
            m.move(getOpenSpace());
            Thread.Sleep(300);
            m.holdLeftButton();
            Thread.Sleep(300);
            speed = m.Speed;
            m.Speed = ran.Next(2, 4);
            m.move(new Point(m.x + ran.Next(60, 100), ran.Next(m.y - 10, m.y)), 0, 0);
            Thread.Sleep(ran.Next(1500, 2000));
            m.Speed = speed;
            pm.synchronize(m);
            m.releaseLeftButton();
            return true;
        }




    }
}
