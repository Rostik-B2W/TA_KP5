using System;
using System.Diagnostics;

class TA7
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Left { get; set; }

        public Node(T data) => Data = data;
    }

    public class BinaryTree
    {
        private Node<int> root;
        private string path;

        public bool Add(Node<int> node)
        {
            Node<int> current = null, next = root;
            while (next != null)
            {
                current = next;
                next = node.Data < next.Data ? next.Left : node.Data > next.Data ? next.Right : null;
                if (next == null && node.Data == current.Data) return false;
            }

            if (root == null)
                root = node;
            else if (node.Data < current.Data)
                current.Left = node;
            else
                current.Right = node;

            return true;
        }

        public void Find(int value)
        {
            path = "";
            Find(value, root);
        }

        private Node<int> Find(int value, Node<int> parent)
        {
            if (parent != null)
            {
                path += $"{parent.Data}\t";
                if (value == parent.Data)
                {
                    Console.WriteLine(path);
                    return parent;
                }
                return value < parent.Data ? Find(value, parent.Left) : Find(value, parent.Right);
            }

            Add(new Node<int>(value));
            path += $"{value}\t";
            Console.WriteLine(path);
            return null;
        }

        public void ShowTree() => Console.WriteLine(Show(root) + "\n");

        private string Show(Node<int> node)
        {
            if (node == null) return "";
            if (node.Left == null && node.Right == null) return $"{node.Data}";
            return $"{node.Data}:({Show(node.Left)};{Show(node.Right)})";
        }
    }

    public class AVL
    {
        private Node<int> root;
        private string path;

        public void Add(int data)
        {
            Node<int> newItem = new Node<int>(data);
            root = root == null ? newItem : RecursiveInsert(root, newItem);
        }

        private Node<int> RecursiveInsert(Node<int> current, Node<int> node)
        {
            if (current == null) return node;
            if (node.Data < current.Data)
                current.Left = RecursiveInsert(current.Left, node);
            else if (node.Data > current.Data)
                current.Right = RecursiveInsert(current.Right, node);

            return BalanceTree(current);
        }

        private Node<int> BalanceTree(Node<int> current)
        {
            int bFactor = BalanceFactor(current);

            if (bFactor > 1)
                current = BalanceFactor(current.Left) > 0 ? RotateLL(current) : RotateLR(current);
            else if (bFactor < -1)
                current = BalanceFactor(current.Right) > 0 ? RotateRL(current) : RotateRR(current);

            return current;
        }

        public void Find(int value)
        {
            path = "";
            Find(value, root);
        }

        private Node<int> Find(int value, Node<int> parent)
        {
            if (parent != null)
            {
                path += $"{parent.Data}\t";
                if (value == parent.Data)
                {
                    Console.WriteLine(path);
                    return parent;
                }
                return value < parent.Data ? Find(value, parent.Left) : Find(value, parent.Right);
            }

            Add(value);
            path += $"{value}\t";
            Console.WriteLine(path);
            return null;
        }

        private int GetHeight(Node<int> current) => current == null ? 0 : 1 + Math.Max(GetHeight(current.Left), GetHeight(current.Right));

        private int BalanceFactor(Node<int> current) => GetHeight(current.Left) - GetHeight(current.Right);

        private Node<int> RotateLL(Node<int> parent)
        {
            Node<int> pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }

        private Node<int> RotateRR(Node<int> parent)
        {
            Node<int> pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }

        private Node<int> RotateLR(Node<int> parent)
        {
            parent.Left = RotateRR(parent.Left);
            return RotateLL(parent);
        }

        private Node<int> RotateRL(Node<int> parent)
        {
            parent.Right = RotateLL(parent.Right);
            return RotateRR(parent);
        }

        public void ShowTree() => Console.WriteLine(Show(root) + "\n");

        private string Show(Node<int> node)
        {
            if (node == null) return "";
            if (node.Left == null && node.Right == null) return $"{node.Data}";
            return $"{node.Data}:({Show(node.Left)};{Show(node.Right)})";
        }

        public void Delete(int target) => root = Delete(root, target);

        private Node<int> Delete(Node<int> current, int target)
        {
            if (current == null) return null;

            if (target < current.Data)
                current.Left = Delete(current.Left, target);
            else if (target > current.Data)
                current.Right = Delete(current.Right, target);
            else
            {
                if (current.Right != null)
                {
                    Node<int> min = current.Right;
                    while (min.Left != null) min = min.Left;
                    current.Data = min.Data;
                    current.Right = Delete(current.Right, min.Data);
                }
                else return current.Left;
            }

            return BalanceTree(current);
        }
    }

    static void Main()
    {
        try
        {
            BinaryTree tree = new();
            int[] values = { 16, 789, 300, 1, 2, 88, 50, 26, 24 };
            foreach (int v in values) tree.Add(new Node<int>(v));

            int power = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Before");
            tree.ShowTree();

            Stopwatch timer = Stopwatch.StartNew();
            tree.Find(power);
            timer.Stop();
            Console.WriteLine(timer.Elapsed);

            Console.WriteLine("After");
            tree.ShowTree();
            Console.WriteLine("------------------------------------------------------------------------");

            AVL tree1 = new();
            foreach (int v in values) tree1.Add(v);

            Console.WriteLine("Before");
            tree1.ShowTree();

            Stopwatch timer1 = Stopwatch.StartNew();
            tree1.Find(power);
            timer1.Stop();
            Console.WriteLine(timer1.Elapsed);

            Console.WriteLine("After");
            tree1.ShowTree();
        }
        catch
        {
            Console.WriteLine("Помилка введених даних");
        }
    }
}
