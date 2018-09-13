using UnityEditor;
using UnityEngine;


namespace Design.DialogueEditor {
	public partial class Main {

		Texture2D BackgroundTexture, PanelTexture;
		Vector2 WorkSize = new Vector2(3000,3000);

		Color BigLines = new Color(0.25f,0.25f,0.25f);
		Color SmallLines = new Color(0.30f,0.30f,0.30f);
		//Used for scrolling in the main view
		Vector2 ScrollPosition = Vector2.zero;
		Vector2 PreviousPosition = Vector2.zero;
		Rect viewPos => new Rect(0,0,position.width,position.height);
		Rect workRect => new Rect(Vector2.zero,WorkSize);

		void MakeTextures() {
			BackgroundTexture = new Texture2D(1,1,TextureFormat.RGBA32,false);
			BackgroundTexture.SetPixel(0,0,new Color(0.35f,0.35f,0.35f));
			BackgroundTexture.Apply();

			PanelTexture = new Texture2D(1,1,TextureFormat.RGBA32,false);
			PanelTexture.SetPixel(0,0,new Color(0.65f,0.65f,0.65f));
			PanelTexture.Apply();
		}

		void DrawGrid() {
			if (BackgroundTexture == null || PanelTexture == null) { MakeTextures(); }
			GUI.DrawTexture(workRect,BackgroundTexture,ScaleMode.StretchToFill);

			ScrollPosition = GUI.BeginScrollView(viewPos,ScrollPosition,workRect);
			//Draws the small, light squares all over the work area
			for (int c = 0; c < WorkSize.x; c += 10) { EditorGUI.DrawRect(new Rect(c,0,2,WorkSize.y),SmallLines); }
			for (int c = 0; c < WorkSize.y; c += 10) { EditorGUI.DrawRect(new Rect(0,c,WorkSize.x,2),SmallLines); }
			//Draws the larger, thicker lines on the work area
			for (int i = 0; i < WorkSize.x / 100; i++) { EditorGUI.DrawRect(new Rect(i * 100,0,2,WorkSize.y),BigLines); }
			for (int i = 0; i < WorkSize.y / 100; i++) { EditorGUI.DrawRect(new Rect(0,i * 100,WorkSize.x,2),BigLines); }
			GUI.EndScrollView(false);
		}

	}
}