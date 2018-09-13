using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Design.DialogueEditor {
	public partial class Main {

		// <summary>
		// The button that was clicked(ID, ConnectionID, isInput, isNode)
		// </summary>
		//(int, int, bool, bool)? activeCon;

		void ChoiceConInput(int ConID) {
			if (Event.current.type == EventType.Repaint && _frameDelay < 150) { _frameDelay++; return; }
			int ID = ConID - 11000;
			var c = ChoiceByID(ID);
			for (int i = 0; i < 5; i++) {
				var x = GUILayoutUtility.GetRect(3,3);
				Rect cRect = GUILayoutUtility.GetRect(5,10);
				if (choiceIN[ID,i]) { GUI.backgroundColor = choiceInputColors[i]; }
				else { GUI.backgroundColor = Color.white; }
				if (GUI.Button(cRect,"")) { ChoiceButton_Input(ID,i); }
			}
		}

		void ChoiceConOutput(int ConID) {
			if (Event.current.type == EventType.Repaint && _frameDelay < 150) { _frameDelay++; return; }
			int ID = ConID - 12000;
			var c = ChoiceByID(ID);
			GUILayoutUtility.GetRect(3,10);
			Rect cRect = GUILayoutUtility.GetRect(15,15);
			bool con1exists = c.Connections.mainNodeID != -1 && c.Connections.mainConnectionID != -1;
			GUI.backgroundColor = con1exists ? new Color(0,1,0.4f) : Color.white; 
			if (GUI.Button(cRect,"")) { ChoiceButton_Output(ID,true); }

			GUILayoutUtility.GetRect(3,15);
			cRect = GUILayoutUtility.GetRect(15,15);

			bool con2exists = c.Connections.secondaryNodeID != -1 && c.Connections.secondaryConnectionID != -1;
			GUI.backgroundColor = con2exists ? new Color(1,0.4f,0.4f) : Color.white;
			if (GUI.Button(cRect,"")) { ChoiceButton_Output(ID,false); }
		}

		void ChoiceButton_Input(int choiceID,int conID) {
			if (activeCon == null || !activeCon.Value.Item4) { activeCon = (choiceID, conID, true, false); return; }
			if (activeCon.Value.Item3) { //If it's input
				activeCon = (choiceID, conID, true, false); return;
			}
			else if (activeCon.Value.Item4) { //if output and node
				NodeByID(activeCon.Value.Item1).Connections[activeCon.Value.Item2].targetChoiceID = choiceID;
				NodeByID(activeCon.Value.Item1).Connections[activeCon.Value.Item2].targetConnectionInput = conID;
			}
			activeCon = null;
		}

		void ChoiceButton_Output(int choiceID,bool main) {
			if (activeCon == null || !activeCon.Value.Item4) { activeCon = (choiceID, main ? 0 : 1, false, false); return; }
			var c = ChoiceByID(choiceID);
			if (!activeCon.Value.Item3) { //If it's output
				activeCon = (choiceID, main ? 0 : 1, true, false); return;
			}
			else if (activeCon.Value.Item4) { //if input and node
				if (main) {
					c.Connections.mainNodeID = activeCon.Value.Item1;
					c.Connections.mainConnectionID = activeCon.Value.Item2;
				}
				else {
					c.Connections.secondaryNodeID = activeCon.Value.Item1;
					c.Connections.secondaryConnectionID = activeCon.Value.Item2;
				}
			}
			activeCon = null;
		}

	}
}