module TypesAST
open InterfacesAST

//TYPES
type ReferenseType(identifier : Identifier, bracketsCount : int) =
    class
        let identifier = identifier
        let bracketsCount = bracketsCount
        
        member this.Identifier with get() = identifier
        member this.BracketsCount with get() = bracketsCount
        interface Type
    end

type ArrayType(basicType : BasicType, bracketsCount : int) =
    class
        let basicType = basicType
        let bracketsCount = bracketsCount

        member this.BasicType with get() = basicType
        member this.BracketsCount with get() = bracketsCount
        interface Type
    end

type Int() =
    class
        interface BasicType
    end

type Byte() =
    class
        interface BasicType
    end

type Char() =
    class
        interface BasicType
    end

type Boolean() =
    class
        interface BasicType
    end

type Long() =
    class
        interface BasicType
    end

type Float() =
    class
        interface BasicType
    end

type Double() =
    class
        interface BasicType
    end
//TYPES