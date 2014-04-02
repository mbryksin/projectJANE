namespace AST

[<AbstractClass>]
type Statement(pos : Position) =
    inherit Node(pos)

type Block(statements : Statement list, pos : Position) =
    inherit Statement(pos)
    member x.Statements = statements

type DeclarationStatement(declarationType : Type, name : string, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body

type AssignmentStatement(path : string list, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Path = path
    member x.Body = body

type MemberCallStatement(body : Expression, pos : Position) =
    inherit Statement(pos)
    member x.Body = body
    
type IfStatement(condition : Expression, trueStatement : Statement, falseStatement : Statement option, pos : Position) =
    inherit Statement(pos)
    member x.Condition      = condition
    member x.TrueStatement  = trueStatement
    member x.FalseStatement = falseStatement

type WhileStatement(condition : Expression, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Condition = condition
    member x.Body      = body

type ForStatement(init : DeclarationStatement list, condition : Expression option, 
                  update : AssignmentStatement list, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body

type BreakStatement(pos : Position) =
    inherit Statement(pos)

type ContinueStatement(pos : Position) =
    inherit Statement(pos)

type ReturnStatement(expression : Expression option, pos : Position) =
    inherit Statement(pos)
    member x.Expression = expression

type SuperStatement(arguments : Expression list, pos : Position) =
    inherit Statement(pos)
    member x.Arguments = arguments
