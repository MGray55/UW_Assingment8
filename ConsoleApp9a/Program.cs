using System;
using System.Collections.Generic;

namespace ConsoleApp9a
{
    internal class WeightedGraph
    {
        // A collection of weighted edges built with pairs to start the process
        private List<WeightedEdge> _weightedEdges;

        // Creates a dictionary of parent Vertexes, which each have adjacent children collections
        private Dictionary<string, Vertex> _adjencyDictionary;

        // Integer to set the number of rows needed in the Dijkstra table
        private int _numberOfVertexes;

        // Where we will calculate the shortest paths
        private DijkstraTable _dijkstraTable;

        // The key to identify the source Vertex
        private string _sourceVertexKey = null;

        /**
         * Read-only
         */
        public List<WeightedEdge> GetWeightedEdges()
        {
            return _weightedEdges;
        }

        /**
         * And object that represents a source vertex,
         * with a collection of adjacent (child) vertexes
         * Allows mapping connections from this vertex to it's neighbors
         */
        private class Vertex
        {
            private string _key;
            private List<AdjacentVertex> _adjacent;

            public string GetKey()
            {
                return _key;
            }

            public void SetAdjacent(AdjacentVertex v)
            {
                _adjacent.Add(v);
            }

            public List<AdjacentVertex> GetAdjacent()
            {
                return _adjacent;
            }

            public Vertex(string key)
            {
                _key = key;
                _adjacent = new List<AdjacentVertex>();
            }
        }

        /**
         * Used as child objects of Vertex
         */
        private class AdjacentVertex
        {
            private string _key; // Vertex name identifier
            private int _distanceFromParentVertex = 0;

            public string GetKey()
            {
                return _key;
            }

            public int GetDistanceToPrevious()
            {
                return _distanceFromParentVertex;
            }

            public AdjacentVertex(string key, int distanceFromParent)
            {
                _key = key;
                _distanceFromParentVertex = distanceFromParent;
            }
        }

        /**
         * Object that represents an edge path
         * between two Vertex objects.
         * The path is non-directional
         */
        public class WeightedEdge
        {
            private readonly string _startVertex;
            private readonly string _endVertex;
            private readonly int _distance;

            public string GetStartVertex()
            {
                return _startVertex;
            }

            public string GetEndVertex()
            {
                return _endVertex;
            }

            public int GetEdgeDistance()
            {
                return _distance;
            }

            public WeightedEdge(string startVertex, string endVertex, int distance)
            {
                _startVertex = startVertex;
                _endVertex = endVertex;
                _distance = distance;
            }
        }

        /**
         * Used to build shortest path DijkstraTable
         * Contains current vertex, previous vertex, and distance back to source vertex
         */
        private class DijkstraTableRow
        {
            private readonly string _key;
            private string _previousVertex;
            private int _distanceFromStart;
            private int _distanceFromPrevious;

            public string GetKey()
            {
                return _key;
            }

            public string GetPreviousVertex()
            {
                return _previousVertex;
            }

            public int GetDistanceFromStart()
            {
                return _distanceFromStart;
            }

            public void SetDistanceFromPrevious(int distance)
            {
                _distanceFromPrevious = distance;
            }

            public int GetDistanceFromPrevious()
            {
                return _distanceFromPrevious;
            }

            public void SetPreviousVertex(string previous)
            {
                _previousVertex = previous;
            }

            public void SetDistanceFromStart(int distance)
            {
                _distanceFromStart = distance;
            }

            /**
             * The previous vertex isn't necessarily the original previous vertex,
             * but the next neighbor that will lead this vertex to the shortest path to the source Vertex
             */
            public DijkstraTableRow(string key, string previousVertex, int distanceFromStart, int distanceFromPrevious)
            {
                _key = key;
                _previousVertex = previousVertex;
                _distanceFromStart = distanceFromStart;
                _distanceFromPrevious = distanceFromPrevious;
            }
        }

        private class DijkstraTable
        {
            private Dictionary<string, DijkstraTableRow> _rows;

            public Dictionary<string, DijkstraTableRow> GetRows()
            {
                return _rows;
            }

            public void SetRow(DijkstraTableRow row)
            {
                // The SetRow function will not add duplicates
                if (!_rows.ContainsKey(row.GetKey()))
                {
                    _rows.Add(row.GetKey(), row);
                }
            }

            public DijkstraTableRow GetRow(string key)
            {
                return _rows[key];
            }

            public DijkstraTable(int size)
            {
                _rows = new Dictionary<string, DijkstraTableRow>();
            }
        }

        public WeightedGraph(int numberOfVertexes)
        {
            _weightedEdges = new List<WeightedEdge>();
            _numberOfVertexes = numberOfVertexes;
            _adjencyDictionary = new Dictionary<string, Vertex>();
        }

