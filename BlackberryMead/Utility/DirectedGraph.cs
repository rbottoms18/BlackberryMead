#nullable enable
using System;
using System.Collections.Generic;

namespace BlackberryMead.Utility
{
    public class DirectedGraph<T> where T : notnull
    {
        /// <summary>
        /// Set of graph verteces
        /// </summary>
        public List<T> VertexSet { get; protected set; }

        /// <summary>
        /// Set of graph edges
        /// </summary>
        public Dictionary<T, List<T>> EdgeSet { get; protected set; }

        /// <summary>
        /// Entry point of the graph
        /// </summary>
        public T Root { get; protected set; }

        /// <summary>
        /// Current vertex in the graph
        /// </summary>
        public T CurrentVertex { get; protected set; }

        public bool CurrentVertexHasEdges { get { return GetEdges(CurrentVertex).Count > 0; } }

        /// <summary>
        /// Creates a blank directed graph. Root element must be manually set.
        /// </summary>
        public DirectedGraph()
        {
            VertexSet = new List<T>();
            EdgeSet = new Dictionary<T, List<T>>();
        }

        public DirectedGraph(List<T> vertices)
        {
            VertexSet = vertices;
            EdgeSet = new Dictionary<T, List<T>>();
            Root = VertexSet[0];
            CurrentVertex = Root;
        }

        public DirectedGraph(List<T> vertices, Dictionary<T, List<T>> edges)
        {
            VertexSet = vertices;
            EdgeSet = edges;
            Root = VertexSet[0];
            CurrentVertex = Root;
        }

        /// <summary>
        /// Sets the Root vertex of the graph to the specified vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetRoot(T vertex)
        {
            if (VertexSet.Contains(vertex))
            {
                Root = vertex;
                return;
            }
            throw new InvalidOperationException("VertexSet does not contain value.");
        }

        /// <summary>
        /// Sets the Root vertex of the graph to the vertex with the specified index
        /// </summary>
        /// <param name="vertexIndex"></param>
        public void SetRoot(int vertexIndex)
        {
            if (vertexIndex < VertexSet.Count)
            {
                Root = VertexSet[vertexIndex];
                return;
            }
            throw new IndexOutOfRangeException("VertexSet does not contain value.");
        }

        /// <summary>
        /// Gets the edges of the specified Index
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<T> GetEdges(T vertex)
        {
            // Search the EdgeSet dictionary for all edges corresponding to 'vertex'
            if (EdgeSet.TryGetValue(vertex, out List<T>? edges))
            {
                if (edges != null)
                {
                    return edges;
                }
            }
            return new List<T>();
        }
        /// <summary>
        /// Adds a vertex to the graph
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertex(T vertex)
        {
            VertexSet.Add(vertex);
        }

        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(T parentVertex, T childVertex)
        {
            if (!VertexSet.Contains(parentVertex))
            {
                throw new InvalidOperationException("VertexSet does not contain value.");
            }

            // Get the edge set for the vertex (edge.Value1)
            List<T> vertexEdges = GetEdges(parentVertex);

            // If the edge does not already exist
            if (!vertexEdges.Contains(childVertex))
            {
                vertexEdges.Add(childVertex);

                // Reset vertex edges in dictionary
                EdgeSet.Remove(parentVertex);
                EdgeSet.Add(parentVertex, vertexEdges);
            }
        }

        /// <summary>
        /// Sets the set of verteces of the graph.
        /// </summary>
        /// <param name="verteces"></param>
        public void SetVerteces(List<T> verteces)
        {
            VertexSet = verteces;
        }

        /// <summary>
        /// Proceeds to the next vertex in the graph. If multiple edges exist,
        /// the graph will proceed to the edge with the lowest index
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            return Next(0);
        }

        /// <summary>
        /// Proceeds the graph to the next vertex along the edge with the specified index
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Next(int edgeIndex)
        {
            // get edges of CurrentVertex
            List<T> edges = GetEdges(CurrentVertex);
            if (edges == null || edges.Count == 0)
            {
                throw new InvalidOperationException("Vertex has no edges.");
            }

            CurrentVertex = edges[edgeIndex];
            return CurrentVertex;
        }

        /// <summary>
        /// Progresses the graph to the specified vertex
        /// </summary>
        /// <param name="nextVertex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Next(T nextVertex)
        {
            // get edges of CurrentVertex
            List<T> edges = GetEdges(CurrentVertex);
            if (edges == null)
            {
                throw new InvalidOperationException("Vertex has contains no edges.");
            }
            else if (!edges.Contains(nextVertex))
            {
                throw new InvalidOperationException("Vertex does not contain specified edge.");
            }

            CurrentVertex = nextVertex;
            return CurrentVertex;
        }

        /// <summary>
        /// Restarts the graph at its Root
        /// </summary>
        public void Reset()
        {
            CurrentVertex = Root;
        }
    }
}
