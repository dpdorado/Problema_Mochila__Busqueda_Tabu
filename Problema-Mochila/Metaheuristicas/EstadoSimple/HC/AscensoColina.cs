using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class AscensoColina: Algoritmo
    {
        public double pm = 0.5;
        public double radio = 10;

        public override void Ejecutar(Knapsack elProblema, Random aleatorio)
        {
            EFOs = 0;

            // Inicializacion aleatoria
            var s = new Solucion(elProblema, this);
            s.InicializarAleatorio(aleatorio);

            // ciclo de evolución
            while (EFOs < MaxEFOs)
            {
                // r es copia de s
                var r = new Solucion(s);
                //tweak
                
                r.Tweak(aleatorio);
                //r.Tweak(aleatorio, pm, radio);

                if (r.fitness > s.fitness) //se esta maximizando
                {
                    s = r;
                }
                if (s.esOptimoConocido()) break;
            }

            MejorSolucion = s;
        }

        public override string ToString()
        {
            return "Ascenso a la Colina";
        }
    }
}
