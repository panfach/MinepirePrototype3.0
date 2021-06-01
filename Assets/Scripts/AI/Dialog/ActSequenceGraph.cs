﻿using System;
using UnityEngine;
using XNode;

namespace ActSequenceSystem
{
	[CreateAssetMenu]
	public class ActSequenceGraph : NodeGraph 
	{ 
		public ActSequenceIndex Index
        {
			get => ((StartAction)GetStart()).Index;
        }

		public ActionNode GetStart()
        {
			foreach (Node node in nodes)
            {
				if (node is StartAction) return node as ActionNode;
            }
			return null;
        }
	}

	[Serializable]
	public struct Connection { }
}

public enum ActSequenceIndex
{
	NONE,
	EAT,
	VILLAGER_GENERAL,
	EVENING
}