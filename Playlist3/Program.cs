using Common;
using System;
using System.Collections.Generic;

namespace Playlist3
{
    class Program
    {
        public static void Main(string[] args)
        {
            IPlaylist playlist = new Playlist();
            Menu menu = new Menu(playlist);
            menu.Display();
        }
    }

    public class Playlist : IPlaylist
    {
        private SortedDictionary<int, Song> songs = new SortedDictionary<int, Song>();
        private Dictionary<string, int> titleToKey = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private int nextPosition = 0;
        private IEnumerator<KeyValuePair<int, Song>>? enumerator;

        public void AddSong(Song song)
        {
            songs[nextPosition] = song;
            titleToKey[song.Title] = nextPosition;
            nextPosition++;
            Reset();
        }

        public bool RemoveSong(Song song)
        {
            if (titleToKey.TryGetValue(song.Title, out int key))
            {
                songs.Remove(key);
                titleToKey.Remove(song.Title);
                Reset();
                return true;
            }
            return false;
        }

        public Song? PlayNext()
        {
            if (enumerator == null)
                enumerator = songs.GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current.Value;

            return null;
        }

        public void Reset()
        {
            enumerator?.Dispose();
            enumerator = null;
        }

        public bool MoveSongUp(Song song)
        {
            if (!titleToKey.TryGetValue(song.Title, out int key)) return false;

            var keys = new List<int>(songs.Keys);
            int index = keys.IndexOf(key);
            if (index > 0)
            {
                var prevKey = keys[index - 1];
                var temp = songs[prevKey];
                songs[prevKey] = songs[key];
                songs[key] = temp;

                titleToKey[songs[prevKey].Title] = prevKey;
                titleToKey[songs[key].Title] = key;
                Reset();

                return true;
            }
            return false;
        }

        public bool MoveSongDown(Song song)
        {
            if (!titleToKey.TryGetValue(song.Title, out int key)) return false;

            var keys = new List<int>(songs.Keys);
            int index = keys.IndexOf(key);
            if (index < keys.Count - 1 && index >= 0)
            {
                var nextKey = keys[index + 1];
                var temp = songs[nextKey];
                songs[nextKey] = songs[key];
                songs[key] = temp;

                titleToKey[songs[nextKey].Title] = nextKey;
                titleToKey[songs[key].Title] = key;
                Reset();

                return true;
            }
            return false;
        }

        public void ShowPlaylist()
        {
            Console.WriteLine("Playlist (Reorderable Dictionary):");
            foreach (var kvp in songs)
            {
                var s = kvp.Value;
                Console.WriteLine($"{s.Title} by {s.Artist} ({s.Length} sec)");
            }
        }

        public int Length() => songs.Count;
    }
}
