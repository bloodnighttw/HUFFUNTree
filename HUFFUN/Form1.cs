using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HUFFUN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var compare = Comparer<HuffunTree>.Create((left, right) => left.Count - right.Count);
            List<HuffunTree> set = new List<HuffunTree>();
            var count = new int[256];
            foreach (var i in textBox1.Text.Where(i => i != '\0'))
            {
                count[i]++;
            }

            for (int i = 0; i < 256; i++)
            {
                if (count[i] == 0) continue;
                var temp = new HuffunTree(count[i], (char) i);
                set.Add(temp);
            }
            
            while (set.Count > 1)
            {
                set.Sort(compare);
                var a = set[0];
                var b = set[1];
                set.RemoveAt(0);
                set.RemoveAt(0);

                set.Add(new HuffunTree(a.Count+b.Count,a,b));
                
            }

            var top = set.Min();
            var dict = new SortedDictionary<char, string>();
            top.getMap(ref dict);

            textBox2.Text = "";
            foreach (var i in dict)
            {
                textBox2.Text += i.Key + "\t" + count[i.Key] + "\t" + i.Value+"\r\n";
            }

            textBox3.Text = "";
            foreach (var i in textBox1.Text)
            {
                textBox3.Text += dict[i];
            }

            label1.Text = (double)textBox3.Text.Length+ "bits/"+textBox1.Text.Length*8+"bit=" +(double)textBox3.Text.Length*100/
                (textBox1.Text.Length*8) + "%";


        }
    }
    
    class HuffunTree
    {
        public bool Leaf { get; }
        internal readonly HuffunTree? _left;
        internal readonly HuffunTree? _right ;
        public readonly int Count;
        public readonly char Character;
        

        public HuffunTree(int count,HuffunTree left,HuffunTree right)
        {
            this.Count = count;
            Character = '\0';
            Leaf = false;
            _left = left;
            _right = right;
        }

        public HuffunTree(int count, char character)
        {
            this.Count = count;
            this.Character = character;
            Leaf = true;
        }

        public void getMap(ref SortedDictionary<char, string> m,string args="")
        {
            if (this.Leaf)
            {
                m.Add(Character,args);
            }
            _left?.getMap(ref m,args+"0");
            _right?.getMap(ref m,args+"1");
        }


        protected bool Equals(HuffunTree other)
        {
            if (this._left != other._left || this._right != other._right || Character != other.Character || Count != other.Count && Leaf != other.Leaf)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HuffunTree) obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    
}