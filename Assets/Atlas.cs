using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Atlas : MonoBehaviour {
	
	public Texture2D texture;
	public Dictionary<string, Rect> frames;
	
	// Use this for initialization
	public void Init () {
		UnityEngine.Object[] objs = Resources.LoadAll("spritesheet");
		XmlDocument xml = new XmlDocument();
		
		for (int i = 0; i < objs.Length; ++i) {
			if (objs[i] is Texture2D) {
				texture = objs[i] as Texture2D;
			
			}
		}
		
		frames = new Dictionary<string, Rect>();

		for (int i = 0; i < objs.Length; ++i) {
			if (objs[i] is TextAsset) {
				TextAsset text = objs[i] as TextAsset;
			
				xml.LoadXml(text.text);
				XmlNodeList list = xml.GetElementsByTagName("SubTexture");
				
				foreach (XmlNode node in list) {
					string s = node.Attributes["name"].InnerText;
					float x = float.Parse(node.Attributes["x"].InnerText);
					float y = float.Parse(node.Attributes["y"].InnerText);
					float width = float.Parse(node.Attributes["width"].InnerText);
					float height = float.Parse(node.Attributes["height"].InnerText);
				
					Rect rect = new Rect(x, y, width, height);
					frames[s] = rect;
							
				}
			}
		}
	}
	
	public List<Rect> getFrames(string prefix) {
		List<Rect> result = new List<Rect>();
		foreach (string key in frames.Keys) {
			if (key.StartsWith(prefix)) {
				result.Add(frames[key]);
			}
		}
		
		return result;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
