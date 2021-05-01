// MAAT 2020/2021
// Code Written by : Jonathan Richard

using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

class node
{
    public static List<node> nodes = new List<node>();
    public string Character;
    public string Huff;
    public node Left;
    public node Right;
    public bool End;
    public int level;
    public node(string character, string huff, bool end = false, node left = null, node right = null )
    {
        Character = character;
        Huff = huff;
        End = end;
        Left = left;
        Right = right;
        level = huff.Length;
    }
}
class Program
{
  
    static BitArray readFile()
    {
        byte[] data = File.ReadAllBytes(@"D:\MAAT\Huffman\compressed.bin");
        BitArray codewords = new BitArray(data);
        return codewords;
    }

    static void readTable()
    {
        StreamReader sr = new StreamReader(@"D:\MAAT\Huffman\table.txt");
        int Length = int.Parse(sr.ReadLine());
        string[] character = new string[Length];
        
        for(int i= 0; i < Length; i++){
            character[i] = sr.ReadLine();
        }
        for(int i= 0; i <Length; i++)
        {
            node.nodes.Add(new node(character[i], sr.ReadLine(), true));
        }
    }

    static string decode(BitArray compressed)
    {
        string original = "";
        node current = node.nodes[0];
        foreach(bool a in compressed)
        {
            if (a == false)
                current = current.Left;
            if (a == true)
                current = current.Right;
            if (current.End)
            {
                original += current.Character;
                current = node.nodes[0];
            }
                

        }
        return original;
    }
    static void generateTable()
    {
        node Left = null;
        node Right = null;
        while (node.nodes.Count > 1)
        {
            
            node.nodes.Sort((x, y) => y.level.CompareTo(x.level));
            int i = 0;
            string k = node.nodes[0].Huff.Remove(node.nodes[0].level - 1);
            char x = node.nodes[0].Huff[node.nodes[i].level - 1];
            for (i = 1; i < node.nodes.Count; i++)
            {
                if (k == node.nodes[i].Huff.Remove(node.nodes[i].level - 1)  )
                {
                    switch (x)
                    {
                        case ('0'):
                            Left = node.nodes[0];
                            Right = node.nodes[i];
                            break;
                        case ('1'):
                            Left = node.nodes[i];
                            Right = node.nodes[0];
                            break;
                    }
                    break;
                }


            }
            node.nodes.Add(new node("", k, left: Left, right: Right));
            node.nodes.Remove(Left);
            node.nodes.Remove(Right);

        }
    }
        

    static void Main()
    {
        BitArray Compressed = readFile();
        readTable();
        generateTable();
        Console.Write(decode(Compressed));

    }
}
