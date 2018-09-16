using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Design.DialogueEditor {
	public partial class Main:OdinMenuEditorWindow {


		//NOT USED
		static DialogueEditorSettings settings => DialogueEditorSettings.instance;

		public NPCID cur => MenuTree?.Selection?.Count > 0 ? (NPCID)(MenuTree?.Selection[0]?.Value) : null;


		public static Main instance => GetWindow<Main>();

		[MenuItem("Window/Open Dialogue Editor")]
		static void Init() => GetWindow<Main>(typeof(Main));
		protected override void OnGUI() {
			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;

			if (cur != null) {
				DrawGrid(); //Draw the Grid 
				MouseFunction(); //Make Mouse Functional
				DrawEverything(); //Draw Windows and Connections
				EditorUtility.SetDirty(cur); //Must set dirty or it wont get saved to disk
			}
			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;
			EditorGUI.DrawPreviewTexture(new Rect(0,0,450,60),PanelTexture,null,ScaleMode.StretchToFill);
			base.OnGUI(); //Draw menu on top of the Grid
		}

		protected override OdinMenuTree BuildMenuTree() {
			var tree = new OdinMenuTree(false);
			tree.DefaultMenuStyle.IconSize = 28.00f;
			tree.Config.DrawSearchToolbar = true;

			tree.Add("NPCS",null);
			tree.Add("NPCS/None",null);
			//If you decide to use the DialogueEditorSettings, you can use it as such, after you Create one
			//tree.AddAllAssetsAtPath("NPCS",DialogueEditorSettings.instance.NPCPath,typeof(NPCDialogue),true);
			try { //Try to avoid null ref exception
				tree.AddAllAssetsAtPath("NPCS","",typeof(NPCDialogue),true);
				tree.EnumerateTree().AddIcons<NPCDialogue>(x => x.icon);
			} catch { } 
			return tree;
		}

		protected override void OnBeginDrawEditors() {
			var selected = this.MenuTree.Selection.FirstOrDefault();
			var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

			// Draws a toolbar with the name of the currently selected menu item.
			SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
			{
				if (selected != null && cur != null) {
					GUILayout.MaxWidth(30);
					EditorGUILayout.LabelField("Name: ",GUILayout.MaxWidth(40));
					selected.Name = EditorGUILayout.TextField(selected.Name,GUILayout.MaxWidth(100));
					EditorGUILayout.LabelField("ID: ",GUILayout.MaxWidth(30));
					cur.ID = EditorGUILayout.IntField(cur.ID,GUILayout.MaxWidth(30));
				}
			}
			SirenixEditorGUI.EndHorizontalToolbar();
		}

		static void OpenPanel(Node n) => NodeInspector.Inspect(n);
		static void OpenPanel(DialogueChoice c) => NodeInspector.Inspect(c);


		public static Node focusedNode;

		void WindowFunction(int id) {
			Node n = focusedNode;
			if (n == null) { return; }
		}

	}
}
