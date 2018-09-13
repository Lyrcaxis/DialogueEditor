using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design.DialogueEditor {

	[System.Serializable]
	public class NodeConnection {
		public int targetChoiceID = -1;
		public int targetConnectionInput = -1;
	}

	[System.Serializable]
	public class ChoiceConnection {
		[ShowInInspector] public List<System.Func<bool>> choice1Conditions = new List<System.Func<bool>>();
		public bool ShouldGoToOption1() {
			foreach (var con in choice1Conditions) { if (!con()) { return false; } }
			return true;
		}

		[HorizontalGroup("a")] public int mainNodeID = -1;
		[HorizontalGroup("a")] public int mainConnectionID = -1;
		[HorizontalGroup("b")] public int secondaryConnectionID = -1;
		[HorizontalGroup("b")] public int secondaryNodeID = -1;

		public void RefreshConnections(int i) {
			if (i == 1) { mainNodeID = mainConnectionID = -1; }
			if (i == 2) { secondaryNodeID = secondaryConnectionID = -1; }
		}

		public ChoiceConnection() { mainNodeID = mainConnectionID = secondaryConnectionID = secondaryNodeID = -1; }

	}

}