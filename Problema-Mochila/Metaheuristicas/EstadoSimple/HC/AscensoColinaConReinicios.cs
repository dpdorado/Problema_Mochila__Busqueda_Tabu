using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class AscensoColinaConReinicios : Algoritmo
    {
        public double ProbabilidadDeMutacion = 0.5;
        public double radio = 10;
        public int maxLocalIter = 15;   // corresponde al T de la diapositiva

        public override void Ejecutar(Knapsack elProblema, Random aleatorio)
        {
            EFOs = 0;
            // linea 2 Inicializacion aleatoria
            var s = new Solucion(elProblema, this);
            s.InicializarAleatorio(aleatorio);

            MejorSolucion = new Solucion(s); // linea 3

            // linea 4 - ciclo de evolución
            while (EFOs < MaxEFOs)
            {
                var time = aleatorio.Next(maxLocalIter); // linea 5

                for (var local = 0; local < time; local++) { // linea 6
                    // linea 7 - r es tweak copia de s
                    var r = new Solucion(s);
                    r.Tweak(aleatorio, ProbabilidadDeMutacion, radio);

                    if (r.fitness < s.fitness) // linea 8 -se esta minimizando
                        s = r; // linea 9
                    if (EFOs >= MaxEFOs) break;
                } // linea 10

                if (s.fitness < MejorSolucion.fitness) // linea 11
                    MejorSolucion = new Solucion(s); // linea 12

                s.InicializarAleatorio(aleatorio);
            }

            MejorSolucion = s;
        }

        public override string ToString()
        {
            return "Ascenso a la Colina con Reinicios";
        }
    }
}
