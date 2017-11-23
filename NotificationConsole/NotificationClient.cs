class NotificationClient
    {
        static void Main(string[] args)
        {
            PushNotification notification = new PushNotification(false);
            notification.SendText("Race control (@add): Some Text");
        }
    }
