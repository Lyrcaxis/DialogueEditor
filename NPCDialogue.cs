using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;

namespace Design.DialogueEditor {
	[CreateAssetMenu(fileName = "Dialogue",menuName = "Dialogue")]
	public class NPCDialogue:SerializedScriptableObject {

		[HorizontalGroup("A",Width = 25), LabelWidth(20)] public int ID;
		[HorizontalGroup("A",Width = 150), LabelWidth(50)] public string Name;
		[HorizontalGroup("A",Width = 0), LabelWidth(0), HideLabel, PreviewField(75)] public Texture icon;
		[HideInInspector] public Node[] Nodes = new Node[50];
		[HideInInspector] public DialogueChoice[] Choices = new DialogueChoice[50];

	}
}