# Помехоустойчивое кодирование
Данный проект является результатом выполнения курсовой работы по предмету 'Теория информации и кодирования' на тему 'Помехоустойчивое кодирование'.
Целью курсовой работы является изучение методов побуквенного и помехоустойчивого кодирования, отработка навыков реализации данных методов с использованием языка высокого уровня и графического интерфейса.

## Алгортим программы
В качестве метода побуквенного и помехоустойчивого кодирования были выбраны метод Шеннона и матричный способ расширенного кода Хэмминга (8,4) соответственно.

### Метод Шеннона
*Инициализация*. Все буквы из алфавита записываются в порядке убывания вероятностей в сообщении.
Каждой букве ставится в соответствие кумулятивная вероятность по правилу:
$$q_1 = 0, q_2 = 0, ... , q_m = \sum_{i=1}^{m-1}p_i$$
где m - мощность первичного алфавита.
*Цикл*. Кодовым словом для j-го сообщения является двоичная последовательность, представляющая собой первые $` L_j = ]-\log_2 P(x_j)[ `$ разрядов после запятой в двоичной записи числа $` q_j `$.
*Примечание*: Символ ] [ означает округление до ближайшего целого числа, больше данного. *Например*, ]2,1[ = 3, ]4[ = 4, ]-2,4[ = -2.
**Также вычисляются статистические характеристики кода.**
Максимальная энтропия: $` H_max = \log_2m `$
Безусловная энтропия Шеннона: 
$$H(X) = - \sum_{j=1}^{m}P(x_j) * \log_2 P(x_j)$$
Среднее число символов кода (средневзвешенная длина кода, математическое ожидание длины кода): 
$$L_{ср} = \sum_{i=1}^{x_n}{L_i * P_i}$$
Относительная избыточность кода: $` p_k = 1 - H(X)/L_{ср} `$
Избыточность кода показывает, на сколько бит тратится больше, чем могло бы быть затрачено в лучшем случае (при нулевой избыточности): $` R = L_{ср} - H(X) `$

### Матричный способ расширенного кода Хэмминга (8,4)
**Краткий алгоритм:**
1. На вход декодера подается сообщение в кодировке Unicode, например, буква "я".
Каждый 16-ти битный код буквы разделяется на четыре группы по 4 информативных бита (0000 0100 0100 1111).
Группа из 4 информативных бит кодируется расширенным кодом Хэмминга (матричный способ) в 8-ми битовую расширенную кодовую комбинацию (кодовые слова). Таким образом для каждой буквы на выходе кодера: 4 группы * 8 бит кодового слова = 32 бита.
На выходе кодера: последовательность бит кодовых комбинаций.
2. Имитируется передача последовательности бит по открытому, зашумленному каналу связи.
3. На вход декодера подаются как искаженные кодовые слова (одна, две, три ошибки), так и неизменные.
Для каждой расширенной кодовой комбинации (8 бит) вычисляется расширенный синдром (4 бита), выдается сообщение о количестве обнаруженных ошибок (или их отсутствии) с указанием мест (номер бита), результат устранения однократной ошибки.
После устранения однократной ошибки 4 информативных бита каждой расширенной кодовой комбинации объединяются в последовательность бит. Данная последовательность бит делится на группы по 16 бит, которые с помощью кодировки Unicode преобразуются в символы.
Результатом работы декодера является сообщение. 