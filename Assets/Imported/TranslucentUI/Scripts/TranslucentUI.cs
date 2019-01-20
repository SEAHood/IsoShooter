using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TranslucentUI
{
	[AddComponentMenu("TranslucentUI/TranslucentUI")]
	[RequireComponent(typeof(Canvas))]
	public class TranslucentUI : MonoBehaviour 
	{
		[Range(0f, 1f)]
		public float Transparency = 0.5f;

		[Range(0f, 1f)]
		public float GreyScale = 0.0f;

		[Range(-1f, 1f)]
		public float Brightness = 0.0f;

		private Image[] uiImages;
		// Use this for initialization
		void Start () 
		{
			uiImages = this.GetComponentsInChildren<Image> ();
			Shader translucentImage = Shader.Find ("Custom/Translucency");
			Material translucencyMat = new Material (translucentImage);
			for (int i = 0; i < uiImages.Length; i++) 
			{
				Translucency translucency = uiImages [i].gameObject.GetComponent<Translucency> ();
				if (translucency == null) 
				{
					translucency = uiImages [i].gameObject.AddComponent<Translucency>();	
					translucency.SetTranslucencyMaterial (translucencyMat);
					translucency.SetTransparency (Transparency);
					translucency.SetGreyScale (GreyScale);
					translucency.SetBrightness (Brightness);
				}
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			for (int i = 0; i < uiImages.Length; i++) 
			{
				Translucency translucency = uiImages [i].gameObject.GetComponent<Translucency>();
				translucency.SetTransparency (Transparency);
				translucency.SetGreyScale (GreyScale);
				translucency.SetBrightness (Brightness);
			}
		}
	}
}