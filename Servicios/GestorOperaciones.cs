namespace PrototipoCajaArequipa
{
    public class GestorOperaciones
    {
        public const decimal MontoMinimo = 0.01m;
        public const decimal MontoMaximo = 5000.00m;

        // Usamos SOLO la cola para las operaciones pendientes
        private readonly Cola<Operacion> colaPendientes = new Cola<Operacion>();

        private readonly Agencia[] agencias =
        {
            new Agencia("Arequipa Centro", true),
            new Agencia("Cayma", true),
            new Agencia("Cusco", true),
            new Agencia("Puno", false), // Empieza sin conexión
            new Agencia("Tacna", true)
        };

        public Cola<Operacion> Pendientes => colaPendientes;
        public Agencia[] Agencias => agencias;

        public string? ValidarMonto(decimal monto)
        {
            if (monto < MontoMinimo || monto > MontoMaximo)
                return "El monto debe estar entre S/ 0.01 y S/ 5,000.00.";

            if (decimal.Round(monto, 2) != monto)
                return "El monto puede tener como máximo 2 decimales.";

            return null;
        }

        // Al registrar, la operación simplemente entra a la cola (Enqueue)
        public void Registrar(Operacion operacion)
        {
            colaPendientes.Enqueue(operacion);
        }

        // Simula el procesamiento FIFO de la siguiente operación
        public void ProcesarSiguiente()
        {
            if (colaPendientes.IsEmpty())
            {
                Console.WriteLine("No hay operaciones en la cola para procesar.");
                return;
            }

            // Se saca el primero de la cola (Dequeue)
            Operacion op = colaPendientes.Dequeue();
            Agencia? agencia = BuscarAgencia(op.AgenciaOrigen);

            Console.WriteLine($"\n-> Procesando operación: {op.NumeroOperacion}");

            if (agencia != null && agencia.Conectada)
            {
                op.Estado = EstadoOperacion.Enviada;
                Console.WriteLine($"   [OK] Enviada a la agencia {agencia.Nombre}.");
            }
            else
            {
                // Si no hay conexión, se vuelve a encolar al final para intentar después
                Console.WriteLine($"   [ALERTA] Agencia {op.AgenciaOrigen} sin conexión. La operación vuelve al final de la cola.");
                colaPendientes.Enqueue(op);
            }
        }

        public Agencia? BuscarAgencia(string nombre)
        {
            foreach (Agencia agencia in agencias)
                if (agencia.Nombre == nombre) return agencia;
            return null;
        }
    }
}