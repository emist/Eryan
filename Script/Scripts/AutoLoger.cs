using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using Eryan.Script;
using Eryan.Wrappers;
using Eryan.Security;

namespace Eryan.Script.Scripts
{
    /// <summary>
    /// Logs into the Eve account
    /// </summary>
    public class AutoLoger : Scriptable
    {

        private Boolean hasCredentials = false;
        private string username;
        private string password;

        public override Boolean onStart()
        {
            name = "Autologer";
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        /// <summary>
        /// Load the user credentials
        /// </summary>
        /// <param name="acc">The account manager reference that has the credentials</param>
        public void userCredentials(AccountManager acc)
        {
            if (acc == null)
            {
                return;
            }
            
            if (acc.Username.Equals("") || acc.Password.Equals(""))
                return;

            username = acc.Username;
            password = acc.Password;
            hasCredentials = true;
        }

        public override int run()
        {
            if (ESession.isLoading())
                return 6000;
            
            if (ESession.atLogin())
            {
                ESession.login(username, password);
                return 3000;
            }

            if (ESession.atCharSel())
                ESession.click(ESession.getEnterButton());
           

            return 200;
        }

        /// <summary>
        /// Returns true if credentials have been loaded, false otherwise
        /// </summary>
        public bool HasCredentials
        {
            get
            {
                return hasCredentials;
            }
        }
    }
}
