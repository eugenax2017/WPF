using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        enum TypeOfButton
        {
            Digit,
            Operator,
            Equality
        }
        static class Memory
        {
            public static string Var1{ get; set; }
            public static string Var2 { get; set; }
            public static string Operation { get; set; }
            public static TypeOfButton Press_Btn { get; set; }
            public static void FillMemory(TextBox Result, string sign)
            {
                if (String.IsNullOrEmpty(Memory.Operation) || Memory.Press_Btn == TypeOfButton.Equality)
                {
                    Memory.Var1 = Result.Text;
                    if (sign == "%")
                    {
                        Result.Text = Memory.CalculatePercent();
                        return;
                    }
                    Memory.Var2 = "";
                }
                else
                {
                    Memory.Var2 = Result.Text; // Memory.Press_Btn == TypeOfButton.Digit
                    if (Memory.Press_Btn != TypeOfButton.Digit) // Memory.Press_Btn == TypeOfButton.Operator
                       if (sign != "%") 
                            Memory.Var2 = "";             
                    if (sign == "%")
                    {
                        Result.Text = Memory.CalculatePercent();
                        if (Memory.Press_Btn == TypeOfButton.Operator) 
                            Memory.Var2 = Result.Text;
                        return;
                    }                            
                    if (!String.IsNullOrEmpty(Memory.Var1) && !String.IsNullOrEmpty(Memory.Var2))
                    {
                        Result.Text = Memory.Execute();
                        Memory.Var1 = Result.Text;
                    }                        
                }
                Memory.Operation = sign;
                Memory.Press_Btn = TypeOfButton.Operator;
            }
            
            public static void Clear()
            {
                Var1 = "";
                Var2 = "";
                Operation = "";
            }
            public static string Execute()
            {
               if (Operation == "+")
                    return AddNumbers();
               else if (Operation == "-")
                    return DeductionNumbers();
               else if (Operation == "/")
                    return DevideNumbers();
               else if (Operation == "x")
                    return MultiplicateNumbers();   
               return "";
            }
            public static string CalculatePercent()
            {
                if (String.IsNullOrEmpty(Memory.Var2))
                    return (decimal.Parse(Var1)/100).ToString();
                return (decimal.Parse(Var1)* decimal.Parse(Var2)/100).ToString();
            }

            public static string MultiplicateNumbers()
            {
                return (decimal.Parse(Var1) * decimal.Parse(Var2)).ToString();
            }
            public static string DeductionNumbers()
            {
                return (decimal.Parse(Var1) - decimal.Parse(Var2)).ToString();
            }
            public static string DevideNumbers()
            {
                return decimal.Parse(Var2) == 0 ? "0" : (decimal.Parse(Var1) / decimal.Parse(Var2)).ToString();
            }
            public static string AddNumbers()
            {
                return (decimal.Parse(Var1) + decimal.Parse(Var2)).ToString();
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in Grid.Children)
                if (item is Button)               
                    ((Button)item).Click += Button_Click;  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            /*if (sender is Button button)
            {
                button.Content = "New content";
            }*/
            string current_result;
            if (Memory.Press_Btn == TypeOfButton.Operator || Memory.Press_Btn == TypeOfButton.Equality)
                current_result = "0";
            else
                current_result = Result.Text;       
                
            string current_button = button.Content.ToString();
            switch (current_button)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":                
                    if (Memory.Press_Btn == TypeOfButton.Equality)
                        Memory.Clear();                                               
                    if (current_result == "0")                   
                        Result.Text = current_button;                   
                    else if (current_result == "-0")
                        Result.Text = "-" + current_button;
                    else      
                        Result.Text += current_button;       
                    Memory.Press_Btn = TypeOfButton.Digit;
                    break;
                case ",":
                    if (Memory.Press_Btn == TypeOfButton.Equality)
                    {
                        Memory.Clear();
                        Result.Text = "0,";
                    }                        
                    if (Memory.Press_Btn == TypeOfButton.Operator)                   
                        Result.Text = "0,";                  
                    else if (!Result.Text.Contains(","))                                       
                        Result.Text += current_button;                                     
                    Memory.Press_Btn = TypeOfButton.Digit;
                    break;
                case "+":
                    Memory.FillMemory(Result, "+");                    
                    break;
                case "-":
                    Memory.FillMemory(Result, "-");
                    break;
                case "x":
                    Memory.FillMemory(Result, "x");
                    break;
                case "/":
                    Memory.FillMemory(Result, "/");                    
                    break;
                case "=":
                    if (!String.IsNullOrEmpty(Memory.Var1) && !String.IsNullOrEmpty(Memory.Operation))
                    {
                        if (Memory.Press_Btn == TypeOfButton.Digit || String.IsNullOrEmpty(Memory.Var2))
                            Memory.Var2 = Result.Text;

                        Result.Text = Memory.Execute();
                    }                    
                    Memory.Var1 = Result.Text;
                    Memory.Press_Btn = TypeOfButton.Equality;
                    break;
                case "+/-":
                    if (Result.Text.Contains("-"))
                        Result.Text = Result.Text.Substring(1);
                    else
                        Result.Text = "-" + Result.Text;
                    if (Memory.Press_Btn == TypeOfButton.Operator)                   
                        Result.Text = "-0";            
                    if (Memory.Press_Btn == TypeOfButton.Equality)
                        Memory.Clear();
                    Memory.Press_Btn = TypeOfButton.Digit;
                    break;
                case "%":
                    if (Memory.Press_Btn == TypeOfButton.Equality)
                        Memory.Var2 = "";
                    Memory.FillMemory(Result, "%");
                    break;
                case "AC":                    
                    Result.Text = "0";
                    Memory.Clear();
                    break;
                default:
                    break;
            }
        }
    }
}
