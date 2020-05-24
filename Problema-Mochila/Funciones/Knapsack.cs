using System.Collections.Generic;
using System.IO;

namespace OptimizacionBinaria.Funciones
{
    public class Knapsack
    {
        private const string RootDirectory = "/home/dpdaniel/Pruebas/Parcial 1 - Metaheuristicas/Problema-Mochila/knapsack-files/";        
        public int TotalItems;
        public double Capacity;
        public double OptimalKnown;
        private List<Variable> _variables;

        public double Weight(int index)
        {
            return _variables[index].Weight;
        }

        public double Density(int index)
        {
            return _variables[index].Value/ _variables[index].Weight;
        }

        public Knapsack(string fileName)
        {
            ReadFile(RootDirectory + fileName);
        }

        public void ReadFile(string fullFileName)
        {
            //read the problem
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(' ');
            TotalItems = int.Parse(firstline[0]);
            Capacity = double.Parse(firstline[1]);

            _variables = new List<Variable>();

            var positionLine = 1;
            for (var i = 0; i < TotalItems; i++)
            {
                var line = lines[positionLine++].Split(' ');
                var value = double.Parse(line[0]);
                var weight = double.Parse(line[1]);
                var newVariable = new Variable(i, value, weight);
                _variables.Add(newVariable);
            }

            OptimalKnown = double.Parse(lines[positionLine]);
        }

        public double Evaluar(int[] dim)
        {
            var suma = 0.0;
            for (var i = 0; i < TotalItems; i++)
                suma += dim[i] * _variables[i].Value;

            return suma;
        }

        public override string ToString()
        {
            var result = "Capacity:" + Capacity.ToString("##0") + "\n" +
                   "OptimalKnown:" + OptimalKnown.ToString("##0.00") + "\n";

            _variables.Sort((x,y) => x.Position.CompareTo(y.Position));

            for (var i = 0; i < TotalItems; i++)
                result += _variables[i] + "\n";
            return result;
        }
    }
}
