using System;
using System.Collections.Generic;

namespace LuaTanks {
    /// <summary>
    /// Игровое поле
    /// </summary>
    public class Field {
        public int Width { get; private set; }
        public int Height { get; private set; }
        ulong count = 0UL;
        IEnumerable<Tank> tanks;

        public Field(int w, int h) {
            Width = w;
            Height = h;
        }

        public void SetTanks(IEnumerable<Tank> tanks)
        {
            this.tanks = tanks;
        }

        public void Show() {
            if (tanks == null)
                return;

            string[,] field = new string[Height, Width];
            // TODO перехватывать IndexOutOfBoundException
            foreach (var t in tanks) {
                field[t.I, t.J] = t.ToString();
            }

            Console.Clear();
            Console.WriteLine(count++);
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    Console.Write(field[i, j]==null?".":field[i, j]);
                }
                Console.WriteLine();
            }
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }
    }
}

