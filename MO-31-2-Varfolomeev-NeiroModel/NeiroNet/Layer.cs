using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using MO_31_2_Varfolomeev_NeiroModel.NeiroNet;

namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    abstract class Layer
    {
        protected string name_Layer; // название слоя
        string pathDirWeights; // путь к каталогу, где находится файл синаптических весов
        string pathFileWeights; // путь к файлу саниптическов весов
        protected int numofneirons; // число нейронов текущего слоя
        protected int numofprevneirons; // число нейронов предыдущего слоя
        protected const double learningrate = 0.06; // скорость обучения 0.06
        protected const double momentum = 0.000d; // момент инерции 0.050d
        protected double[,] lastdeltaweights; // веса предыдущей итерации
        protected Neiron[] neirons; // массив нейронов текущего слоя

        // свойства
        public Neiron[] Neirons { get => neirons; set => neirons = value; }

        // активация нейрона
        public double[] Data // передача входных сигналов на нейроны слоя и активатор
        {
            set
            {
                for (int i = 0; i < numofneirons; i++)
                    Neirons[i].Activator(value);
            }
        }

        // конструктор
        protected Layer(int non, int nopn, NeironType nt, string nm_Layer)
        {
            numofneirons = non; // количество нейронов текущего слоя
            numofprevneirons = nopn; // количество нейронов предыдущего слоя
            Neirons = new Neiron[non]; // определение массива нейронов
            name_Layer = nm_Layer; // наиминование слоя
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; // временный массив синаптических весов
            lastdeltaweights = new double[non, nopn + 1];

            if (File.Exists(pathFileWeights)) // определяет существует ли pathFileWeights
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights); //считывает данные из файла
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++) // цикл формирования нейронов слоя и заполнения
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neirons[i] = new Neiron(tmp_weights, nt); // заполнение массива нейронами
            }
        }


        // метод работы с массивом синаптических весов слоя
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string[] tmpStrWeights;
            double[,] weights = new double[numofneirons, numofprevneirons + 1];

            switch (mm)
            {
                // парсинг в тип double строкового формата веса нейронов из csv - получает значения весов нейронов
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);        // считывание строк текстового файла csv весов нейрона (в tmpStrWeights каждый i-ый элемент это строка весов)
                    string[] memory_elemnt; // массив, где каждый i-ый элемент это один вес нейрона (берётся одна строка из tmpStrWeights)

                    // строка весов нейронов
                    for (int i = 0; i < numofneirons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);  // разбивает строку
                        // каждый отдельный вес нейрона
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                // парсинг в строковой формат веса нейронов в csv (обратный MemoryMode.GET) - сохраняет готовые веса нейронов
                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneirons]; // создаём строку из весов нейрона (tmpStrWeights это массив, где каждый i-ый элемент это строка весов) 
                    for (int i = 0; i < numofneirons; i++)
                    {
                        string[] memory_elemnt2 = new string[numofprevneirons + 1];
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            memory_elemnt2[j] = neirons[i].Weights[j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        tmpStrWeights[i] = string.Join(";", memory_elemnt2);
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                // инициализация весов для нейронов
                case MemoryMode.INIT:
                    // нерабочая версия
                    /*
                    tmpStrWeights = new string[numofneirons];
                    Random random = new Random();

                    for (int i = 0; i < numofneirons; i++)
                    {
                        double weightSum = 0.0;
                        double weightSquaredSum = 0.0;

                        // Picking random weights from [-1; +1] 
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] = random.NextDouble() * 2.0 - 1.0;
                            weightSum += weights[i, j];
                            weightSquaredSum += weights[i, j] * weights[i, j];
                        }

                        // Calculating average weight and base offset
                        double averageWeight = weightSum / (numofprevneirons + 1);
                        double baseOffset = Math.Sqrt((weightSquaredSum / (numofprevneirons + 1))
                            - (averageWeight * averageWeight));

                        // Standartizing weights and writing them to file
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] = (weights[i, j] - averageWeight) / baseOffset;
                            tmpStrWeights[i] += weights[i, j].ToString().Replace(',', '.') + ";";
                        }
                    }

                    File.WriteAllLines(path, tmpStrWeights);
                    break;
                    */

                    tmpStrWeights = new string[numofneirons];
                    Random random = new Random();

                    for (int i = 0; i < numofneirons; i++)
                    {
                        double weightSum = 0.0;

                        // первый проход: генерируем веса и считаем сумму
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] = random.NextDouble() * 2.0 - 1.0;
                            weightSum += weights[i, j];
                        }

                        // второй проход: корректируем для нулевого среднего
                        double averageWeight = weightSum / (numofprevneirons + 1);
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] -= averageWeight;
                            /*
                            // гарантируем, что веса в пределах [-1, 1]
                            if (weights[i, j] > 1.0) weights[i, j] = 1.0;
                            if (weights[i, j] < -1.0) weights[i, j] = -1.0;
                            */
                        }

                        // запись в файл
                        string[] memory_elemnt2 = new string[numofprevneirons + 1];
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            memory_elemnt2[j] = weights[i, j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        tmpStrWeights[i] = string.Join(";", memory_elemnt2);
                    }

                    File.WriteAllLines(path, tmpStrWeights);
                    break;

            }
            return weights;
        }

        abstract public void Recognize(Network net, Layer nextLayer); // для прямых проходов

        abstract public double[] BackwardPass(double[] stuff); // и их обратных
    }
}
