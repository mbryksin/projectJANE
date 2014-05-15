namespace AST

[<AbstractClass>]
type Statement(pos : Position) =
    inherit Node(pos)

    let mutable parent  : Block option  = None
    let mutable context : Variable list = []

    member x.Parent
        with get() = parent
        and  set(value) = parent <- value

    member x.Context 
        with get () = context
        and  set (value) = context <- value

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

and Block(statements : Statement list, pos : Position) =
    inherit Statement(pos)

    member x.Statements = statements

    override x.ToString() = statements
                            |> List.map string
                            |> String.concat ""
                            |> sprintf "{\n%s}\n" 

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type DeclarationStatement(declarationType : Type, name : ID, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body

    override x.ToString() = sprintf "%A %A = %A;\n" declarationType name body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type AssignmentStatement(name : ID, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Name = name
    member x.Body = body

    override x.ToString() = sprintf "%A = %A;\n" name body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type MemberCallStatement(body : Expression, pos : Position) =
    inherit Statement(pos)
    member x.Body = body
    
    override x.ToString() = sprintf "%A;\n" body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IfStatement(condition : Expression, trueStatement : Statement, 
                 falseStatement : Statement option, pos : Position) =
    inherit Statement(pos)
    member x.Condition      = condition
    member x.TrueStatement  = trueStatement
    member x.FalseStatement = falseStatement
    
    override x.ToString() = let ifPart = sprintf "if (%A) %A" condition trueStatement
                            match falseStatement with
                            | None           -> ifPart
                            | Some statement -> sprintf "%s\nelse %A" ifPart statement

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type WhileStatement(condition : Expression, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Condition = condition
    member x.Body      = body

    override x.ToString() = sprintf "while (%A) %A" condition body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ForStatement(init : DeclarationStatement, condition : Expression, 
                  update : AssignmentStatement, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body

    override x.ToString() = sprintf "for (%A %A = %A; %A; %A = %A) %A" init.Type init.Name init.Body 
                                    condition update.Name update.Body body
                            
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BreakStatement(pos : Position) =
    inherit Statement(pos)

    override x.ToString() = "break;"

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ContinueStatement(pos : Position) =
    inherit Statement(pos)

    override x.ToString() = "continue;"

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ReturnStatement(expression : Expression option, pos : Position) =
    inherit Statement(pos)
    member x.Expression = expression

    override x.ToString() = sprintf "return %A;" expression

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type SuperStatement(arguments : Arguments, pos : Position) =
    inherit Statement(pos)
    member x.Arguments = arguments

    override x.ToString() = sprintf "super%A;" arguments
