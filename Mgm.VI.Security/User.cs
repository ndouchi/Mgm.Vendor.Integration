using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Security
{
    public class User : IUser
    {
        private string _currentUser;

        public string CurrentUser
        {
            get
            {
                if (string.IsNullOrEmpty(_currentUser))
                {
                    SetUser();
                }
                return _currentUser;
            }
        }
        public User()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetUser();
        }
        private void SetUser()
        {
            var who = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (who == null)
                _currentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            else
                _currentUser = who.Name;
        }
    }
}
