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

        public void userCredentials(AccountManager acc)
        {
            if (acc == null)
            {
                Console.WriteLine("Account manager is null");
                return;
            }
            
            Console.WriteLine("USERNAME " + acc.Username);

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
                Console.WriteLine("USERNAME XXX " + username);
                ESession.login(username, password);
                return 3000;
            }

            if (ESession.atCharSel())
                ESession.click(ESession.getEnterButton());
           

            return 200;
        }

        public bool HasCredentials
        {
            get
            {
                return hasCredentials;
            }
        }
    }
}