        public void AddEdge(string v1, string v2, int weight)
        {
            // Create a collection of vertex pairs, with weights/distances
            WeightedEdge edge = new WeightedEdge(v1, v2, weight);
            _weightedEdges.Add(edge);

            // Only add new/unique key entries into the dictionary
            if (!_adjencyDictionary.ContainsKey(v1))
            {
                Vertex newVertex = new Vertex(v1);
                newVertex.SetAdjacent(new AdjacentVertex(v2, weight));
                _adjencyDictionary.Add(v1, newVertex);
            }
            else
            {
                _adjencyDictionary[v1].SetAdjacent(new AdjacentVertex(v2, weight));
            }
        }

        public static WeightedGraph CreateWeightedGraph(int graphNum)
        {
            WeightedGraph g;
            // Build weighted graph with a path with a loop
            if (graphNum == 1)
            {
                g = new WeightedGraph(4);
                g.AddEdge("a", "b", 12);
                g.AddEdge("b", "c", 3);
                g.AddEdge("b", "d", 5);
                g.AddEdge("d", "c", 1);
                g.AddEdge("c", "a", 2);
            }
            // Build weighted graph with some bigger values, and loops
            else if (graphNum == 2)
            {
                g = new WeightedGraph(8);
                g.AddEdge("a", "b", 3);
                g.AddEdge("b", "c", 44);
                g.AddEdge("c", "d", 1);
                g.AddEdge("d", "d1", 2);
                g.AddEdge("d1", "d2", 15);
                g.AddEdge("d2", "a", 1);
                g.AddEdge("c", "c1", 11);
                g.AddEdge("c1", "c2", 2);
            }

            // Build weighted graph w/equidistant nodes, and a loop back to source
            // Note: d should end up only 1 away from source
            else if (graphNum == 3)
            {
                g = new WeightedGraph(10);
                g.AddEdge("a", "b", 1);
                g.AddEdge("a", "c", 1);
                g.AddEdge("b", "d", 1);
                g.AddEdge("b", "e", 1);
                g.AddEdge("c", "f", 1);
                g.AddEdge("c", "g", 1);
                g.AddEdge("d", "h", 1);
                g.AddEdge("d", "i", 1);
                g.AddEdge("d", "a", 1);
            }
            else
            {
                g = new WeightedGraph(3);
                g.AddEdge("a", "b", 1);
                g.AddEdge("a", "c", 2);
            }

            return g;
        }

        static void Main(string[] args)
        {
            // Choose a type 
            Console.WriteLine("Enter a number 1,2, or 3 for different Dykstra calculations");
            // Console.WriteLine("(Assumes first vertex is the 'source')");
            Console.WriteLine("Or 'test' to run tests");
            string input = Console.ReadLine();
            if (input == "test")
            {
                runTests();
            }
            else
            {
                int graphNum = int.Parse(input);

                WeightedGraph g = CreateWeightedGraph(graphNum);
                // Verify that it looks like the original
                g.PrintWeightedEdges();

                // Print the selected graph
                g.initializeDijkstraTable();
                g.PrintDijkstraTable();

                // Print the Dijkstra distances from first vertex provided
                g.doDijkstraCalculations();
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Dijkstra's Shortest Path:");
                Console.WriteLine("------------------------------------");
                g.PrintDijkstraTable();
            }
        }

        static void runTests()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("It should create a List of weighted edges from provided input");
            Console.WriteLine("------------------------------------");
            // Setup
            WeightedGraph graph = CreateWeightedGraph(1);

            // Act
            List<WeightedEdge> EdgeList = graph.GetWeightedEdges();

            // Assert
            if (EdgeList.Count == 5)
                Console.WriteLine("pass");
            else
                Console.WriteLine("fail");

            Console.WriteLine("------------------------------------");
            Console.WriteLine("It should create an empty Dijkstra table on initialization");
            Console.WriteLine("And return the source vertex key");
            Console.WriteLine("------------------------------------");

            // Act
            string startingPoint = graph.initializeDijkstraTable();

            // Assert
            int numberOfRows = graph._dijkstraTable.GetRows().Count;
            string firstRowKey = graph._dijkstraTable.GetRow("a").GetKey();
            String stringForTestingComparison = new String("a");

            if (startingPoint.Equals(stringForTestingComparison) && firstRowKey == startingPoint && numberOfRows == 4)
                Console.WriteLine("pass");
            else
                Console.WriteLine("fail");


            Console.WriteLine("------------------------------------");
            Console.WriteLine("It calculate the shortest distances between vertexes");
            Console.WriteLine("regardless of original edges (i.e. find shorter paths)");
            Console.WriteLine("------------------------------------");

            // The distance from B to A should now be shorter than it's original distance of 12

            // Act
            graph.doDijkstraCalculations();

