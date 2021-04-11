using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildPanelButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int onPointerReaction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Connector.panelInvoker.OpenBuildPanelInfo(onPointerReaction);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Connector.panelInvoker.CloseBuildPanelInfo();
    }
}
