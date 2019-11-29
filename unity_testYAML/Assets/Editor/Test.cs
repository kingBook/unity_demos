using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using YamlDotNet.RepresentationModel;

public class Test{
	[MenuItem("Tools/test")]
	public static void test(){
		string filePath=System.Environment.CurrentDirectory.Replace('\\','/');
		filePath+="/ProjectSettings/Physics2DSettings.yaml";

		StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
		//记录头头3行
		string[] headLines={streamReader.ReadLine(),streamReader.ReadLine(),streamReader.ReadLine()};
		
		YamlStream yaml = new YamlStream();
		yaml.Load(streamReader);
		streamReader.Dispose();
		streamReader.Close();
		

		YamlMappingNode rootNode=(YamlMappingNode)yaml.Documents[0].RootNode;
		YamlMappingNode firstNode=(YamlMappingNode)rootNode["Physics2DSettings"];
		YamlNode gravity=firstNode["m_Gravity"];
		Debug.Log(gravity);//{ { x, 0 }, { y, -9.81 } }
		Debug.LogFormat("x:{0},y:{1}",gravity["x"],gravity["y"]);//x:0,y:-9.81

		Debug.Log(firstNode["m_JobOptions"]);
		
		//测试添加节点
		//firstNode.Add("speed","999");
		//Debug.Log(firstNode["speed"]);
		
		//保存
		StreamWriter streamWriter=new StreamWriter(filePath);
		for (int i=0;i<headLines.Length;i++){
			streamWriter.WriteLine(headLines[i]);
		}
		yaml.Save(streamWriter,false);
		//
		streamWriter.Dispose();
		streamWriter.Close();
	}
}