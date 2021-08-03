using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

// Sani Katan -MJ_bis ende Ferien
namespace C_Sharp_memory_game
{
    public partial class Form1 : Form
    {
        List<gRecht> RechtanglesList = null;
        Random random = null;
        int rowsize = 4;
        int squareHeight = 150;
        int squareWidth = 150;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Xml xml = new Xml("cards.xml");
            random = new Random();
            RechtanglesList = new List<gRecht>();
            int countCards = xml.CountCards();

            for (int i = 0; i < countCards * 2; i += 2)
            {
                gRecht recht = new gRecht();
                recht.SetValue(i / 2);
                RechtanglesList.Add(recht);

                recht = new gRecht(); 
                recht.SetValue(i / 2); 
                RechtanglesList.Add(recht);
            }

            int columnsize = (RechtanglesList.Count - 1) / rowsize + 2;
            for (int i = 0; i < RechtanglesList.Count; i++)
            {
                RechtanglesList.ElementAt(i).width = 150;
                RechtanglesList.ElementAt(i).height = 100;
                RechtanglesList.ElementAt(i).x1 = (this.Width / rowsize) * (i % rowsize) + 15;   // 1/4 ostanek je 1 2/4 je ostanek 2....4/4 je ostanek 0 zato gre v naslednjo vrstico
                RechtanglesList.ElementAt(i).y1 = (this.Height / columnsize) * (i / rowsize);
            }
            Refresh();

            button1.Left = 15;
            button1.Top = this.Height - button1.Height - 55;
        }
        public class Xml
        { 
            public XmlDocument doc = new XmlDocument();
            public Xml(string name)
            {
                string currentDirectory = Directory.GetCurrentDirectory() + "/" + name;
                doc.Load(currentDirectory);
            }

            public int CountCards()
            {
                int cnt = 0;

                foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("card"))
                {
                    cnt++;
                }

                return cnt;
            }
            public List<string> Cards()
            {
                List<string> cardnames = new List<string>();
                foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("card"))
                {
                    foreach (XmlNode names in node.ChildNodes)
                    {
                        XmlNode name = names.ChildNodes[0];
                        cardnames.Add(@"C:/Users/Pirkuleee/source/repos/C-Sharp_memory_game/C-Sharp_memory_game/bin/Debug/Bilder/" + name.InnerText);
                    }
                }
                return cardnames;
            }

        }

        public class gRecht {

            public int x1;
            public int y1;
            public int height;
            public int width;
            public double Wert;
            public int value;
            public int pictureID;

            public void SetValue(int pictureID)
            {
                this.pictureID = pictureID;
            }

            public virtual void Draw(Graphics g)
            {
                Rectangle rect = new Rectangle(x1, y1, width, height);
                g.DrawRectangle(new Pen(Color.Black), rect);
                SolidBrush brush = new SolidBrush(Color.Black);

                Font font = new Font("Lucida Console", 22);

                g.DrawString(pictureID.ToString(), font, brush, rect);

            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < RechtanglesList.Count; i++)
            {
                gRecht o = RechtanglesList.ElementAt(i);
                o.Draw(e.Graphics);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
                for (int i = 0; i < RechtanglesList.Count; i++)
                {
                    int random1 = random.Next(0, RechtanglesList.Count );    
                    int random2 = random.Next(0, RechtanglesList.Count );

                    int rect = RechtanglesList.ElementAt(random1).pictureID;
                    RechtanglesList.ElementAt(random1).pictureID = RechtanglesList.ElementAt(random2).pictureID;
                    RechtanglesList.ElementAt(random2).pictureID = rect;
                }
            drawimage();
            //this.Refresh();
        }
        public void drawimage() {

            Xml xml = new Xml("cards.xml");
            int countCards = xml.CountCards();
            List<string> names = xml.Cards();
            Graphics g = this.CreateGraphics();

            for (int i = 0; i < countCards*2; i++)
            {
                g.DrawImage(Image.FromFile(names.ElementAt(RechtanglesList.ElementAt(i).pictureID)), RechtanglesList.ElementAt(i).x1, RechtanglesList.ElementAt(i).y1, RechtanglesList.ElementAt(i).width, RechtanglesList.ElementAt(i).height);
                //g.DrawImage(Image.FromFile("path",x,y,width, height);
            }
            g.Dispose();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int columnsize = (RechtanglesList.Count - 1) / rowsize + 2;
            int height = this.Height;
            int width = this.Width;
            int padding = 15;
            if (width < (squareWidth + padding + padding))
                this.Width = (squareWidth + padding + padding + padding);
            width = this.Width;
            rowsize = (width) / (squareWidth+(2*padding));
            Console.WriteLine(rowsize);




            for (int i = 0; i < RechtanglesList.Count; i++)
            {
                try
                {
                    RechtanglesList.ElementAt(i).width = 150;
                    RechtanglesList.ElementAt(i).height = 100;
                    RechtanglesList.ElementAt(i).x1 = padding + (squareWidth + padding) * (i % rowsize);   // 1/4 ostanek je 1 2/4 je ostanek 2....4/4 je ostanek 0 zato gre v naslednjo vrstico
                                                                                                           //RechtanglesList.ElementAt(i).y1 = (this.Height / rowsize) * (i / rowsize); 1st WAY
                    RechtanglesList.ElementAt(i).y1 = padding + (squareHeight + padding) * (i / rowsize); //2nd WAY
                }
                catch (System.DivideByZeroException err)
                {
                    RechtanglesList.ElementAt(i).x1 = padding + (squareWidth + padding);
                }
            }
            //SET HEIGHT AUTOMATIC
            int rows = RechtanglesList.Count / rowsize;
            int rowsConfirm = RechtanglesList.Count % rowsize;
            if (rowsConfirm != 0)
                rows++;
            this.Height = padding + (squareHeight + padding) * ( rows) + 55;

            
            Refresh();
            button1.Left = 15;
            button1.Top = this.Height - button1.Height - 55;
        }
    }
}
