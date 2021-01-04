using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public interface INotificationService
    {
        void SendConfirmationCode(string cellPhone, int code);
    }
}
