using System;
using System.Collections;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class BusquedaTabuConCaracteristicas : Algoritmo
    {
        public int MaxLongitudListaTabu;
        public int atrNumeroTweaks;
        private ArrayList atrListaTabu = new ArrayList();
        //public double pm = 0.5;
        //public double radio = 10;
        public int atrIteracionActual;


        public override void Ejecutar(Knapsack parProblema, Random ParAleatorio)
        {
            this.retoqueParametros(parProblema);
            EFOs = 0;
            // Solución inicial
            var S = new Solucion(parProblema, this);
            S.InicializarAleatorio(ParAleatorio);
            MejorSolucion = new Solucion(S);
            // Agrego la Mejor solución a la lista Tabú            
            this.AddListaCarateristicas(S,this.atrIteracionActual = 0);
            while (EFOs < this.MaxEFOs && !MejorSolucion.esOptimoConocido())
            {
                this.atrIteracionActual++;
                // Remover de la lista Tabú todas las tublas en la iteracion c - d > l
                this.DeleteListaCaracteristicas();
                var R = new Solucion(S);                               
                //R.Tweak(ParAleatorio, pm, radio, this.atrListaTabu);
                R.Tweak(ParAleatorio,this.atrListaTabu);
                for (var i = 0; i < this.atrNumeroTweaks - 1; i++)
                {
                    var W = new Solucion(S);
                    //W.Tweak(ParAleatorio, pm, radio,atrListaTabu);
                    W.Tweak(ParAleatorio,this.atrListaTabu);
                    if (W.fitness > R.fitness)
                        R = W;
                    if (EFOs >= this.MaxEFOs || R.esOptimoConocido()) break;               
                }
                S = R;
                this.AddListaCarateristicas(S, atrIteracionActual);
                if (S.fitness > MejorSolucion.fitness)
                    MejorSolucion = new Solucion(S);            
            }

        }
        public void retoqueParametros(Knapsack parProblema)
        {            
            this.MaxLongitudListaTabu = parProblema.TotalItems;            
        }

        private void AddListaCarateristicas(Solucion parSolucion, int parIteracion)
        {
            // 1. Obtener el vector solución
            int[] Dimensiones = parSolucion.getDimensiones();

            // 2. Obtener las características imersas o unos dentro de la solución sus dimensiones???
            for (var i = 0; i < Dimensiones.Length; i++)
            {
                if (Dimensiones[i] == 1 && !this.estaCaracteristica(i))
                {
                    Caracteristica objCaracteristica = new Caracteristica(i, parIteracion);
                    //3. Guardar en la lista Tabú
                    this.atrListaTabu.Add(objCaracteristica);
                }
            }
            
        }   

        private bool estaCaracteristica(int i){
            var resultado = false;
            foreach(Caracteristica c in this.atrListaTabu){
                if (c.getCaracteristica() == i)
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }
        private void DeleteListaCaracteristicas()
        {
            for(var i = 0; i < this.atrListaTabu.Count ; i++)
            {
                if (this.atrIteracionActual - ((Caracteristica)atrListaTabu[i]).atrIteracion > this.MaxLongitudListaTabu)
                {
                    this.atrListaTabu.RemoveAt(i);
                }
            }
        } 
    }
    public class Caracteristica
    {
        public int atrCaracteristica; //valor de la dimension dentro del vector
        public int atrIteracion;

        public Caracteristica(int parCarateristica, int parIteracion)
        {
            atrCaracteristica = parCarateristica;
            atrIteracion = parIteracion;
        }  

        public int getCaracteristica()
        {
            return this.atrCaracteristica;
        }      

    }
}