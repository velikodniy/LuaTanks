using System;
using System.Security.Cryptography.X509Certificates;

namespace LuaTanks {
    public enum TankState
    {
        Dead, Alive
    }
    
    // TODO Разделить танк и исходник
    /// <summary>
    /// Танк
    /// </summary>
    public class Tank {
        public int I, J;
        public int dI, dJ;
        public string Source;
        public TankState State = TankState.Alive;

        public Tank(int i, int j, string src) {
            I = i;
            J = j;
            dI = 1;
            dJ = 0;
            Source = src;
        }

        public override string ToString()
        {
            if (State == TankState.Dead)
                return "x";

            if (dI == 1 && dJ == 0)
                return "v";
            if (dI == -1 && dJ == 0)
                return "^";
            if (dI == 0 && dJ == 1)
                return ">";
            if (dI == 0 && dJ == -1)
                return "<";
            return "*";
        }
    }

    // TODO Тестировать действия
    public static class TankActions {
        public static void TankRight(Tank t, Field f) {
            // dI dJ   ->  NEW
            //  0  1   ->  1  0  
            //  0 -1   -> -1  0
            //  1  0   ->  0 -1
            // -1  0   ->  0  1
            
            int di =  t.dJ * Math.Abs(t.dJ);
            int dj = -t.dI * Math.Abs(t.dI);

            t.dI = di;
            t.dJ = dj;
        }

        public static void TankForward(Tank t, Field f) {
            int i = t.I + t.dI;
            int j = t.J + t.dJ;

            if (i < 0 || j < 0 || i >= f.Height || j >= f.Width)
                t.State = TankState.Dead;
            else {
                t.I = i;
                t.J = j;
            }
        }
    }
}

