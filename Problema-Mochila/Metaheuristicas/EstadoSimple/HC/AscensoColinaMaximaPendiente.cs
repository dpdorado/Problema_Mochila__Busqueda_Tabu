﻿using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class AscensoColinaMaximaPendiente : Algoritmo
    {
        public double pm = 0.5;
        public double radio = 10;
        public int vecinos = 5;

        public override void Ejecutar(Knapsack elProblema, Random aleatorio)
        {
            EFOs = 0;
            // Inicializacion aleatoria
            var s = new Solucion(elProblema, this);
            s.InicializarAleatorio(aleatorio);

            // ciclo de evolución
            while (EFOs < MaxEFOs)
            {
                // r es Tweak de la copia de s
                var r = new Solucion(s);
                r.Tweak(aleatorio, pm, radio);

                for (var v = 0; v < vecinos - 1; v++)
                {
                    var w = new Solucion(s);
                    w.Tweak(aleatorio, pm, radio);

                    if (w.fitness < r.fitness)
                        r = w;
                    if (EFOs >= MaxEFOs) break;
                }

                if (r.fitness < s.fitness) //se esta minimizando
                {
                    s = r;
                }
            }

            MejorSolucion = s;
        }

        public override string ToString()
        {
            return "Ascenso a la Colina por la Maxima Pendiente";
        }
    }
}
