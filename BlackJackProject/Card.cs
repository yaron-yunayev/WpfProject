public class Card
{
    public string Suit { get; set; }
    public string Rank { get; set; }
    public int Value { get; set; }
    public string ImageName { get; set; }  // This will store the image name

    public Card(string suit, string rank, int value, string imageName)
    {
        Suit = suit;
        Rank = rank;
        Value = value;
        ImageName = imageName;
    }
}
