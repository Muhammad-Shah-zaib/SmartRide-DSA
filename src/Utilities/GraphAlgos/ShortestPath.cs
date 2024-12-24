using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRide.src.DataStructures;
namespace SmartRide.src.Utilities.GraphAlgos
{

    public class ShortestPath<T> where T : IComparable<T>
    {
        public List<T> Dijsktra(Graph<T> adjacencyList, T start, T  goal)
        {
            var distances = new HashMap<T, double>();
            var Nodes = new HashMap<T,T>();
            var priorityQueue = new PriorityQueues<T>();
            var path = new List<T>();


            //looking for the starting and the goal node in the graph
            if (!adjacencyList.ContainsNode(start) || !adjacencyList.ContainsNode(goal))
                throw new ArgumentException("Start or goal node is not present in the graph.");



            foreach (var vertex in adjacencyList.GetAllNodes())
            {
                distances.Put(vertex,double.MaxValue);
                
            }
            distances.Put(start, 0);
            //set the distance of the starting node to zero
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0) { 
            
                var (currentNode, currentDistance) = priorityQueue.Dequeue();


                //goal is reached
                if(EqualityComparer<T>.Default.Equals(currentNode, goal))
                {
                    //highlight the path followed
                    path = makepath(Nodes,start,goal);
                    break;
                }

                

                foreach (var neighbor in adjacencyList.GetNeighbors(currentNode))
                {
                    var node = neighbor.Item1;
                    var weight = neighbor.Item2;
                    var new_distance = currentDistance + weight;

                    if(new_distance < distances.Get(node))
                    {
                        distances.Put(node, new_distance);
                        Nodes.Put(node, currentNode);
                        priorityQueue.Enqueue(node, new_distance);
                    }

                }
            }
            return path;
        }

        private List<T> makepath(HashMap<T,T> visited,T start,T goal)
        {   

            var path = new List<T>();
            var current = goal;
            
            while (!EqualityComparer<T>.Default.Equals(current, start))
            {
                path.Add(current);
                current = visited.Get(current);

            }

            path.Add(start); // Add the start node at the end
            path.Reverse(); // Reverse to get the path from start to goal
            return path;
        }
    }
}
