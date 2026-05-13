using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exercice3.Messages
{
    public class NotificationMessage : ValueChangedMessage<string>
    {
        public NotificationMessage(string message) : base(message)
        {
        }
    }
}
