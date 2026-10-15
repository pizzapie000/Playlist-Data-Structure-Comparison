using NUnit.Framework;
using Common;
using Playlist2;
using System.Diagnostics;

namespace PlaylistTests
{
    [Category("Performance")]
    public class PerformanceTests_Playlist2
    {
        [Test]
        public void AddManySongs_PerformanceIsAcceptable()
        {
            var playlist = new Playlist();
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 10000; i++)
                playlist.AddSong(new Song($"Song {i}", "Artist", 120));

            stopwatch.Stop();
            Assert.That(playlist.Length(), Is.EqualTo(10000));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(200));
        }

        [Test]
        public void RemoveManySongs_PerformanceIsAcceptable()
        {
            var playlist = new Playlist();
            for (int i = 0; i < 10000; i++)
                playlist.AddSong(new Song($"Song {i}", "Artist", 120));

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
                playlist.RemoveSong(new Song($"Song {i}", "Artist", 120));
            stopwatch.Stop();

            Assert.That(playlist.Length(), Is.EqualTo(0));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(500));
        }

        [Test]
        public void MoveSongs_PerformanceIsAcceptable()
        {
            var playlist = new Playlist();
            for (int i = 0; i < 10000; i++)
                playlist.AddSong(new Song($"Song {i}", "Artist", 120));

            var stopwatch = Stopwatch.StartNew();
            for (int i = 1; i < 10000; i++)
                playlist.MoveSongUp(new Song($"Song {i}", "Artist", 120));
            stopwatch.Stop();

            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(500));
        }

        [Test]
        public void PlayNext_PerformanceIsAcceptable()
        {
            var playlist = new Playlist();
            for (int i = 0; i < 10000; i++)
                playlist.AddSong(new Song($"Song {i}", "Artist", 120));

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
                playlist.PlayNext();
            stopwatch.Stop();

            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(200));
        }

        [Test]
        public void Reset_PerformanceIsAcceptable()
        {
            var playlist = new Playlist();
            for (int i = 0; i < 1000; i++)
                playlist.AddSong(new Song($"Song {i}", "Artist", 120));

            playlist.PlayNext(); // advance once

            var stopwatch = Stopwatch.StartNew();
            playlist.Reset();
            stopwatch.Stop();

            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(50));
        }
    }
}
