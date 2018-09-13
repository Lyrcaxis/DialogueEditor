using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Design.DialogueEditor {
	public partial class Main {

		DialogueChoice ChoiceByID(int ID) => ID >= 0 ? cur.Choices[ID] : null;

		void ChoiceWinFunct(int ID) {
			DialogueChoice c = ChoiceByID(ID-10000);
			if (c == null) {Debug.Log("EMPTY"); return; }
			if (Event.current.type == EventType.Repaint && _frameDelay < 100) { _frameDelay++; return; }

			//TEXT FIELD
			EditorStyles.textField.wordWrap = true;
			c.ChoiceText = EditorGUILayout.TextArea(c.ChoiceText,GUILayout.ExpandHeight(true));
			EditorStyles.textField.wordWrap = false;

			#region X BUTTON
			float s = 0.08f * windowX; // XButton size
			GUI.backgroundColor = Color.red;
			if (GUI.Button(new Rect(windowX + 1 - s,0,s,s),"x")) {
				DisconnectAll(c);
				cur.Choices[ID-10000] = null;
			}
			#endregion
			GUI.DragWindow();

		}

	}//Class End

}
