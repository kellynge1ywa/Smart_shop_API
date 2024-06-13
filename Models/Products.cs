namespace duka;

public class Product
{
    public Guid Id {get;set;}
    public string Name {get;set;}="";
    public string ImageURL {get;set;}="";
    public int Price {get;set;}
    public string CategoryIdentifier {get;set;}="";
    public Guid CategoryId {get;set;}

}
