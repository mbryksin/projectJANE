
type Node() =
    class
    end

type Identifier = string

//TYPES

type Type() =
    class
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


type ArrayInitializer(varibleInitializers : VariableInitializer list) =
    class
        inherit VariableInitializer()
    end

type Expression((*args*)) =
    class
        inherit VariableInitializer()
        interface Statement
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
