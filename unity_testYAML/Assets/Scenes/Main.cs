using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static YamlDotNet.Samples.DeserializeObjectGraph;

public class Main:MonoBehaviour{
	private void Start(){
		string filePath=System.Environment.CurrentDirectory.Replace('\\','/');
		filePath+="/ProjectSettings/Physics2DSettings.yaml";
		
		var input = new StreamReader(filePath, Encoding.UTF8);
		var yaml = new YamlStream();
		yaml.Load(input);
		for(int i=0;i<yaml.Documents.Count;i++){
			YamlDocument document=yaml.Documents[i];
			
			foreach (YamlNode yamlNode in document.AllNodes){
				Debug.Log(yamlNode);
			}
		}
		
		
		/*var mapping=(YamlMappingNode)yaml.Documents[0].RootNode;
		foreach (var entry in mapping.Children){
			Debug.Log(((YamlScalarNode)entry.Key).Value);
		}*/
		
	}
	
	
}
