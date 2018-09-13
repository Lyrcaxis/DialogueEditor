using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Design.DialogueEditor {
	public partial class Main {

		void MouseFunction() {
			ScrollWheel();
			MiddleMouse();
			RightClick();
		}

		void ScrollWheel() {
			//ZOOM NOT IMPLEMENTED
			if (Event.current.type != EventType.ScrollWheel) { return; }
			if (!viewPos.Contains(Event.current.mousePosition)) { return; }
			currentZoom += 0.05f * Event.current.delta.y;

		}

		void MiddleMouse() {
			if (Event.current.type != EventType.MouseDrag || Event.current.button != 2) { return; }
			if (!viewPos.Contains(Event.current.mousePosition)) { return; }
			Vector2 CurrentPos = Event.current.mousePosition;
			if (Vector2.Distance(CurrentPos,PreviousPosition) > 50) { PreviousPosition = CurrentPos; return; }

			Vector2 mouseOffset = PreviousPosition - CurrentPos;
			ScrollPosition += mouseOffset;
			PreviousPosition = CurrentPos;
			Event.current.Use(); //this event should not be used to drag windows
		}

		Vector2 clickPos;
		void RightClick() {
			if (Event.current.type !=EventType.MouseDown || Event.current.button != 1) { return; }
			if (!viewPos.Contains(Event.current.mousePosition)) { return; }
			if (activeCon != null) { CancelAction(); return; }
			clickPos = ScrollPosition + Event.current.mousePosition;
			Event.current.Use();
			for (int i = 0; i < 50; i++) {
				bool validNode = cur.Nodes[i].ID != -1 && cur.Nodes[i].PositionInGrid.Contains(clickPos);
				bool validChoice = cur.Choices[i].ID != -1 && cur.Choices[i].PositionInGrid.Contains(clickPos);
				if (validNode) { RightClick_Window_Node(i); return; }
				if (validChoice) { RightClick_Window_Choice(i); return; }
			}
			RightClick_EmptySpace();
		}

		void CancelAction() {
			if (!activeCon.Value.Item3) {  //IF ACTIVE IS OUTPUT
				if (activeCon.Value.Item4) { NodeByID(activeCon.Value.Item1).RefreshConnections(activeCon.Value.Item2); }
				else {
					var c = ChoiceByID(activeCon.Value.Item1);
					if (activeCon.Value.Item2 == 0) { c.Connections.mainNodeID = c.Connections.mainConnectionID = -1; }
					else if (activeCon.Value.Item2 == 1) { c.Connections.secondaryNodeID = c.Connections.secondaryConnectionID = -1; }
				}
			}
			else { //IF ACTIVE IS INPUT -- gotta scan through all outputs to see for connections
				for (int i = 0; i < 50; i++) {
					bool validNode = NodeByID(i) != null && NodeByID(i).ID != -1;
					bool validChoice = ChoiceByID(i) != null && ChoiceByID(i).ID != -1;

					if (activeCon.Value.Item4) { //IF ACTIVE IS NODE 
						if (validChoice) {	
							var con = cur.Choices[i].Connections;
							var nodeID = activeCon.Value.Item1;
							var conID = activeCon.Value.Item2;
							if (con.mainNodeID == nodeID && con.mainConnectionID == conID) { con.mainNodeID = con.mainConnectionID = -1; }
							if (con.secondaryNodeID == nodeID && con.secondaryConnectionID == conID) { con.secondaryNodeID = con.secondaryConnectionID = -1; }
						}
					}
					else if (validNode) { //IF ACTIVE IS CHOICE && NODE IS VALID
						foreach (var con in cur.Nodes[i].Connections) {
							if (con.targetChoiceID == activeCon.Value.Item1 && con.targetConnectionInput == activeCon.Value.Item2) { con.targetChoiceID = con.targetConnectionInput = -1; }
						}
					}
				}
			}
			activeCon = null;
		}

		void RightClick_EmptySpace() {
			GenericMenu Menu = new GenericMenu();
			Menu.AddItem(new GUIContent("New Node"),false,NewNode);
			Menu.AddItem(new GUIContent("New Choice"),false,NewChoice);
			Menu.AddItem(new GUIContent("Clear all"),false,ClearAll);
			Menu.ShowAsContext();
			void NewNode() {
				var nodeRect = new Rect(clickPos,Vector2.zero);
				for (int i = 0; i < cur.Nodes.Length; i++) {
					if (cur.Nodes[i] == null || cur.Nodes[i].ID == -1) {
						cur.Nodes[i] = new Node(i,nodeRect);
						return;
					}
				}
				Debug.Log("Cannot create new node");
			}

			void NewChoice() {
				var choiceRect = new Rect(clickPos,Vector2.zero);
				for (int i = 0; i < cur.Choices.Length; i++) {
					if (cur.Choices[i] == null || cur.Choices[i].ID == -1) {
						cur.Choices[i] = new DialogueChoice(i,choiceRect);
						return;
					}
				}
				Debug.Log("Cannot create new choice");

			}

			void ClearAll() {
				cur.Nodes = new Node[50];
				cur.Choices = new DialogueChoice[50];
			}
		}

		void RightClick_Window_Node(int i) {
			Node n = cur.Nodes[i];

			GenericMenu Menu = new GenericMenu();
			Menu.AddItem(new GUIContent("Inspect"),false,Inspect);
			Menu.AddItem(new GUIContent("Clear Connections"),!n.Connections.Any(x => x.targetChoiceID != -1),ClearCons);
			Menu.AddItem(new GUIContent("Clear Functions"),n.actionsOnAppear.Count == 0,ClearFuncts);
			Menu.AddItem(new GUIContent("Clear All"),false,ClearAll);
			Menu.ShowAsContext();

			void Inspect() { NodeInspector.Inspect(n); }
			void ClearCons() { DisconnectAll(n); }
			void ClearFuncts() { n.actionsOnAppear = new List<System.Action>(); }
			void ClearAll() { cur.Nodes[i] = new Node(n.ID,n.PositionInGrid); }
		}

		void RightClick_Window_Choice(int i) {
			DialogueChoice c = cur.Choices[i];	

			GenericMenu Menu = new GenericMenu();
			Menu.AddItem(new GUIContent("Inspect"),false,Inspect);
			bool hasConnections = c.Connections.mainNodeID != -1 || c.Connections.secondaryNodeID != -1;
			Menu.AddItem(new GUIContent("Clear Connections"),!hasConnections,ClearCons);
			Menu.AddItem(new GUIContent("Clear Conditions"),c.conditionsToAppear.Count == 0,ClearFuncts);
			Menu.AddItem(new GUIContent("Clear All"),false,ClearAll);
			Menu.ShowAsContext();

			void Inspect() { NodeInspector.Inspect(c); }
			void ClearCons() { DisconnectAll(c); }
			void ClearFuncts() { c.conditionsToAppear = new List<System.Func<bool>>(); }
			void ClearAll() { cur.Choices[i] = new DialogueChoice(c.ID,c.PositionInGrid); }
		}

		void LeftClick() {

			if (!viewPos.Contains(Event.current.mousePosition)) { return; }
			if (Event.current.type != EventType.MouseDown || Event.current.button != 0) { return; }
			if (activeCon != null) { activeCon = null; Debug.Log("Canceling"); return; }
		}

		void DisconnectAll(Node n) {
			foreach (var choice in cur.Choices) {
				if (choice == null || choice.ID == -1) { continue; }
				if (choice.Connections.mainNodeID == n.ID) { choice.Connections.RefreshConnections(1); }
				if (choice.Connections.secondaryNodeID == n.ID) { choice.Connections.RefreshConnections(2); }
			}
			n.RefreshConnections();
		}

		void DisconnectAll(DialogueChoice c) {
			foreach (var node in cur.Nodes) {
				if (node == null || node.ID == -1) { continue; }
				for (int i = 0; i < node.Connections.Length; i++) {
					var con = node.Connections[i];
					int tarID = con.targetChoiceID;
					int tarCon = con.targetConnectionInput;
					if (con == null || tarID == -1 || tarCon == -1) { continue; }
					if (tarID == c.ID) { node.RefreshConnections(i); }

				}
			}
			c.RefreshConnections();
		}

	}
}
