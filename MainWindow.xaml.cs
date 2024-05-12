using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Курсовая_работа_ТиК;

namespace ГрафичВып1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class StatisticalCodeCharacteristics
        {
            public double shannonEntropy { get; set; }
            public double averageWeightedCodeLength { get; set; }
            public double relativeCodeRedundancy { get; set; }
            public double codeRedundancy { get; set; }
            public StatisticalCodeCharacteristics(double shannonEntropy, double averageWeightedCodeLength, double relativeCodeRedundancy, double codeRedundancy)
            {
                this.shannonEntropy = shannonEntropy;
                this.averageWeightedCodeLength = averageWeightedCodeLength;
                this.relativeCodeRedundancy = relativeCodeRedundancy;
                this.codeRedundancy= codeRedundancy;
            }
        }
        public class SymbolData
        {
            public string symbol { get; set; }
            public double probability { get; set; }
            public double cumulativeFrequency { get; set; }
            public double codeLength { get; set; }
            public string binaryCumulFreq { get; set; }
            public string symbolCode { get; set; }
            public SymbolData(string symbol, double probability, double cumulativeFrequency, double codeLength, string binaryCumulFreq, string symbolCode)
            {
                this.symbol = symbol;
                this.probability = probability;
                this.cumulativeFrequency = cumulativeFrequency;
                this.codeLength = codeLength;
                this.binaryCumulFreq = binaryCumulFreq;
                this.symbolCode = symbolCode;
            }
            
        }
        List<SymbolData> dataCollection = new List<SymbolData>();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private static Dictionary<string, double> FormAlphabet(string inputString)
        {
            Dictionary<string, double> alphabet = FormUnsortedAlhabet(inputString);
            alphabet = SortAlphabetByValue(alphabet);
            return alphabet;
        }
        private static Dictionary<string, double> FormUnsortedAlhabet(string inputString)
        {
            Dictionary<string, double> unsortedAlphabet = new Dictionary<string, double>();
            List<string> inputStringList = inputString.Select(c => c.ToString()).ToList();
            foreach (var symbol in inputStringList)
            {
                try
                {
                    double tempCount = inputString.Count(x => x == Convert.ToChar(symbol));
                    double tempProbability = tempCount / inputString.Length;
                    unsortedAlphabet.Add(symbol, tempProbability);
                }
                catch (System.ArgumentException)
                {

                    continue;
                }
            }
            return unsortedAlphabet;
        }
        private static Dictionary<string, double> SortAlphabetByValue(Dictionary<string, double> unsortedAlphabet)
        {
            Dictionary<string, double> sortedAlphabet = new Dictionary<string, double>();
            sortedAlphabet = unsortedAlphabet.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return sortedAlphabet;
        }
        private static double CalculateAverageWeightedCodeLength(List<double> li, List<double> probability)
        {
            double averageWeightedCodeLength = 0;
            for (int i = 0; i < li.Count; i++)
            {
                averageWeightedCodeLength += li[i] * probability[i];
            }
            return averageWeightedCodeLength;
        }
        public static double CalculateShannonEntropy(List<double> probabilities)
        {
            double shannonEntropy = 0;
            for (int i = 0; i < probabilities.Count(); i++)
            {
                shannonEntropy -=(probabilities[i] * Math.Log2(probabilities[i]));
            }
            shannonEntropy = Math.Round(shannonEntropy, 4);
            return shannonEntropy;
        }
        public static double CalculateRelativeCodeRedundancy (double shannonEntropy,double averageWeightedCodeLength)
        {
            return Math.Round((1 - (shannonEntropy / averageWeightedCodeLength)),4);
        }
        public static double CalculateCodeRedundancy(double averageWeightedCodeLength, double shannonEntropy)
        {
            return Math.Round((averageWeightedCodeLength - shannonEntropy),4);
        }
        private static string ConvertToBin(double num, double count)
        {
            string binnum = string.Empty;
            for (int i = 0; i < count; i++)
            {
                num *= 2;
                if (num == 0)
                    binnum += "0";
                else if (num >= 1)
                {
                    binnum += '1';
                    num -= 1;
                }
                else if(num < 1)
                    binnum += "0";
            }
            return binnum;
        }
        public static string EncryptMessage(Dictionary<string, string> codes, string sentance)
        {
            string final = "";
            for (int i = 0; i < sentance.Length; i++)
            {
                foreach (var item in codes)
                {
                    if (sentance[i] == Convert.ToChar(item.Key))
                    {
                        final += item.Value;
                    }
                }
            }
            return final;
        }
        private static List<double> GetDecimalPlac(List<double> tempList)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList[i] = Convert.ToInt32(Math.Ceiling(-Math.Log2(tempList[i])));
            }
            return tempList;
        }
        private static List<double> GetCumulProb(List<double> tempList)
        {
            double ComProb = 0.0;
            for (int i = 0; i < tempList.Count; i++)
            {
                double tempProb = tempList[i];
                tempList[i] = Math.Round(ComProb, 3);
                ComProb += tempProb;
            }
            return tempList;
        }
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string inputString = InputMessage_TextBox.Text;
            Dictionary<string, double> alphabet = FormAlphabet(inputString);
            List<string> symbols = alphabet.Keys.ToList();
            List<double> probabilitys = alphabet.Values.ToList();
            List<double> cumulativeFrequencyList = GetCumulProb(alphabet.Values.ToList());
            List<double> codeLengthList = GetDecimalPlac(alphabet.Values.ToList());
            List<string> binaryCumulFreqList = new List<string>();
            List<string> symbolCode = new List<string>();
            for (int i = 0; i < cumulativeFrequencyList.Count; i++)
            {
                binaryCumulFreqList.Add("0," + ConvertToBin(cumulativeFrequencyList[i], codeLengthList[i]));
                symbolCode.Add(ConvertToBin(cumulativeFrequencyList[i], codeLengthList[i]));
            }
            
            Dictionary<string, string> alphabetSymbolCode = new Dictionary<string, string>();
            int j = 0;
            foreach (var symbol in alphabet)
            {
                alphabetSymbolCode.Add(symbol.Key, symbolCode[j]);
                j++;
            }

            
            for (int i = 0; i < symbols.Count(); i++)
            {
                dataCollection.Add(new SymbolData(symbols[i], probabilitys[i], cumulativeFrequencyList[i], codeLengthList[i], binaryCumulFreqList[i], symbolCode[i]));
            }

            FillDataGrid(dataCollection);

            double shannonEntropy = CalculateShannonEntropy(probabilitys);
            double averageWeightedCodeLength = CalculateAverageWeightedCodeLength(codeLengthList, probabilitys);
            double relativeCodeRedundancy = CalculateRelativeCodeRedundancy(shannonEntropy, averageWeightedCodeLength);
            double codeRedundancy = CalculateCodeRedundancy(averageWeightedCodeLength, shannonEntropy);
            StatisticalCodeCharacteristics statisticalCodeCharacteristics = new StatisticalCodeCharacteristics(shannonEntropy, averageWeightedCodeLength, relativeCodeRedundancy, codeRedundancy);
            FillTextBoxes(statisticalCodeCharacteristics, alphabetSymbolCode, inputString);

            
            if (averageWeightedCodeLength <= shannonEntropy + 1)
            {
                CodeOptimality_TextBox.Text = "Оптимальный";
            }
            else
            {
                CodeOptimality_TextBox.Text = "Неоптимальный";
            }
        }
        private void FillTextBoxes(StatisticalCodeCharacteristics statisticalCodeCharacteristics, Dictionary<string, string> alphabetSymbolCode, string inputString)
        {
            ShannonEntropy_TextBox.Text = Convert.ToString(statisticalCodeCharacteristics.shannonEntropy);
            AverageWeightedCodeLength_TextBox.Text = Convert.ToString(statisticalCodeCharacteristics.averageWeightedCodeLength);
            RelativeCodeRedundancy_TextBox.Text = Convert.ToString(statisticalCodeCharacteristics.relativeCodeRedundancy);
            CodeRedundancy_TextBox.Text = Convert.ToString(statisticalCodeCharacteristics.codeRedundancy);
            EncryptMessage_TextBox.Text = EncryptMessage(alphabetSymbolCode, inputString);
        }
        private void FillDataGrid(List<SymbolData> dataCollection)
        {
            AlphabetDataGrid.ItemsSource = dataCollection;
            AlphabetDataGrid.Columns[0].Header = "Символ";
            AlphabetDataGrid.Columns[1].Header = "Вероятность";
            AlphabetDataGrid.Columns[2].Header = "Qi";
            AlphabetDataGrid.Columns[3].Header = "Li";
            AlphabetDataGrid.Columns[4].Header = "Qi(2)";
            AlphabetDataGrid.Columns[5].Header = "Код";
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ShannonEntropy_TextBox.Text = "";
            AverageWeightedCodeLength_TextBox.Text = "";
            RelativeCodeRedundancy_TextBox.Text = "";
            CodeRedundancy_TextBox.Text = "";
            CodeOptimality_TextBox.Text = "";
            EncryptMessage_TextBox.Text = "";
            AlphabetDataGrid.ItemsSource = null;
            dataCollection.Clear();
        }
        private void HammingCodeButton_Click(object sender, RoutedEventArgs e)
        {
            HammingCodeWindow hammingCodeWindow = new HammingCodeWindow(EncryptMessage_TextBox.Text, dataCollection);
            hammingCodeWindow.Show();
        }
    }
}
