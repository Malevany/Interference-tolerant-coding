using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static ГрафичВып1.MainWindow;

namespace Курсовая_работа_ТиК
{
    
    public partial class HammingCodeWindow : Window
    {
        ObservableCollection<ObservableCollection<int>> GenerativeMatrix = new ObservableCollection<ObservableCollection<int>>()
        {
            new ObservableCollection<int> { 1, 0, 0, 0, 1, 0, 1 },
            new ObservableCollection<int> { 0, 1, 0, 0, 1, 1, 1 },
            new ObservableCollection<int> { 0, 0, 1, 0, 1, 1, 0 },
            new ObservableCollection<int> { 0, 0, 0, 1, 0, 1, 1 }
        };
        ObservableCollection<ObservableCollection<int>> VerificationMatrix = new ObservableCollection<ObservableCollection<int>>()
        {
            new ObservableCollection<int> { 1, 1, 1, 0, 1, 0, 0 },
            new ObservableCollection<int> { 0, 1, 1, 1, 0, 1, 0 },
            new ObservableCollection<int> { 1, 1, 0, 1, 0, 0, 1 },

        };
        List<SymbolData> dataCollection = new List<SymbolData>();
        int countPadding = 0;
        public HammingCodeWindow(string shannonCode, List<SymbolData> dataCollection)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//окно по центру
            //матрица
            GenerativeMatrixDataGrid.ItemsSource = GenerativeMatrix;//привязка к матрице DataGrid
            VerificationMatrixDataGrid.ItemsSource = VerificationMatrix;
            if (GenerativeMatrix.Count > 0)
            {
                GenerativeMatrixDataGrid = CreateDataGrid(GenerativeMatrixDataGrid, GenerativeMatrix[0].Count);//матрица проинициализирована значениями, то создание DataGrid
            }
            if (VerificationMatrix.Count > 0)
            {
                VerificationMatrixDataGrid = CreateDataGrid(VerificationMatrixDataGrid, VerificationMatrix[0].Count);//матрица проинициализирована значениями, то создание DataGrid
            }
            this.dataCollection = dataCollection;
            List<string> informativeBitsList = Padding(shannonCode);
            ExtendedHammingСode(informativeBitsList);
        }
        private List<string> Padding(string informativeBitsString)
        {
            List<string> codeStructure = new List<string>();
            for (int i = 0; i < informativeBitsString.Length; i++)
            {
                try
                {
                    codeStructure.Add(informativeBitsString.Substring(i, 4));
                    i += 3;
                }
                catch
                {
                    string tempString = informativeBitsString.Substring(i);
                    tempString += "1";
                    countPadding++;
                    while (tempString.Length < 4)
                    {
                        tempString += "0";
                        countPadding++;
                    }
                    codeStructure.Add(tempString);
                    break;
                }
            }
            return codeStructure;
        }
        //создание DataGrid (динамическое добавление столбцов)
        private DataGrid CreateDataGrid(DataGrid dgMatrix, int countColumn)
        {
            dgMatrix.Columns.Clear();
            if (countColumn > 0)
            {
                for (int i = 0; i < countColumn; i++)//столбец
                {
                    //формирование DataGrid
                    DataGridTextColumn column = new DataGridTextColumn
                    {
                        Header = (i + 1).ToString(),//заголовок стобца i
                        Binding = new Binding(String.Format("[{0}]", i))// биндинг (привязка) столбца
                    };// текстовый формат для столбца
                    dgMatrix.Columns.Add(column);//добавление столбца в DataGrid
                }
            }
            return dgMatrix;
        }
        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        public class KeyValue2
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }
        private void UpdateGrid1(List<KeyValuePair<string, string>> list)
        {
            List<KeyValue> myList = list.Select(x => new KeyValue() { Key = x.Key, Value = x.Value }).ToList();
            ErrorsAndSindroms.ItemsSource = myList;
        }
        private void ExtendedHammingСode(List<string> inputBits)
        {
            List<KeyValuePair<string, string>> SindromsAndErrors = new List<KeyValuePair<string, string>>();
            string output = string.Empty;
            List<string> bits = inputBits;
            int[,] E4 =  {
                { 1, 0, 0, 0},
                { 0, 1, 0, 0},
                { 0, 0 ,1, 0},
                { 0, 0, 0, 1}
            };
            int[,] E3 =  {
                { 1, 0, 0},
                { 0, 1, 0},
                { 0, 0 ,1},
            };
            int[,] R ={
                { 1, 0, 1},
                { 1, 1, 1},
                { 1, 1, 0},
                { 0, 1, 1}
            };
            foreach (var item in bits)
            {
                int[] V = ConvertToArray(item);
                int[,] G = ConcatenationMatrix(E4, R);
                int[] U = AddParityBit(MultiplicationPolynomial(V, G));
                foreach (var itemU in U)
                {
                    output += Convert.ToString(itemU);
                }
                output += "\n";
                int[,] TR = TransposeMatrix(R);
                int[,] H = ConcatenationMatrix(TR, E3);
                H = AddOneRowAndZeroColumn(H);
                int[,] HT = TransposeMatrix(H);
                int[] S = MultiplicationPolynomial(U, HT);
                string Ustring = string.Empty;
                foreach (var itemU in U)
                {
                    Ustring += Convert.ToString(itemU);
                }
                string Sstring = string.Empty;
                foreach (var itemS in S)
                {
                    Sstring += Convert.ToString(itemS);
                }
                SindromsAndErrors.Add(new KeyValuePair<string, string>(Ustring, Sstring));
            }

            UpdateGrid1(SindromsAndErrors);
            ErrorsChecker.Text = output.Trim('\n');
        }
        private static int[] ConvertToArray(string str)
        {
            int[] resArr = new int[str.Length];
            for (int i = 0; i < resArr.GetLength(0); i++)
            {
                resArr[i] = Convert.ToInt32(str[i].ToString());

            }
            return resArr;
        }
        private static int[,] ConcatenationMatrix(int[,] a, int[,] b)
        {
            int[,] resArr = new int[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < resArr.GetLength(0); i++)
            {
                for (int j = 0; j < resArr.GetLength(1); j++)
                {
                    if (j <= a.GetLength(1) - 1)
                        resArr[i, j] = a[i, j];
                    else
                        resArr[i, j] = b[i, j - a.GetLength(1)];
                }
            }
            return resArr;
        }
        private static int[,] AddOneRowAndZeroColumn(int[,] a)
        {
            int[,] b = new int[a.GetLength(0) + 1, a.GetLength(1) + 1];
            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    if (i == 0)
                        b[i, j] = 1;
                    else if (j == 0)
                        b[i, j] = 0;
                    else
                        b[i, j] = a[i - 1, j - 1];
                }
            }

            return b;
        }
        private static int[] AddParityBit(int[] a)
        {
            int[] b = new int[a.GetLength(0) + 1];
            int count = 0;
            foreach (var item in a)
            {
                if (item == 1)
                    count++;
            }
            for (int i = 0; i < a.GetLength(0) + 1; i++)
            {
                if (i == 0 && (count % 2 == 1))
                    b[i] = 1;
                else if (i == 0)
                    b[i] = 0;
                else
                    b[i] = a[i - 1];
            }
            return b;
        }
        private static int[] MultiplicationPolynomial(int[] v, int[,] g)
        {
            int[] u = new int[g.GetLength(1)];
            for (int i = 0; i < g.GetLength(1); i++)
            {
                int s = 0;
                for (int j = 0; j < g.GetLength(0); j++)
                {
                    if (g[j, i] == v[j] && v[j] == 1)
                        s += 1;
                }
                if (s % 2 == 1)
                    u[i] = 1;
                else
                    u[i] = 0;
            }
            return u;
        }
        public static int[,] TransposeMatrix(int[,] matrix)
        {
            int[,] resMatr = new int[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < resMatr.GetLength(0); i++)
            {
                for (int j = 0; j < resMatr.GetLength(1); j++)
                {
                    resMatr[i, j] = matrix[j, i];
                }
            }

            return resMatr;
        }
        private void UpdateGrid3(List<KeyValuePair<string, string>> list)
        {
            List<KeyValue> myList = list.Select(x => new KeyValue() { Key = x.Key, Value = x.Value }).ToList();
            Errors.ItemsSource = myList;
        }
        private void UpdateGrid4(List<string> list)
        {
            // Создайте список объектов KeyValue на основе элементов из list
            List<KeyValue> myList = list.Select(x => new KeyValue() { Key = x }).ToList();

            // Установите источник данных для вашего элемента управления (например, DataGrid)
            CorrectGrid.ItemsSource = myList;
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            int[,] E4 =  {
                    { 1, 0, 0, 0},
                    { 0, 1, 0, 0},
                    { 0, 0 ,1, 0},
                    { 0, 0, 0, 1}
                };
            int[,] E3 =  {
                    { 1, 0, 0},
                    { 0, 1, 0},
                    { 0, 0 ,1},
                };
            int[,] R ={
                    { 1, 0, 1},
                    { 1, 1, 1},
                    { 1, 1, 0},
                    { 0, 1, 1}
            };
            string text = ErrorsChecker.Text;
            int[,] TR = TransposeMatrix(R);
            int[,] H = ConcatenationMatrix(TR, E3);
            int[,] H84 = AddOneRowAndZeroColumn(H);
            List<KeyValuePair<string, string>> SindromsAndErrors = new List<KeyValuePair<string, string>>();
            string[] errors = text.Split('\n');
            int count = 1;
            foreach (var item in errors)
            {
                string temp = string.Empty;
                int[] U = ConvertToArray(item);
                int[,] H84T = TransposeMatrix(H84);
                int[] S = MultiplicationPolynomial(U, H84T);
                for (int k = 0; k < S.Length; k++)
                {
                    temp += S[k];
                }
                SindromsAndErrors.Add(new KeyValuePair<string, string>(temp, Convert.ToString(count)));
                UpdateGrid3(SindromsAndErrors);
                count++;
            }
            string output = CorrectError(H84, SindromsAndErrors);
            int numOfError;
            foreach (var item in SindromsAndErrors)
            {
                if (item.Key != "0000")
                {
                    numOfError = Convert.ToInt32(item.Value) - 1;
                    errors[numOfError] = output.Substring(0, 8);
                }
            }
            text = string.Empty;
            foreach (var item in errors)
            {
                text += item + "\n";
            }
            ErrorsChecker.Text = text.Trim('\n');
            List<string> list = new List<string>();
            string[] spl = output.Split('\n');
            for (int i = 0; i < spl.Length; i++)
            {
                list.Add(spl[i]);
            }
            UpdateGrid4(list);
        }
        private string CorrectError(int[,] H, List<KeyValuePair<string, string>> SindromsAndErrors)
        {
            string res = string.Empty;
            foreach (var item in SindromsAndErrors)
            {
                string tempS = item.Key;
                int index = 0;
                if (tempS != "0000")
                {
                    for (int i = 0; i < H.GetLength(1); i++)
                    {
                        string temp = string.Empty;
                        for (int j = 0; j < H.GetLength(0); j++)
                        {
                            temp += Convert.ToString(H[j, i]);
                        }
                        if (tempS == temp)
                        {
                            index = i;
                            break;
                        }
                    }
                    int num = Convert.ToInt32(item.Value);
                    string Errors = ErrorsChecker.Text;
                    string[] e = Errors.Split('\n');
                    int[] E = ConvertToArray(e[Convert.ToInt32(item.Value) - 1]);
                    if (E[index] == 1)
                        E[index] = 0;
                    else
                        E[index] = 1;
                    foreach (var itemE in E)
                    {
                        res += Convert.ToString(itemE);
                    }
                    res += "\n";
                }
            }
            return res;
        }
        private void DecodingButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> codeList = ErrorsChecker.Text.Split("\n").ToList();
            string codeString = "";
            foreach (var code in codeList)
            {
                codeString += code.Substring(1, 4);
            }
            codeString = codeString.Substring(0, codeString.Length - countPadding);
            string decodeString = DecodingString(codeString);
            DecodingTextBox.Text = decodeString;
        }
        private string DecodingString(string codeString)
        {
            bool flag = false;
            string tempString = codeString;
            string decodeString = string.Empty;
            for (int i = 0; i < codeString.Length; i++)
            {
                foreach (var item in dataCollection)
                {
                    if (tempString.IndexOf(item.symbolCode) == 0)
                    {
                        decodeString += item.symbol;
                        tempString = tempString.Substring(Convert.ToInt32(item.codeLength));
                        i += Convert.ToInt32(item.codeLength)-1;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    decodeString += "*";
                    tempString = tempString.Substring(1);
                }
                flag = false;
                if (tempString == "")
                    break;
            }

            return decodeString;
        }
    }
}
