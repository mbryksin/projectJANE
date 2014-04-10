namespace AST

[<AbstractClass>]
type Statement(pos : Position) =
    inherit Node(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type Block(statements : Statement list, pos : Position) =
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

type AssignmentStatement(path : string list, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Path = path
    member x.Body = body

    override x.ToString() = sprintf "%s = %A;\n" (String.concat "." path) body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type MemberCallStatement(body : Expression, pos : Position) =
    inherit Statement(pos)
    member x.Body = body
    
    override x.ToString() = sprintf "%A;\n" body

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IfStatement(condition : Expression, trueStatement : Statement, falseStatement : Statement option, pos : Position) =
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

    override x.ToString() = sprintf "for (%A %A = %A; %A; %s = %A) %A" init.Type init.Name init.Body 
                                    condition (String.concat "." update.Path) update.Body body
                            

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