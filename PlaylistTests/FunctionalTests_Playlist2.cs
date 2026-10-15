using NUnit.Framework;
using Common;
using Playlist2;

namespace PlaylistTests
{
    public class FunctionalTests_Playlist2
    {
        [Test]
        public void AddSong_IncreasesLength()
        {
            var playlist = new Playlist();
            var song = new Song("Never Gonna Give You Up", "Rick Astley", 212);

            playlist.AddSong(song);

            Assert.That(playlist.Length(), Is.EqualTo(1));
        }

        [Test]
        public void PlayNext_ReturnsSongsInOrder()
        {
            var playlist = new Playlist();
            playlist.AddSong(new Song("Song A", "Artist A", 100));
            playlist.AddSong(new Song("Song B", "Artist B", 200));

            var first = playlist.PlayNext();
            var second = playlist.PlayNext();

            Assert.That(first?.Title, Is.EqualTo("Song A"));
            Assert.That(second?.Title, Is.EqualTo("Song B"));
        }

        [Test]
        public void RemoveSong_RemovesMatchingSong()
        {
            var playlist = new Playlist();
            var song = new Song("Song A", "Artist A", 100);
            playlist.AddSong(song);

            var removed = playlist.RemoveSong(new Song("Song A"));
            Assert.That(removed, Is.True);
            Assert.That(playlist.Length(), Is.EqualTo(0));
        }

        [Test]
        public void Reset_AllowsReplayFromStart()
        {
            var playlist = new Playlist();
            playlist.AddSong(new Song("Song A", "Artist A", 100));
            playlist.AddSong(new Song("Song B", "Artist B", 200));

            playlist.PlayNext(); // plays Song A
            playlist.Reset();
            var replay = playlist.PlayNext();

            Assert.That(replay?.Title, Is.EqualTo("Song A"));
        }

        [Test]
        public void MoveSongUp_MovesSongCorrectly()
        {
            var playlist = new Playlist();
            playlist.AddSong(new Song("Song A", "Artist", 100));
            playlist.AddSong(new Song("Song B", "Artist", 200));

            var moved = playlist.MoveSongUp(new Song("Song B"));
            Assert.That(moved, Is.True);

            var first = playlist.PlayNext();
            Assert.That(first?.Title, Is.EqualTo("Song B"));
        }

        [Test]
        public void MoveSongDown_MovesSongCorrectly()
        {
            var playlist = new Playlist();
            playlist.AddSong(new Song("Song A", "Artist", 100));
            playlist.AddSong(new Song("Song B", "Artist", 200));

            var moved = playlist.MoveSongDown(new Song("Song A"));
            Assert.That(moved, Is.True);

            var first = playlist.PlayNext();
            Assert.That(first?.Title, Is.EqualTo("Song B"));
        }
    }
}
