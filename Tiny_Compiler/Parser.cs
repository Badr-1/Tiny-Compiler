using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        // Program → FunctionStatements MainFunction
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(FunctionStatements());
            program.Children.Add(Main());
            MessageBox.Show("Success");
            return program;
        }

        //MainFunction → DataType main () FunctionBody
        Node Main()
        {

            Node main = new Node("Main");
            main.Children.Add(DataType());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LeftParentheses));
            main.Children.Add(match(Token_Class.RightParentheses));
            main.Children.Add(FunctionBody());
            return main;
        }

        // FunctionStatements → FunctionStatement functions | ε
        Node FunctionStatements()
        {
            Node functionStatements = new Node("FunctionStatements");
            if (InputPointer < TokenStream.Count && isItADataType() && InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type != Token_Class.Main)
            {
                functionStatements.Children.Add(FunctionStatement());
                functionStatements.Children.Add(Functions());
                return functionStatements;
            }
            else
            {
                return null;
            }
        }

        // functions → FunctionStatement functions | ε
        Node Functions()
        {
            Node functions = new Node("Functions");
            if (InputPointer < TokenStream.Count && isItADataType() && InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type != Token_Class.Main)
            {
                functions.Children.Add(FunctionStatement());
                functions.Children.Add(Functions());
                return functions;
            }
            else
            {
                return null;
            }
        }

        // FunctionDeclartion → DataType identifer (Paramter Paramters)
        Node FunctionDeclartion()
        {
            Node functionDeclartion = new Node("FunctionDeclartion");
            functionDeclartion.Children.Add(DataType());
            functionDeclartion.Children.Add(match(Token_Class.Idenifier));
            functionDeclartion.Children.Add(match(Token_Class.LeftParentheses));
            functionDeclartion.Children.Add(Paramter());
            functionDeclartion.Children.Add(Paramters());
            functionDeclartion.Children.Add(match(Token_Class.RightParentheses));

            return functionDeclartion;
        }

        // Paramter → DataType Identifier | ε
        Node Paramter()
        {
            Node paramter = new Node("Paramter");
            if (InputPointer < TokenStream.Count && isItADataType())
            {
                paramter.Children.Add(DataType());
                paramter.Children.Add(match(Token_Class.Idenifier));
                return paramter;
            }
            else
            {
                return null;
            }
        }

        // Paramters → , DataType Identifier Paramters |  ε
        Node Paramters()
        {
            Node paramters = new Node("Paramters");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                paramters.Children.Add(match(Token_Class.Comma));
                paramters.Children.Add(DataType());
                paramters.Children.Add(match(Token_Class.Idenifier));
                paramters.Children.Add(Paramters());
                return paramters;
            }
            else
            {
                return null;
            }
        }

        // FunctionBody → { Statements ReturnStatement }
        Node FunctionBody()
        {
            Node functionBody = new Node("FunctionBody");
            functionBody.Children.Add(match(Token_Class.LeftBraces));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(ReturnStatement());
            functionBody.Children.Add(match(Token_Class.RightBraces));
            return functionBody;
        }

        // FunctionStatement → FunctionDeclartion FunctionBody
        Node FunctionStatement()
        {
            Node functionStatement = new Node("FunctionStatement");
            functionStatement.Children.Add(FunctionDeclartion());
            functionStatement.Children.Add(FunctionBody());
            return functionStatement;
        }



        //1.Statements → Statement State
        Node Statements()
        {
            Node statments = new Node("Statements");
            statments.Children.Add(Statement());
            statments.Children.Add(State());
            return statments;
        }


        // 2.State  → Statement State | ε
        Node State()
        {

            Node state = new Node("State");
            if (InputPointer < TokenStream.Count && isItAStartOfStatement())
            {
                state.Children.Add(Statement());
                state.Children.Add(State());
                return state;
            }
            else
            {
                return null;
            }

        }


        /* Statement -> comment
         *       | IfStatement
         *       | ReadStatement
         *       | WriteStatement
         *       | RepeatStatement
         *       | DeclarationStatement
         *       | AssignmentStatement;
         *       | FunctionCall;
         *       | ε
         */
        Node Statement()
        {
            Node statement = new Node("Statement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comment)
            {
                statement.Children.Add(match(Token_Class.Comment));
                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statement.Children.Add(IfStatement());
                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(ReadStatement());
                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statement.Children.Add(WriteStatement());
                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                statement.Children.Add(RepeatStatement());
                return statement;
            }
            else if (InputPointer < TokenStream.Count && isItADataType())
            {
                statement.Children.Add(DeclarationStatement());
                return statement;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.Assign)
                {
                    statement.Children.Add(AssignmentStatement());      
                }
                else
                {
                    statement.Children.Add(FunctionCall());
                    
                }
                statement.Children.Add(match(Token_Class.Semicolon));
                return statement;
            }
            else
                return null;
        }

        //DeclarationStatement → DataType VarsDeclartion;
        Node DeclarationStatement()
        {
            Node declaration_Statement = new Node("Declaration_Statement");
            declaration_Statement.Children.Add(DataType());
            declaration_Statement.Children.Add(VarsDeclation());
            declaration_Statement.Children.Add(match(Token_Class.Semicolon));
            return declaration_Statement;
        }


        //VarsDeclartion → VarDeclation Declartions
        Node VarsDeclation()
        {
            Node varsDeclation = new Node("VarsDeclation");
            varsDeclation.Children.Add(VarDeclation());
            varsDeclation.Children.Add(Declartions());
            return varsDeclation;
        }
       
        //VarDeclation → identifier | AssignmentStatement
        Node VarDeclation()
        {
            Node varDeclation = new Node("VarDeclation");
            if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer+1].token_type == Token_Class.Assign)
            {
                varDeclation.Children.Add(AssignmentStatement());
            }
            else
            {
                varDeclation.Children.Add(match(Token_Class.Idenifier));
            }
            return varDeclation;
        }

        //Declartions → ,VarDeclation Declartions | ε
        Node Declartions()
        {
            Node declartions = new Node("Declartions");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declartions.Children.Add(match(Token_Class.Comma));
                declartions.Children.Add(VarDeclation());
                declartions.Children.Add(Declartions());
                return declartions;
            }
            else
            {
                return null;
            }
        }
        //AssignmentStatement → identifier := epx
        Node AssignmentStatement()
        {
            Node assignmentStatement = new Node("AssignmentStatement");
            assignmentStatement.Children.Add(match(Token_Class.Idenifier));
            assignmentStatement.Children.Add(match(Token_Class.Assign));
            assignmentStatement.Children.Add(Expression());
            return assignmentStatement;
        }

    

        //Term → Number | idetifier | fun-call | equations
        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer+1].token_type == Token_Class.LeftParentheses)
                    term.Children.Add(FunctionCall());
                else
                    term.Children.Add(match(Token_Class.Idenifier));
            }
            else
            {
                term.Children.Add(Equation());
                return term;
            }
            return term;
        }

        // FunctionCall → identifier (arg args) 
        Node FunctionCall()
        {
            Node functionCall = new Node("FunctionCall");
            functionCall.Children.Add(match(Token_Class.Idenifier));
            functionCall.Children.Add(match(Token_Class.LeftParentheses));
            functionCall.Children.Add(Arg());
            functionCall.Children.Add(Args());
            functionCall.Children.Add(match(Token_Class.RightParentheses));
            return functionCall;

        }

        // args → , Term args |  ε
        Node Args()
        {
            Node args = new Node("Args");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                args.Children.Add(match(Token_Class.Comma));
                args.Children.Add(Term());
                args.Children.Add(Args());
                return args;
            }
            else
            {
                return null;
            }
        }

        // arg → Term | ε
        Node Arg()
        {
            Node arg = new Node("Arg");
            if (InputPointer < TokenStream.Count && isItAStartOfATerm(0))
            {
                arg.Children.Add(Term());
                return arg;
            }
            else
            {
                return null;
            }
        }



        // WriteStatement → write (exp|Endl);       
        Node WriteStatement()
        {
            Node writeStatement = new Node("WriteStatement");
            writeStatement.Children.Add(match(Token_Class.Write));
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endline)
                writeStatement.Children.Add(match(Token_Class.Endline));
            else
                writeStatement.Children.Add(Expression());
            writeStatement.Children.Add(match(Token_Class.Semicolon));
            return writeStatement;
        }

        // ReadStatement → read identifier;
        Node ReadStatement()
        {
            Node readStatement = new Node("ReadStatement");
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Idenifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));
            return readStatement;
        }

        //  ReturnStatement → return exp;
        Node ReturnStatement()
        {
            Node returnStatement = new Node("ReturnStatement");
            returnStatement.Children.Add(match(Token_Class.Return));
            returnStatement.Children.Add(Expression());
            returnStatement.Children.Add(match(Token_Class.Semicolon));
            return returnStatement;
        }



        // Condition → identifier ConOp term
        Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Idenifier));
            condition.Children.Add(ConOp());
            condition.Children.Add(Term());
            return condition;
        }


        // ConditionStatement → condition Conds
        Node ConditionStatement()
        {
            Node conditionStatement = new Node("ConditionStatement");
            conditionStatement.Children.Add(Condition());
            conditionStatement.Children.Add(Conds());
            return conditionStatement;
        }

        // Conds → Boolop Condition Conds |  ε
        Node Conds()
        {
            Node conds = new Node("Conds");
            if (InputPointer < TokenStream.Count && isItAStartOfAnotherCondition())
            {
                conds.Children.Add(BoolOp());
                conds.Children.Add(Condition());
                conds.Children.Add(Conds());
                return conds;
            }
            else
            {
                return null;
            }
        }

        // RepeatStatement → repeat Statements until ConditionStatement
        Node RepeatStatement()
        {
            Node repeatStatement = new Node("RepeatStatement");
            repeatStatement.Children.Add(match(Token_Class.Repeat));
            repeatStatement.Children.Add(Statements());
            repeatStatement.Children.Add(match(Token_Class.Until));
            repeatStatement.Children.Add(ConditionStatement());
            return repeatStatement;
        }




        //DataType → int | float | string
        Node DataType()
        {
            Node dataType = new Node("DataType");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DataType_Int)
            {
                dataType.Children.Add(match(Token_Class.DataType_Int));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DataType_String)
            {
                dataType.Children.Add(match(Token_Class.DataType_String));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.DataType_Float)
            {
                dataType.Children.Add(match(Token_Class.DataType_Float));
            }
            return dataType;
        }


        //IfStatement → if ConditionStatement then Statements ElseIfStatements ElseStatement end
        Node IfStatement()
        {
            Node ifStatement = new Node("IfStatement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(ConditionStatement());
            ifStatement.Children.Add(match(Token_Class.Then));
            ifStatement.Children.Add(Statements());
            ifStatement.Children.Add(ElseIfStatements());
            ifStatement.Children.Add(ElseStatement());

            ifStatement.Children.Add(match(Token_Class.End));
            return ifStatement;


        }

        //ElseIfStatements → ElseIfStatement ElseIfs
        Node ElseIfStatements()
        {
            Node elseIfStatements = new Node("ElseIfStatements");
            elseIfStatements.Children.Add(ElseIfStatement());
            elseIfStatements.Children.Add(ElseIfs());
            return elseIfStatements;

        }
        
        // ElseIfs → ElseIfStatement ElseIfs | ε
        Node ElseIfs()
        {
            Node elseIfs = new Node("ElseIfs");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                elseIfs.Children.Add(ElseIfStatement());
                elseIfs.Children.Add(ElseIfs());
                return elseIfs;
            }
            else
            {
                return null;
            }


        }

        // ElseIfStatement → elseif ConditionStatement then Statements | ε
        Node ElseIfStatement()
        {
            Node elseIfStatement = new Node("ElseIfStatement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                elseIfStatement.Children.Add(match(Token_Class.Elseif));
                elseIfStatement.Children.Add(ConditionStatement());
                elseIfStatement.Children.Add(match(Token_Class.Then));
                elseIfStatement.Children.Add(Statements());
                return elseIfStatement;
            }
            else
            {
                return null;
            }
        }

        // ElseStatement → else Statements | ε
        Node ElseStatement()
        {
            Node elseStatement = new Node("ElseStatement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                elseStatement.Children.Add(match(Token_Class.Else));
                elseStatement.Children.Add(Statements());
                return elseStatement;
            }
            else
            {
                return null;
            }

        }


        //Expression → string | Term | Equation
        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                expression.Children.Add(match(Token_Class.String));
            }
            // TODO: Ask if possible
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LeftParentheses)
            {
                expression.Children.Add(Equation());
            }
            else if (InputPointer < TokenStream.Count && isItAStartOfATerm(0))
            {
                if (InputPointer + 1 < TokenStream.Count && (isItAMultOP(1) || isItAnAddOP(1)))
                    expression.Children.Add(Equation());
                else
                    expression.Children.Add(Term());
            }
            return expression;
        }


        // Equation → SubEquation SubEquations
        Node Equation ()
        {
            Node equation = new Node("Equation");
            equation.Children.Add(SubEquation());
            equation.Children.Add(SubEquations());
            return equation;

        }
        // SubEquations → AddOp SubEquation SubEquations | ε
        Node SubEquations()
        {
            Node subEquations = new Node("SubEquations");
            if (InputPointer < TokenStream.Count && !(TokenStream[InputPointer].token_type == Token_Class.Minus || TokenStream[InputPointer].token_type == Token_Class.Plus))
                return null;
            subEquations.Children.Add(AddOp());
            subEquations.Children.Add(SubEquation());
            subEquations.Children.Add(SubEquations());
            return subEquations;
        }
        // SubEquation → Equ E
        Node SubEquation()
        {
            Node subEquation = new Node("SubEquation");
            subEquation.Children.Add(Equ());
            subEquation.Children.Add(E());
            return subEquation;
        }
        // E → MultOp Equ E | ε
        Node E()
        {
            Node e = new Node("E");
            if (InputPointer < TokenStream.Count && !(TokenStream[InputPointer].token_type == Token_Class.Multiply || TokenStream[InputPointer].token_type == Token_Class.Division))
                return null;
            e.Children.Add(MultOp());
            e.Children.Add(Equ());
            e.Children.Add(E());
            return e;
        }
        //Equ → EquationWith | EquationWithout
        Node Equ()
        {
            Node equ = new Node("Equ");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LeftParentheses)
            {
                equ.Children.Add(EquationWith());
            }
            else
            {
                equ.Children.Add(EquationWithout());
            }
            return equ;
        }


        // EquationWith → (Oprand Equations)
        // EquationWithout → Oprand Equations
        // Equations → AddOp Oprand Equations | ε
        // Oprand → Term Ter
        // Ter → MultOp Term Ter | ε
        //Term → Number | idetifier | fun-call


        // EquationWith → (Oprand Equations)
        Node EquationWith()
        {
            Node equationWith = new Node("EquationWith");
            equationWith.Children.Add(match(Token_Class.LeftParentheses));
            equationWith.Children.Add(Oprand());
            equationWith.Children.Add(Equations());
            equationWith.Children.Add(match(Token_Class.RightParentheses));
            return equationWith;
        }
        // EquationWithout → Oprand Equations
        Node EquationWithout ()
        {
            Node equationWithout = new Node("EquationWithout");
            equationWithout.Children.Add(Oprand());
            equationWithout.Children.Add(Equations());
            return equationWithout;
        }
        // Equations → AddOp Oprand Equations | ε
        Node Equations()
        {
            Node equations = new Node("Equations");
            if (InputPointer < TokenStream.Count && isItAnAddOP(0))
            {
                equations.Children.Add(AddOp());
                equations.Children.Add(Oprand());
                equations.Children.Add(Equations());

                return equations;
            }
            else
            {
                return null;
            }
        }
        // Oprand → Term Ter
        Node Oprand()
        {
            Node oprand = new Node("Oprand");
            oprand.Children.Add(Term());
            oprand.Children.Add(Ter());
            return oprand;
        }
        // Ter → MultOp Term Ter | ε
        Node Ter()
        {
            Node ter = new Node("Ter");
            if (InputPointer + 1 < TokenStream.Count && isItAMultOP(0) && isItAStartOfATerm(1))
            {

                ter.Children.Add(MultOp());
                ter.Children.Add(Term());
                ter.Children.Add(Ter());
                return ter;
            }
            else
            {
                return null;
            }
        }
        //Term → Number | idetifier | fun-call | EquationWith

        // EquationWith → (Oprand Equations)
        // Equations → AddOp Oprand Equations | ε
        // Oprand → Term Ter
        // Ter → MultOp Term Ter | ε
        //Term → Number | idetifier | fun-call



        //ConOp → = | <> | > | < 
        Node ConOp()
        {
            Node conOp = new Node("ConOp");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Equal)
            {
                conOp.Children.Add(match(Token_Class.Equal));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.NotEqual)
            {
                conOp.Children.Add(match(Token_Class.NotEqual));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.GreaterThan)
            {
                conOp.Children.Add(match(Token_Class.GreaterThan));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LessThan)
            {
                conOp.Children.Add(match(Token_Class.LessThan));
            }
            return conOp;
        }

        // BoolOp →  || | && 
        Node BoolOp()
        {
            Node boolOp = new Node("BoolOp");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Or)
            {
                boolOp.Children.Add(match(Token_Class.Or));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.And)
            {
                boolOp.Children.Add(match(Token_Class.And));
            }
            return boolOp;
        }

        //AddOp → + | -
        Node AddOp()
        {
            Node addOp = new Node("AddOp");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Plus)
            {
                addOp.Children.Add(match(Token_Class.Plus));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Minus)
            {
                addOp.Children.Add(match(Token_Class.Minus));
            }
            return addOp;
        }

        // 	MultOp → * | / 
        Node MultOp()
        {
            Node multOp = new Node("MultOp");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Multiply)
            {
                multOp.Children.Add(match(Token_Class.Multiply));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Division)
            {
                multOp.Children.Add(match(Token_Class.Division));
            }
            return multOp;
        }

        bool isItAStartOfStatement()
        {
            return TokenStream[InputPointer].token_type == Token_Class.Comment
                            ||
                        TokenStream[InputPointer].token_type == Token_Class.If
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.Read
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.Write
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.Repeat
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.If
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.Idenifier
                            ||
                            isItADataType()
                            ;
        }

        bool isItADataType()
        {
            return TokenStream[InputPointer].token_type == Token_Class.DataType_Float
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.DataType_Int
                            ||
                            TokenStream[InputPointer].token_type == Token_Class.DataType_String;
        }

        bool isItAMultOP(int offset)
        {
            return TokenStream[InputPointer + offset].token_type == Token_Class.Division
                            ||
                            TokenStream[InputPointer + offset].token_type == Token_Class.Multiply;
        }

        bool isItAnAddOP(int offset)
        {
            return TokenStream[InputPointer + offset].token_type == Token_Class.Plus
                            ||
                            TokenStream[InputPointer+ offset].token_type == Token_Class.Minus;
        }

        bool isItAStartOfATerm(int offset)
        {
            return TokenStream[InputPointer+offset].token_type == Token_Class.Number
                            ||
                            TokenStream[InputPointer+offset].token_type == Token_Class.Idenifier;
        }

        bool isItAStartOfAnotherCondition()
        {
            return (TokenStream[InputPointer].token_type == Token_Class.And || TokenStream[InputPointer].token_type == Token_Class.Or);
        }
        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());
                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
        
    }
}
