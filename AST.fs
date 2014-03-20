module AST

type ExpressionOrType =
    interface
    end

type Node() =
    class
    end

type Identifier = string

//TYPES

type Type() =
    class
        interface ExpressionOrType
    end

type ReferenseType(name : Identifier) =
    class
        inherit Type()
        let name = name
        member this.Name with get() = name
    end

type BasicType() =
    class
        inherit Type()
    end

type Int() =
    class
        inherit BasicType()
    end

type Byte() =
    class
        inherit BasicType()
    end

type Char() =
    class
        inherit BasicType()
    end

type Boolean() =
    class
        inherit BasicType()
    end

type Long() =
    class
        inherit BasicType()
    end

type Float() =
    class
        inherit BasicType()
    end

type Double() =
    class
        inherit BasicType()
    end
//TYPES

//BLOCK

type VariableInitializer() =
    class
    end

type BlockStatement = 
    interface
    end


type LocalVariableDeclarator(typeName : Type, name : Identifier, bracketCount : int, variableInitializer : VariableInitializer) = 
    class
        let typeName = typeName
        let name = name
        let bracketCount = bracketCount
        let variableInitializer = variableInitializer

        member this.TypeName with get() = typeName
        member this.Name with get() = name
        member this.BracketCount with get() = bracketCount
        member this.VariableInitializer with get() = variableInitializer

        interface BlockStatement
    end
    
type Statement =
    interface
        inherit BlockStatement
    end
and Block(blockStatements : BlockStatement list) = 
    class
        let blockStatements = blockStatements
        
        member this.BlockStatements with get() = blockStatements
        
        interface Statement
    end

//BLOCK

//EXPRESSION

type CreatedName(identifiers: Identifier list) =
    class
        let identifiers = identifiers

        member this.Identifiers with get() = identifiers
    end

type CreatorRest =
    interface
    end

type Creator(createdName: CreatedName, creatorRest: CreatorRest) =
    class
        let createdName = createdName
        let creatorRest = creatorRest

        member this.CreatedName with get() = createdName
        member this.CreatorRest with get() = creatorRest
    end

type Primary =
    interface
    end

type ThisPrimary() =
    class
        interface Primary
    end

type NewCreatorPrimary(creator: Creator) =
    class
        let creator = creator

        member this.Creator with get() = creator
        interface Primary
    end


type IdentifierSuffix =
    interface
    end


type MemberCall(identifiers: Identifier list, identifierSuffix: IdentifierSuffix option) =
    class
        let identifiers = identifiers
        let identifierSuffix = identifierSuffix

        member this.Identifiers with get() = identifiers
        member this.IdentifierSuffix with get() = identifierSuffix

        interface Primary
    end

type Literal =
    interface
        inherit Primary
    end

type IntegerLiteral(value: int) = 
    class
        let value = value

        member this.Value with get() = value

        interface Literal
    end

type FoatingPointLiteral(value: float) = 
    class
        let value = value

        member this.Value with get() = value

        interface Literal
    end

type CharacterLiteral(value: char) = 
    class
        let value = value

        member this.Value with get() = value

        interface Literal
    end

type StringLiteral(value: string) = 
    class
        let value = value

        member this.Value with get() = value

        interface Literal
    end

type BooleanLiteral(value: bool) = 
    class
        let value = value

        member this.Value with get() = value

        interface Literal
    end

//воспринимаем как null
type NullLiteral() = 
    class
        interface Literal
    end


type Selector =
    interface
    end




type Operation(operation: string) =
    class
        let operation = operation
        member this.Operation with get() = operation
    end

type PrefixOp(operation: string) =
    class
        inherit Operation(operation)
        let operation = operation

        member this.Operation with get() = operation
    end
        
type InfixOp(operation: string) =
    class
        inherit Operation(operation)
        let operation = operation

        member this.Operation with get() = operation
    end

type PostfixOp(operation: string) =
    class
        inherit Operation(operation)
        let operation = operation

        member this.Operation with get() = operation
    end

type Expression3 =
    interface
    end

type ExpressionWithBrackets(exprOrType : ExpressionOrType, expr3 : Expression3) = 
    class
        let exprOrType = exprOrType
        let expr3 = expr3

        member this.ExpressionOrType with get() = exprOrType
        member this.Expression3 with get() = expr3

        interface Expression3
    end



