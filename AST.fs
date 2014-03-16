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
        interface BlockStatement
    end
    
type Statement =
    interface
        inherit BlockStatement
    end
and Block(blockStatements : BlockStatement list) = 
    class
        interface Statement
    end

//BLOCK

//EXPRESSION

type CreatedName(identifiers: Identifier list) =
    class
    end

type CreatorRest =
    interface
    end

type Creator(createdName: CreatedName, creatorRest: CreatorRest) =
    class
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
        interface Primary
    end


type IdentifierSuffix =
    interface
    end


type MemberCall(identifiers: Identifier list, identifierSuffix: IdentifierSuffix option) =
    class
        interface Primary
    end

type Literal =
    interface
        inherit Primary
    end

type IntegerLiteral(value: int) = 
    class
        interface Literal
    end

type FoatingPointLiteral(value: float) = 
    class
        interface Literal
    end

type CharacterLiteral(value: char) = 
    class
        interface Literal
    end

type StringLiteral(value: string) = 
    class
        interface Literal
    end

type BooleanLiteral(value: bool) = 
    class
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
    end
        
type InfixOp(operation: string) =
    class
        inherit Operation(operation)
    end

type PostfixOp(operation: string) =
    class
        inherit Operation(operation)
    end

type Expression3 =
    interface
    end

type ExpressionWithBrackets(exprOrType : ExpressionOrType, expr3 : Expression3) = 
    class
        interface Expression3
    end



type MemberCallExpression(primary : Primary, selectors: Selector list, postficsOp: PostfixOp list) =
    class
        interface Expression3
    end

type PrefixExpression(prefixOp : PrefixOp, expr3 : Expression3) =
    class
        interface Expression3
    end

type Expression2Rest =
    interface
    end

type infixExpression(op : InfixOp list, exprs3 : Expression3 list) = 
    class
        interface Expression2Rest
    end

type instanseOf(typ : Type) = 
    class
        interface Expression2Rest
    end

type Expression2(expression3 : Expression3, expr2Rest: Expression2Rest option) =
    class
    end

type Expression(expressionBeforeEqual : Expression2, expressionAfterEqual : Expression2 option) =
    class
        inherit VariableInitializer()
        interface Statement
        interface ExpressionOrType
    end

type SuperSuffix =
    interface
    end

type OptionExpression(expression: Expression option) =
    class
        interface IdentifierSuffix
        interface Selector
    end

type SuperPrimary(superSuffix: SuperSuffix) =
    class
        interface Primary
    end

type Arguments(arguments: Expression list option) =
    class
        interface SuperSuffix
        interface IdentifierSuffix
    end

type CallIdentifierWithArguments(identifier: Identifier, arguments: Arguments) =
    class
        interface Selector
    end

type ArrayCreatorRest =
    interface
        inherit CreatorRest
    end

type ArrayInitializer(varibleInitializers : VariableInitializer list) =
    class
        inherit VariableInitializer()
        interface ArrayCreatorRest
    end

type ListExpression(listExpression: Expression list) =
    class
        interface ArrayCreatorRest
    end
    
type ExpressionInBrackets(expression: Expression) =
    class
        interface Primary
    end
    
type IfStatement(expression : Expression, thenStatement : Statement, elseStatement : Statement option) = 
    class
        interface Statement
    end

type WhileStatement(expression : Expression, body : Statement) = 
    class
        interface Statement
    end
    
type ForStatement(forInitExpression : Expression list, forConditionExpression : Expression list, forUpdateExpression : Expression list option, body : Statement) = 
    class
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
    end

type FormalParameter(name : Identifier, bracketCount : int) =
    class
    end

type MethodDeclarator(typeName : Type, name : Identifier, formalParameters : FormalParameter list, body : Block option) =
    class
        inherit MethodOrFieldDecl()
    end

type VoidMethodDeclarator(name : Identifier, formalParameters : FormalParameter list, body : Block option) = 
    class
        inherit MemberDecl()
    end

type ClassBodyDeclaration(isStatic : bool, memberDecl : MemberDecl) =
    class
    end

type ConstructorDeclaratorRest(formalParameters : FormalParameter list, body : Block) =
    class
    end

type ClassBody(constructorName : Identifier, constructorDeclaratorRest : ConstructorDeclaratorRest, classBodyDeclarations : ClassBodyDeclaration list) =
    class
    end

type ClassCreatorRest(arguments: Arguments, classBody: ClassBody option) =
    class
        interface CreatorRest
    end

type InterfaceBody() =
    class
    end

type InterfacesDeclaration(name : Identifier, extends : Identifier list, body : InterfaceBody) =
    class
    end

type ClassDeclaration(name : Identifier, extends : Identifier list, implements : Identifier list, body : ClassBody) =
    class
    end

//CLASS OR INTERFACE

type Program(classes: ClassDeclaration list, interfaces: InterfacesDeclaration list) =
    class
    end
