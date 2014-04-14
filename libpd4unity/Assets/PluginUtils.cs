using System;
using System.IO;
using UnityEngine;

public class PluginUtils
{
	public static void ResolvePath()
	{
		String currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		String dllPath = Application.dataPath + "/" + "Plugins";
		dllPath.Replace("/", Path.DirectorySeparatorChar.ToString()); // Correct dir format for win platforms 
		if (currentPath.Contains(dllPath) == false)
		{
			Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
		}
	}
}
