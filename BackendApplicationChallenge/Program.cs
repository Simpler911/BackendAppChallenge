using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using StreamReader streamReader = new StreamReader("input.txt");

Node[] previous = null;
Node root = null;
for (int lineNumber = 0; !streamReader.EndOfStream; lineNumber++)
{
    Node[] nodes = Regex.Matches(streamReader.ReadLine(), @"\d+").Select(m => new Node(int.Parse(m.Value), new())).ToArray();
    if (lineNumber == 0)
    {
        root = nodes[0];
    }
    else
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i < nodes.Length - 1)
            {
                previous[i].Children.Add(nodes[i]);
            }
            if (i > 0)
            {
                previous[i - 1].Children.Add(nodes[i]);
            }
        }
    }
    previous = nodes;
}

List<(List<Node> Path, int Value)> paths = new();

void TraverseGraphAndBuildPaths(Node node, List<Node> path)
{
    path.Add(node);
    if (node.Children.Count is 0)
    {
        bool isCorrectPath = true;
        bool[] evens = path.Select(n => n.Value % 2 == 0).ToArray();
        for (int i = 0; i < path.Count - 1; i++)
        {
            if (evens[i] == evens[i + 1])
                isCorrectPath = false;
        }
        if (isCorrectPath)
            paths.Add((path, path.Select(n => n.Value).Sum()));
    }
    else
    {
        foreach (Node child in node.Children)
        {
            TraverseGraphAndBuildPaths(child, new(path));
        }
    }
}

TraverseGraphAndBuildPaths(root, new());

int indexMax = !paths.Any() ? Int32.MinValue : paths
    .Select((value, index) => new { Value = value, Index = index })
    .Aggregate((a, b) => (a.Value.Value > b.Value.Value) ? a : b)
    .Index;
if (indexMax != Int32.MinValue)
{
    Console.WriteLine("Max sum: " + paths[indexMax].Value);
    Console.WriteLine("Path: " + string.Join(", ", paths[indexMax].Path.Select(n => n.Value)));
    //OUTPUT:
    //Max sum: 8186
    //Path: 215, 192, 269, 836, 805, 728, 433, 528, 863, 632, 931, 778, 413, 310, 253
}


public record Node(int Value, List<Node> Children);