using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Diagnostics;

namespace Notification
{
public class PushNotification
    {
        SubscriptionClient m_SubscriptionClient;
        TopicClient m_TopicClient;

        public PushNotification(bool subscribe)
        {
            m_SubscriptionClient = null;

            if (subscribe)
            {
                string subscriptionName = Guid.NewGuid().ToString();

                // Create a new subscription on the topic
                NamespaceManager manager = NamespaceManager.CreateFromConnectionString
                    (WorkshopSettings.PushNotificationConnectionString);

                try
                {
                    var subDescription = new SubscriptionDescription
                    (WorkshopSettings.PushNotificationTopicName, subscriptionName)
                    {
                        AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                    };

                    manager.CreateSubscription(subDescription);

                    m_SubscriptionClient = SubscriptionClient.CreateFromConnectionString
                        (WorkshopSettings.PushNotificationConnectionString,
                        WorkshopSettings.PushNotificationTopicName, subscriptionName);

                    OnMessageOptions options = new OnMessageOptions()
                    {
                        MaxConcurrentCalls = 1,
                        AutoComplete = false
                    };

                    m_SubscriptionClient.OnMessage(message => ProcessMessage(message));
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void SendText(string text)
        {
            if (m_TopicClient == null)
            {
                m_TopicClient = TopicClient.CreateFromConnectionString
                    (WorkshopSettings.PushNotificationConnectionString,
                    WorkshopSettings.PushNotificationTopicName);
            }

            BrokeredMessage message = new BrokeredMessage(text);
            message.ContentType = "text";

            try
            {
                m_TopicClient.Send(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception sending push: " + ex.Message);
            }
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            message.Complete();

            if (message.ContentType.ToLower().Equals("text"))
            {
                if (RaiseTextNotificationEvent() != null)
                {
                    string text = message.GetBody<string>();
                    TextNotificationEventArgs args = new TextNotificationEventArgs(text);
                    RaiseTextNotificationEvent(this, args);
                }
            }

            if (message.ContentType.ToLower().Equals("laptimes"))
            {

            }
        }

        private object RaiseTextNotificationEvent()
        {
            throw new NotImplementedException();
        }

        private void RaiseTextNotificationEvent(PushNotification pushNotification, TextNotificationEventArgs args)
        {
            throw new NotImplementedException();
        }


    }
}
