using System.Xml;

public class SaveGame : UserFile
{
    #region Fields

    private string name;

    #endregion Fields

    public SaveGame(string filename) : base(filename)
    {
    }

    #region Properties

    public string Name { get => name; set => name = value; }

    #endregion Properties

    #region Save/Load

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        //Name
        SaveElement(doc, rootNode, "Name", name);
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        //Name
        name = rootNode.SelectSingleNode("Name").InnerText;
    }

    #endregion Save/Load
}