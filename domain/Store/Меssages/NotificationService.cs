using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Store
{
    public class NotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: '{0}', code: {1:000#}.", cellPhone, code);
        }
    }
}
