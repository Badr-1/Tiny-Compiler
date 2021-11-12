using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
public enum Token_Class
{

    // DatatTypes
    DataType_Int,DataType_Float,DataType_String,
    
    // reserved
    Read,Write,Repeat,Until,If,Elseif,Else,Then,Return,Endline,End,


    // Oprators
    Equal, LessThan, GreaterThan, NotEqual, Plus, Minus, Multiply, Division,And,Or, Assign,

    //
    Dot, Semicolon, Comma, LeftParentheses, RightParentheses,LeftBraces,RightBraces,

    // else
    Idenifier, Number ,Comment, String
}
namespace Tiny_Compiler
{

    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();


        public Scanner()
        {

            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("int", Token_Class.DataType_Int);
            ReservedWords.Add("float", Token_Class.DataType_Float);
            ReservedWords.Add("string", Token_Class.DataType_String);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endline);


            Operators.Add(":=", Token_Class.Assign);
            Operators.Add("=", Token_Class.Equal);
            Operators.Add("<", Token_Class.LessThan);
            Operators.Add(">", Token_Class.GreaterThan);
            Operators.Add("<>", Token_Class.NotEqual);
            Operators.Add("+", Token_Class.Plus);
            Operators.Add("-", Token_Class.Minus);
            Operators.Add("*", Token_Class.Multiply);
            Operators.Add("/", Token_Class.Division);
            Operators.Add("&&", Token_Class.And);
            Operators.Add("||", Token_Class.Or);
            Operators.Add(")", Token_Class.RightParentheses);
            Operators.Add("(", Token_Class.LeftParentheses);
            Operators.Add("}", Token_Class.RightBraces);
            Operators.Add("{", Token_Class.LeftBraces);
            Operators.Add(".", Token_Class.Dot);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add(";", Token_Class.Semicolon);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (isItEmpty(CurrentChar))
                    continue;

                // if it starts with a char it can be a reserevedword or an identifier (allow  only digits and letters to the lexeme) 
                if (isItALetter(CurrentChar)) //if you read a character
                {
                    // if it's not the last char
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        while (j < SourceCode.Length && (isItALetterOrADigit(SourceCode[j])))
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                        }
                        j--;
                        i = j;
                    }
                }

                // if it starts with a digit it can only be a number (allow only digits and one dot)
                else if (isItADigit(CurrentChar))
                {

                    // if it's not the last char
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        while (j < SourceCode.Length && (isItADigit(SourceCode[j])))
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                        }
                        if (j < SourceCode.Length && SourceCode[j] == '.')
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                            while (j < SourceCode.Length && (isItADigit(SourceCode[j])))
                            {
                                CurrentLexeme += SourceCode[j];
                                j++;
                            }
                        }
                        j--;
                        i = j;
                    }
                }

                // if it starts with a " then every thing is allowed till typing " again
                else if (CurrentChar == '"')
                {
                    // if it's not the last char
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        while (j < SourceCode.Length && !(SourceCode[j] == '"'))
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                        }
                        if (j < SourceCode.Length)
                            CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                // if it starts with : and followed by a = it's an assign lexeme
                else if (CurrentChar == ':')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        if (SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                        }

                        i = j;
                    }
                }
                // if it starts with < if the next is > then it's a notEqualOp or it can be just LessThanoOp
                else if (CurrentChar == '<')
                {
                    if (j + 1 < SourceCode.Length)
                    {

                        if (SourceCode[j + 1] == '>')
                        {
                            j++;
                            CurrentLexeme += SourceCode[j];
                        }
                        i = j;
                    }
                }
                // if it starts with | check if it's followed by another | to get OrOp
                else if (CurrentChar == '|')
                {
                    if (j + 1 < SourceCode.Length)
                    {

                        if (SourceCode[j + 1] == '|')
                        {
                            j++;
                            CurrentLexeme += SourceCode[j];
                        }
                        i = j;
                    }
                }
                // if it starts with | check if it's followed by another | to get AndOp
                else if (CurrentChar == '&')
                {
                    if (j + 1 < SourceCode.Length)
                    {

                        if (SourceCode[j + 1] == '&')
                        {
                            j++;
                            CurrentLexeme += SourceCode[j];
                        }
                        i = j;
                    }
                }
                // if it starts with / followed by * then allow every thing except */
                else if (j + 1 < SourceCode.Length && CurrentChar == '/' && SourceCode[j + 1] == '*')
                {

                    if (SourceCode[++j] == '*')
                    {

                        bool isAboutToFinish = false;
                        bool isFinished = false;
                        CurrentLexeme += SourceCode[j];
                        j++;
                        while (j < SourceCode.Length && !isFinished)
                        {

                            if (isAboutToFinish && SourceCode[j] == '/')
                            {
                                CurrentLexeme += SourceCode[j];
                                isFinished = true;
                            }
                            else
                            {
                                CurrentLexeme += SourceCode[j];
                                isAboutToFinish = (SourceCode[j] == '*');
                            }
                            j++;
                        }
                        j--;

                        i = j;

                    }
                }
                else
                {


                }
              
                FindTokenClass(CurrentLexeme);
            }

            Tiny_Compiler.TokenStream = Tokens;



        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            bool isIdentified = false;
          

            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
                isIdentified = true;
            }

            //Is it an identifier?
            if (!ReservedWords.ContainsKey(Lex) && isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
                isIdentified = true;

            }

            //Is it a Number?
            if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
                isIdentified = true;

            }

            //Is it a Number?
            if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
                isIdentified = true;

            }

            //Is it an operator?
            if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
                isIdentified = true;

            }

            // Is it a comment
            if(isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
                isIdentified = true;
            }

            //Is it an undefined?
            if (!isIdentified)
                Errors.Error_List.Add(Lex + " is undefined");
        }

        private bool isComment(string lex)
        {
            Regex regex = new Regex(@"^((\/\*)(.*)(\*\/))$");
            return regex.IsMatch(lex);
        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            Regex regex = new Regex("^([a-zA-Z][a-zA-Z0-9]*)$");
            isValid = regex.IsMatch(lex);

            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex regex = new Regex("^([0-9]+(\\.[0-9]+)?)$");
            isValid = regex.IsMatch(lex);
            return isValid;
        }

        bool isString(string lex)
        {
            bool isValid = true;
            Regex regex = new Regex("(\"(.*)\")");
            isValid = regex.IsMatch(lex);
            return isValid;
        }
        bool isItALetter(char c)
        {
            return (c >= 'A' && c <= 'z');
        }
        bool isItADigit(char c)
        {
            return (c >= '0' && c <= '9');
        }
        bool isItALetterOrADigit(char c)
        {
            return isItADigit(c) || isItALetter(c); 
        }

        bool isItEmpty(char c)
        {
            return (c == ' ' || c == '\r' || c == '\n' || c == '\t');
        }

    }
}
