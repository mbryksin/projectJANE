namespace AST 

type Val = 
    | Empty
    | Int   of int64
    | Float of float
    | Str   of string
    | Char  of char
    | Bool  of bool
    | Array of Val array
    | ClassVal of string
    | MethodVal of string * Val list // id + elements
    | Null
