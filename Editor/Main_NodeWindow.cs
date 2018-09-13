using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Design.DialogueEditor {
	public partial class Main {

		Node NodeByID(int ID) => ID >= 0 ? cur.Nodes[ID] : null;

		static int _frameDelay;
		void NodeWinFunc(int ID) {
			Node n = NodeByID(ID);
			if (n == null) { return; }
			if (Event.current.type == EventType.Repaint && _frameDelay < 50) { _frameDelay++; return; }
			float s = 0.08f* windowX; //Button size

			//TEXT FIELD
			EditorStyles.textField.wordWrap = true;
			n.Text = EditorGUILayout.TextArea(n.Text,GUILayout.ExpandHeight(true));
			EditorStyles.textField.wordWrap = false;


			GUI.backgroundColor = Color.red;
			if (GUI.Button(new Rect(windowX + 1 - s,0,s,s),"x")) {
				DisconnectAll(n);
				cur.Nodes[ID] = null;
			}
			GUI.DragWindow();
		}

	}//Class End

}