using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NLua;

namespace LuaTanks {

    /// <summary>
    /// Диспетчер
    /// Контролирует состояние танков, выполняет программы.
    /// </summary>
    public partial class Dispatcher {
        class TankProgram {
            public Tank tank; // TODO Хранить идентификатор танка, а не объект (запрашивать у Field по id)
            public Lua lua = new Lua();
            public ManualResetEvent mre = new ManualResetEvent(false);
        }
        
        List<TankProgram> tank_progs = new List<TankProgram>();
        Queue<Action> queries = new Queue<Action>();

        Field field;

        Dispatcher(Field f, List<Tank> lt) {
            foreach (var t in lt)
            {
                var tp = new TankProgram() { tank = t };

                // Отключение библиотек
                tp.lua.DoString(@"
                    import = function () end
                ");
                tank_progs.Add(tp);
            }

            field = f;
            field.SetTanks(lt);
        }

        /// <summary>
        /// Подготовить состояние для следующего кадра
        /// </summary>
        public void Step() {
            lock (queries)
            {
                while(queries.Count > 0)
                {
                    queries.Dequeue()();
                }
            }
        }

        // TODO Регистрировать при создании экземпляра
        // TODO Сделать регистрацию действия с параметром, функции, действия без паузы
        public void RegisterAction(string name, Action<Tank, Field> act) {
            foreach (var tp in tank_progs)
            {
                tp.lua[name] = (Action)(() =>
                {
                    lock (queries)
                    {
                        queries.Enqueue(() =>
                        {
                            act(tp.tank, field);
                            // TODO Продолжать по внешнему событию?
                            tp.mre.Set();
                        });
                    }
                    tp.mre.WaitOne();
                    tp.mre.Reset();
                });
            }
        }

        HashSet<Thread> threads = new HashSet<Thread>();

        // TODO: Сделать метод для остановки
        public void Start()
        {
            foreach (var tp in tank_progs)
                threads.Add(new Thread(() => {
                    tp.lua.DoString(tp.tank.Source);
                    tp.tank.State = TankState.Dead;
                }));

            foreach (var th in threads)
                th.Start();
        }
    }
}

