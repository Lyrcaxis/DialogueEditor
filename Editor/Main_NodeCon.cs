using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Design.DialogueEditor {
	public partial class Main {

		/// <summary>
		/// The button that was clicked(ID, ConnectionID, isInput, isNode)
		/// </summary>
		(int, int, bool, bool)? activeCon;

		void NodeConInput(int ID) {
			if (Event.current.type == EventType.Repaint && _frameDelay < 150) { _frameDelay++; return; }
			int nodeID = ID - 1000;
			var n = NodeByID(nodeID);
			var c = n.Connections;

			for (int i = 0; i < c.Length; i++) {
				var x = GUILayoutUtility.GetRect(3,3);
				Rect nodeRect = GUILayoutUtility.GetRect(5,10);
				GUI.backgroundColor = nodeIN[nodeID,i]?nodeInputColors[i]: Color.white;
				if (GUI.Button(nodeRect,"")) { NodeButton_Input(nodeID,i); }
			}

		}

		void NodeConOutput(int ID) {
			if (Event.current.type == EventType.Repaint && _frameDelay < 150) { _frameDelay++; return; }
			int nodeID = ID - 2000;
			var n = NodeByID(nodeID);
			var c = n.Connections;

			for (int i = 0; i < c.Length; i++) {
				GUILayoutUtility.GetRect(3,3);
				Rect nodeRect = GUILayoutUtility.GetRect(5,10);
				bool conExists = (c[i].targetChoiceID != -1 && c[i].targetConnectionInput != -1);
				GUI.backgroundColor = conExists ? nodeOutputColors[4-i] : Color.white;
				if (GUI.Button(nodeRect,"")) { NodeButton_Output(nodeID,i); }
			}
		}


		void NodeButton_Input(int nodeID,int con) {
			var n = NodeByID(nodeID);
			if (activeCon == null || activeCon.Value.Item4) { activeCon = (nodeID, con, true, true); return; }
			DialogueChoice D = ChoiceByID(activeCon.Value.Item1);
			int DconID = activeCon.Value.Item2;
			bool Dinput = activeCon.Value.Item3;
			if (Dinput) { return; }
			if (DconID == 0) { D.Connections.mainNodeID = n.ID; D.Connections.mainConnectionID = con; }
			else if (DconID == 1) { D.Connections.secondaryNodeID = n.ID; D.Connections.secondaryConnectionID = con; }
			activeCon = null;
		}

		void NodeButton_Output(int nodeID,int con) {
			var n = NodeByID(nodeID);
			if (activeCon == null || activeCon.Value.Item4) { activeCon = (nodeID, con, false, true); return; }
			DialogueChoice D = ChoiceByID(activeCon.Value.Item1);
			int DconID = activeCon.Value.Item2;
			bool Dinput = activeCon.Value.Item3;
			if (!Dinput) { return; }
			n.Connections[con].targetChoiceID = D.ID;
			n.Connections[con].targetConnectionInput = DconID;
			activeCon = null;
		}

	}
}