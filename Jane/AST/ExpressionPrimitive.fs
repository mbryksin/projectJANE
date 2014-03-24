namespace AST

type Constant =
    interface 
    end

type NullConstant() =
    interface Constant

type ByteConstant(content : int8) =
    interface Constant
    member x.Get = content

type CharConstant(content : char) =
    interface Constant
    member x.Get = content

type StringConstant(content : string) =
    interface Constant
    member x.Get = content

type IntConstant(content : int) =
    interface Constant
    member x.Get = content

type LongConstant(content : int64) =
    interface Constant
    member x.Get = content

type ShortConstant(content : int16) =
    interface Constant
    member x.Get = content

type BooleanConstant(content : bool) =
    interface Constant
    member x.Get = content

type FloatConstant(content : float32) =
    interface Constant
    member x.Get = content

type DoubleConstant(content : float) =
    interface Constant
    member x.Get = content