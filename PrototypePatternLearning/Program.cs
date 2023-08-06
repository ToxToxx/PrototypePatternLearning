using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrototypePatternLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Circle figure = new Circle(30, 50, 60);
            // применяем глубокое копирование
            Circle clonedFigure = figure.Clone() as Circle;
            figure.Point.X = 100;
            figure.GetInfo();
            clonedFigure.GetInfo();

            Console.WriteLine(new string('-',25)); ;

            Rectangle figure2 = new Rectangle(20, 40);
            Rectangle clonedFigure2 = figure2.DeepCopy() as Rectangle;
            figure2.GetInfo();
            clonedFigure2.GetInfo();
            figure2.SetWidth(30);
            figure2.GetInfo();
            clonedFigure2.GetInfo();
        }
    }

    //prototype interface for cloning
    interface IFigure
    {
        IFigure Clone();
        void GetInfo();
    }


    [Serializable]
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    //Concrete prototype 1
    [Serializable]
    class Circle : IFigure
    {
        private int radius;
        public Point Point { get; set; }
        public Circle(int r, int x, int y)
        {
            radius = r;
            this.Point = new Point { X = x, Y = y };
        }

        public IFigure Clone() // simple clone, for ref type both clone and prototype had the same values after change
        {
            return this.MemberwiseClone() as IFigure;
        }

        
        public void GetInfo()
        {
            Console.WriteLine("Круг радиусом {0} и центром в точке ({1}, {2})", radius, Point.X, Point.Y);
        }

       
    }

    //Concrete prototype 2
    [Serializable]
    class Rectangle : IFigure
    {
        private int Width { get; set; }
        private int Height { get; set; }
        public Point Point { get; set; }
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
            this.Point = new Point { X = Width / 2, Y = height/2};
        }

        public IFigure Clone()
        {
            return this.MemberwiseClone() as IFigure;
        }

        public object DeepCopy() //second type of copy, where we need to change in figure parametres but don't need to change it in clone type when we created both of them before
        {
            object figure = null;
            using (MemoryStream tempStream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter(null,
                    new StreamingContext(StreamingContextStates.Clone));

                binFormatter.Serialize(tempStream, this);
                tempStream.Seek(0, SeekOrigin.Begin);

                figure = binFormatter.Deserialize(tempStream);
            }
            return figure;
        }
        public void GetInfo()
        {
            Console.WriteLine("Прямоугольник с шириной {0}, высотой {1} и центром в точке ({2}, {3})", Width,Height, Point.X, Point.Y);
        }

        public void SetWidth(int width)
        {
            Width = width;
            Point.X = Width / 2;

        }
    }
}
