# DynamicConnectivity

C# implementation of dynamic connectivity based on [javascript implementation](https://github.com/mikolalysenko/dynamic-forest) by mikolalysenko.

# Example
```csharp
var dc = new DynamicConnectivity(5);
dc.Link(1, 2);
dc.Link(0, 1);
dc.Link(3, 2);
dc.Link(0, 4);
dc.Link(4, 3);
Console.WriteLine(dc.IsConnected(0, 3));
dc.Cut(2, 1);
Console.WriteLine(dc.IsConnected(0, 3));
dc.Cut(4, 3);
Console.WriteLine(dc.IsConnected(0, 3));
foreach (var a in dc.GetConnectedNodes(0))
{
    Console.WriteLine(a);
}
```

When the above code is run, the output is as follows.

```
True
True
False
1
0
4
```

Currently, multiple edges are prohibited.


# Usage

## `var dc = new DynamicConnectivity(n);`

Creates a undirected graph with n vertices.

*Complexity* `O(n)`.

## `dc.Link(i, j);`

Creates an edge between i and j.

*Complexity* `O(log^2 n)`.

## `dc.Cut(i, j);`

Cuts an edge between i and j.

*Complexity* `O(log^2 n)`

## `dc.IsConnected(i, j)`

Returns if i and j are connected by a path or not.

*Complexity* `O(log n)`

---

Implementation reference

https://github.com/mikolalysenko/dynamic-forest

(c) 2013 Mikola Lysenko.

---

License

(c) 2017 Kazunori Tamura. MIT License
