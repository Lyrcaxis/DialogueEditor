using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Design.DialogueEditor {
	public class NodeInspector:OdinEditorWindow {

		static NodeInspector _instance;
		static NPCDialogue cur => Main.instance.cur;

		public object tar;
		public static NodeInspector instance => _instance ? _instance : (_instance = GetWindow<NodeInspector>());

		void Update() {
			if (this.GetTarget() != tar) { Inspect(tar); }
		}

		public static void Inspect(object obj) {
			InspectObject(instance,obj);
			//instance.position = new Rect(Event.current.mousePosition,new Vector2(400,400));
			instance.tar = obj;
			string title = !string.IsNullOrEmpty(cur?.Name) ? cur?.Name : "New NPC";
			instance.titleContent = new GUIContent(title);
			instance.Focus();
		}
	}
}