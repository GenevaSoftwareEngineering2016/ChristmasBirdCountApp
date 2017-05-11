// Class for Creating BirdCount Objects
public class BirdCount
{
    public string Name { get; set; }
    public int Count { get; set; }
    public bool InList { get; set; }
    
    public BirdCount() {}

    public BirdCount(string name, int count, bool inList)
    {
        Name = name;
        Count = count;
        InList = inList;
    }
}