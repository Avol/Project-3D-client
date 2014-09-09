using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

/// <summary>
/// Managing profile data, saving/loading.
/// </summary>
public  class ProfileManager
{
    private KinectManager kinectManager;

    public ProfileManager(Core core)
    {
        this.kinectManager = core.gameObject.GetComponent<KinectManager>();
    }

    /// <summary>
    /// Saves all the profile data to profile dir.
    /// </summary>
    /// <param name="profile"></param>
    public void saveProfile(Profile profile)
    {
        // create dir.
        Directory.CreateDirectory(@Application.dataPath + "Resources/Profiles/" + profile.name);
        // create n save xml.
        XmlDocument profileXml = new XmlDocument();
        XmlElement name = profileXml.CreateElement("name");
        name.InnerText = profile.name;
        profileXml.AppendChild(name);
        profileXml.Save(@Application.dataPath + "Resources/Profiles/" + profile.name + "/profile.xml");
        // save profile foto.
        byte[] bytes = profile.profileFoto.EncodeToPNG();
        File.WriteAllBytes(@Application.dataPath + "Resources/Profiles/" + profile.name + "/profile.png", bytes);
        // save wall fotos.
        for (int i = 0; i < 5; i++)
        {
            byte[] data = profile.wallFotos[i].EncodeToPNG();
            File.WriteAllBytes(@Application.dataPath + "Resources/Profiles/" + profile.name + "/foto" + i + ".png", data);
        }
        // save sounds
        SavWav.Save("Resources/Profiles/" + profile.name + "/phone.wav", profile.phone);
        SavWav.Save("Resources/Profiles/" + profile.name + "/music.wav", profile.music);
    }

    /// <summary>
    /// Loads a single profile by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Profile loadProfile(string name)
    {
        XmlDocument profileXml = new XmlDocument();
        profileXml.Load(@Application.dataPath + "Resources/Profiles/" + name + "/profile.xml");
        Texture2D profileFoto = (Texture2D)Resources.Load("Profiles/" + name + "/profile.png");
        List<Texture2D> wallFotos = new List<Texture2D>();
        for (int i = 0; i < 5; i++)
            wallFotos.Add((Texture2D)Resources.Load("Profiles/" + name + "/foto"+i+".png"));
        AudioClip phone = (AudioClip)Resources.Load("Profiles/" + name + "/phone.wav");
        AudioClip music = (AudioClip)Resources.Load("Profiles/" + name + "/music.wav");
        return new Profile(name, profileFoto, wallFotos, phone, music);
    }

    /// <summary>
    /// Load all profiles.
    /// </summary>
    /// <returns>all profiles</returns>
    public List<Profile> loadAllProfiles()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "Resources/Profiles/");
        DirectoryInfo[] info = dir.GetDirectories();
        List<Profile> profiles = new List<Profile>();
        for (int i = 0; i < info.Length; i++)
            profiles.Add(this.loadProfile(info[i].Name));
        return profiles;
    }
}

/// <summary>
/// Contains all the profile data.
/// </summary>
public class Profile
{
    public string name { get; private set; }
    public Texture2D profileFoto { get; private set; }
    public List<Texture2D> wallFotos { get; private set; }
    public AudioClip phone { get; private set; }
    public AudioClip music { get; private set; }

    public Profile(string name, Texture2D profileFoto, List<Texture2D> wallFotos, AudioClip phone, AudioClip music)
    {
        this.name = name;
        this.profileFoto = profileFoto;
        this.wallFotos = wallFotos;
        this.phone = phone;
        this.music = music;
    }
}