using System;
using OptimizacionBinaria.Funciones;
using System.Collections;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class BusquedaTabuCaracterizada : Algoritmo
    {
        //Atributos
        public int atrNumeroTweaks;
        private ArrayList atrListaTabu = new ArrayList();
        public double pm = 0.5;
        public double radio = 10;
        public int atrIteracionActual;
        public int atrTimeTabu;            
       
        public override void Ejecutar(Knapsack parProblema, Random ParAleatorio)
        {
            EFOs = 0;
            // Solución inicial
            var S = new Solucion(parProblema, this);
            S.InicializarAleatorio(ParAleatorio);
            MejorSolucion = new Solucion(S);
            // Agrego la Mejor solución a la lista Tabú            
            this.AddListaCarateristicas(S,this.atrIteracionActual = 1);
            while (EFOs < this.MaxEFOs && !MejorSolucion.esOptimoConocido())
            {
                this.atrIteracionActual++;
                // Remover de la lista Tabú todas las tublas en la iteracion c - d > l
                this.DeleteListaCaracteristicas();
                var R = new Solucion(S);                
                //TODO: Revisar el Tweak, R.Tweak(Copy(S), L)
                R.Tweak(ParAleatorio, pm, radio, this.atrListaTabu);
                for (var i = 0; i < this.atrNumeroTweaks - 1; i++)
                {
                    var W = new Solucion(S);
                    W.Tweak(ParAleatorio, pm, radio,atrListaTabu);
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
        private void AddListaCarateristicas(Solucion parSolucion, int parIteracion)
        {
            // 1. Obtener el vector solución
            int[] Dimensiones = parSolucion.getDimensiones();

            // 2. Obtener las características imersas o unos dentro de la solución sus dimensiones???
            for (var i = 0; i <= Dimensiones.Length - 1; i++)
            {
                if (Dimensiones[i] == 1)
                {
                    caracteristica objCaracteristica = new caracteristica(i, parIteracion);
                    //3. Guardar en la lista Tabú
                    this.atrListaTabu.Add(objCaracteristica);
                }
            }
            
        }
        private void DeleteListaCaracteristicas()
        {
            for(var i = 0; i <= this.atrListaTabu.Count-1 ; i++)
            {
                if (this.atrIteracionActual - ((caracteristica)atrListaTabu[i]).atrIteracion > this.atrTimeTabu)
                {
                    this.atrListaTabu.RemoveAt(i);
                }
            }
        }
        private Boolean perteneceListaTabu(Solucion parSolucion)
        {
            Boolean varRespuesta = false;
            foreach (Solucion varSolucion in atrListaTabu)
            {
                if (varSolucion.Equals(parSolucion))
                {
                    varRespuesta = true;
                }
            }
            return varRespuesta;
        }
    }

    public class caracteristica
    {
        public int atrCaracteristica; //valor de la dimension dentro del vector
        public int atrIteracion;

        public caracteristica(int parCarateristica, int parIteracion)
        {
            atrCaracteristica = parCarateristica;
            atrIteracion = parIteracion;
        }        

    }



}
