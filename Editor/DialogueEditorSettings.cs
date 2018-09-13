using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Design.DialogueEditor {
	//NOT USED ANYWHERE ATM..
	public class DialogueEditorSettings:SerializedScriptableObject {
		public Vector2 GridSize = new Vector2(3000,3000);
		public string NPCPath;
		public Texture _NodeTexture;
		public static DialogueEditorSettings _instance;

		//easily found if put inside a Resources folder
		public static DialogueEditorSettings instance => _instance ?? (_instance = Resources.Load<DialogueEditorSettings>("DialogueEditor_Settings"));
		public static Texture NodeTexture => instance._NodeTexture;
	}
}