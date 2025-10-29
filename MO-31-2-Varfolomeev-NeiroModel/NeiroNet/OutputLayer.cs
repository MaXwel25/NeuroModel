namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    class OutputLayer : Layer
    {

        public OutputLayer(int non, int nopn, NeironType nt, string type) : base (non, nopn, nt, type)
        {

        }

        // прямой подход
        public override void Recognize(Network net, Layer NextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < neirons.Length; i++)
                e_sum += neirons[i].Output;
            for (int i = 0; i < neirons.Length; i++)
                net.Fact[i] = neirons[i].Output / e_sum;
        }

        // обратный проход
        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneirons + 1];
            for (int j = 0; j < numofprevneirons + 1; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneirons; k++)
                    sum += neirons[k].Weights[j] * errors[k];

                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneirons; i++) // цикл коррекции синаптических весов
            {
                for (int n = 0; n < numofprevneirons + 1; n++)
                {
                    double delwat;
                    if (n == 0)  // если порог
                        delwat = momentum * lastdeltaweights[i, 0] + learningrate * errors[i];
                    else
                        delwat = momentum * lastdeltaweights[i, n] + learningrate * neirons[n].Inputs[n - 1] * errors[i];
                
                    lastdeltaweights[i, n] = delwat;
                    neirons[i].Weights[n] += delwat; // коррекция весов

                }
            }
            return gr_sum;
        }
    }
}
