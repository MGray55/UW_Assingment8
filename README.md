# Dijkstra's Shortest Path Assignment:

### Direct link to code:
[Program.cs](https://github.com/MGray55/UW_Assingment8/blob/main/ConsoleApp9a/Program.cs)

### What I did:
The assumption is that all graphs are non-directional, and paths should be whatver the shortest combination of distances on a weighted edge.
I created several classes in C#
- Vertex - a simple object with a key, and a collection of children that are adjacent to this vertex.
- AdjacentVertex - The child vertexes of the previous class
- WeightedEdge - A representation of the data entered. Includes a start vertex, end vertex, and the distance between them as specified by the input data.
- DijkstraTable - An object for updating the distance calculation
- DijkstraTableRow - Represents a row in the table that can be updated

For tracking the items while building the Dijkstra table, I used
``` private Dictionary<string, Vertex> _adjencyDictionary; ```

As an example, choosing '1' to run the application, the following edges will be used:
- {a, b, 12}
- {b, c, 3}
- {b, d, 5}
- {d, c, 1}
- {c, a, 2}

This represents a graph like this:
![Graph](/IMG_2288.jpg)

### What I would do differently:
- I definitely made some decisions early on, that I came to not like later. My original thought was to use the Vertex object more like a linked list, but the Dictionary turned out to be a much better mechanism.
- I would not have created as many iterable collections. I ended up with more than I needed. I could have gotten away with using the Table or the _adjacencyDictionary. This creates too much overhead
- I ended up having too many 'if' conditions as a result of the previous point.
- There is enough repition here that recursion would have made the work more compact and readable. If I didn't have as many collections, it would have been easier.

