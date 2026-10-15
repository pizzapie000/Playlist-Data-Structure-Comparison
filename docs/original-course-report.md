# Historical course report

This is the original submission. The corrected complexity analysis and playback contract are in the root README. Historical timing observations below are not reproducible benchmark results.

# Playlist Report

## Give background and context
In this project I made three different music playlists that had the same features, but a different structure within the code. I made an array/list, a linked list, and a dictionary structure. I was working to compare them by making tests where I can see the performance of them. They each had the same features where they added, removed, and moved songs up and down. I worked to make tests that make sure that the programs run properly but more importantly, I made it possible to see the performance of adding ten thousand songs. This is helpful for seeing the efficiency of each of them if they were scaled up for adding songs. It doesnt account for things like if I need to move or organize songs within the playlist, but it's a good start.

## Talk about the implementation and performance of your playlist with an Array / List
Well this ended up taking me ages longer because instead of just using the obvious classes in Common I totally missed them and proceeded to make my own IPlaylist class, Menu class, and Song class because apparently I dont know what i'm doing haha. So the playlist with an Array is great because it is very simple to read down and understand especially beginners and still gets the job done. It might not be as efficient because it doesnt scale well. I was able to add some good unit tests and make sure that the code works just fine through them. The performace tests compared the small and large playlists to show that the more advanced structures in Playlist 1 and 2 might help better.
## Talk about the implementation and performance of your playlist with a Linked List
I added a playlist using a custom double linked list instead of the built in List<song> that I used in Playlist 1. Because of this I was able to work with nodes and manipulate pointers. I needed some more practice with that. It was quite a bit more complicated but it is more flexible. This playlist can make any song insertion and any removals a lot more efficient because only a few of the node references have to be updated compared to having to shif tall of the elements like in my first playlist. It is a little slower in this playlist though because I have to go through each node one by one. It only took 1 milisecond to complete compared to the array/list with 2 miliseconds which still is a very small bit of time.

## Talk about the implementation and performance of your playlist with a Binary Search Tree

The third playlist I made uses a dictionary structure with position mapped by keys. I had to use the keys in order to make it possible to reorder the playlist without messing up the dictionary. I was able to move songs up and down by storing the songs in a SortedDictionary<int, Song>. This made it possible for me to keep the playlist efficient for when I need to add or remove lots of songs.

Adding the 10,000 songs took 8 ms which was slower than the array and the linked list. Still pretty dang fast all things considered. It took longer because the program has to manage the keys and dictionary hashing. It is really efficient to reorder by swapping songs. The test case only tests adding songs, which this program is not as efficient at.

## Which would you choose? Why? What if you had to implement other features such as Shuffle or Play Next - how would that affect your choice?

All three of the options worked pretty well, but each of them have upsides and downsides. If I were to make this for a real company I would use the Dictionary/BST because it is very easy to scale up and reorder songs very easily. Even though initially adding songs takes a bit longer, when the songs are actually in the playlist it is a lot more organized.

For playlist 1 adding 10,000 songs took 2ms because adding and removing songs used shifts which I assume is because it is a little slower than swapping and updating pointers. The efficiency of searching for songs is O(n) which is pretty good because it searches through each of the songs only once. However if I had to add or remove songs to the end it would only be O(1), where if I wanted to add or remove songs from the middle of the playlist it wouldbe O(n)

For playlist 2 adding 10,000 songs only took a single ms because updating and swapping the pointers for adding/removing songs and moving songs found within the linked list is very fast. The efficiency of searching for songs is still O(n) though because I still need to search through each node to find it.

For playlist 3 adding 10,000 songs took 8ms which is multiple times longer than the previous playlists. Removing songs is pretty fast because I just need to remove the hash, as well as moving songs through the key that handles the swaps. The efficiency of searching for songs is O(1) in a hash dictionary, whereas in a Sorted Dictionary lookup by searching by the key is O(logn)

All of these playlists worked pretty good and even were able to deal with 10,000 soungs under 10ms on my computer. The array/list was really simple to code and very fast for adding songs sequentially. The linked list was really good for adding and removing songs. The dictionary/bst was super good for scaling up and making sure that the data is organized. I would use playlist 3 for a real company because it has a great balance of keeping data organized, and is really good for looking up songs efficiently. I wouldnt have to worry about how large the playlists get.
