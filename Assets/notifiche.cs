using System;
using System.Collections;
using UnityEngine;
#if UNITY_IPHONE
using Unity.Notifications.iOS;
#endif
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class notifiche : MonoBehaviour
{
    void Start()
    {
#if UNITY_IPHONE
        StartCoroutine(RequestAuthorization());
#endif
#if UNITY_ANDROID
        InviaNotificaAndroid();
#endif
    }
    #if UNITY_IPHONE

    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
        }

        InviaNotificaIOS();
    }

    void InviaNotificaIOS()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(36, 0, 0),
            Repeats = true
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "_notification_01",
            Title = "Come join us!",
            Body = "join a large group playing Av: Space Shooter!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }
    #endif
    #if UNITY_ANDROID
    void InviaNotificaAndroid()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "notifica",
            Importance = Importance.Default,
            Description = "join a large group playing Av: Space Shooter!",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = "Come join us!";
        notification.Text = "join a large group playing Av: Space Shooter!";
        notification.FireTime = System.DateTime.Now.AddDays(1);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
#endif
}