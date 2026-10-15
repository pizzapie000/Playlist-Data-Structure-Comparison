using Common;
using NUnit.Framework;

namespace PlaylistTests;

public class PlaybackRegressionTests
{
    private static IPlaylist Create(int implementation) => implementation switch
    {
        1 => new Playlist1.Playlist(),
        2 => new Playlist2.Playlist(),
        _ => new Playlist3.Playlist()
    };

    [TestCase(1)] [TestCase(2)] [TestCase(3)]
    public void EndOfPlaylistDoesNotRestartUntilReset(int implementation)
    {
        var playlist = Create(implementation);
        playlist.AddSong(new Song("A"));
        Assert.That(playlist.PlayNext()?.Title, Is.EqualTo("A"));
        Assert.That(playlist.PlayNext(), Is.Null);
        Assert.That(playlist.PlayNext(), Is.Null);
        playlist.Reset();
        Assert.That(playlist.PlayNext()?.Title, Is.EqualTo("A"));
    }

    [TestCase(1)] [TestCase(2)] [TestCase(3)]
    public void AddDuringPlaybackRestartsSafely(int implementation)
    {
        var playlist = Create(implementation);
        playlist.AddSong(new Song("A"));
        playlist.AddSong(new Song("B"));
        playlist.PlayNext();
        playlist.AddSong(new Song("C"));
        Assert.That(ReadAll(playlist), Is.EqualTo(new[] {"A", "B", "C"}));
    }

    [TestCase(1)] [TestCase(2)] [TestCase(3)]
    public void ReorderThenAppendPreservesEverySong(int implementation)
    {
        var playlist = Create(implementation);
        foreach (var title in new[] {"A", "B", "C"}) playlist.AddSong(new Song(title));
        Assert.That(playlist.MoveSongUp(new Song("C")), Is.True);
        playlist.AddSong(new Song("D"));
        Assert.That(playlist.Length(), Is.EqualTo(4));
        Assert.That(ReadAll(playlist), Is.EqualTo(new[] {"A", "C", "B", "D"}));
    }

    [TestCase(1)] [TestCase(2)] [TestCase(3)]
    public void RemoveDuringPlaybackRestartsSafely(int implementation)
    {
        var playlist = Create(implementation);
        foreach (var title in new[] {"A", "B", "C"}) playlist.AddSong(new Song(title));
        playlist.PlayNext();
        Assert.That(playlist.RemoveSong(new Song("B")), Is.True);
        Assert.That(ReadAll(playlist), Is.EqualTo(new[] {"A", "C"}));
    }

    [TestCase(1)] [TestCase(2)] [TestCase(3)]
    public void MoveDuringPlaybackRestartsSafely(int implementation)
    {
        var playlist = Create(implementation);
        foreach (var title in new[] {"A", "B", "C"}) playlist.AddSong(new Song(title));
        playlist.PlayNext();
        Assert.That(playlist.MoveSongDown(new Song("A")), Is.True);
        Assert.That(ReadAll(playlist), Is.EqualTo(new[] {"B", "A", "C"}));
    }

    private static string[] ReadAll(IPlaylist playlist)
    {
        var titles = new List<string>();
        for (var i = 0; i < playlist.Length() + 1; i++)
        {
            var song = playlist.PlayNext();
            if (song == null) return titles.ToArray();
            titles.Add(song.Title);
        }
        Assert.Fail("Playback did not terminate at the end of the playlist.");
        return titles.ToArray();
    }
}
