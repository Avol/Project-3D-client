using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public  class ProfileManager
{
    private KinectManager kinectManager;

    public ProfileManager(Core core)
    {
        this.kinectManager = core.gameObject.GetComponent<KinectManager>();
    }

    public void saveProfile(Profile profile)
    {
        // create dir
        Directory.CreateDirectory(@Application.dataPath + "Resources/Profiles/" + profile.name);
        // create n save xml
        XmlDocument profileXml = new XmlDocument();
        XmlElement name = profileXml.CreateElement("name");
        name.InnerText = profile.name;
        profileXml.AppendChild(name);
        profileXml.Save(@Application.dataPath + "Resources/Profiles/" + profile.name + "/profile.xml");
        // save foto
        byte[] bytes = profile.foto.EncodeToPNG();
        File.WriteAllBytes(@Application.dataPath + "Resources/Profiles/" + profile.name + "/foto.png", bytes);
    }

    public Texture2D takeFoto()
    {
        return this.kinectManager.GetUsersClrTex();
    }

    public Profile loadProfile(string name)
    {
        XmlDocument profileXml = new XmlDocument();
        profileXml.Load(@Application.dataPath + "Resources/Profiles/" + name + "/profile.xml");
        return new Profile(name, (Texture2D)Resources.Load("/Profiles/" + name + "/foto.png"));
    }

     public List<Profile> loadAllProfiles()
     {
         DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "Resources/Profiles/");
         DirectoryInfo[] info = dir.GetDirectories();
         List<Profile> profiles = new List<Profile>();

         for (int i = 0; i < info.Length; i++)
         {
             profiles.Add(this.loadProfile(info[i].Name));
         }
  
         return profiles;
     }
}

public class Profile
{
    public string name { get; private set; }
    public Texture2D foto { get; private set; }
        
    public Profile(string name, Texture2D foto)
    {
        this.name = name;
        this.foto = foto;
    }
}