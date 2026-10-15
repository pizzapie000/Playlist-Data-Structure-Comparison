namespace Common
{
    public class Song
    {
        public string Title { get; set; } = string.Empty;
        public double Length { get; set; }
        public string Artist { get; set; } = string.Empty;

        public Song() { }

        public Song(string title, string artist = "", double length = 0.0)
        {
            Title = title;
            Artist = artist;
            Length = length;
        }

        public override string ToString() => $"{Title} by {Artist} ({Length} sec)";
    }
}
