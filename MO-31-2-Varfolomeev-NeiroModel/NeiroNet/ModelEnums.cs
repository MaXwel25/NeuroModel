namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    enum MemoryMode
    {
        GET, // считывание памяти
        SET, // сохранение памяти
        INIT // инициализация памяти
    }
    enum NeironType
    {
        Hidden, // скрытый
        Output // выходной
    }
    enum NetworkMode
    {
        Train, // обучение
        Test, // проверка
        Demo // распознавание
    }
}