            // Assert
            DijkstraTableRow B_RowKey = graph._dijkstraTable.GetRow("b");
            WeightedEdge originalEdge = EdgeList.Find(x => x.GetStartVertex() == "a" && x.GetEndVertex() == "b");

            if (B_RowKey.GetDistanceFromStart() == 5 && originalEdge.GetEdgeDistance() == 12)
                Console.WriteLine("pass");
            else
                Console.WriteLine("fail");
        }

        /**
         * Initialize a map to start doing the calculation
         * Returns the 'start' Vertex key using the first
         * Vertex in the edge list created earlier
         */
        private string initializeDijkstraTable()
        {
            // string sourceVertexKey = null;
            _dijkstraTable = new DijkstraTable(_numberOfVertexes);
            foreach (WeightedEdge edge in _weightedEdges)
            {
                if (String.IsNullOrEmpty(_sourceVertexKey))
                {
                    _sourceVertexKey = edge.GetStartVertex();
                }

                // Build a table with starting vertex, empty previous vertex, and infinity for distances
                _dijkstraTable.SetRow(new DijkstraTableRow(edge.GetStartVertex(), null, int.MaxValue, int.MaxValue));
                // If vertexes are not starting, make sure they are added to empty table too
                _dijkstraTable.SetRow(new DijkstraTableRow(edge.GetEndVertex(), null, int.MaxValue, int.MaxValue));
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine("DijkstraTable initialized:");
            Console.WriteLine("------------------------------------");
            return _sourceVertexKey;
        }

        private void PrintWeightedEdges()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Weighted edges for this execution: ");
            Console.WriteLine("------------------------------------");
            foreach (WeightedEdge edge in _weightedEdges)
            {
                Console.WriteLine("{" + edge.GetStartVertex() + ", " + edge.GetEndVertex() + ", " +
                                  edge.GetEdgeDistance() + "}");
            }

            Console.WriteLine("");
        }

        private void PrintDijkstraTable()
        {
            foreach (var distEntry in _dijkstraTable.GetRows())
            {
                DijkstraTableRow row = distEntry.Value;
                string prevoiusVertex = row.GetPreviousVertex();
                int distanceToStartVertex = row.GetDistanceFromStart();
                if (row.GetKey() == _sourceVertexKey)
                {
                    Console.WriteLine("Source Vertex: " + row.GetKey());
                }
                else if (String.IsNullOrEmpty(prevoiusVertex) && distanceToStartVertex == int.MaxValue)
                {
                    Console.WriteLine("Source Vertex: " + row.GetKey() + ", Previous: Empty , Distance: Infinity");
                }
                else
                {
                    Console.WriteLine("Source: " + row.GetKey() + ", Previous: " + row.GetPreviousVertex() +
                                      ", Distance: " + row.GetDistanceFromStart());
                }
            }

            Console.WriteLine("");
        }

