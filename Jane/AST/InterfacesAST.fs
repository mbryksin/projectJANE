module InterfacesAST

type Identifier = string

type ExpressionOrType =
    interface
    end

//TYPE
type Type =
    interface
        inherit ExpressionOrType
    end

type BasicType =
    interface
        inherit Type
    end
//TYPE

//STATEMENT
type BlockStatement = 
    interface
    end

type Statement =
    interface
        inherit BlockStatement
    end
//STATEMENT

type VariableInitializer =
    interface
    end

//CREATOR
type CreatorRest =
    interface
    end

type ArrayCreatorRest =
    interface
        inherit CreatorRest
    end
//CREATOR

//PRIMARY
type Primary =
    interface
    end

type Literal =
    interface
        inherit Primary
    end
//PRIMARY

//SUFFIX
type IdentifierSuffix =
    interface
    end

type SuperSuffix =
    interface
    end
//SUFFIX

type Selector =
    interface
    end

//EXPRESSION
type Expression3 =
    interface
    end

type Expression2Rest =
    interface
    end
//EXPRESSION

//MEMBER
type MemberDecl =
    interface
    end

type MethodOrFieldDecl =
    interface
        inherit MemberDecl
    end

type InterfaceMemberDecl =
    interface
    end

type InterfaceMethodOrFieldRest = 
    interface
    end
//MEMBER