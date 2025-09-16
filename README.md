# Tiny_Compiler

## Tiny CFG


$$
\begin{align}
    \text{Program} &\to \text{[FunctionStatements] MainFunction} \\

    \text{MainFunction} &\to \text{DataType} \ main() \text{FunctionBody} \\

    \text{FunctionStatements} &\to 
        \begin{cases}
        \text{FunctionStatement FunctionStatements} \\
        \epsilon
        \end{cases} \\

    \text{Parameters} &\to
        \begin{cases}
        \text{DataType Identifier OtherParameters} \\
        \epsilon
        \end{cases} \\

    \text{OtherParameters} &\to
        \begin{cases}
        , \text{DataType Identifier OtherParameters} \\
        \epsilon
        \end{cases} \\

    \text{FunctionStatement} &\to \text{FunctionDeclaration FunctionBody} \\

    \text{FunctionDeclaration} &\to \text{DataType Identifier (Parameters)} \\

    \text{FunctionBody} &\to \{\text{Statements ReturnStatement}\} \\

    \text{Statements} &\to \text{Statement State} \\

    \text{State} &\to
        \begin{cases}
        \text{Statements State} \\
        \epsilon
        \end{cases} \\

    \text{Statement} &\to
        \begin{cases}
        \text{IfStatement} \\
        \text{ReturnStatement} \\
        \text{ReadStatement} \\
        \text{WriteStatement} \\
        \text{RepeatStatement} \\
        \text{DeclarationStatement} \\
        \text{AssignmentStatement} \\
        \text{FunctionCall} \\
        \epsilon
        \end{cases} \\

    \text{DeclarationStatement} &\to \text{DataType VarsDeclaration;} \\

    \text{VarsDeclaration} &\to \text{Identifier Initialization Declarations} \\

    \text{Declarations} &\to
        \begin{cases}
        , \text{Identifier Initialization Declarations} \\
        \epsilon
        \end{cases} \\

    \text{Initialization} &\to
        \begin{cases}
        := \text{Expression} \\
        \epsilon
        \end{cases} \\

    \text{AssignmentStatement} &\to \text{Identifier := Expression} \\ 

    \text{Term} &\to
        \begin{cases}
        \text{Number} \\
        \text{Identifier} \\
        \text{FunctionCall}
        \end{cases} \\

    \text{FunctionCall} &\to \text{Identifier (Args)} \\

    \text{Args} &\to
        \begin{cases}
        \text{Arg OtherArgs} \\
        \epsilon
        \end{cases} \\

    \text{OtherArgs} &\to
        \begin{cases}
        , \text{Arg OtherArgs} \\
        \epsilon
        \end{cases} \\

    \text{Arg} &\to
        \begin{cases}
        \text{Identifier} \\
        \text{Number}
        \end{cases} \\

    \text{WriteStatement} &\to \text{write Printed;} \\

    \text{Printed} &\to
        \begin{cases}
        \text{Expression} \\
        Endl
        \end{cases} \\

    \text{ReadStatement} &\to \text{read Identifier;} \\

    \text{ReturnStatement} &\to \text{return Expression;} \\

    \text{Condition} &\to \text{Identifier ConOp Term} \\

    \text{ConditionStatement} &\to \text{Condition Conditions} \\ 

    \text{Conditions} &\to
        \begin{cases}
        \text{Boolop Condition Conditions} \\
        \epsilon
        \end{cases} \\

    \text{RepeatStatement} &\to \text{repeat Statements until ConditionStatement} \\

    \text{DataType} &\to
        \begin{cases}
        int \\
        float \\
        string
        \end{cases} \\

    \text{IfStatement} &\to \text{if ConditionStatement then Statements [ElseifStatements] [ElseStatement] end} \\

    \text{ElseIfStatements} &\to
        \begin{cases}
        \text{elseif ConditionStatement then Statements ElseIfStatements} \\
        \epsilon
        \end{cases} \\

    \text{ElseStatement} &\to \text{else Statements} \\

    \text{Expression} &\to
        \begin{cases}
        \text{String} \\
        \text{Term} \\
        \text{Equation}
        \end{cases} \\

    \text{Equation} &\to \text{SubEquation SubEquations} \\

    \text{SubEquations} &\to
        \begin{cases}
        \text{AddOp SubEquation SubEquations} \\
        \epsilon
        \end{cases} \\

    \text{SubEquation} &\to \text{SmallEquation E} \\

    \text{E} &\to
        \begin{cases}
        \text{MultOp SmallEquation E} \\
        \epsilon
        \end{cases} \\

    \text{SmallEquation} &\to
        \begin{cases}
        \text{Oprand Equations} \\
        ( \text{Oprand Equations} )
        \end{cases} \\

    \text{Equations} &\to
        \begin{cases}
        \text{AddOp Oprand Equations} \\
        \epsilon
        \end{cases} \\

    \text{Oprand} &\to \text{Term Ter} \\

    \text{Ter} &\to
        \begin{cases}
        \text{MultOp Term Ter} \\
        \epsilon
        \end{cases} \\

    \text{BoolOp} &\to
        \begin{cases}
        || \\
        \&\&
        \end{cases} \\

    \text{AddOp} &\to
        \begin{cases}
        + \\
        -
        \end{cases} \\

    \text{MultOp} &\to
        \begin{cases}
        * \\
        /
        \end{cases}
\end{align}
$$
