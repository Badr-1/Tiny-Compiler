using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
public enum Token_Class
{

    // Reserved
    Int,Float,String,Read,Write,Repeat,Until,If,Elseif,Else,Then,Return,Endl,End,


    // Oprators
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,AndOp,OrOp, AssignOP,

    //
    Dot, Semicolon, Comma, LParanthesis, RParanthesis,LCurlyBracket,RCurlyBracket,

    // else
    Idenifier, Constant
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
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);


            Operators.Add(":=", Token_Class.AssignOP);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("{", Token_Class.LCurlyBracket);
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
                        bool isThereADot = false;
                        while (j < SourceCode.Length && (isItADigit(SourceCode[j]) || (SourceCode[j] == '.' && !isThereADot)))
                        {
                            // to allow only one dot then reset
                            if (SourceCode[j] == '.')
                                isThereADot = true;
                            CurrentLexeme += SourceCode[j];
                            j++;
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
                        
                        if (SourceCode[j+1] == '>')
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
                      
                        if (SourceCode[j+1] == '|')
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
                        
                        if (SourceCode[j+1] == '&')
                        {
                            j++;
                            CurrentLexeme += SourceCode[j];
                        }
                        i = j;
                    }
                }
                // if it starts with / followed by * then allow every thing except */
                else if (j + 1 < SourceCode.Length && CurrentChar == '/' && SourceCode[j+1] == '*')
                {
                  
                    if (SourceCode[++j] == '*')
                    {
                        int start = j - 1;
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
                       
                        if(!isFinished)
                        {
                            j = start;
                            CurrentLexeme = SourceCode[j].ToString();
                        }
                        i = j;

                    }
                }
                else
                {

                   
                }
                // if it's a comment then ignore
                if (!isComment(CurrentLexeme))
                    FindTokenClass(CurrentLexeme);
            }
            
            Tiny_Compiler.TokenStream = Tokens;



        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            bool isIdentifed = false;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
                isIdentifed = true;
            }

            //Is it an identifier?
            if (!ReservedWords.ContainsKey(Lex) && isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
                isIdentifed = true;

            }

            //Is it a Constant?
            if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
                isIdentifed = true;

            }

            //Is it an operator?
            if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
                isIdentifed = true;

            }
           
            //Is it an undefined?
            if (!isIdentifed)
                Errors.Error_List.Add("\"" + Lex + "\" is undefined");
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
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex regex = new Regex("^(([0-9]+(\\.[0-9]+)?)|(\"(.*)\"))$");
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
            return (c >= 'A' && c <= 'z') || (c >= '0' && c <= '9');
        }

        bool isItEmpty(char c)
        {
            return (c == ' ' || c == '\r' || c == '\n' || c == '\t');
        }

    }
}
