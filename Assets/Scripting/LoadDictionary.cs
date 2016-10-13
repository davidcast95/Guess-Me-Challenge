using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Word {
	public string phrase = "";
	public List<string> meanings = new List<string> ();
};

public class LoadDictionary : MonoBehaviour {

	public string phrase;
	public Text phraseText;
	public Text meaningsText;
	// Use this for initialization
	void Start () {
		
	}

	IEnumerator LoadPhrase() {
		string url = "https://glosbe.com/gapi/translate?from=eng&dest=eng&format=json&phrase=" + phrase + "&pretty=true";
		WWW www = new WWW(url);
		yield return www;
		JSONObject json = new JSONObject(www.text);
		Word word = new Word ();
		word.phrase = phrase;
		ReadData(json, word);

		phraseText.text = phrase;
		meaningsText.text = "";
		foreach(string meaning in word.meanings) {
			meaningsText.text += meaning + "\n";
		}
	}

	void ReadData(JSONObject obj, Word word) {
		for (int i = 0; i < obj.list.Count; i++) {
			if (obj.keys [i] == "tuc") {
				ReadMeanings (obj.list [i].list[0], word);
			}
		}
	}

	void ReadMeanings(JSONObject obj, Word word) {
		for (int i = 0; i < obj.list.Count; i++) {
			if (obj.keys [i] == "meanings") {
				for (int j = 0; j < obj.list [i].list.Count; j++) {
					word.meanings.Add(obj.list [i].list [j].GetField("text").str);
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			StartCoroutine (LoadPhrase ());
		}
	}
}