type MemberCallExpression(primary : Primary, selectors: Selector list, postfixOpList: PostfixOp list) =
    class
        let primary = primary
        let selectors = selectors
        let postfixOpList = postfixOpList

        member this.Primary with get() = primary
        member this.Selectors with get() = selectors
        member this.PostfixOpList with get() = postfixOpList

        interface Expression3
    end

type PrefixExpression(prefixOp : PrefixOp, expression3 : Expression3) =
    class
        let prefixOp = prefixOp
        let expression3 = expression3

        member this.PrefixOp with get() = prefixOp
        member this.Expression3 with get() = expression3

        interface Expression3
    end

type Expression2Rest =
    interface
    end

type infixExpression(infixOpList : InfixOp list, expression3List : Expression3 list) = 
    class
        let infixOpList = infixOpList
        let expression3List = expression3List

        member this.InfixOpList with get() = infixOpList
        member this.Expression3List with get() = expression3List
        
        interface Expression2Rest
    end

type instanseOf(instanceType : Type) = 
    class
        let instanceType = instanceType

        member this.InstanceType with get() = instanceType

        interface Expression2Rest
    end

type Expression2(expression3 : Expression3, expr2Rest: Expression2Rest option) =
    class
        let expression3 = expression3
        let expr2Rest = expr2Rest

        member this.Expression3 with get() = expression3
        member this.Expr2Rest with get() = expr2Rest
    end

type Expression(expressionBeforeEqual : Expression2, expressionAfterEqual : Expression2 option) =
    class
        inherit VariableInitializer()
        
        let expressionBeforeEqual = expressionBeforeEqual 
        let expressionAfterEqual = expressionAfterEqual

        member this.ExpressionBeforeEqual with get() = expressionBeforeEqual
        member this.ExpressionAfterEqual with get() = expressionAfterEqual

        interface Statement
        interface ExpressionOrType
    end

type SuperSuffix =
    interface
    end

type OptionExpression(expression: Expression option) =
    class
        let expression = expression

        member this.Expression with get() = expression

        interface IdentifierSuffix
        interface Selector
    end

type SuperPrimary(superSuffix: SuperSuffix) =
    class
        let superSuffix = superSuffix

        member this.SuperSuffix with get() = superSuffix

        interface Primary
    end

type Arguments(arguments: Expression list option) =
    class
        let arguments = arguments

        member this.Arguments with get() = arguments

        interface SuperSuffix
        interface IdentifierSuffix
    end

type CallIdentifierWithArguments(identifier: Identifier, arguments: Arguments) =
    class
        let identifier = identifier
        let arguments = arguments

        member this.Identifier with get() = identifier
        member this.Arguments with get() = arguments

        interface Selector
    end

type ArrayCreatorRest =
    interface
        inherit CreatorRest
    end

type ArrayInitializer(varibleInitializerList : VariableInitializer list) =
    class
        inherit VariableInitializer()
        
        let varibleInitializerList = varibleInitializerList

        member this.VaribleInitializerList with get() = varibleInitializerList
        
        interface ArrayCreatorRest
    end

type ListExpression(listExpression: Expression list) =
    class
        let listExpression = listExpression

        member this.ListExpression with get() = listExpression

        interface ArrayCreatorRest
    end
    
type ExpressionInBrackets(expression: Expression) =
    class
        let expression = expression

        member this.Expression with get() = expression

        interface Primary
    end
    
type IfStatement(expression : Expression, thenStatement : Statement, elseStatement : Statement option) = 
    class
        let expression = expression
        let thenStatement = thenStatement
        let elseStatement = elseStatement

        member this.Expression with get() = expression
        member this.ThenStatement with get() = thenStatement
        member this.ElseStatement with get() = elseStatement

        interface Statement
    end

type WhileStatement(expression : Expression, body : Statement) = 
    class
        let expression = expression
        let body = body

        member this.Expression with get() = expression
        member this.Body with get() = body

        interface Statement
    end

//TO DO list option?
//TO DO List Expression?

type ForStatement(forInitExpressions : Expression list, forConditionExpressions : Expression list, forUpdateExpression : Expression list option, body : Statement) = 
    class
        let forInitExpressions = forInitExpressions
        let forConditionExpressions = forConditionExpressions
        let forUpdateExpression = forUpdateExpression
        let body = body

        member this.ForInitExpressions with get() = forInitExpressions
        member this.ForConditionExpressions with get() = forConditionExpressions
        member this.ForUpdateExpression with get() = forUpdateExpression
        member this.Body with get() = body

        interface Statement
    end

