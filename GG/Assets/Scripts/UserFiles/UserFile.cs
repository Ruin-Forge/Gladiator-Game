using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public abstract class UserFile
{
    private string filename;

    public UserFile(string filename)
    {
        this.filename = filename;
    }

    public string Filepath => $"UserFiles\\Save\\{filename}.xml";

    #region Save/Load

    public void Save()
    {
        XmlDocument doc = new XmlDocument();
        XmlElement rootNode = doc.CreateElement("UserFile");
        doc.AppendChild(rootNode);

        DoSave(doc, rootNode);

        Directory.CreateDirectory("UserFiles\\Save");
        doc.Save(Filepath);
    }

    protected abstract void DoSave(XmlDocument doc, XmlElement rootNode);

    protected void SaveElement(XmlDocument doc, XmlElement targetNode, string nodeName, object data, params (string, object)[] attributes)
    {
        XmlElement node = doc.CreateElement(nodeName);
        node.InnerText = data.ToString();
        for (int i = 0; i < attributes.Length; i++)
            node.SetAttribute(attributes[i].Item1, attributes[i].Item2.ToString());
        targetNode.AppendChild(node);
    }

    public void Load()
    {
        if (!File.Exists(Filepath))
            return;

        XmlDocument doc = new XmlDocument();
        doc.Load(Filepath);
        XmlNode rootNode = doc.FirstChild;

        DoLoad(rootNode);
    }

    protected abstract void DoLoad(XmlNode rootNode);

    #endregion Save/Load
}