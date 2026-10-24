using System.Text;

namespace PrototipoCajaArequipa
{
    class Program
    {
        static GestorOperaciones gestor = new GestorOperaciones();

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("   CAJA AREQUIPA - SIMULACIÓN DE COLA (FIFO)");
                Console.WriteLine("==============================================");
                Console.WriteLine(" 1. Registrar una nueva operación");
                Console.WriteLine(" 2. Ver operaciones en cola");
                Console.WriteLine(" 3. Procesar siguiente operación");
                Console.WriteLine(" 4. Ver estado de las agencias");
                Console.WriteLine(" 0. Salir");
                Console.WriteLine("==============================================");
                Console.Write("Elija una opción: ");

                opcion = (Console.ReadLine() ?? "").Trim();

                switch (opcion)
                {
                    case "1": RegistrarOperacion(); break;
                    case "2": VerCola(); break;
                    case "3": gestor.ProcesarSiguiente(); Pausar(); break;
                    case "4": MostrarAgencias(); Pausar(); break;
                    case "0": Console.WriteLine("\nSaliendo..."); break;
                    default: Console.WriteLine("\nOpción no válida."); Pausar(); break;
                }

            } while (opcion != "0");
        }

        static void RegistrarOperacion()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR OPERACIÓN (Se agregará al final de la cola)\n");

            Console.Write("Número de operación (ej: OP010): ");
            string numero = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (numero == "") return;

            Console.WriteLine("\nTipo de operación: 1. Transferencia  2. Recarga  3. Pago Servicio");
            Console.Write("Elija (1-3): ");
            string tipoStr = (Console.ReadLine() ?? "").Trim();
            TipoOperacion tipo = tipoStr == "2" ? TipoOperacion.Recarga :
                                  tipoStr == "3" ? TipoOperacion.PagoServicio : TipoOperacion.Transferencia;

            Console.WriteLine("\nAgencia origen:");
            string origen = PedirAgencia(null);

            string? destino = null;
            if (tipo == TipoOperacion.Transferencia)
            {
                Console.WriteLine("\nAgencia destino:");
                destino = PedirAgencia(origen);
            }

            Console.Write("\nMonto: ");
            decimal.TryParse((Console.ReadLine() ?? "").Replace(',', '.'), out decimal monto);

            string? error = gestor.ValidarMonto(monto);
            if (error != null) { Console.WriteLine(error); Pausar(); return; }

            Operacion op = new Operacion(numero, tipo, origen, destino, monto);
            gestor.Registrar(op);
            Console.WriteLine($"\nOperación {numero} encolada exitosamente.");
            Pausar();
        }

        static string PedirAgencia(string? excluir)
        {
            Agencia[] ags = gestor.Agencias;
            for (int i = 0; i < ags.Length; i++)
                Console.WriteLine($"  {i + 1}. {ags[i].Nombre} ({ags[i].EstadoTexto()})");

            while (true)
            {
                Console.Write($"Elija (1-{ags.Length}): ");
                if (int.TryParse(Console.ReadLine(), out int n) && n >= 1 && n <= ags.Length)
                {
                    string nombre = ags[n - 1].Nombre;
                    if (nombre == excluir) { Console.WriteLine("Debe ser distinta al origen."); continue; }
                    return nombre;
                }
            }
        }

        static void CargarEjemplos()
        {
            Console.Clear();
            Console.WriteLine("CARGANDO EJEMPLOS...");
            Operacion[] ejemplos = DatosEjemplo.Crear();
            foreach (var op in ejemplos)
            {
                gestor.Registrar(op);
                Console.WriteLine($"  Encolada: {op.NumeroOperacion} ({op.AgenciaOrigen})");
            }
            Pausar();
        }

        static void VerCola()
        {
            Console.Clear();
            Console.WriteLine("OPERACIONES EN COLA (ESPERANDO ENVÍO)");
            Console.WriteLine("El primero en la lista es el próximo en procesarse (FIFO).\n");

            if (gestor.Pendientes.IsEmpty())
            {
                Console.WriteLine("La cola está vacía.");
            }
            else
            {
                Console.WriteLine($"Total en cola: {gestor.Pendientes.Count}");
                Console.WriteLine($"Próximo a procesar (Front): {gestor.Pendientes.Front().NumeroOperacion}");
            }
            Pausar();
        }

        static void MostrarAgencias()
        {
            Console.WriteLine("ESTADO DE AGENCIAS:");
            foreach (var a in gestor.Agencias)
                Console.WriteLine($"  - {a.Nombre}: {a.EstadoTexto()}");
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}