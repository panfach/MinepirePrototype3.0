using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    const float showDelay = 3f;

    public GameObject notif;
    public TextMeshProUGUI notifText;
    //public Animator anim;

    public static void Invoke(NotifType type)
    {
        Connector.notification.InvokeNotification(type);
    }

    public void InvokeNotification(NotifType type)
    {
        notif.SetActive(false);
        notif.SetActive(true);

        Connector.effectSoundManager.PlayCancelSound();

        switch(type)
        {
            case NotifType.NONE:
                notifText.text = "Уведомление";
                break;
            case NotifType.RESBUILD:
                notifText.text = "Недостаточно ресурсов на складах для строительства";
                break;
            case NotifType.PLACEBUILD:
                notifText.text = "Неправильное место для постройки";
                break;
            case NotifType.EMPTYHOME:
                notifText.text = "Еще не все жители успели добраться домой, подождите";
                break;
            case NotifType.RESSOURCE:
                notifText.text = "Пока нельзя собрать данный ресурс";
                break;
        }
    }

}

public enum NotifType
{
    NONE,
    RESBUILD,
    PLACEBUILD,
    EMPTYHOME,
    RESSOURCE
}
