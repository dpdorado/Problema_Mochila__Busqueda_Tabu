using System;
using System.Diagnostics;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class BusquedaAleatoria:Algoritmo
    {
        public override void Ejecutar(Knapsack elProblema, Random aleatorio)
        {
            EFOs = 0;
            // Inicializacion aleatoria
            MejorSolucion = new Solucion(elProblema, this);
            MejorSolucion.InicializarAleatorio(aleatorio);
            //Debug.WriteLine(EFOs + " " + MejorSolucion.fitness);

            // ciclo de evolución
            while (EFOs < MaxEFOs)
            {
                // r es otra solucion aleatoria
                var r = new Solucion(elProblema, this);
                r.InicializarAleatorio(aleatorio);

                if (r.fitness > MejorSolucion.fitness) //se esta minimizando
                {
                    MejorSolucion = r;
                }
                if (MejorSolucion.esOptimoConocido()) break;
                //Debug.WriteLine(EFOs + " " + MejorSolucion.fitness);
            }
        }

        public override string ToString()
        {
            return "Búsqueda Aleatoria";
        }
    }
}
