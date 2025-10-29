using System;
using System.IO;

namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    class InputLayer
    {
        // поля
        private double[,] trainset; // 100 изображений
        private double[,] testset; // 10 изображений в тестоваой выборке
        
        // свойства
        public double[,] Trainset { get => trainset;}
        public double[,] Testset { get => testset;}

        // конструктор
        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;

            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    trainset = new double[tmpArrStr.Length, 16];

                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(trainset);
                    break;

                case NetworkMode.Test:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    trainset = new double[tmpArrStr.Length, 16];

                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(trainset);
                    break;
            }
        }
        public void Shuffling_Array_Rows(double[,] arr)
        {
            // дома напимать код этого метода
        }
    }
}