type BreakStatement() = 
    class
        interface Statement
    end
    
type ContinueStatement() = 
    class
        interface Statement
    end
    
type ReturnStatement(expression : Expression option) = 
    class
        let expression = expression
        
        member this.Expression with get() = expression
        interface Statement
    end
//EXPRESSION

//CLASS OR INTERFACE

type MemberDecl() =
    class
    end

type MethodOrFieldDecl() =
    class
        inherit MemberDecl()
    end

type FieldDeclarator(typeName : Type, name : Identifier, bracketCount : int, variableInitializer : VariableInitializer) =
    class
        inherit MethodOrFieldDecl()
        let typeName = typeName
        let name = name
        let bracketCount = bracketCount
        let variableInitializer = variableInitializer

        member this.TypeName with get() = typeName
        member this.Name with get() = name
        member this.BracketCount with get() = bracketCount
        member this.VariableInitializer with get() = variableInitializer
    end

type FormalParameter(name : Identifier, bracketCount : int) =
    class
        let bracketCount = bracketCount
        let name = name

        member this.Name with get() = name
        member this.BracketCount with get() = bracketCount
    end

type MethodDeclarator(typeName : Type, name : Identifier, formalParameters : FormalParameter list, body : Block option) =
    class
        inherit MethodOrFieldDecl()
        
        let typeName = typeName
        let methodName = name
        let formalParameters = formalParameters
        let methodBody = body

        member this.TypeName with get() = typeName
        member this.MethodName with get() = methodName
        member this.FormalParameters with get() = formalParameters
        member this.MethodBody with get() = methodBody
    end

type VoidMethodDeclarator(name : Identifier, formalParameters : FormalParameter list, body : Block option) = 
    class
        inherit MemberDecl()
        
        let voidMethodName = name
        let formalParameters = formalParameters
        let voidMethodBody = body

        member this.VoidMethodName with get() = voidMethodName
        member this.FormalParameters with get() = formalParameters
        member this.VoidMethodBody with get() = voidMethodBody
    end

type ClassBodyDeclaration(isStatic : bool, memberDecl : MemberDecl) =
    class
        let isStatic = isStatic
        let memberDecl = memberDecl

        member this.IsStatic with get() = isStatic
        member this.MemberDecl with get() = memberDecl
    end

type ConstructorDeclaratorRest(formalParameters : FormalParameter list, body : Block) =
    class
        let formalParameters = formalParameters
        let constuctorBody = body

        member this.FormalParameters with get() = formalParameters
        member this.ConstuctorBody with get() = constuctorBody
    end

type ClassBody(constructorName : Identifier, constructorDeclaratorRest : ConstructorDeclaratorRest, classBodyDeclarations : ClassBodyDeclaration list) =
    class
        let constructorName = constructorName
        let constructorDeclaratorRest = constructorDeclaratorRest
        let classBodyDeclarations = classBodyDeclarations

        member this.ConstructorName with get() = constructorName
        member this.ConstructorDeclaratorRest with get() = constructorDeclaratorRest
        member this.ClassBodyDeclarations with get() = classBodyDeclarations
    end

type ClassCreatorRest(arguments: Arguments, classBody: ClassBody option) =
    class
        let arguments = arguments
        let classBody = classBody

        member this.Arguments with get() = arguments
        member thus.ClassBody with get() = classBody
        
        interface CreatorRest
    end

type InterfaceBody() =
    class
    end

type InterfacesDeclaration(name : Identifier, extends : Identifier list, body : InterfaceBody) =
    class
       let interfaceName = name
       let extendsList = extends
       let interfaceBody = body
   
       member this.InterfaceName with get() = interfaceName 
       member this.ExtendsList with get() = extendsList
       member this.InterfaceBody with get() = interfaceBody
    
    end

type ClassDeclaration(name : Identifier, extends : Identifier list, implements : Identifier list, body : ClassBody) =
    class
        let className = name
        let extendsList = extends
        let implementsList = implements
        let classBody = body

        member this.ClassName with get() = className 
        member this.ExtendsList with get() = extendsList
        member this.ImplementsList with get() = implementsList
        member this.ClassBody with get() = classBody
    end

type Program(classes: ClassDeclaration list, interfaces: InterfacesDeclaration list) =
    class
        let classes = classes
        let interfaces = interfaces

        member this.Classes with get() = classes
        member this.Interfaces with get() = interfaces
    end