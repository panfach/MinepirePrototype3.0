using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnologySystem
{
    [CreateAssetMenu(fileName = "TechnologyData", menuName = "ScriptableObjects/Technology")]
    public class TechData : ScriptableObject
    {
        [SerializeField] string techName;
        [SerializeField] TechIndex index;
        [SerializeField] ResourceQuery requiredStatRes;
        [SerializeField] ResourceQuery requiredRes;
        [SerializeField] string description;
        [SerializeField] TechNode node;

        public string Name => techName;
        public TechIndex Index => index;
        public ResourceQuery RequiredStatRes => requiredStatRes; 
        public ResourceQuery RequiredRes => requiredRes; 
        public string Description => description; 
        public TechNode Node => node; 
    }
}
