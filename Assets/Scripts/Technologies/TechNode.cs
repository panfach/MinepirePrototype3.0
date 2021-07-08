using System;
using UnityEngine;
using System.Collections;
using XNode;
using UnityEngine.UI;

namespace TechnologySystem
{
    public class TechNode : Node
    {
        [Input] public Connection enter;

        public Image graphButtonImage;
        public TechData data;

        [Output] public Connection exit;


        public bool CheckInputResearchings
        {
            get
            {
                foreach (NodePort port in GetInputPort("enter").GetConnections())
                {
                    if (Connector.techManager.GetTechStatus(((TechNode)port.node).data.Index) != TechStatus.RESEARCHED) return false;
                }
                return true;
            }       
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }

    public enum TechIndex
    {
        FOODPRESERVATION,
        FISHING,
        SKINCLOTHING,
        STONETOOLS,
        ADVANCEDFOODGATHERING,
        STARTTECH
    }
}
