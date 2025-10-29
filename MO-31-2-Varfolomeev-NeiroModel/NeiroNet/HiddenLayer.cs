namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeironType nt, string nm_Layer) : base(non, nopn, nt, nm_Layer)
        {

        }

        // прямой подход
        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[numofneirons];
            for (int i = 0; i < numofneirons; i++)
                hidden_out[i] = neirons[i].Output;

            nextLayer.Data = hidden_out; // передача выходного сигнала на вход следующего слоя
        }

        // обратный проход
        public override double[] BackwardPass(double[] fr_sum)
        {
            double[] gr_sum = new double[numofprevneirons];
            for (int j = 0; j < numofprevneirons; j++) // цикл вычисления градиента (сумма j-ого нейрона)
            {
                double sum = 0;
                for (int k=0; k < numofneirons; k++)
                {
                    sum += neirons[k].Weights[j] * neirons[k].Derivative * gr_sum[k]; // через градиентные суммы и производные
                }
                gr_sum[j] = sum;
            }


            for (int i = 0; i < numofneirons; i++)
            {
                for (int n=0; n < numofprevneirons; n++)
                {
                    double delwat;
                    if (n == 0) // если порог
                        delwat = momentum * lastdeltaweights[i, 0] + learningrate * neirons[i].Derivative * gr_sum[i];
                    else
                        delwat = momentum * lastdeltaweights[i, n] + learningrate * neirons[i].Inputs[n - 1] * neirons[i].Derivative * gr_sum[i];

                    lastdeltaweights[i, n] = delwat;
                    neirons[i].Weights[n] += delwat; // коррекция весов
                }
            }
            return gr_sum;
        }
    }
}
