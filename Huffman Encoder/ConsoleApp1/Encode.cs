using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

class node
{
    static public List<node> nodes = new List<node>();
    public string characters;
    public int frequency;
    public node left;
    public node right;
    public String huff;
    public node(string Character, int Frequency, node Left = null  , node Right = null )
    {
        characters = Character;
        frequency = Frequency;
        left = Left;
        right = Right;
        huff = "";
    }

}
class Encode
{
    static Dictionary<string, string> huffmanTable = new Dictionary<string, string>();
    static Dictionary<string,int> calculateFrequency(string original)
    {
        Dictionary<string, int> table = new Dictionary<string, int>();
        foreach(char x in original)
        {
            if (table.ContainsKey(x.ToString()))
            {
                table[x.ToString()]++;
            }
            else
            {
                table.Add(x.ToString(), 1);
            }
            
        }
        return table;
    }
    static void encode(string original)
    {
        string codeword = "";
        foreach (char c in original)
        {
            codeword += huffmanTable[c.ToString()];
        }
        
        int x = codeword.Length;
        int i = 0;
        if (x % 8 != 0)
        {
            for(i =0; i < (x % 8); i++)
            {
                codeword += "0";
                x++;
            }
            
        }

        i = 0;
        BitArray codewordbin = new BitArray(x);
        string binLoc = @"D:\MAAT\Huffman\compressed.bin";    
        byte[] codes = new byte[x/8];
        FileStream bin = new FileStream(binLoc,FileMode.Create,FileAccess.Write);
        foreach (char code in codeword)
        {

            if (code.ToString() == "0")
            {
                codewordbin[i] = false;
            }
            if (code.ToString() == "1")
            {
                codewordbin[i] = true;
            }
            i++;
        }
        codewordbin.CopyTo(codes, 0);
        bin.Write(codes);
        bin.Close();
        Console.WriteLine("Original" + original);


    }
    static void generateTable(node a,string huff ="")
    {
        string Value = huff + a.huff;
        if(a.left != null)
        {
            generateTable(a.left, Value);
        }
        if (a.right != null)
        {
            generateTable(a.right, Value);
        }
        if(a.left == null && a.right == null)
        {
            huffmanTable.Add(a.characters, Value);
        }
        

    }
    static void exportTable()
    {
        string location = @"D:\MAAT\Huffman\table.txt";
        StreamWriter huffman = new StreamWriter(location);
        huffman.Write(huffmanTable.Count.ToString()+"\n");
        foreach(string c in huffmanTable.Keys)
        {
            huffman.Write(c+"\n");
        }
        foreach(string c in huffmanTable.Values)
        {
            huffman.Write(c+"\n");
        }
        huffman.Close();

    }
    static string openFile()
    {
        string location = @"D:\MAAT\Huffman\C#\Huffman Encoder\ConsoleApp1\string.txt";
        string original = File.ReadAllText(location);
        return original;
    }
    static void Main(string[] args)
    {
        string original = openFile();
        Dictionary<string, int> frequencies = calculateFrequency(original);
        foreach(KeyValuePair<string,int> kvp in frequencies)
        {
            node.nodes.Add(new node(kvp.Key, kvp.Value));
        }
        while(node.nodes.Count > 1)
        {
            node.nodes.Sort((x, y) => x.frequency.CompareTo(y.frequency));       
            node Left = node.nodes[0];
            node Right = node.nodes[1];
            Left.huff += '0';
            Right.huff += '1';
            node.nodes.Remove(Left);
            node.nodes.Remove(Right);
            node.nodes.Add(new node("", Left.frequency + Right.frequency, Left, Right));

        }

        generateTable(node.nodes[0]);
        encode(original);
        exportTable();

    }
}
