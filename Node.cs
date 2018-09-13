using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design.DialogueEditor {
	[System.Serializable]
	public class Node {
		[HideInInspector] public int ID;

		[Multiline]
		public string Text;
		[HideInInspector] public Rect PositionInGrid;
		[HideInInspector] public NodeConnection[] Connections = new NodeConnection[5];

		[HorizontalGroup("F",Width = 350), LabelWidth(200), HideReferenceObjectPicker, PropertySpace(), ShowInInspector]
		public List<System.Action> actionsOnAppear = new List<System.Action>();

		public void RefreshConnections(int i) { Connections[i].targetChoiceID = Connections[i].targetConnectionInput = -1; }
		public void RefreshConnections() { for (int i = 0; i < Connections.Length; i++) { RefreshConnections(i); } }

		public Node() {
			ID = -1;
			Connections = new NodeConnection[5];
			for (int i = 0; i < Connections.Length; i++) { Connections[i] = new NodeConnection(); }
		}

		public Node(int id,Rect pos) {
			ID = id; PositionInGrid = pos;
			for (int i = 0; i < Connections.Length; i++) { Connections[i] = new NodeConnection(); }
		}

	}
}