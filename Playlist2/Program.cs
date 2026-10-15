using Common;

namespace Playlist2
{
    class Program
    {
        static void Main(string[] args)
        {
            IPlaylist playlist = new Playlist();  // Create the linked list playlist

            Menu menu = new Menu(playlist); // Pass the playlist to the shared Menu class


            menu.Display(); // Display the interactive menu
        }
    }

    public class Playlist : IPlaylist
    {
        // Internal Node class for linked list
        private class Node
        {
            public Song Data { get; set; }
            public Node? Next { get; set; }
            public Node? Prev { get; set; }
            public Node(Song data) => Data = data;
        }

        private Node? head;
        private Node? tail;
        private Node? current;
        private int count = 0;
        private bool playbackFinished;

        public void AddSong(Song song)
        {
            var node = new Node(song);
            if (head == null)
            {
                head = tail = node;
            }
            else
            {
                tail!.Next = node;
                node.Prev = tail;
                tail = node;
            }
            count++;
            Reset();
        }

        public bool RemoveSong(Song song)
        {
            var node = FindNode(song);
            if (node == null) return false;

            if (node.Prev != null) node.Prev.Next = node.Next;
            else head = node.Next;

            if (node.Next != null) node.Next.Prev = node.Prev;
            else tail = node.Prev;

            if (current == node) current = node.Next;

            count--;
            Reset();
            return true;
        }

        public Song? PlayNext()
        {
            if (playbackFinished) return null;
            if (current == null) current = head;
            else current = current.Next;

            if (current == null) playbackFinished = true;
            return current?.Data;
        }

        public void Reset()
        {
            current = null;
            playbackFinished = false;
        }

        public bool MoveSongUp(Song song)
        {
            var node = FindNode(song);
            if (node == null || node.Prev == null) return false;

            var prev = node.Prev;
            var next = node.Next;

            // Swap node with prev
            if (prev.Prev != null) prev.Prev.Next = node;
            node.Prev = prev.Prev;

            node.Next = prev;
            prev.Prev = node;
            prev.Next = next;

            if (next != null) next.Prev = prev;

            if (head == prev) head = node;
            if (tail == node) tail = prev;
            Reset();
            return true;
        }

        public bool MoveSongDown(Song song)
        {
            var node = FindNode(song);
            if (node == null || node.Next == null) return false;

            var next = node.Next;
            var prev = node.Prev;
            var afterNext = next.Next;

            // Swap node with next
            if (prev != null) prev.Next = next;
            next.Prev = prev;

            next.Next = node;
            node.Prev = next;
            node.Next = afterNext;

            if (afterNext != null) afterNext.Prev = node;

            if (head == node) head = next;
            if (tail == next) tail = node;
            Reset();

            return true;
        }

        public void ShowPlaylist()
        {
            Console.WriteLine("Playlist (Linked List):");
            var node = head;
            while (node != null)
            {
                Console.WriteLine($"{node.Data.Title} by {node.Data.Artist} ({node.Data.Length} sec)");
                node = node.Next;
            }
        }

        public int Length() => count;

        // Helper thatfinds the node by title (not case sensitive)
        private Node? FindNode(Song song)
        {
            var node = head;
            while (node != null)
            {
                if (string.Equals(node.Data.Title, song.Title, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrEmpty(song.Artist) ||
                     string.Equals(node.Data.Artist, song.Artist, StringComparison.OrdinalIgnoreCase)))
                {
                    return node;
                }
                node = node.Next;
            }
            return null;
        }
    }
}
