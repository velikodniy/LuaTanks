using System;
using System.Threading;
using NLua;

namespace LuaTanks {
    class MainClass {
        public static void Main(string[] args) {
            Tank t1 = new Tank(2, 4, @"
                while true do
                    right()
                    forward()
                    right()
                    forward()
                    right()
                    forward()
                    right()
                    forward()
                end
            ");
            Tank t2 = new Tank(8, 11, @"
                right()
                right()
             ");

            Tank t3 = new Tank(14, 6, @"
                while true do
                    forward()
                end
             ");

            Field f = new Field(20, 20);

            Dispatcher d = Dispatcher.Builder(f)
                .AddTank(t1)
                .AddTank(t2)
                .AddTank(t3)
                .Build();

            d.RegisterAction("right", TankActions.TankRight);
            d.RegisterAction("forward", TankActions.TankForward);

            d.Start();
            while (true) {
                d.Step();
                f.Show();
                Thread.Sleep(500);
            }
        }
    }
}
