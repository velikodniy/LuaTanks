using System.Collections.Generic;

namespace LuaTanks {
    public partial class Dispatcher {

        public static DispatcherFactory Builder(Field f) {
            return new DispatcherFactory(f);
        }

        /// <summary>
        /// Фабрика диспетчеров
        /// </summary>
        public class DispatcherFactory {
            Field field;
            List<Tank> tanks;
            public DispatcherFactory (Field f) {
                field = f;
                tanks = new List<Tank>();
            }

            /// <summary>
            /// Добавляет танк в создаваемый диспетчер
            /// </summary>
            /// <returns>Фабрика</returns>
            /// <param name="t">Добавляемый танк</param>
            public DispatcherFactory AddTank(Tank t) {
                tanks.Add(t);
                return this;
            }

            /// <summary>
            /// Завершает построение диспетчера
            /// </summary>
            public Dispatcher Build() {
                return new Dispatcher(field, tanks);
            }
        }
    }
}