namespace PrototipoCajaArequipa
{
    // Implementación de una Cola (Queue) usando un arreglo.
    // Cumple con el principio FIFO (First In, First Out).
    public class Cola<T>
    {
        private T[] elementos;
        private int frente;   // Índice del primer elemento
        private int final;    // Índice donde se insertará el siguiente
        private int cantidad; // Cuántos elementos hay actualmente

        public Cola(int capacidadInicial = 4)
        {
            elementos = new T[capacidadInicial];
            frente = 0;
            final = 0;
            cantidad = 0;
        }

        // Enqueue: Agrega un elemento al final de la cola
        public void Enqueue(T item)
        {
            if (cantidad == elementos.Length)
            {
                Redimensionar();
            }

            elementos[final] = item;
            final = (final + 1) % elementos.Length;
            cantidad++;
        }

        // Dequeue: Quita y devuelve el primer elemento
        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La cola está vacía.");

            T item = elementos[frente];
            elementos[frente] = default(T)!; // Limpiar referencia
            frente = (frente + 1) % elementos.Length;
            cantidad--;

            return item;
        }

        // Front: Devuelve el primer elemento sin quitarlo
        public T Front()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La cola está vacía.");

            return elementos[frente];
        }

        // IsEmpty: Verifica si la cola está vacía
        public bool IsEmpty()
        {
            return cantidad == 0;
        }

        public int Count => cantidad;

        // Método privado para duplicar el tamaño del arreglo cuando se llena
        private void Redimensionar()
        {
            T[] nuevoArreglo = new T[elementos.Length * 2];
            for (int i = 0; i < cantidad; i++)
            {
                nuevoArreglo[i] = elementos[(frente + i) % elementos.Length];
            }
            elementos = nuevoArreglo;
            frente = 0;
            final = cantidad;
        }
    }
}