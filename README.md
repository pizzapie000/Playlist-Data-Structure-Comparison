# Playlist Data Structure Comparison

Three C# implementations of the same music-playlist contract: a dynamic array, a custom doubly linked list, and a sorted dictionary with a title lookup index. This project explores how storage choices affect insertion, removal, playback, and reordering.

## Engineering focus

- A shared `IPlaylist` interface and console menu keep user-facing operations consistent across implementations.
- The linked-list version manages head, tail, and bidirectional node references directly.
- The sorted implementation separates playlist positions from title-based lookup.
- NUnit tests cover functional behavior and regressions. Separate timing experiments exercise 10,000-song workloads.

## Run

Install the .NET 8 SDK, then run one implementation from the repository root:

```sh
dotnet run --project Playlist1
dotnet run --project Playlist2
dotnet run --project Playlist3
```

The menu supports adding/removing songs, moving a song up/down, playback, reset, and listing songs. It models playlist operations; it does not decode or play audio files.

## Playback contract

Playback proceeds in order and stays at end-of-list until reset or an edit. Every successful add, remove, or move restarts playback at the first song. This makes mutation behavior consistent across the three storage strategies and prevents an invalidated dictionary enumerator from being reused.

## Complexity of the current implementations

`n` is the number of songs. Search/reordering workloads assume unique song titles.

| Operation | List | Doubly linked list | Sorted dictionary plus title index |
| --- | --- | --- | --- |
| Append | Amortized O(1) | O(1) | O(log n) |
| Find/remove by title | O(n) | O(n) search, O(1) unlink | Expected O(1) title lookup plus O(log n) tree removal |
| Move up/down | O(n) search, O(1) swap | O(n) search, O(1) relink | O(n): materializes ordered keys and searches for position |
| Sequential playback | O(1) per step | O(1) per step | Amortized O(1) across traversal; enumerator setup can cost O(log n) |
| Storage | O(n) | O(n) | O(n), with an additional lookup index |

`SortedDictionary` is tree-based, not a hash table. Only the separate title index uses hashing. Big-O alone does not establish which version is faster for a particular workload.

## Validate

```sh
dotnet test PlaylistTests/PlaylistTests.csproj --configuration Release --filter "TestCategory!=Performance"
```

The correctness suite passed **33 tests** during portfolio preparation on October 14, 2026, including regressions for moving a former tail before appending, edits during playback, and repeated end-of-list calls.

Timing experiments are opt-in and have machine-dependent thresholds:

```sh
dotnet test PlaylistTests/PlaylistTests.csproj --configuration Release --filter "TestCategory=Performance"
```

Timing thresholds are not part of CI correctness validation, and no cross-machine speedup claim is made. For a rigorous comparison, use repeated warmups/samples and record runtime, hardware, data distribution, allocations, and all operations rather than only append time.

In the local timing run, 14 of 15 experiments met their original thresholds. The sorted-dictionary move workload took about 781 ms against a 500 ms threshold, reflecting its current O(n) key-list construction per move. This limit remains visible rather than being relaxed to make the experiment pass.

## Limits and provenance

The title-index implementation assumes unique titles; duplicate-title/artist identity requires a richer indexing policy. The console menu and support classes originate in the CSE 212 coursework context. The original assignment and report are retained under `docs/` for attribution. The portfolio copy adds regression fixes, an explicit playback contract, documentation, and CI. See `NOTICE.md` before redistributing course material.
