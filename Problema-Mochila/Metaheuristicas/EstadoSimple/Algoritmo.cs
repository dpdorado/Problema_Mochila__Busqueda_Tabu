using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple
{
    public abstract class Algoritmo
    {
        public int MaxEFOs;
        public int EFOs;
        public Solucion MejorSolucion;

        public abstract void Ejecutar(Knapsack elProblema, Random aleatorio);
    
    }
}
