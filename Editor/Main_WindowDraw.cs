using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Design.DialogueEditor {
	public partial class Main {

		void DrawEverything() {
			if (cur == null) { return; }
			CheckTickedInputs(); //Check for connections to determine the color 
			DrawAllWindows(); //Draw Windows
			DrawConnections(); //Draw Connections
		}

		static float aR = 0.5f; //aspect ratio 720x860;
		float windowX => 200;// * currentZoom; //ZOOM NOT IMPLEMENTED
		float windowY => windowX * aR;
		Vector3 winSize => new Vector2(windowX,windowY);
		public float currentZoom = 1;


		void DrawAllWindows() {
			ScrollPosition = GUI.BeginScrollView(viewPos,ScrollPosition,workRect);
			BeginWindows();
			float s = 0.12f * windowX; // Χ Button size
			foreach (var n in cur.Nodes) {
				if (n == null || n.ID < 0) { continue; }
				GUI.backgroundColor = new Color(0.3f,1,0.6f);
				n.PositionInGrid.size = winSize;
				n.PositionInGrid = GUI.Window(n.ID,n.PositionInGrid,NodeWinFunc,"Text Node (ID = " + n.ID + ")");

				float x = n.PositionInGrid.x;				float y = n.PositionInGrid.y;
				Rect cons = new Rect(x - 25,y,25,90);
				//cons.size *= currentZoom; //ZOOM NOT IMPLEMENTED
				//Inputs
				if (n.ID != 0) { GUI.Window(n.ID + 1000,cons,NodeConInput,"->"); }
				cons = new Rect(x + windowX,y,25,90);
				//cons.size *= currentZoom; //ZOOM NOT IMPLEMENTED
				//Outputs
				GUI.Window(n.ID + 2000,cons,NodeConOutput,"->");
			}

			for (int i = 0; i < cur.Choices.Length; i++) {
				var c = cur.Choices[i];
				if (c == null || c.ID < 0) { continue; }
				GUI.backgroundColor = new Color(0.3f,0.8f,1);
				c.PositionInGrid.size = winSize;
				c.PositionInGrid = GUI.Window(c.ID+10000,c.PositionInGrid,ChoiceWinFunct,"Choice Node (ID = " + c.ID + ")");

				float x = c.PositionInGrid.x; float y = c.PositionInGrid.y;
				Rect cons = new Rect(x - 25,y,25,90);
				//cons.size *= currentZoom; //ZOOM NOT IMPLEMENTED
				//Inputs
				GUI.Window(c.ID + 11000,cons,ChoiceConInput,"->"); 
				cons = new Rect(x + windowX,y,25,90);
				//cons.size *= currentZoom; //ZOOM NOT IMPLEMENTED
				//Outputs
				GUI.Window(c.ID + 12000,cons,ChoiceConOutput,"->");

			}
			EndWindows();
			GUI.backgroundColor = Color.white;
			GUI.EndScrollView(false);
		}


	}//Class End

}