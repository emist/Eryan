using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using Eryan.UI;
using Eryan.InputHandler;
using Eryan.Input;
using Eryan.IPC;
using Eryan.Responses;
using Eryan.Factories;

namespace Eryan.Wrappers
{
    public class AddressBook
    {
        Communicator com;
        KeyBoard kb;
        Random ran = new Random();
        Mouse m;
        PreciseMouse pm;
        WindowHandler wh;

        public AddressBook(WindowHandler wh)
        {
            this.com = wh.COMMUNICATOR;
            this.kb = wh.KEYBOARD;
            m = wh.MOUSE;
            pm = wh.PMOUSE;
            this.wh = wh;
        }

        private Button getOkButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALOKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("OK", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Open the address book
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool open()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETPEOPLEANDPLACES, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("addressbook is null");
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 4, iresp.Y + iresp.Height - 4)));
            Thread.Sleep(ran.Next(200, 300));
            m.click(true);
            pm.synchronize(m);
            return true;
        }




        /// <summary>
        /// Select the places tab in the addressbook
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool selectPlacesTab()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETADDRESSBOOKPLACESTAB, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 4, iresp.Y + iresp.Height - 4)));
            Thread.Sleep(ran.Next(200, 300));
            m.click(true);
            pm.synchronize(m);
            return true;
        }


        private bool clickAddressBookBMButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETADDRESSBOOKBMBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 4, iresp.Y + iresp.Height - 4)));
            Thread.Sleep(ran.Next(200, 300));
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Close the address book
        /// </summary>
        public void close()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETADDRESSBOOKWINDOW, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("addressbook is closed");
                return;
            }

            m.move(new Point(ran.Next(iresp.X + iresp.Width - 20, iresp.X + iresp.Width - 20), ran.Next(iresp.Y + 5, iresp.Y + 10)));
            //EPreciseMouse.synchronize(EMouse);
            //EPreciseMouse.move(new Point(ran.Next(iresp.X + iresp.Width - 5, iresp.X + iresp.Width - 3), ran.Next(iresp.Y + 2, iresp.Y + 4)));

            //EMouse.synchronize(EPreciseMouse);
            Thread.Sleep(ran.Next(200, 300));
            m.click(true);
            pm.synchronize(m);

        }



        private bool click(Button button)
        {
            if (button == null)
                return false;

            m.move(new Point(ran.Next(button.X + 5, button.X + button.Width - 5), ran.Next(button.Y + 5, button.Y + button.Height - 5)));
            Thread.Sleep(ran.Next(300, 500));
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Bookmark from the address book
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool bm()
        {
            if (!isOpen())
            {
                open();
                Thread.Sleep(ran.Next(200, 300));
            }

            selectPlacesTab();
            Thread.Sleep(ran.Next(200, 300));

            clickAddressBookBMButton();
            Thread.Sleep(ran.Next(200, 300));

            Button ok = getOkButton();
            if (ok == null)
                return false;

            bool outcome = click(ok);
            while (isOpen())
            {
                close();
                Thread.Sleep(ran.Next(200, 300));
            }

            return outcome;
        }

        /// <summary>
        /// Check if the addressbook is open
        /// </summary>
        /// <returns>True if open, false otherwise</returns>
        public bool isOpen()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETADDRESSBOOKWINDOW, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("addressbook is closed");
                return false;
            }

            return true;
        }

    }
}