        private void doDijkstraCalculations()
        {
            // Now go through each weighted edge combo to build the shortest distance matrix
            string currentKey;
            DijkstraTableRow currentRow;
            Vertex currentVertex;

            foreach (KeyValuePair<string, Vertex> vertexEntry in _adjencyDictionary)
            {
                currentVertex = _adjencyDictionary[vertexEntry.Key];
                currentKey = currentVertex.GetKey();
                // Is the current row we have the source/first row in the shortest distance table?
                bool isSourceRow = vertexEntry.Key == _sourceVertexKey;

                // Look to see if the current vertex has collection of adjacent vertexes
                if (currentVertex.GetAdjacent().Count > 0)
                {
                    // Pull the row from the  shortest distance table matching the current key
                    currentRow = _dijkstraTable.GetRow(currentKey);

                    // Loop through adjacents
                    foreach (AdjacentVertex adjacentVertex in currentVertex.GetAdjacent())
                    {
                        // Get the row (from the shortest distance table) for the adjacent vertex
                        DijkstraTableRow adjacentRow = _dijkstraTable.GetRow(adjacentVertex.GetKey());

                        if (!isSourceRow && _sourceVertexKey == adjacentRow.GetKey())
                        {
                            // If here, the current row is not the source row, 
                            // but this adjacent row has a direct path back to the source
                            // ONLY IF the direct path is shorter than last calculation, update it
                            // (A direct path to source does NOT guarantee shortest distance!)
                            if (adjacentVertex.GetDistanceToPrevious() < currentRow.GetDistanceFromStart())
                            {
                                // Update with the new shorter distance, and the new previous vertex
                                currentRow.SetPreviousVertex(_sourceVertexKey);
                                currentRow.SetDistanceFromStart(adjacentVertex.GetDistanceToPrevious());
                                currentRow.SetDistanceFromPrevious(adjacentVertex.GetDistanceToPrevious());
                                // Now we need to make sure any previous rows
                                // that have this one as an adjacent vertex, are updated AGAIN
                                // In case this latest update creates a shorter path back to source
                                foreach (KeyValuePair<string, Vertex> entry in _adjencyDictionary)
                                {
                                    foreach (AdjacentVertex adj in entry.Value.GetAdjacent())
                                    {
                                        if (adj.GetKey() == currentKey)
                                        {
                                            DijkstraTableRow row = _dijkstraTable.GetRow(entry.Key);
                                            int newDistanceToSource = adj.GetDistanceToPrevious() +
                                                                      currentRow.GetDistanceFromStart();
                                            // If this row's distance to source is more than 
                                            // this row's distance to current vertex 
                                            // PLUS it's distance to source, update the DijkstraTableRow
                                            if (newDistanceToSource < row.GetDistanceFromStart())
                                            {
                                                row.SetPreviousVertex(currentRow.GetKey());
                                                row.SetDistanceFromPrevious(adj.GetDistanceToPrevious());
                                                row.SetDistanceFromStart(newDistanceToSource);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If here, the adjacent row is not our source row (first vertex in AddEdge)
                            // Try to calculate the distance back to source,
                            // and if it is less than the current entry for that table row update it
                            if (isSourceRow)
                            {
                                // We are at the source or 'A' row
                                // That means the child adjacent vertexes have a direct link back to the source
                                // So update their distance as the diff between the two
                                // Again, ONLY IF less than int.MaxInt or a previous value
                                if (adjacentVertex.GetDistanceToPrevious() < adjacentRow.GetDistanceFromStart())
                                {
                                    adjacentRow.SetPreviousVertex(vertexEntry.Key);
                                    adjacentRow.SetDistanceFromStart(adjacentVertex.GetDistanceToPrevious());
                                    adjacentRow.SetDistanceFromPrevious(adjacentVertex.GetDistanceToPrevious());
                                }
                            }
                            else
                            {
                                // If here, the **current** row in the table is not the source vertex
                                // If the row already has a link back to source, only update if necessary
                                if (adjacentRow.GetPreviousVertex() != _sourceVertexKey)
                                {
                                    // If here, the **adjacent** child to the current row, is NOT the source row
                                    // Meaning we are comparing two rows that are no the source
                                    // Get the total distance of adjacent to this row, plus this row's distance to source
                                    int newDistanceToSource = currentRow.GetDistanceFromStart() != int.MaxValue
                                        ? currentRow.GetDistanceFromStart() +
                                          adjacentVertex.GetDistanceToPrevious()
                                        : int.MaxValue;
                                    // If the new value using this combo results in a shorter path to source
                                    // for the adjacent row, update it
                                    if (newDistanceToSource < adjacentRow.GetDistanceFromStart())
                                    {
                                        adjacentRow.SetDistanceFromStart(newDistanceToSource);
                                        adjacentRow.SetPreviousVertex(vertexEntry.Key);
                                        adjacentRow.SetDistanceFromPrevious(adjacentVertex.GetDistanceToPrevious());
                                    }
                                }
                                else
                                {
                                    // If here, the adjacent row IS the source table row
                                    // If distance from this element to the adjacent element 
                                    // back to source, is less than current, then update.
                                    int existingDistanceToSource = currentRow.GetDistanceFromStart() != int.MaxValue
                                        ? currentRow.GetDistanceFromStart()
                                        : int.MaxValue;
                                    int newDistanceToSource = currentRow.GetDistanceFromStart() != int.MaxValue
                                        ? currentRow.GetDistanceFromStart() +
                                          adjacentRow.GetDistanceFromPrevious()
                                        : int.MaxValue;

                                    if (adjacentRow.GetDistanceFromStart() == int.MaxValue)
                                    {
                                        adjacentRow.SetDistanceFromStart(newDistanceToSource);
                                        adjacentRow.SetPreviousVertex(currentKey);
                                    }
                                    else if (newDistanceToSource < adjacentRow.GetDistanceFromStart())
                                    {
                                        // Update this row with the new path and distance
                                        adjacentRow.SetDistanceFromStart(newDistanceToSource);
                                        adjacentRow.SetPreviousVertex(currentKey);
                                    }

                                    // try to get the path back to the source
                                    if (existingDistanceToSource < int.MaxValue)
                                    {
                                        adjacentRow.SetPreviousVertex(vertexEntry.Key);
                                        adjacentRow.SetDistanceFromPrevious(adjacentVertex.GetDistanceToPrevious());
                                        int totalWeightToStart = adjacentVertex.GetDistanceToPrevious() +
                                                                 currentRow.GetDistanceFromStart();
                                        if (totalWeightToStart < adjacentRow.GetDistanceFromStart())
                                        {
                                            adjacentRow.SetDistanceFromStart(totalWeightToStart);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}