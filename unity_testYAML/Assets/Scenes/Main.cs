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
using System;

public class Main:MonoBehaviour{
	private void Start(){

		string filePath=System.Environment.CurrentDirectory.Replace('\\','/');
		filePath+="/ProjectSettings/Physics2DSettings.yaml";

		StreamReader input = new StreamReader(filePath, Encoding.UTF8);
		YamlStream yaml = new YamlStream();
		yaml.Load(input);
		input.Dispose();
		input.Close();

		YamlMappingNode rootNode=(YamlMappingNode)yaml.Documents[0].RootNode;
		YamlMappingNode firstNode=(YamlMappingNode)rootNode["Physics2DSettings"];

		YamlNode gravity=firstNode["m_Gravity"];
		Debug.Log(gravity);//{ { x, 0 }, { y, -9.81 } }
		Debug.LogFormat("x:{0},y:{1}",gravity["x"],gravity["y"]);//x:0,y:-9.81

		Debug.Log(firstNode["m_JobOptions"]);

		firstNode.Add("speed","999");

		Debug.Log(firstNode["speed"]);
		StreamWriter streamWriter=new StreamWriter(filePath);
		yaml.Save(streamWriter);
		streamWriter.Flush();
		streamWriter.Dispose();
		streamWriter.Close();
	}
	
	
}
