using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Regex_Tester
{
    public partial class Form1 : Form
    {

        string source_code;

        string Number = @"([0-9]+(\.[0-9]+)?)";
        string String = "(\"(.*)\")";
        string ReservedKeyWords = "(int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|end)";
        string Comment_Statement = @"((\/\*)(.*[^(\*\/)])(\*\/))";
        string Identifiers = "([a-zA-Z][a-zA-Z0-9]*)";
        string FunctionName = "([a-zA-Z][a-zA-Z0-9]*)";
        string Arithmatic_Operator = @"(\+|-|\*|\/)";
        string Datatype = "(int|float|string)";
        string Condition_Operator = "(<>|<|>|=)";
        string Boolean_Operator = @"(&&|\|\|)";


        // composed
        string Function_Call;
        string Term;
        string Equation;
        string Expression;
        string Assignment_Statement;
        string Declaration_Statement;
        string Write_Statement;
        string Read_Statement;
        string Return_Statement;
        string Condition;
        string Condition_Statement;
        string If_Statement;
        string Else_If_Statement;
        string Else_Statement;
        string Repeat_Statement;
        string Parameter;
        string Function_Declaration;
        string Function_Body;
        string Function_Statement;
        string Main_Function;
        string Program;


        // sub routens
        string sub_Equation;
        string small_Equation;
        string small_term;
        string set_of_statement;



        List<string> Reserved = new List<string>();
        public Form1()
        {


            InitializeComponent();
            setRegex();

        }
        private void setRegex()
        {
            Reserved.Add("int");
            Reserved.Add("float");
            Reserved.Add("string");
            Reserved.Add("read");
            Reserved.Add("write");
            Reserved.Add("repeat");
            Reserved.Add("until");
            Reserved.Add("if");
            Reserved.Add("elseif");
            Reserved.Add("else");
            Reserved.Add("then");
            Reserved.Add("return");
            Reserved.Add("endl");
            Reserved.Add("end");
            Function_Call = "(" + Identifiers + @"\(" + "(" + "(" + Identifiers + "|" + Number + ")" + "(," + "(" + Identifiers + "|" + Number + ")" + ")*" + ")?" + @"\)" + ")";




            Term = "(" + Function_Call + "|" + Identifiers + "|" + Number + ")";




            //Equation = "(" + Term + "|" + @"(\(" + Term + "(" + Arithmatic_Operator + Term + @")+\))" + "(" + Arithmatic_Operator + "(" + Term + "|" + @"(\(" + Term + "(" + Arithmatic_Operator + Term + @")+\))" + "))+)";

            sub_Equation = @"(\(" + Term + "(" + Arithmatic_Operator + Term + @")+\))";
            Equation = "((" + Term + "(" + Arithmatic_Operator + "(" + Term + "|" + sub_Equation + "))+)|(" + sub_Equation + "(" + Arithmatic_Operator + "(" + Term + "|" + sub_Equation + "))*))";
            Equation = @"((\(" + Equation + @"\))|" + Equation + ")";

            //small_term = "(" + @"(\(" + Term + @"\))" + "|" + Term + ")";
            //sub_Equation = "(" + small_term + "(" + Arithmatic_Operator + small_term + ")+" + ")";
            //small_Equation = "(" + @"(\(" + sub_Equation + @"\))" + "|" + sub_Equation + ")";
            //Equation = "(" + "(" + small_Equation + "|" + small_term + ")" + "(" + Arithmatic_Operator + "(" + small_Equation + "|" + small_term + ")" + ")*" + ")";


            Expression = "(" + String + "|" + Term + "|" + Equation + ")";



            Assignment_Statement = "(" + Identifiers + ":=" + Expression + ")";

            Declaration_Statement = "(" + Datatype + "(" + Identifiers + "|" + Assignment_Statement + ")" + "(" + ",(" + Identifiers + "|" + Assignment_Statement + "))*" + ";)";


            Write_Statement = "(write" + "(" + Expression + "|" + "endl)" + ";)";

            Read_Statement = "(read" + Identifiers + ";)";

            Return_Statement = "(return" + Expression + ";)";


            Condition = "(" + Identifiers + Condition_Operator + Term + ")";




            Condition_Statement = "(" + Condition + "(" + Boolean_Operator + Condition + ")*" + ")";





            set_of_statement = "(" + Read_Statement + "|" + Write_Statement + "|" + Assignment_Statement + ";|" + Declaration_Statement + "|" + Comment_Statement + ")+";

            Repeat_Statement = "(" + "repeat" + set_of_statement + "until" + Condition_Statement + ")";

            set_of_statement = "(" + Repeat_Statement + "|" + Read_Statement + "|" + Write_Statement + "|" + Assignment_Statement + ";|" + Declaration_Statement + "|" + Comment_Statement + ")+";



            Else_Statement = "(" + "else" + set_of_statement + "end" + ")";


            Else_If_Statement += "(" + "(" + "elseif" + Condition_Statement + "then" + set_of_statement + ")+" + "(" + Else_Statement + "|" + "end" + ")" + ")";



            If_Statement = "(" + "if" + Condition_Statement + "then" + set_of_statement + "(" + Else_If_Statement + "|" + Else_Statement + "|" + "end" + ")" + ")";





            set_of_statement = "((" + If_Statement + "|" + Repeat_Statement + "|" + Read_Statement + "|" + Write_Statement + "|" + Assignment_Statement + ";|" + Declaration_Statement + "|" + Comment_Statement + ")+)";


            Parameter = "(" + Datatype + Identifiers + ")";
            Function_Declaration = "(" + Datatype + FunctionName + @"\(" + "(" + Parameter + "(" + "," + Parameter + ")*" + ")?" + @"\)" + ")";




            Function_Body = @"(\{" + set_of_statement + "?" + Return_Statement + @"\})";
            Function_Statement = "(" + Function_Declaration + Function_Body + ")";
            Main_Function = "(" + Datatype + @"main\(\)" + Function_Body + ")";
            Program = "(" + Function_Statement + "*" + Main_Function + ")";


            regx.Text = Program;

        }
        private void Compile_Click(object sender, EventArgs e)
        {
            string test = "^" + regx.Text + "$";
            Regex regex = new Regex(test);

            source_code = code.Text.Replace(" ", "").Replace("\r","").Replace("\n","").Replace("\t","").ToLower();
            if (regex.IsMatch(source_code))
            {
                MessageBox.Show("Done");
            }
        }

        private void code_TextChanged(object sender, EventArgs e)
        {


            //for (int i = 0; i < Reserved.Count; i++)
            //    Checkkeyword(Reserved[i], 0, Color.Blue);


        }

        private void Checkkeyword(string word, int startindex, Color color)
        {
            if (this.code.Text.Contains(word))
            {
                int index = -1;
                int selectstart = this.code.SelectionStart;

                while ((index = code.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.code.Select((index + startindex), word.Length);
                    this.code.SelectionColor = color;
                    this.code.Select(selectstart, 0);
                    this.code.SelectionColor = Color.Black;
                }
            }
        }
    }
}
