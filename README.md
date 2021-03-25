# Dijkstra's Shortest Path Assignment:

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

### What I would do differently:
- I would not have created as many iterable collections. I ended up with more than I needed. I could have gotten away with using the Table or the _adjacencyDictionary.
- I ended up having too many if conditions as a result of the previous point
- There is enough repition here that recursion would have made the work more compact. If I didn't have as many collections, it would have been easier.
