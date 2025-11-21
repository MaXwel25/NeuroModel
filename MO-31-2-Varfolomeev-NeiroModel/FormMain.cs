using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MO_31_2_Varfolomeev_NeiroModel.NeiroNet;

namespace MO_31_2_Varfolomeev_NeiroModel
{
    public partial class FormMain : Form
    {
        private double[] inputPixels; // хранение состояния пикселей (0 - белый, 1 - чёрный)
        private Network network; // объявление нейросети

        // для проверки создания весов (неактуально)
        //private NeiroNet.HiddenLayer hidden_layer1;
        //private NeiroNet.HiddenLayer hidden_layer2;
        //private NeiroNet.OutputLayer output_layer;

        // конструктор
        public FormMain()
        {
            InitializeComponent();

            inputPixels = new double[15];

            network = new Network(); // инициализайия нейросети
        }

        // обработчик кнопки
        private void Changing_State_Pixel_Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Black) // если кнопка чёрная
            {
                ((Button)sender).BackColor = Color.White; // меняем цвет
                inputPixels[((Button)sender).TabIndex] = 1d; 
            }
            else // если кнопка белая
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }

        // кнопка для сохранения Train
        private void Button_SaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();
        
            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; // новая строка

            File.AppendAllText(path, tmpStr);
        }



        // кнопка для сохранения Test
        private void Button_SaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; // новая строка

            File.AppendAllText(path, tmpStr);
        }

        // создание скрытго слоя (неактуально)
        /*
        private void button18_Click(object sender, EventArgs e)
        {
            hiddenlayer1 = new NeiroNet.HiddenLayer(10, 10, NeiroNet.NeironType.Hidden, nameof(hiddenlayer1));
        }
        */


        // создание весов (для проверки)
        /*
        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                // создаем все три слоя одной кнопкой
                hidden_layer1 = new NeiroNet.HiddenLayer(70, 15, NeiroNet.NeironType.Hidden, "hidden_layer1");
                hidden_layer2 = new NeiroNet.HiddenLayer(32, 70, NeiroNet.NeironType.Hidden, "hidden_layer2");
                output_layer = new NeiroNet.OutputLayer(10, 32, NeiroNet.NeironType.Output, "output_layer");

                // проверяем что все слои созданы успешно
                if (hidden_layer1 != null && hidden_layer2 != null && output_layer != null) // для вывода информации о состоянии слоёв
                {
                    MessageBox.Show("Все три слоя нейронной сети успешно созданы!\n\n" +
                                   "Архитектура сети: 15-70-32-10\n\n" +
                                   $"Скрытый слой 1: 70 нейронов (создан)\n" +
                                   $"Скрытый слой 2: 32 нейрона (создан)\n" +
                                   $"Выходной слой: 10 нейронов (создан)",
                                   "Успех",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка: не все слои были созданы",
                                  "Ошибка создания",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании нейронной сети: {ex.Message}",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
        */

        private void button_recognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixels);
            labelout.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelprobability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }


        // обработчик события клика кнопки "Обучить"
        private void button_Training_Click(object sender, EventArgs e)
        {
            network.Train(network);

            for (int i=0; i < network.E_errors_avr.Length; i++)
            {
                chart_Eavr.Series[0].Points.AddY(network.E_errors_avr[i]);
            }

            MessageBox.Show("Обучение успешно завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button_Test_Click(object sender, EventArgs e)
        {
            network.Test(network);

            for (int i = 0; i < network.E_errors_avr.Length; i++)
            {
                chart_Eavr.Series[0].Points.AddY(network.E_errors_avr[i]);
            }

            MessageBox.Show("Тестирование успешно завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
