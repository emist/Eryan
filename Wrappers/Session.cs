using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;

using Eryan.Responses;
using Eryan.Factories;
using Eryan.IPC;
using Eryan.Input;
using Eryan.UI;
using Eryan.InputHandler;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Hold eve session information
    /// </summary>
    public class Session
    {
        Communicator com;
        KeyBoard kb;
        Random ran = new Random();
        Mouse m;
        PreciseMouse pm;
        WindowHandler wh;
        AddressBook addBook;
        LocalHandler local;


        /// <summary>
        /// Builds the session object with the given windowhandler
        /// </summary>
        /// <param name="wh">The reference to the bot's windowhandler</param>
        public Session(WindowHandler wh)
        {
            this.com = wh.COMMUNICATOR;
            this.kb = wh.KEYBOARD;
            m = wh.MOUSE;
            pm = wh.PMOUSE;
            this.wh = wh;
            addBook = new AddressBook(wh);
            local = new LocalHandler(wh);
        }


        /// <summary>
        /// Check if we are loading something
        /// </summary>
        /// <returns>True if there is a progress dialog open, false otherwise</returns>
        public bool isLoading()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISLOADING, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;
            return (Boolean)bresp.Data;
        }


        /// <summary>
        /// Check if the system menu is open
        /// </summary>
        /// <returns>True if it is, false otherwise</returns>
        public bool isSystemMenuOpen()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISSYSTEMMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;
            return (Boolean)bresp.Data;
        }

        /// <summary>
        /// Get the No button of an interface if it exists
        /// </summary>
        /// <returns>The No button or null on failure</returns>

        public Button getNoButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALYESBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("NO", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Check if we are fleeted
        /// </summary>
        /// <returns>True if we are fleeted, false otherwise</returns>
        public bool amIFleeted()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISFLEETED, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;

            return (Boolean)bresp.Data;
        }

        /// <summary>
        /// Check if we are at the login screen
        /// </summary>
        /// <returns>True if we are, false otherwise</returns>
        public bool atLogin()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ATLOGIN, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;

            return (Boolean)bresp.Data;
        }
            
        /// <summary>
        /// Check if we are at the character selection screen
        /// </summary>
        /// <returns>True if we are, false otherwise</returns>
        public bool atCharSel()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISATCHARSEL, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;

            return (Boolean)bresp.Data;
        }

        /// <summary>
        /// Bookmarks a space location using the addressbook
        /// </summary>
        /// <returns>True if succeeded, false otherwise</returns>
        public bool bookMarkInSpace()
        {
            return addBook.bm();
        }


        /// <summary>
        /// Disable the autologin script
        /// </summary>
        /// <returns></returns>
        public void disableAutoLogin()
        {
            foreach (Eryan.Script.Scriptable script in wh.BACKGROUNDSCRIPTS)
                if (script.Name.Equals("Autologer"))
                    script.Enabled = false;
        }

        /// <summary>
        /// Check if the given background script is enabled
        /// </summary>
        /// <param name="scriptname">The name of the background script to check</param>
        /// <returns>True if enabled, false otherwise</returns>
        public bool isBgScriptEnabled(string scriptname)
        {
            foreach (Eryan.Script.Scriptable script in wh.BACKGROUNDSCRIPTS)
                if (script.Name.Equals("Autologer"))
                    return script.Enabled;
            return false;
        }
        /// <summary>
        /// Enable the autologin script
        /// </summary>
        public void enableAutoLogin()
        {
            foreach (Eryan.Script.Scriptable script in wh.BACKGROUNDSCRIPTS)
                if (script.Name.Equals("Autologer"))
                    script.Enabled = true;
        }

        /// <summary>
        /// Get the enter game button
        /// </summary>
        /// <returns>The button, null on failure</returns>
        public Button getEnterButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETENTERGAMEBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button(iresp.Name, iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }


        /// <summary>
        /// Get the connect button of the login interface
        /// </summary>
        /// <returns>The button, null on failure</returns>
        public Button getConnectButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETCONNECTBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button(iresp.Name, iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the username box of the login interface
        /// </summary>
        /// <returns>The box, null on failure</returns>
        public InterfaceResponse getUsernameBox()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETUSERNAMEBOX, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return iresp;
        }

        /// <summary>
        /// Check if the edit field has the info we want
        /// </summary>
        /// <param name="edit">The edit field</param>
        /// <param name="text">The text we want it to contain</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool checkEdit(InterfaceResponse edit, string text)
        {
            return edit.Name.Equals(text);
        }

        private void populateEdit(InterfaceResponse edit, string text)
        {
            string currText = edit.Name;

            if (!currText.Equals(text))
            {
                if (currText != "")
                {
                    
                    m.move(new Point(ran.Next(edit.X + 5, edit.X + edit.Width - 15), ran.Next(edit.Y + 5, edit.Y + edit.Height - 5)));
                    Thread.Sleep(ran.Next(300, 500));
                    m.click(true);
                    Thread.Sleep(ran.Next(100, 300));
                    for (int i = 0; i < currText.Length; i++)
                    {
                        kb.sendChar((char)KeyBoard.VKeys.VK_BACK);
                        Thread.Sleep(ran.Next(150, 190));
                    }
                    pm.synchronize(m);

                }

                if (currText == "")
                {
                    m.move(new Point(ran.Next(edit.X + 5, edit.X + edit.Width - 5), ran.Next(edit.Y + 5, edit.Y + edit.Height - 5)));
                    Thread.Sleep(ran.Next(300, 500));
                    m.click(true);
                    Thread.Sleep(ran.Next(100, 300));
                    kb.sendKeyPresses(text);
                    pm.synchronize(m);
                }

            }


        }

        public bool click(Button button)
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
        /// Login
        /// </summary>
        /// <param name="username">The username to login with</param>
        /// <param name="password">The password to login with</param>
        /// <returns>True on succes, false otherwise</returns>
        public bool login(string username, string password)
        {
            InterfaceResponse pentry = getPasswordBox();
            if (pentry == null)
                return false;

            populateEdit(pentry, password);

            pentry = getPasswordBox();
            if (pentry == null)
                return false;

            if (!checkEdit(pentry, password))
                return false;

            InterfaceResponse uentry = getUsernameBox();
            if (uentry == null)
                return false;

            populateEdit(uentry, username);

            uentry = getUsernameBox();
            if (uentry == null)
                return false;

            if (!checkEdit(uentry, username))
                return false;

            Button connect = getConnectButton();

            if (connect == null)
                return false;


            if (!click(connect))
                return false;



            return true;
        }


        /// <summary>
        /// Get the password box of the login interface
        /// </summary>
        /// <returns></returns>
        public InterfaceResponse getPasswordBox()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETPASSWORDBOX, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return iresp;
        }

        /// <summary>
        /// Logout
        /// </summary>
        public void logout()
        {
            if (!isSystemMenuOpen())
                openSystemMenu();

            Thread.Sleep(600);

            Button logoff = getLogoutButton();
            if (logoff != null)
            {   
                m.move(new Point(ran.Next(logoff.X + 5, logoff.X + logoff.Width - 5), ran.Next(logoff.Y + 5, logoff.Y + logoff.Height - 5)));
                Thread.Sleep(500);
                m.click(true);
                pm.synchronize(m);
                Thread.Sleep(600);
                Button yes = getYesButton();
                if (yes != null)
                {
                    m.move(new Point(ran.Next(yes.X + 5, yes.X + yes.Width - 5), ran.Next(yes.Y + 5, yes.Y + yes.Height - 5)));
                    Thread.Sleep(600);
                    m.click(true);
                    pm.synchronize(m);
                }
            }



        }

        /// <summary>
        /// Open the system menu
        /// </summary>
        public void openSystemMenu()
        {
           kb.sendChar((char)KeyBoard.VKeys.VK_ESCAPE);
        }

        /// <summary>
        /// Get the logoff button from the system menu
        /// </summary>
        /// <returns>The logout button or No on failure</returns>
        public Button getLogoutButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOGOFFBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("LOGOFF", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }


        /// <summary>
        /// Get the Yes button of an interface if it exists
        /// </summary>
        /// <returns>The Yes Button, or null on failure</returns>
        public Button getYesButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALYESBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("YES", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the OK button of an interface if it exists
        /// </summary>
        /// <returns>The ok button of the interface, null if it doesn't exist</returns>
        public Button getOkButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALOKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("OK", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the cancel button of an interface if it exists
        /// </summary>
        /// <returns>The cancel button of the interface, null if it doesn't exist</returns>
        public Button getCancelButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALCANCELBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("CANCEL", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the last server message
        /// </summary>
        /// <returns>The message or null if none exists</returns>
        public string getServerMessage()
        {

            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSERVERMESSAGE, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return null;
            }


            Regex reg = new Regex("<center>");
            string[] split = reg.Split((string)sresp.Data);
            if (split.Count() > 1)
            {
                return split[1];
            }
            return null;

        }

        /// <summary>
        /// Check if current system is undergoing an incursion
        /// </summary>
        /// <returns>Returns true if there is an incursion, false otherwise</returns>
        public Boolean isIncursionOngoing()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISINCURSIONONGOING, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
            {
                return false;
            }

            return (Boolean)bresp.Data;
        }
        
        /// <summary>
        /// Get the amount of players in local
        /// </summary>
        /// <returns>Number greater than 0 on success, -1 on failure</returns>
        public int getLocalCount()
        {
            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCOUNT, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
                return -1;

            Regex reg = new Regex("[0-9]+");

            return Convert.ToInt32(reg.Match((string)sresp.Data).Value);
        }


        /// <summary>
        /// Check if there's hostiles in local
        /// </summary>
        /// <returns>Returns true if there is hostiles in local, false otherwise</returns>
        public Boolean isLocalHostile()
        {
            local.userlistScrollToTop();
            local.userlistScrollToBottom();

            BooleanResponse tresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.CHECKLOCAL, Response.RESPONSES.BOOLEANRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve local");
                return true;
            }
            return ((Boolean)tresp.Data);
        }

        /// <summary>
        /// Get the current solar system
        /// </summary>
        /// <returns>Returns a solarsystem object on success, null on failure</returns>
        public SolarSystem getSolarSystem()
        {
            SystemResponse sresp = (SystemResponse)com.sendCall(FunctionCallFactory.CALLS.GETSYSTEMINFORMATION, Response.RESPONSES.SOLARYSYSTEMRESPONSE);
            if (sresp == null)
            {
                return null;
            }

            return new SolarSystem(sresp.Name, sresp.Info);
           
        }

    }
}
