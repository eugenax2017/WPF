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
                    //Memory.Operation = sign;
                }
                else
                {
                    if (Memory.Press_Btn != TypeOfButton.Operator) // Memory.Press_Btn == TypeOfButton.Digit
                        Memory.Var2 = Result.Text;
                    else 
                    {                        
                        if (Memory.Press_Btn != TypeOfButton.Digit) // Memory.Press_Btn == TypeOfButton.Operator
                            Memory.Var2 = "";
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
            
            public static string MultiplicateNumbers()
            {
                return (double.Parse(Var1) * double.Parse(Var2)).ToString();
            }
            public static string DeductionNumbers()
            {
                return (double.Parse(Var1) - double.Parse(Var2)).ToString();
            }
            public static string DevideNumbers()
            {
                return (double.Parse(Var1) / double.Parse(Var2)).ToString();
            }
            public static string AddNumbers()
            {
                return (double.Parse(Var1) + double.Parse(Var2)).ToString();
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in Grid.Children)
            {
                if (item is Button)
                {
                    ((Button)item).Click += Button_Click;
                }
            }
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
                    {
                        Result.Text = current_button;
                    }
                    else if (current_result == "-0")
                        Result.Text = "-" + current_button;
                    else  
                    {
                        Result.Text += current_button;
                    }
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
                        if (Memory.Press_Btn == TypeOfButton.Digit)
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
                    {
                        Result.Text = "-0";                        
                    }
                    if (Memory.Press_Btn == TypeOfButton.Equality)
                        Memory.Clear();
                    Memory.Press_Btn = TypeOfButton.Digit;
                    break;
                case "AC":                    
                    Result.Text = "0";
                    Memory.Clear();
                    break;
                default:
                    break;
            }
            /*try
            {
                int val = Int32.Parse(current_button);
                if (current_result == "0")
                {
                    Result.Text = current_button;
                    Memory.Var1 = val;
                }
                else if (current_button.Length < Result.MaxLength)
                {
                    Result.Text += current_button;
                    Memory.Var1 = Memory.Var1 * 10 + val;

                }
            }
            catch (FormatException)
            {
                if (current_button == "AC")
                    Result.Text = "0";
                Result_int = Int32.Parse(Result.Text);

                MessageBox.Show(Result_int.ToString());
            }*/

        }
    }
}
