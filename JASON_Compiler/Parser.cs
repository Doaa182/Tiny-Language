using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
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
        Node Program()
        {
             Node program = new Node("program");
            program.Children.Add(Func());
            program.Children.Add(Main());
            return program;
        }
        Node Func()
        {
            Node func = new Node("func");
            if (InputPointer < TokenStream.Count)
            {
                /* if (Token_Class.Int == TokenStream[InputPointer].token_type ||
                     Token_Class.String == TokenStream[InputPointer].token_type ||
                     Token_Class.Float == TokenStream[InputPointer].token_type)
                 {*/
                if (Function_Statement() != null)
                {
                    func.Children.Add(Function_Statement());
                    func.Children.Add(FuncDash());

                }
                else
                {
                    func.Children.Add(FuncDash());
                }

            }
            return func;
        }
        Node FuncDash()
        {
            Node funcDash = new Node("funcDash");
            if (InputPointer < TokenStream.Count)
            {
                /*if (Token_Class.Int == TokenStream[InputPointer].token_type ||
                    Token_Class.String == TokenStream[InputPointer].token_type ||
                    Token_Class.Float == TokenStream[InputPointer].token_type)
                {*/
                if (Function_Statement() != null) { 
                    funcDash.Children.Add(Function_Statement());
                    funcDash.Children.Add(FuncDash());
                }
                else
                {
                    return null;
                }

            }
            return funcDash;
        }
        Node Main()
        {
            Node main = new Node("main");
            main.Children.Add(Datatype());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LParanthesis));
            main.Children.Add(match(Token_Class.RParanthesis));
            main.Children.Add(Function_Body());
            return main;
        }
        Node Function_Statement()
        {
            Node function = new Node("function");
            function.Children.Add(Function_Declaration());
            function.Children.Add(Function_Body());
            return function;
        }
        Node Datatype()
        {
            Node datatype = new Node("datatype");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Int == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Int));
                }
                else if (Token_Class.Float == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Float));
                }
                else if (Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.String));
                }
            }
            return datatype;
        }
        Node Function_Declaration()
        {
            Node function_declaration = new Node("function_declaration");
            function_declaration.Children.Add(Datatype());
            function_declaration.Children.Add(Function_Name());
            function_declaration.Children.Add(match(Token_Class.LParanthesis));

            
            if (InputPointer < TokenStream.Count)
            {
                if (Parameter() != null)
                {
                    function_declaration.Children.Add(Parameter());
                    if (Par() != null)
                    {
                        function_declaration.Children.Add(Par());
                    }
                    else
                    {
                        function_declaration.Children.Add(match(Token_Class.RParanthesis));
                        return null;
                    }
                }
            }
            else
            {
                function_declaration.Children.Add(match(Token_Class.RParanthesis));
                return null;
            }
            //function_declaration.Children.Add(Par());

            function_declaration.Children.Add(match(Token_Class.RParanthesis));
            return function_declaration;
        }
        Node Function_Name()
        {
            Node function_name = new Node("function_name");
            function_name.Children.Add(match(Token_Class.Identfier));
            return function_name;
        }
        Node Parameter()
        {
            Node parameter = new Node("parameter");
            parameter.Children.Add(Datatype());
            parameter.Children.Add(match(Token_Class.Identfier));
            return parameter;
        }
        Node Par()
        {
            Node par = new Node("par");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    par.Children.Add(match(Token_Class.Comma));
                    par.Children.Add(Parameter());
                    par.Children.Add(ParDash());
                }
                else
                {
                    par.Children.Add(ParDash());
                }
            }
            return par;
        }
        Node ParDash()
        {
            Node parDash = new Node("parDash");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    parDash.Children.Add(match(Token_Class.Comma));
                    parDash.Children.Add(Parameter());
                    parDash.Children.Add(ParDash());
                }
                else
                {
                    return null;
                }
            }
            return parDash;
        }
        Node Function_Body()
        {
            Node function_body = new Node("function_body");
            function_body.Children.Add(match(Token_Class.LCurly));
            function_body.Children.Add(State());
            function_body.Children.Add(Return_Statement());
            function_body.Children.Add(match(Token_Class.RCurly));
            return function_body;
        }
        Node Return_Statement()
        {
            Node return_statement = new Node("return_statement");
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.Semicolon));
            return return_statement;
        }
        Node Read_Statement()
        {
            Node read_statement = new Node("read_statement ");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(match(Token_Class.Identfier));
            read_statement.Children.Add(match(Token_Class.Semicolon));
            return read_statement;
        }
        Node Write_Statement()
        {
            Node write_statement = new Node("write_statement ");
            write_statement.Children.Add(match(Token_Class.Write));
            if (InputPointer < TokenStream.Count)
            {
                if (Expression() != null)
                {
                    write_statement.Children.Add(Expression());
                }
                else if (Token_Class.Endl == TokenStream[InputPointer].token_type)
                {
                    write_statement.Children.Add(match(Token_Class.Endl));
                }
            }
            write_statement.Children.Add(match(Token_Class.Semicolon));
            return write_statement;
        }
        Node Declaration_Statement()
        {
            Node declaration_statement = new Node("declaration_statement");
            declaration_statement.Children.Add(Datatype());
            declaration_statement.Children.Add(match(Token_Class.Identfier));
            declaration_statement.Children.Add(Assign());
            declaration_statement.Children.Add(match(Token_Class.Semicolon));
            //-------------------missing code
            return declaration_statement;
        }
        Node Assign()
        {
            Node assign = new Node("assign");
            if (InputPointer < TokenStream.Count)
            {
                if (AssignDash() == null)
                {
                    if (Token_Class.Comma == TokenStream[InputPointer].token_type &&
                        Token_Class.Identfier == TokenStream[InputPointer + 1].token_type)
                    {

                        assign.Children.Add(match(Token_Class.Comma));
                        assign.Children.Add(match(Token_Class.Identfier));
                        assign.Children.Add(AssignDash());
                    }
                    else if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                    {
                        assign.Children.Add(match(Token_Class.Comma));
                        assign.Children.Add(Assignment_Statement());
                        assign.Children.Add(AssignDash());
                    }
                }

                else { assign.Children.Add(AssignDash()); }
            }
            return assign;
        }
        Node AssignDash()
        {
            Node assignDash = new Node("assignDash");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type &&
                    Token_Class.Identfier == TokenStream[InputPointer + 1].token_type)
                {

                    assignDash.Children.Add(match(Token_Class.Comma));
                    assignDash.Children.Add(match(Token_Class.Identfier));
                    assignDash.Children.Add(AssignDash());
                }
                else if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    assignDash.Children.Add(match(Token_Class.Comma));
                    assignDash.Children.Add(Assignment_Statement());
                    assignDash.Children.Add(AssignDash());
                }

                else { return null; }
            }
            return assignDash;
        }
        Node Assignment_Statement()
        {
            Node assignment_statement = new Node("assignment_statement");
            assignment_statement.Children.Add(match(Token_Class.Identfier));
            assignment_statement.Children.Add(match(Token_Class.Colon));
            assignment_statement.Children.Add(match(Token_Class.EqualOp));
            assignment_statement.Children.Add(Exp());
            return assignment_statement;
        }
        Node Expression() 
        {
            Node expression = new Node("Expression");

            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    expression.Children.Add(match(Token_Class.String));
                }
                else if (Term() != null)
                {
                    expression.Children.Add(Term());
                }
                else if (Equation() != null)
                {
                    expression.Children.Add(Equation());
                }
            }

            return expression;
        }
        Node Term()
        {
            Node term = new Node("Term"); 
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Number == TokenStream[InputPointer].token_type)
                {
                    term.Children.Add(match(Token_Class.Number));
                }
                else if (Token_Class.Identfier == TokenStream[InputPointer].token_type)
                {
                    term.Children.Add(match(Token_Class.Identfier));
                }
                else if (Function_Call() != null)
                {
                    term.Children.Add(Function_Call());

                }
                else if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    term.Children.Add(match(Token_Class.LParanthesis));
                    term.Children.Add(Equation());
                    term.Children.Add(match(Token_Class.RParanthesis));

                }
            }
            return term;
        }
       
        Node Equation()
        {
            Node equation = new Node("equation");
            equation.Children.Add(Term());
            equation.Children.Add(Eq());
            return equation;
        }
        Node Eq()
        {
            Node eq = new Node("eq");
            if (InputPointer < TokenStream.Count)
            {
                if (Arthimetic_Operator() != null)
                {
                    eq.Children.Add(Arthimetic_Operator());
                    eq.Children.Add(Term());
                    eq.Children.Add(Eq());
                }
                else
                {
                    return null;
                }
            }
            return eq;
        }
        Node Arthimetic_Operator()
        {
            Node arthimetic_operator = new Node("arthimetic_operator");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.PlusOp == TokenStream[InputPointer].token_type)
                {
                    arthimetic_operator.Children.Add(match(Token_Class.PlusOp));
                }
                else if (Token_Class.MinusOp == TokenStream[InputPointer].token_type)
                {
                    arthimetic_operator.Children.Add(match(Token_Class.MinusOp));
                }
                else if (Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                {
                    arthimetic_operator.Children.Add(match(Token_Class.DivideOp));
                }
                else if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
                {
                    arthimetic_operator.Children.Add(match(Token_Class.MultiplyOp));
                }
            }
            return arthimetic_operator;
        }
        Node Function_Call()
        {
            Node function_call = new Node("function_call");
            function_call.Children.Add(match(Token_Class.Identfier));
            function_call.Children.Add(match(Token_Class.LParanthesis));
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Identfier == TokenStream[InputPointer].token_type)
                {
                    function_call.Children.Add(match(Token_Class.Identfier));

                    if (Id() != null)
                    {
                        function_call.Children.Add(Id());
                    }
                    else
                    {
                        function_call.Children.Add(match(Token_Class.RParanthesis));
                        return null;
                    }
                }
            }
            else
            {
                function_call.Children.Add(match(Token_Class.RParanthesis));
                return null;
            }
            function_call.Children.Add(match(Token_Class.RParanthesis));
            return function_call;
        }
        Node Id()
        {
            Node id = new Node("id");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    id.Children.Add(match(Token_Class.Comma));
                    id.Children.Add(match(Token_Class.Identfier));
                    id.Children.Add(IdDash());
                }
                else
                {
                    id.Children.Add(IdDash());

                }
            }
            
            return id;
        }
        Node IdDash()
        {
            Node idDash = new Node("idDash");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    idDash.Children.Add(match(Token_Class.Comma));
                    idDash.Children.Add(match(Token_Class.Identfier));
                    idDash.Children.Add(IdDash());
                }
                else
                {
                    return null;
                }
            }
            return idDash;
        }
        Node Exp()
        {
            Node exp = new Node("exp");
            exp.Children.Add(Expression());
            exp.Children.Add(ExpDash());
            return exp;
        }
        Node ExpDash()
        {
            Node expDash = new Node("expDash");
            if (InputPointer < TokenStream.Count)
            {
                if (Expression() != null)
                {
                    expDash.Children.Add(Expression());
                    expDash.Children.Add(ExpDash());
                }
                else
                {
                    return null;
                }
            }
            return expDash;
        }
        Node Repeat_Statement()
        {
            Node repeat_statement = new Node("repeat_statement ");
            repeat_statement.Children.Add(match(Token_Class.Repeat));
            repeat_statement.Children.Add(State());
            repeat_statement.Children.Add(match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_Statement());
            return repeat_statement;
        }
        Node Condition_Statement()
        {
            Node condition_statement = new Node("condition_statement ");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Cond());
            return condition_statement;
        }
        Node Condition()
        {
            Node condition = new Node("condition");
            condition.Children.Add(match(Token_Class.Identfier));
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());
            // not implemented 
            return condition;
        }
        Node Condition_Operator()
        {
            Node condition_operator = new Node("condition_operator");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LessThanOp == TokenStream[InputPointer].token_type)
                {
                    condition_operator.Children.Add(match(Token_Class.LessThanOp));
                }
                else if (Token_Class.GreaterThanOp == TokenStream[InputPointer].token_type)
                {
                    condition_operator.Children.Add(match(Token_Class.GreaterThanOp));
                }
                else if (Token_Class.EqualOp == TokenStream[InputPointer].token_type)
                {
                    condition_operator.Children.Add(match(Token_Class.EqualOp));
                }
                else if (Token_Class.NotEqualOp == TokenStream[InputPointer].token_type)
                {
                    condition_operator.Children.Add(match(Token_Class.NotEqualOp));
                }
            }
            return condition_operator;
        }
        Node Boolean_Operator()
        {
            Node boolean_operator = new Node("boolean_operator");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.AndOp == TokenStream[InputPointer].token_type)
                {
                    boolean_operator.Children.Add(match(Token_Class.AndOp));
                }
                else if (Token_Class.OrOp == TokenStream[InputPointer].token_type)
                {
                    boolean_operator.Children.Add(match(Token_Class.OrOp));
                }

            }
            return boolean_operator;
        }
        Node Cond()
        {
            Node cond = new Node("cond");
            if (InputPointer < TokenStream.Count)
            {
                /*if (Token_Class.AndOp == TokenStream[InputPointer].token_type ||
                    Token_Class.OrOp == TokenStream[InputPointer].token_type)
                {*/
                if (Boolean_Operator() != null) { 
                    cond.Children.Add(Boolean_Operator());
                    cond.Children.Add(Condition());
                    cond.Children.Add(CondDash());
                }
                else
                {
                    cond.Children.Add(CondDash());
                }
            }
            return cond;
        }
        Node CondDash()
        {
            Node condDash = new Node("condDash");
            if (InputPointer < TokenStream.Count)
            {
                /*if (Token_Class.AndOp == TokenStream[InputPointer].token_type ||
                    Token_Class.OrOp == TokenStream[InputPointer].token_type)
                {*/
                if (Boolean_Operator() != null)
                {
                    condDash.Children.Add(Boolean_Operator());
                    condDash.Children.Add(Condition());
                    condDash.Children.Add(CondDash());
                }
                else
                {
                    return null;
                }
            }

            return condDash;
        }
        Node Else_Statement()
        {
            Node else_statement = new Node("else_statement ");
            else_statement.Children.Add(match(Token_Class.Else));
            else_statement.Children.Add(State());
            else_statement.Children.Add(match(Token_Class.End));
            return else_statement;
        }
        Node Else_If_Statement()
        {
            Node else_if_statement = new Node("else_if_statement ");
            else_if_statement.Children.Add(match(Token_Class.ElseIf));
            else_if_statement.Children.Add(Condition_Statement());
            else_if_statement.Children.Add(match(Token_Class.Then));
            else_if_statement.Children.Add(State());
            if (InputPointer < TokenStream.Count)
            {
                if (Else_If_Statement() != null)
                {
                    else_if_statement.Children.Add(Else_If_Statement());
                }
                else if (Else_Statement() != null)
                {
                    else_if_statement.Children.Add(Else_Statement());
                }
                else if (Token_Class.End == TokenStream[InputPointer].token_type)
                {
                    else_if_statement.Children.Add(match(Token_Class.End));
                }
            }
            //------Missing code 
            return else_if_statement;
        }
        Node If_Statement()
        {
            Node if_statement = new Node("if_statement ");
            if_statement.Children.Add(match(Token_Class.If));
            if_statement.Children.Add(Condition_Statement());
            if_statement.Children.Add(match(Token_Class.Then));
            if_statement.Children.Add(State());
            if (InputPointer < TokenStream.Count)
            {
                if (Else_If_Statement() != null)
                {
                    if_statement.Children.Add(Else_If_Statement());
                }
                else if (Else_Statement() != null)
                {
                    if_statement.Children.Add(Else_Statement());
                }
                else if (Token_Class.End == TokenStream[InputPointer].token_type)
                {
                    if_statement.Children.Add(match(Token_Class.End));
                }
            }
            return if_statement;
        }
        Node State()
        {
            Node state = new Node("state");
            state.Children.Add(Statements());
            state.Children.Add(StateDash());
            return state;
        }
        Node StateDash()
        {
            Node stateDash = new Node("stateDash");
            if (InputPointer < TokenStream.Count)
            {
                if (Statements() != null)
                {
                    stateDash.Children.Add(Statements());
                    stateDash.Children.Add(StateDash());
                }
                else
                {
                    return null;
                }
            }
            return stateDash;
        }
        Node Statements()
        {
            Node statements = new Node("statements");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Read == TokenStream[InputPointer].token_type)
                {

                    statements.Children.Add(Read_Statement());
                }
                else if (Token_Class.Write == TokenStream[InputPointer].token_type)
                {
                    statements.Children.Add(Write_Statement());

                }
                else if (Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    statements.Children.Add(If_Statement());
                }
                else if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {
                    statements.Children.Add(Repeat_Statement());
                }
                else if (Assignment_Statement() != null)
                {
                    statements.Children.Add(Assignment_Statement());
                }   
                 else if(Function_Call()!=null)
                 {
                    statements.Children.Add(Function_Call());
                 }
               /* else if (Token_Class.Int == TokenStream[InputPointer].token_type ||
                    Token_Class.String == TokenStream[InputPointer].token_type ||
                    Token_Class.Float == TokenStream[InputPointer].token_type)
                {*/
               else if (Declaration_Statement() != null) { 
                    statements.Children.Add(Declaration_Statement());
               }

            }
            return statements;
        }

        /*------------------------------------------------------------------------------*/

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
