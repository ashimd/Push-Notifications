class NotificationClient
    {
        static void Main(string[] args)
        {
            PushNotification notification = new PushNotification(false);
            notification.SendText("Race control (@aadd): Some Text");
        }
    }
