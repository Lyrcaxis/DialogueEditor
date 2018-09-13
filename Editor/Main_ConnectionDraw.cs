using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Design.DialogueEditor {
	public partial class Main {

		Vector2 rightButtonPos => new Vector2(windowX + 12,28);
		Vector2 leftButtonPos => new Vector2(-12,28);


		void DrawConnections() {
			Handles.BeginGUI();
			ScrollPosition = GUI.BeginScrollView(viewPos,ScrollPosition,workRect);
			Handles.color = new Color(1,0.7f,0.2f);
			for (int i = 0; i < 50; i++) {
				Node n = cur.Nodes[i];
				DialogueChoice c = cur.Choices[i];
				bool validNode = !(n == null || n.ID < 0);
				bool validChoice = !(c == null || c.ID < 0);
				if (validNode) {
					for (int j = 0; j < 5; j++) {
						var con = n.Connections[j];
						if (con.targetChoiceID < 0 || con.targetConnectionInput < 0) { continue; }
						var fromPos = rightButtonPos + (13 * j * Vector2.up);
						fromPos += n.PositionInGrid.position;
						var tarPos = leftButtonPos + (13 * con.targetConnectionInput * Vector2.up);
						tarPos += ChoiceByID(con.targetChoiceID).PositionInGrid.position;
						var p1 = fromPos - (fromPos - tarPos) / 2 + Vector2.up * (fromPos - tarPos).y / 2;
						var p2 = fromPos - (fromPos - tarPos) / 2 - Vector2.up * (fromPos - tarPos).y / 2;
						Handles.DrawBezier(fromPos,tarPos,p1,p2,nodeOutputColors[j],null,5);
					}
				}
				if (validChoice) {
					var con = c.Connections;
					if (!(con.mainNodeID < 0 || con.mainConnectionID < 0)) {
						var fromPos = rightButtonPos + Vector2.up * 10f;
						fromPos += c.PositionInGrid.position;
						var tarPos = leftButtonPos + (13 * con.mainConnectionID * Vector2.up);
						tarPos += NodeByID(con.mainNodeID).PositionInGrid.position;
						var p1 = fromPos - (fromPos - tarPos) / 2 + Vector2.up * (fromPos - tarPos).y / 2;
						var p2 = fromPos - (fromPos - tarPos) / 2 - Vector2.up * (fromPos - tarPos).y / 2;
						Handles.DrawBezier(fromPos,tarPos,p1,p2,Color.green,null,5);
					}
					if (!(con.secondaryNodeID < 0 || con.secondaryConnectionID < 0)) {
						var fromPos = rightButtonPos + Vector2.up * 40;
						fromPos += c.PositionInGrid.position;
						var tarPos = leftButtonPos + (13 * con.secondaryConnectionID * Vector2.up);
						tarPos += NodeByID(con.secondaryNodeID).PositionInGrid.position;
						var p1 = fromPos - (fromPos - tarPos) / 2 + Vector2.up * (fromPos - tarPos).y / 2;
						var p2 = fromPos - (fromPos - tarPos) / 2 - Vector2.up * (fromPos - tarPos).y / 2;
						Handles.DrawBezier(fromPos,tarPos,p1,p2,Color.red,null,5);
					}
				}
			}

			//Draw current line to current position
			if (activeCon != null) {
				var isNode = activeCon.Value.Item4;
				var n = NodeByID(activeCon.Value.Item1);
				var i = activeCon.Value.Item2;
				bool input = activeCon.Value.Item3;

				Vector2 fromPos = new Vector2();
				if (isNode) {
					fromPos = NodeByID(activeCon.Value.Item1).PositionInGrid.position;
					fromPos += (Vector2.up * 13 * i) + (input ? leftButtonPos : rightButtonPos);
				}
				else {
					fromPos = ChoiceByID(activeCon.Value.Item1).PositionInGrid.position;
					if (input) { fromPos += (Vector2.up * 13 * i) + leftButtonPos; }
					else { fromPos += rightButtonPos + Vector2.up * (i == 0 ? 10 : 40); }
				}
				var tarPos = Event.current.mousePosition;// + ScrollPosition;
				var p1 = fromPos - (fromPos - tarPos) / 2 + Vector2.up * (fromPos - tarPos).y / 2;
				var p2 = tarPos + (fromPos - tarPos) / 2 - Vector2.up * (fromPos - tarPos).y / 2;
				Handles.DrawBezier(fromPos,tarPos,p1,p2,Color.white,null,5);
			}
			GUI.EndScrollView(false);
			Handles.EndGUI();
		}



	}
}