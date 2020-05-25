using System;
using OptimizacionBinaria.Funciones;
using System.Collections;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class BusquedaTabu : Algoritmo
    {
        // Atributos
        public int MaxLongituLitaTabu;
        public int atrNumeroTweaks;
        private Queue atrListaTabu = new Queue();
        //public double pm = 0.5;
        //public double radio = 10;
     
        public override void Ejecutar(Knapsack parProblema, Random ParAleatorio)
        {
            //Revisar bien
            this.retoqueParametros(parProblema);
            EFOs = 0;
            // Solución inicial
            var S = new Solucion(parProblema, this);
            S.InicializarAleatorio(ParAleatorio); 
            // Mejor solución           
            this.MejorSolucion = new Solucion(S);
            // Se agrega la MejorSolucion a la lista Tabú
            this.atrListaTabu.Enqueue(MejorSolucion);

            while (EFOs < this.MaxEFOs && !MejorSolucion.esOptimoConocido())
            {
                if (this.atrListaTabu.Count >= this.MaxLongituLitaTabu)
                {
                    this.atrListaTabu.Dequeue();
                }
                var R = new Solucion(S);
                //R.Tweak(ParAleatorio, pm, radio);
                R.Tweak(ParAleatorio);
                for (var i = 0; i < this.atrNumeroTweaks - 1; i++)
                {
                    var W = new Solucion(S);
                    //W.Tweak(ParAleatorio, pm, radio);
                    W.Tweak(ParAleatorio);
                    if (!perteneceListaTabu(W) && (W.fitness > R.fitness || perteneceListaTabu(R)))
                        R = W;  
                    if (EFOs >= this.MaxEFOs || R.esOptimoConocido()) break;
                }
                if (!perteneceListaTabu(R) && R.fitness > S.fitness)
                    {
                        S = R;
                        atrListaTabu.Enqueue(R);
                    }
                if (S.fitness > MejorSolucion.fitness)
                    MejorSolucion = new Solucion(S);
            }

        }

        public void retoqueParametros(Knapsack parProblema)
        {            
            this.MaxLongituLitaTabu = parProblema.TotalItems;            
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
}
