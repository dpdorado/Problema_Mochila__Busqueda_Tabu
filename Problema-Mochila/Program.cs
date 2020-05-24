using System;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS;

namespace OptimizacionBinaria
{
    class Program
    {
        static void Main(string[] args)
        {
            var misFunciones = new List<Knapsack>
            {
                new Knapsack("f1.txt"),
                new Knapsack("f2.txt"),
                new Knapsack("f3.txt"),
                new Knapsack("f4.txt"),
                new Knapsack("f5.txt"),
                new Knapsack("f6.txt"),
                new Knapsack("f7.txt"),
                new Knapsack("f8.txt"),
                new Knapsack("f9.txt"),
                new Knapsack("f10.txt")
            };
            var misAlgoritmos = new List<Algoritmo>
            {
                //HC
                new AscensoColina {pm = 0.5, radio = 10, MaxEFOs = 1000},
                //new AscensoColinaMaximaPendiente {pm = 0.5, radio = 10, vecinos = 10, MaxEFOs = 5000},
                //new AscensoColinaMaximaPendienteConRemplazo {pm = 0.5, radio = 10, vecinos = 10, MaxEFOs = 5000},
                //new AscensoColinaConReinicios {ProbabilidadDeMutacion = 0.5, radio = 10, maxLocalIter = 15, MaxEFOs = 5000},                
                //Búsqueda aleatoria
                new BusquedaAleatoria() {MaxEFOs = 1000},                
                //Tabú sin características
                new BusquedaTabu{pm = 0.5, radio = 10, MaxEFOs = 1000, MaxLongituLitaTabu = 10,atrNumeroTweaks=2},
                //Tabú con caraterísticas,                                
                new BusquedaTabuCaracterizada{pm = 0.5, radio = 10, MaxEFOs = 1000,atrNumeroTweaks=2, atrTimeTabu =5}
            };

            Console.WriteLine("         Ascenso a la Colina (HC)  Búsqueda Aleatoria        Búsqueda Tabú             Búsqueda Tabú con características");
            foreach (var funcion in misFunciones)
            {
                Console.Write("funcion  ");

                foreach (var algoritmo in misAlgoritmos)
                {
                    var maxRep = 30;
                    var mediaF = 0.0;
                    for (var rep = 0; rep < maxRep; rep++)
                    {
                        var aleatorio = new Random(rep);
                        var aleatorio2 = new Random(rep + 2);

                        algoritmo.Ejecutar(funcion, aleatorio);
                        mediaF += algoritmo.MejorSolucion.fitness;
                    }
                    mediaF = mediaF / maxRep;

                    Console.Write($"{mediaF,-25:0.000000000000000}" + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}