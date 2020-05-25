using System;
using System.Collections;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS;

namespace OptimizacionBinaria.Metaheuristicas
{
    public class Solucion
    {
        private int[] _dimensiones; // {0, 1}
        private double _weight;
        public double fitness;
        public Knapsack miProblema;
        public Algoritmo MiAlgoritmo;

        public Solucion(Knapsack elProblema, Algoritmo miAlgo)
        {
            miProblema = elProblema;
            MiAlgoritmo = miAlgo;
            _dimensiones = new int[miProblema.TotalItems];
        }

        public Solucion(Solucion original)
        {
            miProblema = original.miProblema;
            MiAlgoritmo = original.MiAlgoritmo;
            fitness = original.fitness;
            _weight = original._weight;
            _dimensiones = new int[miProblema.TotalItems];
            for(var i = 0; i < miProblema.TotalItems; i++)
                _dimensiones[i] = original._dimensiones[i];
        }

        public void InicializarAleatorio(Random aleatorio)
        {
            _weight = 0;

            var opciones = new List<KeyValuePair<int, double>>();
            for (var i=0; i < miProblema.TotalItems; i++)
                opciones.Add(new KeyValuePair<int, double>(i, miProblema.Weight(i)));

            while (_weight <= miProblema.Capacity)
            {
                var p = aleatorio.Next(opciones.Count);
                _dimensiones[opciones[p].Key] = 1;
                _weight += miProblema.Weight(opciones[p].Key);
                opciones.RemoveAt(p);

                var espacioLibre = miProblema.Capacity - _weight;
                for (var i = opciones.Count - 1; i >=0 ; i--)
                    if (opciones[i].Value > espacioLibre)
                        opciones.RemoveAt(i);

                if (opciones.Count == 0) break;
            }
            Evaluar();
        }

        //Tabú caracterizado
        public void Tweak(Random aleatorio, double pm, double radio, ArrayList parListaTabu)
        {
            // Paso 1 = Intercambiar un objeto seleccionado por uno no seleccionado
            var seleccionados = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < miProblema.TotalItems; i++)
            {
                if (_dimensiones[i] == 1)
                    seleccionados.Add(new KeyValuePair<int, double>(i, miProblema.Weight(i)));
            }

            int p;
            var noSeleccionados = new List<KeyValuePair<int, double>>();
            var pruebas = 0;
            var pesoDisponible = 0.0;
            do
            {
                p = aleatorio.Next(seleccionados.Count);
                pesoDisponible = miProblema.Capacity - (_weight - seleccionados[p].Value);

                for (var i = 0; i < miProblema.TotalItems; i++)
                {
                    if (_dimensiones[i] == 0 && miProblema.Weight(i) <= pesoDisponible && !estaEnlalistaTabu(i,parListaTabu))
                        noSeleccionados.Add(new KeyValuePair<int, double>(i, miProblema.Weight(i)));
                }

                pruebas++;
                if (pruebas >= 3) return; // No trato de hacer Tweak
            } while (noSeleccionados.Count == 0);

            _dimensiones[seleccionados[p].Key] = 0;
            _weight -= seleccionados[p].Value;

            var q = aleatorio.Next(noSeleccionados.Count);
            _dimensiones[noSeleccionados[q].Key] = 1;
            _weight += noSeleccionados[q].Value;

            // Completar
            pesoDisponible = miProblema.Capacity - _weight;
            noSeleccionados.RemoveAt(q);
            for (var i = noSeleccionados.Count - 1; i >= 0; i--)
            {
                if (noSeleccionados[i].Value > pesoDisponible)
                    noSeleccionados.RemoveAt(i);
            }

            while (noSeleccionados.Count != 0)
            {
                var t = aleatorio.Next(noSeleccionados.Count);
                _dimensiones[noSeleccionados[t].Key] = 1;
                _weight += noSeleccionados[t].Value;

                pesoDisponible = miProblema.Capacity - _weight;
                noSeleccionados.RemoveAt(t);
                for (var i = noSeleccionados.Count - 1; i >= 0; i--)
                {
                    if (noSeleccionados[i].Value > pesoDisponible)
                        noSeleccionados.RemoveAt(i);
                }
            }

            Evaluar();
        }

        //Tabu simple, demas algoritmos
        public void Tweak(Random aleatorio, double pm, double radio)
        {
            // Paso 1 = Intercambiar un objeto seleccionado por uno no seleccionado
            var seleccionados = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < miProblema.TotalItems; i++)
            {
                if (_dimensiones[i] == 1)
                    seleccionados.Add(new KeyValuePair<int, double>(i, miProblema.Weight(i)));
            }

            int p;
            var noSeleccionados = new List<KeyValuePair<int, double>>();
            var pruebas = 0;
            var pesoDisponible = 0.0;
            do
            {
                p = aleatorio.Next(seleccionados.Count);
                pesoDisponible = miProblema.Capacity - (_weight - seleccionados[p].Value);

                for (var i = 0; i < miProblema.TotalItems; i++)
                {
                    if (_dimensiones[i] == 0 && miProblema.Weight(i) <= pesoDisponible)
                        noSeleccionados.Add(new KeyValuePair<int, double>(i, miProblema.Weight(i)));
                }

                pruebas++;
                if (pruebas >= 3) return; // No trato de hacer Tweak
            } while (noSeleccionados.Count == 0);

            _dimensiones[seleccionados[p].Key] = 0;
            _weight -= seleccionados[p].Value;

            var q = aleatorio.Next(noSeleccionados.Count);
            _dimensiones[noSeleccionados[q].Key] = 1;
            _weight += noSeleccionados[q].Value;

            // Completar
            pesoDisponible = miProblema.Capacity - _weight;
            noSeleccionados.RemoveAt(q);
            for (var i = noSeleccionados.Count - 1; i >= 0; i--)
            {
                if (noSeleccionados[i].Value > pesoDisponible)
                    noSeleccionados.RemoveAt(i);
            }

            while (noSeleccionados.Count != 0)
            {
                var t = aleatorio.Next(noSeleccionados.Count);
                _dimensiones[noSeleccionados[t].Key] = 1;
                _weight += noSeleccionados[t].Value;

                pesoDisponible = miProblema.Capacity - _weight;
                noSeleccionados.RemoveAt(t);
                for (var i = noSeleccionados.Count - 1; i >= 0; i--)
                {
                    if (noSeleccionados[i].Value > pesoDisponible)
                        noSeleccionados.RemoveAt(i);
                }
            }

            Evaluar();
        }
        private Boolean estaEnlalistaTabu(int parDimension, ArrayList listaTabu)
        {
            Boolean varResultado = false;
            foreach(caracteristica obj in listaTabu)
            {
                if(obj.atrCaracteristica == parDimension)
                {
                    varResultado = true;
                }
            }
            return varResultado;
        }
        public int[] getDimensiones()
        {
            return _dimensiones;
        }

        public void Evaluar()
        {
            fitness = miProblema.Evaluar(_dimensiones);
            MiAlgoritmo.EFOs++;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < miProblema.TotalItems; i++)
                result = result + (_dimensiones[i] + " ");
            result = result + "   f(s) = " + fitness;
            return result;
        }

        public bool esOptimoConocido()
        {
            return this.fitness == this.miProblema.OptimalKnown?true:false;
        }
        public bool seEncontroOptimo()
        {
            return this.esOptimoConocido();
        }

    }
}
