using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for SettingsTool
/// </summary>
public class SettingsTool
{
    private static string _localPath;
    public static string LocalPath
    {
        get
        {
            return _localPath;
        }
        set
        {
            _localPath = value;
        }
    }

    public static string GetSetting(string key)
    {
		string target = "settings\\" + "RiskHuntingApp_Settings.ini";
        try
        {
            FileStream file = new FileStream(target, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string temp = "";
            while (!sr.EndOfStream)
            {
                temp = sr.ReadLine();
                if (temp.StartsWith(key))
                {
                    temp = temp.Substring(temp.IndexOf("=") + 1);
                    return temp;
                }
            }
            sr.Close();
            file.Close();
            return "";
        }
        catch (IOException)
        {
            return "";
        }

    }

	public static string GetApplicationPathMac()
	{
		var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
		var directoryname = Path.Combine (documents, "Projects");
		var directoryname2 = Path.Combine (directoryname, "RiskHuntingAppTest");
		var directoryname3 = Path.Combine (directoryname2, "RiskHuntingAppTest/");

		return directoryname3;
	}

    public static string GetApplicationPath()
    {
        string virtualPath = GetVirtualPath(HttpContext.Current.Request.PhysicalApplicationPath);
        string[] splitter = { "\\" };
        string[] virtualPathArray = virtualPath.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        string applicationPath = String.Empty;

        for (int i = 0; i < virtualPathArray.Length - 1; i++)
        {
            applicationPath += virtualPathArray[i].ToString() + "\\";
        }
		if (applicationPath.Equals (String.Empty))
			applicationPath = GetApplicationPathMac();
		else
			applicationPath += "SearchApp\\";
        return applicationPath;
    }

    public static string GetVirtualPath(string url)
    {
        if (HttpContext.Current.Request.ApplicationPath == "/")
        {
            return "~" + url;
        }

        return Regex.Replace(url, "^" +
                       HttpContext.Current.Request.ApplicationPath + "(.+)$", "~$1");
    }
	
	public static string GetPath(string fileUri)
    {
		string[] splitter = { "\\" };
        string[] pathArray = fileUri.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        string path = String.Empty;

        for (int i = 0; i < pathArray.Length - 1; i++)
        {
            path += pathArray[i].ToString() + "\\";
        }
        return path;
    }
}
