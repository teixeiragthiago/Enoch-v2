using System;
using System.Collections.Generic;
using System.Text;

namespace Enoch.CrossCutting.Notification
{
    public interface INotification
    {
        void Add(string message);
        string GetNotifications();
        T AddWithReturn<T>(string message);
        bool Any();
        bool Notify(bool condition, string value);

    }
}
