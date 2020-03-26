namespace Komment
{
    public class Folder
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public Folder ParentFolder { get; set; }
    }
}
