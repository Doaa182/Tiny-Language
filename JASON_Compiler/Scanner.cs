using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
public enum Token_Class
{
    Int, Float, String, Read, Write, Repeat, Until, If, ElseIf, Else, Then, Return, Endl, Main, End,
    RCurly, LCurly, OrOp, AndOp, GreaterThanOp, LessThanOp, EqualOp, Comma, Semicolon, RParanthesis,
    LParanthesis, DivideOp, MultiplyOp, MinusOp, PlusOp, Dot, Number, Identfier, Colon, NotEqualOp
}
namespace TINY_Compiler
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
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("main()", Token_Class.Main);


            Operators.Add(".", Token_Class.Dot);
            Operators.Add(":", Token_Class.Colon);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("–", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("{", Token_Class.LCurly);
            Operators.Add("}", Token_Class.RCurly);

        }

        public void StartScanning(string SourceCode)
        {

            for (int i = 0; i < SourceCode.Length; i++)
            {

                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                bool Find_Lcur = false;
                if (CurrentChar == ' ' || CurrentChar == '\n' || CurrentChar == '\r' || CurrentChar == '\t')
                    continue;

                if (char.IsLetter(CurrentChar))
                {
                    j++;
                    if (j < SourceCode.Length)
                        CurrentChar = SourceCode[j];
                    while (char.IsLetterOrDigit(CurrentChar))
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j < SourceCode.Length)
                        {


                            CurrentChar = SourceCode[j];
                        }
                        else
                            break;
                    }
                    i = j - 1;
                }
                else if (char.IsDigit(CurrentChar))
                {
                    j++;
                    if (j < SourceCode.Length)
                        CurrentChar = SourceCode[j];

                    while (char.IsDigit(CurrentChar) || CurrentChar == '.')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }

                    }
                    i = j - 1;
                }

                else if (CurrentChar == '\"')
                {

                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar != '\"')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            j++;
                            if (j < SourceCode.Length)
                            {

                                CurrentChar = SourceCode[j];
                            }
                            else
                                break;
                        }
                        if (j < SourceCode.Length && SourceCode[j] == '\"')
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                        }
                    }
                    i = j + 1;
                }
                else if (CurrentChar == '/' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '*')
                {
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentLexeme += SourceCode[i + 1].ToString();
                    j = i + 2;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar != '/')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            j++;
                            if (j < SourceCode.Length)
                            {

                                CurrentChar = SourceCode[j];
                            }
                            else
                                break;
                        }
                    }
                    i = j + 1;
                }

                // read each token between { }
                else if (CurrentChar == '{')
                {
                    int counter = 0; int v = j;
                    j++; counter++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar != '}')
                        {
                            j++; counter++;
                            if (j < SourceCode.Length)
                            {
                                CurrentChar = SourceCode[j];
                            }
                            else
                                break;
                        }
                        if (j < SourceCode.Length && SourceCode[j] == '}')
                        {
                            Find_Lcur = true;
                            //  i = j - counter;
                        }
                    }
                    if (Find_Lcur)
                    {
                        i = j - counter;
                    }
                    else
                    {
                        j = v;
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                            while (CurrentChar != '}')
                            {
                                CurrentLexeme += CurrentChar.ToString();
                                j++;
                                if (j < SourceCode.Length)
                                {
                                    CurrentChar = SourceCode[j];
                                }
                                else
                                    break;
                            }
                            if (j < SourceCode.Length && SourceCode[j] == '}')
                            {
                                CurrentLexeme += SourceCode[j].ToString();
                            }
                        }
                        i = j + 1;
                    }

                }
                //----------------------  For ()  ----------------------

                else if (CurrentChar == '(')
                {
                    int counter = 0; int v = j;
                    j++; counter++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        while (CurrentChar != ')')
                        {
                            j++; counter++;
                            if (j < SourceCode.Length)
                            {
                                CurrentChar = SourceCode[j];
                            }
                            else
                                break;
                        }
                        if (j < SourceCode.Length && SourceCode[j] == ')')
                        {
                            Find_Lcur = true;
                            //  i = j - counter;
                        }
                    }
                    if (Find_Lcur)
                    {
                        i = j - counter;
                    }
                    else
                    {
                        j = v;
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                            while (CurrentChar != ')')
                            {
                                CurrentLexeme += CurrentChar.ToString();
                                j++;
                                if (j < SourceCode.Length)
                                {
                                    CurrentChar = SourceCode[j];
                                }
                                else
                                    break;
                            }
                            if (j < SourceCode.Length && SourceCode[j] == ')')
                            {
                                CurrentLexeme += SourceCode[j].ToString();
                            }
                        }
                        i = j + 1;
                    }

                }

                else if (CurrentChar == '&' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '&')
                {

                    CurrentLexeme += SourceCode[i + 1].ToString();
                    j = i + 2;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == ' ' || Char.IsLetterOrDigit(CurrentChar))
                            i = j;
                    }

                }
                else if (CurrentChar == '|' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '|')
                {

                    CurrentLexeme += SourceCode[i + 1].ToString();
                    j = i + 2;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == ' ' || Char.IsLetterOrDigit(CurrentChar))
                            i = j;
                    }

                }
                else if (CurrentChar == '<' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '>')
                {

                    CurrentLexeme += SourceCode[i + 1].ToString();
                    j = i + 2;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == ' ' || Char.IsLetterOrDigit(CurrentChar))
                            i = j;
                    }

                }
                FindTokenClass(CurrentLexeme);

            }

            TINY_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);

            }
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);

            }
            else if (isInteger(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);

            }
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);

            }

            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identfier;
                Tokens.Add(Tok);
            }
            else
            {
                Errors.Error_List.Add(Lex.ToString() + "  Un recognized");
            }
        }




        bool isInteger(string lex)
        {
            Regex x = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            return (x.IsMatch(lex));
        }
        bool isString(string lex)
        {

            Regex x = new Regex("^\".*\"$", RegexOptions.Compiled);
            return (x.IsMatch(lex));
        }

        bool isIdentifier(string lex)
        {
            Regex c = new Regex("^[a-zA-z][a-zA-z0-9]*$", RegexOptions.Compiled);
            return (c.IsMatch(lex));
        }

        bool isComment(string lex)
        {
            Regex c = new Regex(@"^/\*.?\*/$", RegexOptions.Compiled);
            return (c.IsMatch(lex));
        }

    }
}
