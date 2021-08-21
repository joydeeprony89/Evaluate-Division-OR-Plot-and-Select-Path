using System;
using System.Collections.Generic;

namespace Evaluate_Division_OR_Plot_and_Select_Path
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<IList<string>> queries = new List<IList<string>>();
            IList<IList<string>> equations = new List<IList<string>>();
            List<string> equation1 = new List<string>();
            equation1.Add("x1");
            equation1.Add("x2");

            List<string> equation2 = new List<string>();
            equation2.Add("x2");
            equation2.Add("x3");

            List<string> equation3 = new List<string>();
            equation3.Add("x3");
            equation3.Add("x4");

            List<string> equation4 = new List<string>();
            equation4.Add("x4");
            equation4.Add("x5");

            equations.Add(equation1);
            equations.Add(equation2);
            equations.Add(equation3);
            equations.Add(equation4);
            double[] values = new double[] { 3.0, 4.0, 5.0, 6.0 };
            List<string> query1 = new List<string>();
            query1.Add("x1");
            query1.Add("x5");

            List<string> query2 = new List<string>();
            query2.Add("x5");
            query2.Add("x2");

            List<string> query3 = new List<string>();
            query3.Add("x2");
            query3.Add("x4");

            List<string> query4 = new List<string>();
            query4.Add("x2");
            query4.Add("x2");

            List<string> query5 = new List<string>();
            query5.Add("x2");
            query5.Add("x9");

            List<string> query6 = new List<string>();
            query5.Add("x9");
            query5.Add("x9");

            queries.Add(query1);
            queries.Add(query2);
            queries.Add(query3);
            queries.Add(query4);
            queries.Add(query5);
            queries.Add(query6);

            Program p = new Program();
            Console.WriteLine(string.Join(",", p.CalcEquation(equations,values,queries)));
        }


        public class Node
        {
            public string destination;
            public double cost;
            public Node(string destination, double cost)
            {
                this.destination = destination;
                this.cost = cost;
            }
        }

        public double[] CalcEquation(IList<IList<string>> equations, double[] values, IList<IList<string>> queries)
        {
            double[] results = new double[queries.Count];
            // 1. Create a graph from the equations and values
            Dictionary<string, List<Node>> graph = CreateGraph(equations, values);
            // 2. search each queries against the constructed graph using dfs approach.
            for (int i = 0; i < queries.Count; i++)
            {
                var query = queries[i];
                string source = query[0];
                string destination  = query[1];
                results[i] = DFS(source, destination, new HashSet<string>(), graph);
            }

            return results;
        }

        private double DFS(string source, string destination, HashSet<string> visited, Dictionary<string, List<Node>> graph)
        {
            double answer = -1.0;
            // base conditions
            if (!graph.ContainsKey(source) || !graph.ContainsKey(destination))
                return answer;
            if (source.Equals(destination))
                return 1.0;
            // start DFS
            visited.Add(source);
            var connectedPaths = graph[source];
            foreach(Node path in connectedPaths)
            {
                string newSource = path.destination;
                if (!visited.Contains(newSource))
                {
                    double cost = DFS(newSource, destination, visited, graph);
                    if (cost != answer)
                        return cost * path.cost;
                }
            }

            return answer;
        }

        private Dictionary<string, List<Node>> CreateGraph(IList<IList<string>> equations, double[] values)
        {
            Dictionary<string, List<Node>> graph = new Dictionary<string, List<Node>>();
            for (int i = 0; i < values.Length; i++)
            {
                double value = values[i];
                var equation = equations[i];
                string src = equation[0];
                string destination = equation[1];
                if (!graph.ContainsKey(src))
                    graph.Add(src, new List<Node>());

                if (!graph.ContainsKey(destination))
                    graph.Add(destination, new List<Node>());

                graph[src].Add(new Node(destination, value));
                graph[destination].Add(new Node(src, 1 / value));
            }

            return graph;
        }
    }
}
