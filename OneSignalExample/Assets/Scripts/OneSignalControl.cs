using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SampleProject
{
    public class OneSignalControl : MonoBehaviour
    {
        [SerializeField] Text m_WhenOpened;
        static Text s_WhenOpened;

        void Start()
        {
            s_WhenOpened = m_WhenOpened;
            LogMessage("starting onesignal...");

            // Uncomment this method to enable OneSignal Debugging log output 
            //OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.VERBOSE, OneSignal.LOG_LEVEL.NONE);

            // Replace 'YOUR_ONESIGNAL_APP_ID' with your OneSignal App ID.
            try {
                Com.OneSignal.OneSignalPush.StartInit(Com.OneSignal.OneSignalSettings.Instance.ApplicationId)
                   .HandleNotificationOpened(OneSignalHandleNotificationOpened)
                   .Settings(new Dictionary<string, bool>() {
                                                                { Com.OneSignal.OneSignalPush.kOSSettingsAutoPrompt, false },
                                                                { Com.OneSignal.OneSignalPush.kOSSettingsInAppLaunchURL, false }
                                                            })
                   .EndInit();

            }
            catch (Exception e) {
                LogMessage(e.Message);
                LogMessage(e.Source);
                LogMessage(e.StackTrace);
            }

            LogMessage("init done");

            Com.OneSignal.OneSignalPush.inFocusDisplayType = Com.OneSignal.OneSignalPush.OSInFocusDisplayOption.Notification;
            LogMessage("focus type set");

            // iOS - Shows the iOS native notification permission prompt.
            //   - Instead we recomemnd using an In-App Message to prompt for notification 
            //     permission to explain how notifications are helpful to your users.
            Com.OneSignal.OneSignalPush.PromptForPushNotificationsWithUserResponse(OneSignalPromptForPushNotificationsResponse);
            LogMessage("onesignal started");

            Com.OneSignal.OneSignalPush.SetSubscription(true);

            var state = Com.OneSignal.OneSignalPush.GetPermissionSubscriptionState();
            LogMessage($"Permission status: {state.permissionStatus.status}");
            LogMessage($"Subscription status >> subscribed={state.subscriptionStatus.subscribed} token={state.subscriptionStatus.pushToken} uID={state.subscriptionStatus.userId} settings={state.subscriptionStatus.userSubscriptionSetting}");
            
        }

        // Gets called when the player opens a OneSignal notification.
        private static void OneSignalHandleNotificationOpened(OSNotificationOpenedResult result)
        {
            LogMessage("notification opened");
        }

        // iOS - Fires when the user anwser the notification permission prompt.
        private void OneSignalPromptForPushNotificationsResponse(bool accepted)
        {
            // Optional callback if you need to know when the user accepts or declines notification permissions.
        }

        public static void LogMessage(string message)
        {
            s_WhenOpened.text += Environment.NewLine + message;
            Debug.Log(message);
        }
    }
}
