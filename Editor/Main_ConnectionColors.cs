using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Design.DialogueEditor {
	public partial class Main {


		bool[,] choiceIN = new bool[50,5];
		bool[,] nodeIN = new bool[50,5];
		static Color[] nodeOutputColors = new Color[] {
			new Color(0.15f,0.15f,1),
			new Color(0.3f,0.3f,1),
			new Color(0.45f,0.45f,1),
			new Color(0.6f,0.6f,1),
			new Color(0.7f,0.7f,1),
		};

		static Color[] nodeInputColors = new Color[] {
			new Color(1,0.6f,0.1f),
			new Color(1,0.7f,0.2f),
			new Color(1,0.8f,0.3f),
			new Color(1,0.9f,0.4f),
			new Color(1,1,0.5f),
		};

		static Color[] choiceInputColors = new Color[] {
			new Color(0.6f,0.5f,1),
			new Color(0.6f,0.6f,1),
			new Color(0.6f,0.7f,1),
			new Color(0.6f,0.8f,1),
			new Color(0.6f,0.9f,1),
		};

		void CheckTickedInputs() {

			//Register Choice inputs
			choiceIN = new bool[50,5];

			for (int i = 0; i < cur.Nodes.Length; i++) {
				var n = cur.Nodes[i];
				if (n == null || n.ID == -1) { continue; }
				for (int j = 0; j < n.Connections.Length; j++) {
					var c = n.Connections[j];
					int id = c.targetChoiceID;
					int con = c.targetConnectionInput;

					if (id == -1 || con == -1) { continue; }
					choiceIN[id,con] = true;
				}
			}

			//Register Node inputs
			nodeIN = new bool[50,5];

			for (int i = 0; i < cur.Choices.Length; i++) {
				var c = cur.Choices[i];
				if (c == null || c.ID == -1) { continue; }
				int id1 = c.Connections.mainNodeID;
				int id2 = c.Connections.secondaryNodeID;
				int con1 = c.Connections.mainConnectionID;
				int con2 = c.Connections.secondaryConnectionID;

				if (id1 != -1 && con1 != -1) { nodeIN[id1,con1] = true; }
				if (id2 != -1 && con2 != -1) { nodeIN[id2,con2] = true; }
			}
		}




	}
}