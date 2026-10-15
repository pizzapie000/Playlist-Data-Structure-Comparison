using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Playlist1
{
    class Program
    {
        static void Main(string[] args)
        {
            IPlaylist playlist = new Playlist();
            Menu menu = new Menu(playlist);
            menu.Display();
        }
    }

    public class Playlist : IPlaylist
    {
        private readonly List<Song> songs = new();
        private int currentIndex = 0;

        public void AddSong(Song song)
        {
            songs.Add(song);
            Reset();
        }

        public bool RemoveSong(Song song)
        {
            var match = songs.FirstOrDefault(s =>
                string.Equals(s.Title, song.Title, StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrEmpty(song.Artist) ||
                 string.Equals(s.Artist, song.Artist, StringComparison.OrdinalIgnoreCase)));

            if (match != null)
            {
                songs.Remove(match);
                Reset();
                return true;
            }
            return false;
        }

        public Song? PlayNext()
        {
            if (currentIndex < songs.Count)
                return songs[currentIndex++];
            return null;
        }

        public void Reset() => currentIndex = 0;

        public bool MoveSongUp(Song song)
        {
            int idx = songs.FindIndex(s =>
                string.Equals(s.Title, song.Title, StringComparison.OrdinalIgnoreCase));

            if (idx > 0)
            {
                (songs[idx - 1], songs[idx]) = (songs[idx], songs[idx - 1]);
                Reset();
                return true;
            }
            return false;
        }

        public bool MoveSongDown(Song song)
        {
            int idx = songs.FindIndex(s =>
                string.Equals(s.Title, song.Title, StringComparison.OrdinalIgnoreCase));

            if (idx >= 0 && idx < songs.Count - 1)
            {
                (songs[idx], songs[idx + 1]) = (songs[idx + 1], songs[idx]);
                Reset();
                return true;
            }
            return false;
        }

        public void ShowPlaylist()
        {
            Console.WriteLine("Playlist:");
            foreach (var song in songs)
                Console.WriteLine($"{song.Title} by {song.Artist} ({song.Length} sec)");
        }

        public int Length() => songs.Count;
    }
}
