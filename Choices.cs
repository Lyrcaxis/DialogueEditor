using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design.DialogueEditor {

	[System.Serializable]
	public class DialogueChoice {
		[HideInInspector] public int ID;
		[HideInInspector] public Rect PositionInGrid;

		public string ChoiceText;
		public ChoiceConnection Connections;
		[ShowInInspector] public List<System.Func<bool>> conditionsToAppear = new List<System.Func<bool>>();

		public bool CanAppear() {
			foreach (var con in conditionsToAppear) { if (!con()) { return false; } }
			return true;
		}

		public void RefreshConnections() {
			Connections.RefreshConnections(1);
			Connections.RefreshConnections(2);
		}

		public DialogueChoice() { ID = -1; Connections = new ChoiceConnection(); }
		public DialogueChoice(int id,Rect pos) { ID = id; PositionInGrid = pos; Connections = new ChoiceConnection(); }

	}

}