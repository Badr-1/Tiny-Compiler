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
        bool inFunctionBody = false;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        // CFGs
        // Program → [FunctionStatements] MainFunction
        // MainFunction → DataType main () FunctionBody
        // FunctionStatements → FunctionStatement FunctionStatements | ε
        // Paramters → DataType Identifier OtherParamters | ε
        // OtherParamters → , DataType Identifier OtherParamters |  ε
        // FunctionStatement → FunctionDeclartion FunctionBody
        // FunctionDeclartion → DataType identifer (Paramters)
        // FunctionBody → { Statements ReturnStatement }
        // Statements → Statement State
        // State  → Statement State | ε
        /* Statement -> IfStatement
            *       | ReturnStatement
            *       | ReadStatement
            *       | WriteStatement
            *       | RepeatStatement
            *       | DeclarationStatement
            *       | AssignmentStatement;
            *       | FunctionCall;
            *       | ε
            */
        // DeclarationStatement → DataType VarsDeclartion; 
        // VarsDeclartion → identifier Initialization Declartions
        // Declartions → , identifier Initialization Declartions | ε
        // Initialization → := Expression | ε
        // AssignmentStatement → identifier := epx
        // Term → Number | idetifier | FunctionCall 
        // FunctionCall → identifier (Args) 
        // Args → Arg OtherArgs |  ε
        // OtherArgs → , Arg OtherArgs | ε
        // Arg - > Identifier | Number
        // WriteStatement → write (Expression|Endl);       
        // ReadStatement → read identifier;
        // ReturnStatement → return exp;
        // Condition → identifier ConOp term
        // ConditionStatement → condition Conditions
        // Conditions → Boolop Condition Conditions |  ε
        // RepeatStatement → repeat Statements until ConditionStatement
        // DataType → int | float | string
        // IfStatement → if ConditionStatement then Statements [ElseifStatements] [ElseStatement] end
        // ElseIfStatements → elseif ConditionStatement then Statements ElseIfStatements | ε
        // ElseStatement → else Statements 
        // Expression → string | Term | Equation7
        // TODO: ReWrite Equation
        // BoolOp →  || | && 
        // AddOp → + | -
        // MultOp → * | / 
        // Equation → SubEquation SubEquations
        // SubEquations → AddOp SubEquation SubEquations | ε
        // SubEquation → SmallEquation E
        // E → MultOp SmallEquation E | ε
        // SmallEquation → Oprand Equations | (Oprand Equations)
        // Equations → AddOp Oprand Equations | ε
        // Oprand → Term Ter
        // Ter → MultOp Term Ter | ε









        // Program → [FunctionStatements] MainFunction
        Node Program()
        {
            Node program = new Node("Program");
            if (InputPointer < TokenStream.Count && isItADataType() && InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type != Token_Class.Main)
                program.Children.Add(FunctionStatements());
            program.Children.Add(Main());
           // MessageBox.Show("Success");
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

        // FunctionStatements → FunctionStatement FunctionStatements | ε
        Node FunctionStatements()
        {
            Node functionStatements = new Node("FunctionStatements");
            if (InputPointer < TokenStream.Count && isItADataType() && InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type != Token_Class.Main)
            {
                functionStatements.Children.Add(FunctionStatement());
                functionStatements.Children.Add(FunctionStatements());
                return functionStatements;
            }
            else
            {
                return null;
            }
        }

        // FunctionDeclartion → DataType identifer (Paramters)
        Node FunctionDeclartion()
        {
            Node functionDeclartion = new Node("FunctionDeclartion");
            functionDeclartion.Children.Add(DataType());
            functionDeclartion.Children.Add(match(Token_Class.Identifier));
            functionDeclartion.Children.Add(match(Token_Class.LeftParentheses));
            functionDeclartion.Children.Add(Paramters());
            functionDeclartion.Children.Add(match(Token_Class.RightParentheses));

            return functionDeclartion;
        }

        // Paramters → DataType Identifier OtherParamters | ε
        Node Paramters()
        {
            Node paramters = new Node("Paramters");
            if (InputPointer < TokenStream.Count && isItADataType())
            {
                paramters.Children.Add(DataType());
                paramters.Children.Add(match(Token_Class.Identifier));
                paramters.Children.Add(OtherParamters());
                return paramters;
            }
            else
            {
                return null;
            }
        }

        // OtherParamters → , DataType Identifier OtherParamters |  ε
        Node OtherParamters()
        {
            Node otherparamters = new Node("Paramters");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                otherparamters.Children.Add(match(Token_Class.Comma));
                otherparamters.Children.Add(DataType());
                otherparamters.Children.Add(match(Token_Class.Identifier));
                otherparamters.Children.Add(OtherParamters());
                return otherparamters;
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
            inFunctionBody = true;
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



        // Statements → Statement State
        Node Statements()
        {
            Node statments = new Node("Statements");
            statments.Children.Add(Statement());
            statments.Children.Add(State());
            return statments;
        }


        // State  → Statement State | ε
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


        /* Statement ->
         *         IfStatement
         *       | ReturnStatement
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
           /* if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comment)
            {
                statement.Children.Add(match(Token_Class.Comment));
                return statement;
            }
            else*/ if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statement.Children.Add(IfStatement());
                return statement;
            }
            // TODO: needs to be added
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Return && !inFunctionBody)
            {
                statement.Children.Add(ReturnStatement());
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
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.Assign)
                    statement.Children.Add(AssignmentStatement());
                else
                    statement.Children.Add(FunctionCall());
                statement.Children.Add(match(Token_Class.Semicolon));
                return statement;
            }
            else
            {
                return null;
            }
            
        }

        // DeclarationStatement → DataType VarsDeclartion; 
        Node DeclarationStatement()
        {
            Node declaration_Statement = new Node("DeclarationStatement");
            declaration_Statement.Children.Add(DataType());
            declaration_Statement.Children.Add(VarsDeclation());
            declaration_Statement.Children.Add(match(Token_Class.Semicolon));
            return declaration_Statement;
        }


        // VarsDeclartion → identifier Initialization Declartions
        Node VarsDeclation()
        {
            Node varsDeclation = new Node("VarsDeclation");
            varsDeclation.Children.Add(match(Token_Class.Identifier));
            varsDeclation.Children.Add(Initialization());
            varsDeclation.Children.Add(Declartions());
            return varsDeclation;
        }


        // Initialization → := Expression | ε
        Node Initialization()
        {
            Node initialization = new Node("Initialization");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Assign)
            {
                initialization.Children.Add(match(Token_Class.Assign));
                initialization.Children.Add(Expression());
                return initialization;
            }
            else
            {
                return null;
            }
        }

        // Declartions → , identifier Initialization Declartions | ε
        Node Declartions()
        {
            Node declartions = new Node("Declartions");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declartions.Children.Add(match(Token_Class.Comma));
                declartions.Children.Add(match(Token_Class.Identifier));
                declartions.Children.Add(Initialization());
                declartions.Children.Add(Declartions());
                return declartions;
            }
            else
            {
                return null;
            }
        }   
        
        // AssignmentStatement → identifier := epx
        Node AssignmentStatement()
        {
            Node assignmentStatement = new Node("AssignmentStatement");
            assignmentStatement.Children.Add(match(Token_Class.Identifier));
            assignmentStatement.Children.Add(match(Token_Class.Assign));
            assignmentStatement.Children.Add(Expression());
            return assignmentStatement;
        }

    

        //Term → Number | idetifier | FunctionCall 
        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer+1].token_type == Token_Class.LeftParentheses)
                    term.Children.Add(FunctionCall());
                else
                    term.Children.Add(match(Token_Class.Identifier));
            }
            return term;
        }

        // FunctionCall → identifier (Args) 
        Node FunctionCall()
        {
            Node functionCall = new Node("FunctionCall");
            functionCall.Children.Add(match(Token_Class.Identifier));
            functionCall.Children.Add(match(Token_Class.LeftParentheses));
            functionCall.Children.Add(Args());
            functionCall.Children.Add(match(Token_Class.RightParentheses));
            return functionCall;
        }
        // Arg - > Identifier | Number
        Node Arg()
        {
            Node arg = new Node("Arg");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                arg.Children.Add(match(Token_Class.Identifier));
            }
            else
            {
                arg.Children.Add(match(Token_Class.Number));
            }
            return arg;
            
        }

        // Args → Arg OtherArgs |  ε
        Node Args()
        {
            Node args = new Node("Args");
            if (InputPointer < TokenStream.Count && isItAStartOfATerm(0))
            {
                args.Children.Add(Arg());
                args.Children.Add(OtherArgs());
                return args;
            }
            else
            {
                return null;
            }
        }

        // OtherArgs → , Arg OtherArgs | ε
        Node OtherArgs()
        {
            Node otherArgs = new Node("OtherArgs");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                otherArgs.Children.Add(match(Token_Class.Comma));
                otherArgs.Children.Add(Arg());
                otherArgs.Children.Add(OtherArgs());
                return otherArgs;
            }
            else
            {
                return null;
            }
        }



        // WriteStatement → write Printed;       
        Node WriteStatement()
        {
            Node writeStatement = new Node("WriteStatement");
            writeStatement.Children.Add(match(Token_Class.Write));
            writeStatement.Children.Add(Printed());
            writeStatement.Children.Add(match(Token_Class.Semicolon));
            return writeStatement;
        }

        // Printed →  Expression | Endl
        Node Printed()
        {
            Node printed = new Node("Printed");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Endline)
                printed.Children.Add(match(Token_Class.Endline));
            else
                printed.Children.Add(Expression());
            return printed;
        }
        // ReadStatement → read identifier;
        Node ReadStatement()
        {
            Node readStatement = new Node("ReadStatement");
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Identifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));
            return readStatement;
        }

        // ReturnStatement → return exp;
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
            condition.Children.Add(match(Token_Class.Identifier));
            condition.Children.Add(ConOp());
            condition.Children.Add(Term());
            return condition;
        }


        // ConditionStatement → Condition Conditions
        Node ConditionStatement()
        {
            Node conditionStatement = new Node("ConditionStatement");
            conditionStatement.Children.Add(Condition());
            conditionStatement.Children.Add(Conditions());
            return conditionStatement;
        }

        // Conditions → Boolop Condition Conditions |  ε
        Node Conditions()
        {
            Node conditions = new Node("Conditions");
            if (InputPointer < TokenStream.Count && isItAStartOfAnotherCondition())
            {
                conditions.Children.Add(BoolOp());
                conditions.Children.Add(Condition());
                conditions.Children.Add(Conditions());
                return conditions;
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
            inFunctionBody = false;
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


        // IfStatement → if ConditionStatement then Statements [ElseifStatements] [ElseStatement] end
        Node IfStatement()
        {
            Node ifStatement = new Node("IfStatement");
            inFunctionBody = false;
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(ConditionStatement());
            ifStatement.Children.Add(match(Token_Class.Then));
            ifStatement.Children.Add(Statements());
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                ifStatement.Children.Add(ElseIfStatements());
            }
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                ifStatement.Children.Add(ElseStatement());
            }
            ifStatement.Children.Add(match(Token_Class.End));
            return ifStatement;
        }


        // ElseIfStatements → elseif ConditionStatement then Statements ElseIfStatements | ε
        Node ElseIfStatements()
        {
            Node elseIfStatements = new Node("ElseIfStatements");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                elseIfStatements.Children.Add(match(Token_Class.Elseif));
                elseIfStatements.Children.Add(ConditionStatement());
                elseIfStatements.Children.Add(match(Token_Class.Then));
                elseIfStatements.Children.Add(Statements());
                elseIfStatements.Children.Add(ElseIfStatements());
                return elseIfStatements;
            }
            else
            {
                return null;
            }

        }
        
      

        // ElseStatement → else Statements 
        Node ElseStatement()
        {
            Node elseStatement = new Node("ElseStatement");
            elseStatement.Children.Add(match(Token_Class.Else));
            elseStatement.Children.Add(Statements());
            return elseStatement;
        }


        //Expression → string | Term | Equation
        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.String)
            {
                expression.Children.Add(match(Token_Class.String));
            }
            else if (InputPointer < TokenStream.Count && isItAStartOfATerm(0))
            {
                if (InputPointer + 1 < TokenStream.Count && (isItAMultOP(1) || isItAnAddOP(1)))
                    expression.Children.Add(Equation());
                else
                    expression.Children.Add(Term());
            }
            else if(InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LeftParentheses)
            {
                expression.Children.Add(Equation());
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
            if (InputPointer < TokenStream.Count && isItAnAddOP(0))
            {
                subEquations.Children.Add(AddOp());
                subEquations.Children.Add(SubEquation());
                subEquations.Children.Add(SubEquations());
                return subEquations;
            }
            else
            {
                return null;
            }
        }
        // SubEquation → SmallEquation E
        Node SubEquation()
        {
            Node subEquation = new Node("SubEquation");
            subEquation.Children.Add(SmallEquation());
            subEquation.Children.Add(E());
            return subEquation;
        }
        // E → MultOp SmallEquation E | ε
        Node E()
        {
            Node e = new Node("E");
            if (InputPointer < TokenStream.Count && isItAMultOP(0))
            {
                e.Children.Add(MultOp());
                e.Children.Add(SmallEquation());
                e.Children.Add(E());
                return e;
            }
           else
            {
                return null;
            }
        }
      
        // SmallEquation → Oprand Equations | (Oprand Equations)
        Node SmallEquation ()
        {
            Node smallEquation = new Node("SmallEquation");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LeftParentheses)
            {
                smallEquation.Children.Add(match(Token_Class.LeftParentheses));
                smallEquation.Children.Add(Oprand());
                smallEquation.Children.Add(Equations());
                smallEquation.Children.Add(match(Token_Class.RightParentheses));
                return smallEquation;
            }
            else
            {
                smallEquation.Children.Add(Oprand());
                smallEquation.Children.Add(Equations());
                return smallEquation;
            }
        }
        // Equations → AddOp Oprand Equations | ε
        Node Equations()
        {
            Node equations = new Node("Equations");
            if (InputPointer < TokenStream.Count && isItAnAddOP(0) && isItAStartOfATerm(1))
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

        // AddOp → + | -
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

        // MultOp → * | / 
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
                            TokenStream[InputPointer].token_type == Token_Class.Identifier
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
                            TokenStream[InputPointer+offset].token_type == Token_Class.Identifier;
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
